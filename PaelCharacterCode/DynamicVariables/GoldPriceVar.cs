using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace PaelCharacter.PaelCharacterCode.DynamicVariables;

public class GoldPriceVar : DynamicVar
{
    public const string Key = "GoldPrice";

    public GoldPriceVar(int price) : base(Key, price)
    {
    }
}