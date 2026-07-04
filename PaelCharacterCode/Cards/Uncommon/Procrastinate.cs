using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using PaelCharacter.PaelCharacterCode.Cards;
using PaelCharacter.PaelCharacterCode.Commands;
using PaelCharacter.PaelCharacterCode.DynamicVariables;

namespace PaelCharacter.PaelCharacterCode.Cards.Uncommon;

public class Procrastinate() : PaelCharacterCard(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new SleepVar(1),
        new CardsVar(3)
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (IsUpgraded)
        {
            await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
            await SleepCommand.FromHand(choiceContext, Owner, DynamicVars[SleepVar.Key].IntValue, this);
        }
        else
        {
            await SleepCommand.FromHand(choiceContext, Owner, DynamicVars[SleepVar.Key].IntValue, this);
            await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
        }
    }
}