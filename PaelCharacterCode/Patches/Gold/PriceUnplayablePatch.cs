using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using PaelCharacter.PaelCharacterCode.CustomEnums;
using PaelCharacter.PaelCharacterCode.DynamicVariables;
using PaelCharacter.PaelCharacterCode.Extensions;

namespace PaelCharacter.PaelCharacterCode.Patches.Gold;

[HarmonyPatch(
    typeof(CardModel),
    nameof(CardModel.CanPlay),
    [typeof(UnplayableReason), typeof(AbstractModel)],
    [ArgumentType.Ref, ArgumentType.Ref]
)]
public static class PriceUnplayablePatch
{
    public static void Postfix(
        CardModel __instance,
        ref bool __result,
        ref UnplayableReason reason
    )
    {
        if (
            __instance.DynamicVars.TryGetValue(GoldPriceVar.Key, out DynamicVar? goldPrice) &&
            goldPrice.IntValue > __instance.Owner.Gold
        )
        {
            reason |= PaelUnplayableReasons.Broke;
            __result = false;
        }
    }
}