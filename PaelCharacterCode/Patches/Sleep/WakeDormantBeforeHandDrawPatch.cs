using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using PaelCharacter.PaelCharacterCode.Extensions;

namespace PaelCharacter.PaelCharacterCode.Patches.Sleep;

[HarmonyPatch(typeof(Hook), nameof(Hook.BeforeHandDraw))]
public static class WakeDormantBeforeHandDrawPatch
{
    public static void Prefix(ICombatState combatState, Player player, PlayerChoiceContext playerChoiceContext)
    {
        foreach (CardModel card in PileType.Hand.GetPile(player).Cards.ToList())
        {
            if (card.IsReadyToWakeFromSleep())
            {
                card.WakeUp();
            }
        }
    }
}