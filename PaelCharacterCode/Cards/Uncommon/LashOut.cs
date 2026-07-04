using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using PaelCharacter.PaelCharacterCode.Cards;

namespace PaelCharacter.PaelCharacterCode.Cards.Uncommon;

  
public class LashOut() : PaelCharacterCard(1,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    private const string CalculatedHitsKey = "CalculatedHits";
    
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(5M, ValueProp.Move),
        new CalculationBaseVar(0M), 
        new CalculationExtraVar(1M),
        new CalculatedVar(CalculatedHitsKey).WithMultiplier(
            (card, _) => CombatManager.Instance.History.Entries
                .OfType<BlockGainedEntry>()
                .Count(e =>
                    e.HappenedThisTurn(card.CombatState) &&
                    e.Receiver == card.Owner.Creature
                )
        )
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target, "cardPlay.Target");
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .WithHitCount((int) ((CalculatedVar) DynamicVars[CalculatedHitsKey]).Calculate(play.Target))
            .FromCard(this).Targeting(play.Target)
            .WithHitVfxNode(
                (Func<Creature, Node2D>) 
                (t => (Node2D) NStabVfx.Create(t, true)))
            .WithHitFx(tmpSfx: "blunt_attack.mp3")
            .Execute(choiceContext);
    }

    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(2M);
}