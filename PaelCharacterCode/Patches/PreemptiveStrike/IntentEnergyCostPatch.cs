using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using PaelCharacter.PaelCharacterCode.Utility;

namespace PaelCharacter.PaelCharacterCode.Patches.PreemptiveStrike;

[HarmonyPatch(typeof(NCreature), nameof(NCreature.UpdateIntent))]
public class IntentEnergyCostPatch
{
    [HarmonyPrefix]
    public static void UpdatePreemptiveCosts(
        NCreature __instance)
    {
        UpdatePreemptiveStrikeCost.Update(__instance.Entity.CombatState);
    }
}