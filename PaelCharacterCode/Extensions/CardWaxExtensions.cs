using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Models;
using PaelCharacter.PaelCharacterCode.CardModifiers;
using PaelCharacter.PaelCharacterCode.Cards.Common;
using PaelCharacter.PaelCharacterCode.Patches.Wax;

namespace PaelCharacter.PaelCharacterCode.Extensions;

public static class CardWaxExtensions
{
    public static bool IsWaxed(this CardModel card)
    {
        return CardModifier.Modifiers(card).OfType<WaxModifier>().Any();
    }

    public static void Wax(this CardModel card)
    {
        if (card.IsWaxed()) return;
        
        CardModifier.AddModifier<WaxModifier>(card);
        NCardWaxOverlayPatch.ReloadWaxOverlayFor(card);
    }

    public static async Task UnWax(this CardModel card)
    {
        WaxModifier? waxed = GetWaxModifier(card);

        if (waxed == null) return;
        
        await waxed.OnUnWax();
        CardModifier.RemoveModifier(card, waxed);

        if (card is HoneyPot)
        {
            card.Wax();
        }
        
        NCardWaxOverlayPatch.ReloadWaxOverlayFor(card);
    }
    
    public static WaxModifier? GetWaxModifier(this CardModel card)
    {
        return CardModifier.Modifiers(card)
            .OfType<WaxModifier>()
            .FirstOrDefault();
    }
}