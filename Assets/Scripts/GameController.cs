using UnityEngine;

public class GameController : MonoBehaviour
{
    private BattleManager BattleManager;
    private UIManager UIManager;
    private bool waitingForInput = false;

    private void InitializeBattle()
    {
        // Player Team
        BattleManager.PlayerTeam = new Pokemon[]
        {
            new Pikachu(new Move[] { new Tackle() }),
            new Pikachu(new Move[] { new DebugMove() }),
        };

        BattleManager.ActivePlayerMon = BattleManager.PlayerTeam[0];

        // Enemy Team
        BattleManager.EnemyTeam = new Pokemon[]
        {
            new Pikachu(new Move[] { new Tackle() }),
            new Pikachu(new Move[] { new DebugMove() }),
        };

        BattleManager.ActiveEnemyMon = BattleManager.EnemyTeam[0];

        // UIManager.SwitchActive(Pokemon pokemon, bool IsEnemy = false);
        BattleManager.StartTurn();
        waitingForInput = true;
    }

    private void Awake()
    {
        BattleManager = GetComponent<BattleManager>();
        UIManager = GetComponent<UIManager>();

    }

    private void Start()
    {
        InitializeBattle();
        TurnFlow();
        UIManager.PrintText("    This is a test! I hope it works!   ");
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            UIManager.PrintText("    This is a test! I hope it works!   ");
        }
    }

    private async void TurnFlow()
    {
        Debug.Log("Starting Animation!");
        UIManager.PlayerBar.SetHealthAnimated(0.5f);
        while (!UIManager.PlayerBar.FinishedAnimation)
        {
            await Awaitable.NextFrameAsync();
        }
        Debug.Log("Finished animation!");
    }

    private enum Button
    {
        Move1,
        Move2,
        Move3,
        Move4,
        Fight,
        Switch,
        Back
    }
    private void PressedButton(Button button)
    {
        if (!waitingForInput) return;
        switch (button)
        {
            default:
                break;
        }
    }

    public void Move1() { PressedButton(Button.Move1); }
    public void Move2() { PressedButton(Button.Move2); }
    public void Move3() { PressedButton(Button.Move3); }
    public void Move4() { PressedButton(Button.Move4); }
    public void Fight() { PressedButton(Button.Fight); }
    public void Switch() { PressedButton(Button.Switch); }
    public void Back() { PressedButton(Button.Back); }
}