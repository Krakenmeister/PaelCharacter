using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using PaelCharacter.PaelCharacterCode.Powers;
using PaelCharacter.PaelCharacterCode.Utility;

namespace PaelCharacter.PaelCharacterCode.Powers;

public class SurvivalInstinctPower() : PaelCharacterPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.Static(PaelHoverTips.Wake),
        HoverTipFactory.Static(StaticHoverTip.Block)
    ];

}