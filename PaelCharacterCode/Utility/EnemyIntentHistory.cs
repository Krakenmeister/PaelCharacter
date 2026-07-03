using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;

namespace PaelCharacter.PaelCharacterCode.Utility;

public static class EnemyIntentHistory
{
    private static Dictionary<Creature, List<MoveState>> _stateHistory = new();

    public static void RecordPreviousIntent(Creature enemy)
    {
        if (!enemy.IsMonster || enemy.Monster == null) return;
        
        if (!_stateHistory.TryGetValue(enemy, out List<MoveState>? stateHistory))
        {
            stateHistory = [];
            _stateHistory[enemy] = stateHistory;
        }

        stateHistory.Add(enemy.Monster.NextMove);
        MainFile.Logger.Info("Recording enemy's intent: " + enemy.LogName + " ," + enemy.Monster.NextMove.StateId);
    }

    public static void RewindEnemyIntent(Creature enemy)
    {
        if (!enemy.IsMonster || enemy.Monster == null) return;

        if (!_stateHistory.TryGetValue(enemy, out List<MoveState>? stateHistory)) return;
        if (stateHistory.Count <= 1) return;

        var previousMove = stateHistory[^2];

        MainFile.Logger.Info("Setting enemy " + enemy.LogName + " intent to " + previousMove);
        enemy.Monster.SetMoveImmediate(previousMove);
        stateHistory.RemoveAt(stateHistory.Count - 1);
    }

    public static void Clear(Creature enemy)
    {
        MainFile.Logger.Info("Removing enemy: " + enemy.LogName);
        _stateHistory.Remove(enemy);
    }
}