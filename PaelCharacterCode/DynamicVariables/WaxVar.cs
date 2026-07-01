using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace PaelCharacter.PaelCharacterCode.DynamicVariables;

public class WaxVar : DynamicVar
{
    public const string Key = "WaxAmount";

    public WaxVar(int waxCount) : base(Key, waxCount)
    {
        this.WithTooltip("PAELCHARACTER-WAX");
    }
}