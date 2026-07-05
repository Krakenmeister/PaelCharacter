using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using PaelCharacter.PaelCharacterCode.CustomEnums;

namespace PaelCharacter.PaelCharacterCode.Patches.Gold;

[HarmonyPatch]
public static class PriceUnplayableReasonDialoguePatch
{
    private static MethodBase TargetMethod() =>
        AccessTools.Method(
            "MegaCrit.Sts2.Core.Entities.Cards.UnplayableReasonExtensions:GetPlayerDialogueLine",
            [typeof(UnplayableReason), typeof(AbstractModel)]
        );

    public static bool Prefix(
        UnplayableReason reason,
        ref LocString? __result
    )
    {
        if (!reason.HasFlag(PaelUnplayableReasons.Broke))
        {
            return true;
        }

        __result = new LocString("combat_messages", "BROKE");
        return false;
    }
}