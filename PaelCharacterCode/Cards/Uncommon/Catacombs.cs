using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using PaelCharacter.PaelCharacterCode.Cards;

namespace PaelCharacter.PaelCharacterCode.Cards.Uncommon;

public class Catacombs() : PaelCharacterCard(2,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    private const string CalculatedHitsKey = "CalculatedHits";
    
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(8M, ValueProp.Move),
        new CalculationBaseVar(0M),
        new CalculationExtraVar(1M),
        new CalculatedVar(CalculatedHitsKey).WithMultiplier(
            static (card, _) => Math.Max(0, (card.Owner.PlayerCombatState?.TurnNumber ?? 0) - 1) 
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

    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(3M);
}