using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Models;
using PaelCharacter.PaelCharacterCode.CardModifiers;
using PaelCharacter.PaelCharacterCode.Cards;
using PaelCharacter.PaelCharacterCode.Patches.Sleep;

namespace PaelCharacter.PaelCharacterCode.Extensions;

public static class CardSleepExtensions
{
    public static bool IsDormant(this CardModel card)
    {
        return CardModifier.Modifiers(card).OfType<DormantModifier>().Any();
    }

    public static void Sleep(this CardModel card)
    {
        if (card.IsDormant()) return;
        
        CardModifier.AddModifier<DormantModifier>(card);
        NCardDormantOverlayPatch.ReloadDormantOverlayFor(card);
    }

    public static async Task WakeUp(this CardModel card)
    {
        DormantModifier? dormant = GetDormantModifier(card);

        if (dormant == null) return;
        
        dormant.OnWakeUp();
        CardModifier.RemoveModifier(card, dormant);
        NCardDormantOverlayPatch.ReloadDormantOverlayFor(card);
        
        if (card is PaelCharacterCard paelCard)
        {
            await paelCard.InvokeAfterWokeUp();
        }
    }
    
    public static DormantModifier? GetDormantModifier(this CardModel card)
    {
        return CardModifier.Modifiers(card)
            .OfType<DormantModifier>()
            .FirstOrDefault();
    }
    
    public static void MarkReadyToWakeFromSleep(this CardModel card)
    {
        card.GetDormantModifier()?.MarkReadyToWake();
    }

    public static bool IsReadyToWakeFromSleep(this CardModel card)
    {
        return card.GetDormantModifier()?.ReadyToWakeAtStartOfTurn == true;
    }
}