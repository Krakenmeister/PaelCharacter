
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using PaelCharacter.PaelCharacterCode.Cards.Common;

namespace PaelCharacter.PaelCharacterCode.Utility;

public static class UpdatePreemptiveStrikeCost
{
    public static void Update(ICombatState? combatState)
    {
        if (combatState == null) return;
        
        int attackerCount = 0;
        
        foreach (Creature enemy in combatState.Enemies)
        {
            if (enemy.IsMonster && enemy.Monster != null)
            {
                if (enemy.Monster.IntendsToAttack)
                {
                    attackerCount++;
                }
            }
        }

        foreach (Player player in combatState.Players)
        {
            foreach (CardPile cardPile in player.Piles)
            {
                foreach (CardModel card in cardPile.Cards)
                {
                    if (card is PreemptiveStrike)
                    {
                        card.EnergyCost.SetThisCombat(Math.Max(0, 2 - attackerCount));
                    }
                }
            }
        }
    }
}