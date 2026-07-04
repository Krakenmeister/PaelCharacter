using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.ValueProps;
using PaelCharacter.PaelCharacterCode.Utility;

namespace PaelCharacter.PaelCharacterCode.Cards.Rare;

public class ClaimTribute() : PaelCharacterCard(3,
    CardType.Attack, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    public override bool CanBeGeneratedInCombat => false;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(32M, ValueProp.Move),
        new ("Gold", 32M)
    ];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Fatal)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target, "cardPlay.Target");
        
        bool shouldTriggerFatal = play.Target.Powers.All(p => p.ShouldOwnerDeathTriggerFatal());
        
        AttackCommand attackCommand = await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target).WithHitFx("vfx/vfx_attack_blunt", tmpSfx: "blunt_attack.mp3").Execute(choiceContext);
        if (!shouldTriggerFatal || !attackCommand.Results
                .SelectMany(r => r)
                .Any(r => r.WasTargetKilled))
        {
            return;
        }
        
        Vector2? monsterPos = null;
        if (monsterPos.HasValue)
        {
            VfxCmd.PlayVfx(monsterPos.Value, "vfx/vfx_coin_explosion_regular", NCombatRoom.Instance?.CombatVfxContainer);
        }
            
        await PlayerCmd.GainGold(DynamicVars["Gold"].IntValue, Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(6M);
        DynamicVars["Gold"].UpgradeValueBy(6M);
    }
}