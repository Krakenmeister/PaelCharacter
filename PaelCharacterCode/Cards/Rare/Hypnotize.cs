using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using PaelCharacter.PaelCharacterCode.Cards;
using PaelCharacter.PaelCharacterCode.Utility;
using PaelCharacter.PaelCharacterCode.Vfx;

namespace PaelCharacter.PaelCharacterCode.Cards.Rare;

public class Hypnotize() : PaelCharacterCard(1,
    CardType.Skill, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (play.Target == null) return;

        EnemyIntentHistory.RewindEnemyIntent(play.Target);
        PaelVfx.FloatingImageAboveHead(
            play.Target,
            "res://PaelCharacter/images/vfx/hypnotize_icon.png"
        );
    }

    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
}