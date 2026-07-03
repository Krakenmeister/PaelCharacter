using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using PaelCharacter.PaelCharacterCode.Utility;

namespace PaelCharacter.PaelCharacterCode.Patches.Hypnotize;

[HarmonyPatch(typeof(CombatState), nameof(CombatState.RemoveCreature))]
public class CreatureClearHistoryPatch
{
    [HarmonyPrefix]
    public static void RemoveCreatureHistory(Creature creature)
    {
        EnemyIntentHistory.Clear(creature);
    }
}