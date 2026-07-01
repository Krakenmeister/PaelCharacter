using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using PaelCharacter.PaelCharacterCode.Extensions;

namespace PaelCharacter.PaelCharacterCode.Commands;

public static class SleepCommand
{
    public static async Task<IReadOnlyList<CardModel>> FromHand(
        PlayerChoiceContext choiceContext,
        Player player,
        int amount,
        CardModel? source = null,
        Func<CardModel, bool>? filter = null
    )
    {
        CardSelectorPrefs prefs = new(
            new LocString("card_selection", "PAEL-TO_SLEEP"),
            amount
        );

        List<CardModel> selectedCards = (await CardSelectCmd.FromHand(choiceContext, player, prefs, card =>
            !card.IsDormant()
            && card != source
            && (filter == null || filter(card)), (AbstractModel) source)).ToList();

        foreach (CardModel card in selectedCards)
        {
            card.Sleep();
        }

        return selectedCards;
    }
}