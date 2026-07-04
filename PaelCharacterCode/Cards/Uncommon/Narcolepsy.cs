using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using PaelCharacter.PaelCharacterCode.Cards;
using PaelCharacter.PaelCharacterCode.DynamicVariables;
using PaelCharacter.PaelCharacterCode.Powers;
using PaelCharacter.PaelCharacterCode.Utility;

namespace PaelCharacter.PaelCharacterCode.Cards.Uncommon;

  
public class Narcolepsy() : PaelCharacterCard(1,
    CardType.Power, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new SleepVar(1)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<NarcolepsyPower>(choiceContext, Owner.Creature, DynamicVars[SleepVar.Key].IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade() => DynamicVars[SleepVar.Key].UpgradeValueBy(1M);
}