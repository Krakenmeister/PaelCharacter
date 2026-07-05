using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using PaelCharacter.PaelCharacterCode.Cards;
using PaelCharacter.PaelCharacterCode.Cards.Common;
using PaelCharacter.PaelCharacterCode.CustomEnums;
using PaelCharacter.PaelCharacterCode.Powers;

namespace PaelCharacter.PaelCharacterCode.CardModifiers;

using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Creatures;

public class DormantModifier : CardModifier
{
    private bool addedDormantKeyword;
    
    private const string ReadyToWakeKey = "ReadyToWake";

    public bool ReadyToWakeAtStartOfTurn { get; private set; }
    
    public void MarkReadyToWake()
    {
        ReadyToWakeAtStartOfTurn = true;
    }
    
    public override void StoreSaveData(ModifierSave save)
    {
        save.AdditionalProperties[ReadyToWakeKey] = ReadyToWakeAtStartOfTurn.ToString();
    }

    public override void LoadSaveData(ModifierSave save)
    {
        ReadyToWakeAtStartOfTurn =
            save.AdditionalProperties.TryGetValue(ReadyToWakeKey, out string? value)
            && bool.TryParse(value, out bool parsed)
            && parsed;

        if (Owner != null && !Owner.Keywords.Contains(PaelCardKeywords.Dormant))
        {
            Owner.AddKeyword(PaelCardKeywords.Dormant);
        }
    }

    public override void OnInitialApplication()
    {
        if (Owner == null)
        {
            return;
        }

        if (!Owner.Keywords.Contains(PaelCardKeywords.Dormant))
        {
            Owner.AddKeyword(PaelCardKeywords.Dormant);
            addedDormantKeyword = true;
        }
    }

    public override bool ApplyStacked(CardModifier newApplied)
    {
        // Already dormant; don't duplicate the modifier.
        return true;
    }

    public void OnWakeUp()
    {
        if (Owner == null)
        {
            return;
        }

        if (addedDormantKeyword)
        {
            if (Owner.Owner.HasPower<SurvivalInstinctPower>())
            {
                TaskHelper.RunSafely(CreatureCmd.GainBlock(Owner.Owner.Creature, new BlockVar(Owner.Owner.Creature.GetPower<SurvivalInstinctPower>().Amount, ValueProp.Move), null));
            }
            
            Owner.RemoveKeyword(PaelCardKeywords.Dormant);
            addedDormantKeyword = false;
        }
    }
}