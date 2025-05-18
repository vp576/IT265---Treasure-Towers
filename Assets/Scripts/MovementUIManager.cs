using UnityEngine;

public class MovementUIManager : MonoBehaviour
{
    private GameManager gameManager;
    private ClimbUIManager climbUI;
    private DiceRoller diceRoller;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        climbUI = FindObjectOfType<ClimbUIManager>();
        diceRoller = FindObjectOfType<DiceRoller>();
    }

    private void Update()
    {
        if (gameManager.currentPhase != GameManager.GamePhase.Playing) return;

        GameObject currentPlayerObj = gameManager.GetCurrentPlayerObject();
        PlayerController currentPlayer = currentPlayerObj?.GetComponent<PlayerController>();
        if (currentPlayer == null) return;

        currentPlayer.ForceUpdateTowerStatus();
        bool isInTower = currentPlayer.IsInTower();

        climbUI.ShowClimbButtons(isInTower);
        diceRoller.ShowDiceUI(!isInTower);
    }
}
