using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using PaelCharacter.PaelCharacterCode.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using PaelCharacter.PaelCharacterCode.Cards.Basic;
using PaelCharacter.PaelCharacterCode.Cards.Common;
using PaelCharacter.PaelCharacterCode.Relics.Starter;

namespace PaelCharacter.PaelCharacterCode.Character;
public class PaelCharacter : PlaceholderCharacterModel
{
    public const string CharacterId = "PaelCharacter";

    public static readonly Color Color = new("ffffff");

    public override Color NameColor => Color;
    public override CharacterGender Gender => CharacterGender.Masculine;
    public override int StartingHp => 70;
    public override int StartingGold => 199;

    public override IEnumerable<CardModel> StartingDeck =>
    [
        ModelDb.Card<PaelStrike>(),
        ModelDb.Card<PaelStrike>(),
        ModelDb.Card<PaelStrike>(),
        ModelDb.Card<PaelStrike>(),
        ModelDb.Card<PaelDefend>(),
        ModelDb.Card<PaelDefend>(),
        ModelDb.Card<PaelDefend>(),
        ModelDb.Card<PaelDefend>(),
        ModelDb.Card<Snooze>(),
        ModelDb.Card<ThickHide>()
    ];

    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<DripDrop>()
    ];

    public override CardPoolModel CardPool => ModelDb.CardPool<PaelCharacterCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<PaelCharacterRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<PaelCharacterPotionPool>();

    public override Control CustomIcon
    {
        get
        {
            var icon = NodeFactory<Control>.CreateFromResource(CustomIconTexturePath);
            icon.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            return icon;
        }
    }

    public override string CustomIconTexturePath => "character_icon_pael.png".CharacterUiPath();
    public override string CustomCharacterSelectIconPath => "char_select_pael.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "char_select_pael_locked.png".CharacterUiPath();
    public override string CustomMapMarkerPath => "map_marker_pael.png".CharacterUiPath();
    public override string CustomCharacterSelectBg => "char_select_pael_bg.tscn".CharacterUiPath();
    
    public override string CustomVisualPath => "res://PaelCharacter/scenes/pael.tscn";
    public override string CustomEnergyCounterPath => "res://PaelCharacter/scenes/energy_counter.tscn";
}