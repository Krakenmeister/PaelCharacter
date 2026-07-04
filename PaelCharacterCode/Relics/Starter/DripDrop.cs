using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using PaelCharacter.PaelCharacterCode.Commands;
using PaelCharacter.PaelCharacterCode.DynamicVariables;


namespace PaelCharacter.PaelCharacterCode.Relics.Starter;

  
public class DripDrop() : PaelCharacterRelic
{
    private const int WaxAmount = 3;

    public override RelicRarity Rarity => RelicRarity.Starter;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new WaxVar(WaxAmount)];

    public override Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner || Owner.PlayerCombatState.TurnNumber > 1)
            return Task.CompletedTask;
        Flash();
        WaxCommand.RandomFromHand(player, WaxAmount);
        return Task.CompletedTask;
    }
}