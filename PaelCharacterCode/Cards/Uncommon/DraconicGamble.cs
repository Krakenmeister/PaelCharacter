using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace PaelCharacter.PaelCharacterCode.Cards.Uncommon;

public class DraconicGamble() : PaelCharacterCard(5,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    private const string GambleGainKey = "GambleGain";
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar(GambleGainKey, 9M)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [EnergyHoverTip];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PlayerCmd.GainEnergy(DynamicVars[GambleGainKey].IntValue, Owner);
    }

    protected override void OnUpgrade() => AddKeyword(CardKeyword.Retain);
}