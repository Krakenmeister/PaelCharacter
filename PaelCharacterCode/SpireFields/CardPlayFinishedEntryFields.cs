using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat.History.Entries;

namespace PaelCharacter.PaelCharacterCode.SpireFields;

public static class CardPlayFinishedEntryFields
{
    public static readonly SpireField<CardPlayFinishedEntry, bool> WasWaxed =
        new(() => false);
}