using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using PaelCharacter.PaelCharacterCode.CustomEnums;
using PaelCharacter.PaelCharacterCode.Extensions;

namespace PaelCharacter.PaelCharacterCode.CardModifiers;

public class WaxModifier : CardModifier
{
    private bool addedWaxedKeyword;

    public override void OnInitialApplication()
    {
        if (Owner == null)
        {
            return;
        }

        if (!Owner.Keywords.Contains(PaelCardKeywords.Waxed))
        {
            Owner.AddKeyword(PaelCardKeywords.Waxed);
            addedWaxedKeyword = true;
        }
    }

    public override bool ApplyStacked(CardModifier newApplied)
    {
        // Already waxed; don't duplicate the modifier.
        return true;
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner == null || cardPlay.Card != Owner)
        {
            return;
        }

        await Owner.UnWax();
    }

    public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
    {
        if (Owner == null || card != Owner)
        {
            return;
        }

        await Owner.UnWax();
    }

    public async Task OnUnWax()
    {
        if (Owner == null)
        {
            return;
        }

        if (addedWaxedKeyword)
        {
            Owner.RemoveKeyword(PaelCardKeywords.Waxed);
            addedWaxedKeyword = false;
        }
        
        await Task.Delay(100);
        await CreatureCmd.GainBlock(
            Owner.Owner.Creature,
            new BlockVar(2M, ValueProp.Move),
            null
        );
    }
}