using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using PaelCharacter.PaelCharacterCode.Extensions;

namespace PaelCharacter.PaelCharacterCode.Patches.Sleep;

[HarmonyPatch(typeof(CardModel), "get_IsPlayable")]
public static class DormantIsPlayablePatch
{
    public static void Postfix(CardModel __instance, ref bool __result)
    {
        if (!__result)
        {
            return;
        }

        if (__instance.IsDormant())
        {
            __result = false;
        }
    }
}