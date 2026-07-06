using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using PaelCharacter.PaelCharacterCode.Cards;
using PaelCharacter.PaelCharacterCode.Utility;

namespace PaelCharacter.PaelCharacterCode.Cards.Uncommon;
  
public class SleepingGiant() : PaelCharacterCard(1,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    private const string IncreaseKey = "Increase";
    private Decimal _extraDamageFromSleeping;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(7M, ValueProp.Move),
        new DynamicVar(IncreaseKey, 7M)
    ];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(PaelHoverTips.Wake)];
    
    private Decimal ExtraDamageFromSleeping
    {
        get => _extraDamageFromSleeping;
        set
        {
            AssertMutable();
            _extraDamageFromSleeping = value;
        }
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target, "cardPlay.Target");
        await DamageCmd
            .Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(play.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
    }
    
    protected override Task AfterWokeUp()
    {
        DamageVar damage = DynamicVars.Damage;
        damage.BaseValue += DynamicVars[IncreaseKey].BaseValue;
        ExtraDamageFromSleeping += DynamicVars[IncreaseKey].BaseValue;

        return Task.CompletedTask;
    }
    
    protected override void AfterDowngraded()
    {
        base.AfterDowngraded();
        DamageVar damage = DynamicVars.Damage;
        damage.BaseValue += ExtraDamageFromSleeping;
    }

    protected override void OnUpgrade() => DynamicVars[IncreaseKey].UpgradeValueBy(4M);
}