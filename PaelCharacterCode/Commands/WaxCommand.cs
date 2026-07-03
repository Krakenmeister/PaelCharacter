using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using PaelCharacter.PaelCharacterCode.Extensions;

namespace PaelCharacter.PaelCharacterCode.Commands;

public static class WaxCommand
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
            new LocString("card_selection", "PAEL-TO_WAX"),
            amount
        );

        List<CardModel> selectedCards = (await CardSelectCmd.FromHand(choiceContext, player, prefs, card =>
            !card.IsWaxed()
            && card != source
            && (filter == null || filter(card)), (AbstractModel) source)).ToList();

        foreach (CardModel card in selectedCards)
        {
            card.Wax();
        }

        return selectedCards;
    }
    
    public static IReadOnlyList<CardModel> RandomFromHand(
        Player player,
        int amount,
        CardModel? source = null,
        Func<CardModel, bool>? filter = null
    )
    {
        List<CardModel> validCards = PileType.Hand.GetPile(player).Cards
            .Where(card =>
                !card.IsWaxed()
                && card != source
                && (filter == null || filter(card)))
            .ToList();

        if (validCards.Count == 0)
        {
            return [];
        }

        List<CardModel> selectedCards = [];

        while (selectedCards.Count < amount && validCards.Count > 0)
        {
            CardModel selectedCard = player.RunState.Rng.CombatTargets.NextItem(validCards);

            selectedCards.Add(selectedCard);
            validCards.Remove(selectedCard);
        }

        foreach (CardModel card in selectedCards)
        {
            card.Wax();
        }

        return selectedCards;
    }
}