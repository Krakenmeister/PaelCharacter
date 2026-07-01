using System.Reflection;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using PaelCharacter.PaelCharacterCode.Extensions;

namespace PaelCharacter.PaelCharacterCode.Patches.Sleep;

[HarmonyPatch(typeof(NCard), "ReloadOverlay")]
[HarmonyPatch]
public static class NCardDormantOverlayPatch
{
    private const string DormantOverlayNodeName = "PaelDormantOverlay";
    private const string DormantOverlayScenePath = "res://PaelCharacter/scenes/dormant.tscn";

    private static readonly List<WeakReference<NCard>> TrackedCards = [];

    private static readonly MethodInfo ReloadOverlayMethod =
        AccessTools.Method(typeof(NCard), "ReloadOverlay");

    [HarmonyPatch(typeof(NCard), nameof(NCard.Model), MethodType.Setter)]
    [HarmonyPostfix]
    public static void TrackCard(NCard __instance)
    {
        if (!TrackedCards.Any(reference =>
                reference.TryGetTarget(out NCard? card) && card == __instance))
        {
            TrackedCards.Add(new WeakReference<NCard>(__instance));
        }
    }

    [HarmonyPatch(typeof(NCard), "ReloadOverlay")]
    [HarmonyPostfix]
    public static void AddDormantOverlay(NCard __instance)
    {
        RemoveExistingDormantOverlay(__instance);

        if (__instance.Model?.IsDormant() != true)
        {
            return;
        }

        Control dormantOverlay = PreloadManager.Cache
            .GetScene(DormantOverlayScenePath)
            .Instantiate<Control>();

        dormantOverlay.Name = DormantOverlayNodeName;
        dormantOverlay.MouseFilter = Control.MouseFilterEnum.Ignore;

        // Do NOT use z_index for this. We want normal same-card child order.
        dormantOverlay.ZIndex = 0;

        Control? cardContainer = __instance.GetNodeOrNull<Control>("%CardContainer");

        if (cardContainer == null)
        {
            cardContainer = (Control)__instance.OverlayContainer;
        }

        cardContainer.AddChildSafely(dormantOverlay);

        // Put it at the very end of CardContainer so it draws above:
        // TitleBanner, TitleLabel, TypePlaque, EnergyIcon, Enchantment, etc.
        cardContainer.MoveChild(dormantOverlay, cardContainer.GetChildCount() - 1);
    }

    public static void ReloadDormantOverlayFor(CardModel cardModel)
    {
        TrackedCards.RemoveAll(reference => !reference.TryGetTarget(out _));

        int matches = 0;

        foreach (WeakReference<NCard> reference in TrackedCards)
        {
            if (!reference.TryGetTarget(out NCard? card))
            {
                continue;
            }

            if (card.Model != cardModel)
            {
                continue;
            }

            matches++;

            ReloadOverlayMethod.Invoke(card, []);
        }
    }
    
    private static Control GetDormantOverlayContainer(NCard card)
    {
        Control? cardContainer = card.GetNodeOrNull<Control>("%CardContainer");

        if (cardContainer != null)
        {
            return cardContainer;
        }

        return (Control)card.OverlayContainer;
    }

    private static void RemoveExistingDormantOverlay(NCard card)
    {
        Control? cardContainer = card.GetNodeOrNull<Control>("%CardContainer");

        if (cardContainer != null)
        {
            RemoveExistingDormantOverlayFrom(cardContainer);
        }

        Control? cardVfxContainer = card.GetNodeOrNull<Control>("%CardVfxContainer");

        if (cardVfxContainer != null)
        {
            RemoveExistingDormantOverlayFrom(cardVfxContainer);
        }

        if (card.OverlayContainer is Control overlayContainer)
        {
            RemoveExistingDormantOverlayFrom(overlayContainer);
        }
    }

    private static void RemoveExistingDormantOverlayFrom(Control container)
    {
        Control? existingOverlay = container.GetNodeOrNull<Control>(DormantOverlayNodeName);

        if (existingOverlay == null)
        {
            return;
        }

        container.RemoveChildSafely(existingOverlay);
        existingOverlay.QueueFreeSafely();
    }
}