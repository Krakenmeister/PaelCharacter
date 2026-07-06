using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using PaelCharacter.PaelCharacterCode.CustomEnums;
using PaelCharacter.PaelCharacterCode.SpireFields;

namespace PaelCharacter.PaelCharacterCode.Patches.Wax;

[HarmonyPatch(
    typeof(CardPlayFinishedEntry),
    MethodType.Constructor,
    [
        typeof(CardPlay),
        typeof(int),
        typeof(CombatSide),
        typeof(CombatHistory),
        typeof(IEnumerable<Player>)
    ]
)]
public class WasWaxedPatch
{
    public static void Postfix(
        CardPlayFinishedEntry __instance,
        CardPlay cardPlay)
    {
        CardPlayFinishedEntryFields.WasWaxed[__instance] = cardPlay.Card.Keywords.Contains(PaelCardKeywords.Waxed);
    }
}