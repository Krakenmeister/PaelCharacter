using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace PaelCharacter.PaelCharacterCode.DynamicVariables;

public class PreservedFlowerVar : DynamicVar
{
    public const string Key = "PreservedFlower";

    public PreservedFlowerVar(int preservedFlowerCount) : base(Key, preservedFlowerCount)
    {
    }
}