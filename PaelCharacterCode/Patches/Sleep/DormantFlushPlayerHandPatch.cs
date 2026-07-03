using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using PaelCharacter.PaelCharacterCode.Utility;

namespace PaelCharacter.PaelCharacterCode.Patches.Sleep;

[HarmonyPatch]
public static class DormantFlushPlayerHandPatch
{
    private static MethodBase TargetMethod()
    {
        MethodInfo flushMethod = AccessTools.Method(
            typeof(CombatManager),
            "FlushPlayerHand",
            [
                typeof(Player),
                typeof(HookPlayerChoiceContext)
            ]
        );

        AsyncStateMachineAttribute? asyncAttribute =
            flushMethod.GetCustomAttribute<AsyncStateMachineAttribute>();

        if (asyncAttribute == null)
        {
            throw new MissingMethodException(
                "FlushPlayerHand did not have an AsyncStateMachineAttribute."
            );
        }

        return AccessTools.Method(asyncAttribute.StateMachineType, "MoveNext");
    }

    private static IEnumerable<CodeInstruction> Transpiler(
        IEnumerable<CodeInstruction> instructions
    )
    {
        MethodInfo shouldRetainGetter = AccessTools.PropertyGetter(
            typeof(CardModel),
            nameof(CardModel.ShouldRetainThisTurn)
        );

        MethodInfo dormantRetainMethod = AccessTools.Method(
            typeof(DormantRetainLogic),
            nameof(DormantRetainLogic.ShouldRetainThisTurnOrDormant)
        );

        int replacements = 0;

        foreach (CodeInstruction instruction in instructions)
        {
            if (instruction.Calls(shouldRetainGetter))
            {
                replacements++;

                // Original stack:
                //   CardModel
                //
                // Original:
                //   callvirt instance bool CardModel::get_ShouldRetainThisTurn()
                //
                // Replacement:
                //   call bool DormantRetainLogic::ShouldRetainThisTurnOrDormant(CardModel)

                yield return new CodeInstruction(OpCodes.Call, dormantRetainMethod)
                    .WithLabels(instruction.labels)
                    .WithBlocks(instruction.blocks);

                continue;
            }

            yield return instruction;
        }

        if (replacements == 0)
        {
            throw new InvalidOperationException(
                "Could not find CardModel.ShouldRetainThisTurn getter inside FlushPlayerHand MoveNext."
            );
        }
    }
}