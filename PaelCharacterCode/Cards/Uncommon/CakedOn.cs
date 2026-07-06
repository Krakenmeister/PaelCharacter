using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using PaelCharacter.PaelCharacterCode.Cards;
using PaelCharacter.PaelCharacterCode.SpireFields;
using PaelCharacter.PaelCharacterCode.Utility;

namespace PaelCharacter.PaelCharacterCode.Cards.Uncommon;

public class CakedOn() : PaelCharacterCard(1,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    private const string CakedIncreaseKey = "CakedIncrease";
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CalculationBaseVar(5M),
        new ExtraDamageVar(2M),
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier(
            static (card, _) => CombatManager.Instance.History.Entries
                .OfType<CardPlayFinishedEntry>()
                .Count(e =>
                    e.CardPlay.Card.Owner == card.Owner &&
                    CardPlayFinishedEntryFields.WasWaxed[e]
                )
        )
    ];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(PaelHoverTips.Wax)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target, "cardPlay.Target");
        await DamageCmd.Attack(DynamicVars.CalculatedDamage)
            .FromCard(this)
            .Targeting(play.Target)
            .WithHitFx("vfx/vfx_dramatic_stab", tmpSfx: "blunt_attack.mp3")
            .Execute(choiceContext);
    }

    protected override void OnUpgrade() => DynamicVars.ExtraDamage.UpgradeValueBy(1);
}