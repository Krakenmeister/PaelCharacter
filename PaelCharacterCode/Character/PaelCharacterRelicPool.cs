using BaseLib.Abstracts;
using PaelCharacter.PaelCharacterCode.Extensions;
using Godot;

namespace PaelCharacter.PaelCharacterCode.Character;

public class PaelCharacterRelicPool : CustomRelicPoolModel
{
    public override Color LabOutlineColor => PaelCharacter.Color;

    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}