using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using PaelCharacter.PaelCharacterCode.Powers;

namespace PaelCharacter.PaelCharacterCode.Powers;

  
public class PreservedFlowerPower() : PaelCharacterPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override bool ShouldPlayerResetEnergy(Player player)
    {
        if (player.Creature != Owner) return true;
        if (player.PlayerCombatState == null) return true;
        if (player.PlayerCombatState.TurnNumber == 1) return true;
        
        return false;
    }
    
    public override async Task AfterSideTurnStart(
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (!participants.Contains(Owner)) return;
        
        await PowerCmd.Decrement(this);
    }
}