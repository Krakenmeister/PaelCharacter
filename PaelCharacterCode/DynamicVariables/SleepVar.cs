using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace PaelCharacter.PaelCharacterCode.DynamicVariables;

public class SleepVar : DynamicVar
{
    public const string Key = "Sleep";

    public SleepVar(int sleepCount) : base(Key, sleepCount)
    {
        this.WithTooltip();
    }
}