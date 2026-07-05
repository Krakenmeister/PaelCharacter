using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using PaelCharacter.PaelCharacterCode.Powers;

namespace PaelCharacter.PaelCharacterCode.Cards.Rare;

public class Stalactites() : PaelCharacterCard(1,
    CardType.Power, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<StalactitePower>(4)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Block)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<StalactitePower>(choiceContext, Owner.Creature, DynamicVars.Power<StalactitePower>().IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade() => DynamicVars.Power<StalactitePower>().UpgradeValueBy(2);
}