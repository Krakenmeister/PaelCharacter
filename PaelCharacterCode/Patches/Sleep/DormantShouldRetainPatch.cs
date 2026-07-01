using HarmonyLib;
using MegaCrit.Sts2.Core.Models;

namespace PaelCharacter.PaelCharacterCode.Patches.Sleep;

[HarmonyPatch(typeof(CardModel), "get_ShouldRetainThisTurn")]
public static class DormantShouldRetainPatch
{
    // public static void Postfix(CardModel __instance, ref bool __result)
    // {
    //     if (__result)
    //     {
    //         return;
    //     }
    //
    //     if (__instance.IsDormant())
    //     {
    //         __result = true;
    //     }
    // }
}