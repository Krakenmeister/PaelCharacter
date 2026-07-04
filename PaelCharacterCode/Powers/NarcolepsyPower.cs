using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using PaelCharacter.PaelCharacterCode.Commands;
using PaelCharacter.PaelCharacterCode.Powers;

namespace PaelCharacter.PaelCharacterCode.Powers;

public class NarcolepsyPower() : PaelCharacterPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner.Player) return;

        await SleepCommand.FromHand(choiceContext, player, Amount);
    }
}