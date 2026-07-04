using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using PaelCharacter.PaelCharacterCode.Cards;

namespace PaelCharacter.PaelCharacterCode.Cards.Uncommon;

public class GoldenHoard() : PaelCharacterCard(1,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    private const string GoldDivisorKey = "GoldDivisor";
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CalculationBaseVar(6M),
        new ExtraDamageVar(1M),
        new DynamicVar(GoldDivisorKey, 20M),
        new CalculatedDamageVar(ValueProp.Move)
            .WithMultiplier((card, target) =>
                target != null
                    ? card.Owner.Gold / card.DynamicVars[GoldDivisorKey].BaseValue
                    : 0)
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target, "cardPlay.Target");
        await DamageCmd.Attack(DynamicVars.CalculatedDamage)
            .FromCard(this)
            .Targeting(play.Target)
            .WithHitFx("vfx/vfx_attack_blunt", tmpSfx: "blunt_attack.mp3")
            .Execute(choiceContext);
    }

    protected override void OnUpgrade() => DynamicVars.CalculationBase.UpgradeValueBy(3M);
}