using MegaCrit.Sts2.Core.Models;
using PaelCharacter.PaelCharacterCode.Extensions;

namespace PaelCharacter.PaelCharacterCode.Patches.Sleep;

public static class DormantRetainLogic
{
    public static bool ShouldRetainThisTurnOrDormant(CardModel card)
    {
        if (card.ShouldRetainThisTurn)
        {
            return true;
        }

        if (!card.IsDormant())
        {
            return false;
        }

        card.MarkReadyToWakeFromSleep();
        return true;
    }
}