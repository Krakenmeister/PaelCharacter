using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace PaelCharacter.PaelCharacterCode.Vfx;

public static class PaelVfx
{
    private const string VfxScenePath = "res://PaelCharacter/scenes/vfx/floating_image.tscn";

    public static void FloatingImageAboveHead(
        Creature target,
        string imagePath,
        float lifetimeSeconds = 1.0f,
        float risePixels = 200.0f,
        float scale = 0.5f)
    {
        if (target.GetCreatureNode() is not NCreature targetNode) return;

        Texture2D? texture = GD.Load<Texture2D>(imagePath);

        if (texture == null) return;

        Node2D vfx = PreloadManager.Cache
            .GetScene(VfxScenePath)
            .Instantiate<Node2D>();

        targetNode.AddChildSafely(vfx);

        vfx.Position = new Vector2(0, -250);
        vfx.Scale = new Vector2(scale, scale);
        Sprite2D? sprite = vfx.GetNodeOrNull<Sprite2D>("FloatingImageTexture");

        if (sprite == null)
        {
            vfx.QueueFree();
            return;
        }

        sprite.Texture = texture;
        sprite.Modulate = new Color(sprite.Modulate, 1.0f);

        Vector2 startPosition = vfx.Position;
        Vector2 endPosition = startPosition + new Vector2(0, -risePixels);

        Tween tween = vfx.CreateTween();
        
        tween.SetParallel(true);
        tween.TweenProperty(vfx, "position", endPosition, lifetimeSeconds);
        tween.TweenProperty(sprite, "modulate:a", 0.0f, lifetimeSeconds);
        tween.SetParallel(false);

        tween.TweenCallback(Callable.From(() =>
        {
            if (GodotObject.IsInstanceValid(vfx))
            {
                vfx.QueueFree();
            }
        }));
    }
}