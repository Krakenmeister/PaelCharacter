using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using PaelCharacter.PaelCharacterCode.CustomEnums;

namespace PaelCharacter.PaelCharacterCode.Patches.Sleep;

[HarmonyPatch]
public static class DormantUnplayableReasonDialoguePatch
{
    private static MethodBase TargetMethod()
    {
        Type type = AccessTools.TypeByName(
            "MegaCrit.Sts2.Core.Entities.Cards.UnplayableReasonExtensions"
        );
    
        if (type == null)
        {
            throw new MissingMethodException(
                "Could not find UnplayableReasonExtensions."
            );
        }
    
        return AccessTools.Method(
            type,
            "GetPlayerDialogueLine",
            [
                typeof(UnplayableReason),
                typeof(AbstractModel)
            ]
        );
        
        
    }
    
    public static bool Prefix(
        UnplayableReason reason,
        AbstractModel? preventer,
        ref LocString? __result
    )
    {
        if (!reason.HasFlag(PaelUnplayableReasons.Dormant))
        {
            return true;
        }
    
        __result = new LocString("combat_messages", "DORMANT");
        return false;
    }
}