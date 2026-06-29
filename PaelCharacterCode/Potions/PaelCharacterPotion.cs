using BaseLib.Abstracts;
using BaseLib.Utils;
using PaelCharacter.PaelCharacterCode.Character;

namespace PaelCharacter.PaelCharacterCode.Potions;

[Pool(typeof(PaelCharacterPotionPool))]
public abstract class PaelCharacterPotion : CustomPotionModel;