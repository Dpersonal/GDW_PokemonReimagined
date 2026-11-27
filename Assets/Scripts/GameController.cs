using UnityEngine;

public class GameController : MonoBehaviour
{
    private BattleManager BattleManager;
    private UIManager UIManager;
    private QTEManager QTEManager;
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
        waitingForInput = true;
    }

    private void Awake()
    {
        BattleManager = GetComponent<BattleManager>();
        UIManager = GetComponent<UIManager>();
        QTEManager = GetComponent<QTEManager>();

    }

    private void Start()
    {
        InitializeBattle();
        TurnFlow(0);
        Debug.Log("This goes when the function waits");
        UIManager.PrintText("    This is a test! I hope it works!   ");
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            UIManager.PrintText("    This is a test! I hope it works!   ");
        }
    }

    /// <summary>
    /// Performs the turn based on the player's selection. 
    /// 
    /// Sends the move to the battle manager, and performs animations based on the result
    /// </summary>
    private async void TurnFlow(int playerChoice)
    {
        Debug.Log("Starting Animation!");
        UIManager.PlayerBar.SetHealthAnimated(0.5f);
        while (!UIManager.PlayerBar.FinishedAnimation)
        {
            await Awaitable.NextFrameAsync();
        }
        Debug.Log("Finished animation!");

        Debug.Log("requesting qte");
        float result = await QTEManager.PerformQTE();
        Debug.Log($"Got RNG: {result}");
    }

    private enum Button
    {
        Move1,
        Move2,
        Move3,
        Move4,
        Pokemon1,
        Pokemon2, 
        Fight,
        Switch,
        Back
    }
    private void PressedButton(Button button)
    {
        if (!waitingForInput) return;
        switch (button)
        {
            case Button.Move1:
            case Button.Move2:
            case Button.Move3:
            case Button.Move4:
            case Button.Pokemon1:
            case Button.Pokemon2:
                TurnFlow((int) button); 
                break;
            case Button.Fight:
                // UIManager: switch to move select menu
                break;
            case Button.Switch:
                // UIManager: switch to pokemon select menu
                break;
            case Button.Back:
                // UIManager: switch to fight/switch menu
                break;
            default:
                Debug.LogWarning($"No functionality found for button {button}");
                break;
        }
    }

    public void PressedButton(int i)
    {
        PressedButton((Button)i);
    }
}