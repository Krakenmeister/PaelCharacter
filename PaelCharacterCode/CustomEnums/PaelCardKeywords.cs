using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace PaelCharacter.PaelCharacterCode.CustomEnums;

public static class PaelCardKeywords
{
    [CustomEnum, KeywordProperties(AutoKeywordPosition.Before)]
    public static CardKeyword Dormant;

    [CustomEnum, KeywordProperties(AutoKeywordPosition.Before)]
    public static CardKeyword Waxed;
}