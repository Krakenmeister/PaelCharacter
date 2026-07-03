using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Creatures;
using PaelCharacter.PaelCharacterCode.Utility;

namespace PaelCharacter.PaelCharacterCode.Patches.Hypnotize;

[HarmonyPatch(typeof(Creature), nameof(Creature.PrepareForNextTurn))]
public static class CreaturePrepareForNextTurnPatch
{
    [HarmonyPostfix]
    public static void RecordPreviousIntent(Creature __instance)
    {
        EnemyIntentHistory.RecordPreviousIntent(__instance);
    }
}