using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace PaelCharacter.PaelCharacterCode.Powers;

public class InnerSanctumPower() : PaelCharacterPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override bool IsVisibleInternal => false;

    public override bool ShouldTakeExtraTurn(Player player) => player.Creature == Owner;
    
    public override Task AfterTakingExtraTurn(Player player)
    {
        return player.Creature == Owner ? PowerCmd.Decrement(this) : Task.CompletedTask;
    }
}