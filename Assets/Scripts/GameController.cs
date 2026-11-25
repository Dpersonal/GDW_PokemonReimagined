using UnityEngine;

public class GameController : MonoBehaviour
{
    private BattleManager BattleManager;
    private UIManager UIManager;

    private void InitializeBattle()
    {
        // Player Team
        BattleManager.PlayerTeam = new Pokemon[]
        {
            new Pikachu(new Move[] { new Tackle() }),
            new Pikachu(new Move[] { new DebugMove() }),
        };

        // Enemy Team
        BattleManager.EnemyTeam = new Pokemon[]
        {
            new Pikachu(new Move[] { new Tackle() }),
            new Pikachu(new Move[] { new DebugMove() }),
        };
    }

    private void Awake()
    {
        BattleManager = GetComponent<BattleManager>();
        UIManager = GetComponent<UIManager>();

    }

    private void Start()
    {
        InitializeBattle();
        UIManager.PrintText("    This is a test! I hope it works!   ");
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            UIManager.PrintText("    This is a test! I hope it works!   ");
        }
    }
}