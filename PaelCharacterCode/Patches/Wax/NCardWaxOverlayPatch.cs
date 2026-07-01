using Godot;
using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using PaelCharacter.PaelCharacterCode.Extensions;

namespace PaelCharacter.PaelCharacterCode.Patches.Wax;

[HarmonyPatch(typeof(NCard), "ReloadOverlay")]
[HarmonyPatch]
public static class NCardWaxOverlayPatch
{
    private const string WaxOverlayNodeName = "PaelWaxOverlay";
    private const string WaxOverlayScenePath = "res://PaelCharacter/scenes/waxed.tscn";

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
    public static void AddWaxOverlay(NCard __instance)
    {
        RemoveExistingWaxOverlay(__instance);

        if (__instance.Model?.IsWaxed() != true)
        {
            return;
        }

        Control waxOverlay = PreloadManager.Cache
            .GetScene(WaxOverlayScenePath)
            .Instantiate<Control>();

        waxOverlay.Name = WaxOverlayNodeName;
        waxOverlay.MouseFilter = Control.MouseFilterEnum.Ignore;

        Control? cardVfxContainer = __instance.GetNodeOrNull<Control>("%CardVfxContainer");

        if (cardVfxContainer == null)
        {
            __instance.OverlayContainer.AddChildSafely(waxOverlay);
            return;
        }

        cardVfxContainer.AddChildSafely(waxOverlay);
    }

    public static void ReloadWaxOverlayFor(CardModel cardModel)
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

    private static void RemoveExistingWaxOverlay(NCard card)
    {
        Control? cardVfxContainer = card.GetNodeOrNull<Control>("%CardVfxContainer");

        if (cardVfxContainer != null)
        {
            RemoveExistingWaxOverlayFrom(cardVfxContainer);
        }

        if (card.OverlayContainer is Control overlayContainer)
        {
            RemoveExistingWaxOverlayFrom(overlayContainer);
        }
    }

    private static void RemoveExistingWaxOverlayFrom(Control container)
    {
        Control? existingOverlay = container.GetNodeOrNull<Control>(WaxOverlayNodeName);

        if (existingOverlay == null)
        {
            return;
        }

        container.RemoveChildSafely(existingOverlay);
        existingOverlay.QueueFreeSafely();
    }
}