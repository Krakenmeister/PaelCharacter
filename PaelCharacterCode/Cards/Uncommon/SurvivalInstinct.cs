using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using PaelCharacter.PaelCharacterCode.Cards;
using PaelCharacter.PaelCharacterCode.Powers;
using PaelCharacter.PaelCharacterCode.Utility;

namespace PaelCharacter.PaelCharacterCode.Cards.Uncommon;
  
public class SurvivalInstinct() : PaelCharacterCard(1,
    CardType.Power, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new ("Power", 2M)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(PaelHoverTips.Wake)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<SurvivalInstinctPower>(choiceContext, Owner.Creature, DynamicVars["Power"].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade() => DynamicVars["Power"].UpgradeValueBy(1M);
}