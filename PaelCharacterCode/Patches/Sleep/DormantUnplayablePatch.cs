using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using PaelCharacter.PaelCharacterCode.CustomEnums;
using PaelCharacter.PaelCharacterCode.Extensions;

namespace PaelCharacter.PaelCharacterCode.Patches.Sleep;

[HarmonyPatch]
public static class DormantUnplayablePatch
{
    private static MethodBase TargetMethod()
    {
        return AccessTools.Method(
            typeof(CardModel),
            nameof(CardModel.CanPlay),
            [
                typeof(UnplayableReason).MakeByRefType(),
                typeof(AbstractModel).MakeByRefType()
            ]
        );
    }

    public static void Postfix(
        CardModel __instance,
        ref bool __result,
        ref UnplayableReason reason,
        ref AbstractModel? preventer
    )
    {
        if (!__instance.IsDormant())
        {
            return;
        }

        reason |= PaelUnplayableReasons.Dormant;
        __result = false;
    }
}