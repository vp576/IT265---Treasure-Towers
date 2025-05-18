using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DiceRoller : MonoBehaviour
{
    public GameManager gameManager;
    public TextMeshProUGUI resultText;
    public Button rollButton;
    public TextMeshProUGUI moveCounterText;

    private void Start()
    {
        resultText.text = "";
        rollButton.onClick.AddListener(RollDice);
        ShowDiceUI(); 
    }

    void RollDice()
    {
        if (gameManager.RemainingMoves > 0)
        {
            return;
        }

        if (gameManager.currentPhase != GameManager.GamePhase.Playing)
            return;

        GameObject playerObj = gameManager.GetCurrentPlayerObject();
        if (playerObj != null)
        {
            PlayerController pc = playerObj.GetComponent<PlayerController>();
            if (pc != null && pc.IsInTower())
            return;
        }

        int roll = Random.Range(1, 7);
        resultText.text = "Rolled: " + roll;

        gameManager.SetRemainingMoves(roll);
        UpdateMoveCounter();
        rollButton.interactable = false;
    }

    public void ResetRoll()
    {
        resultText.text = "";
        rollButton.interactable = true;
        UpdateMoveCounter();
    }

    public void ShowDiceUI(bool show = true)
    {
        rollButton.gameObject.SetActive(show);
        resultText.gameObject.SetActive(show);
        moveCounterText.gameObject.SetActive(true);
        rollButton.interactable = show;
        UpdateMoveCounter();
    }

    public void UpdateMoveCounter()
    {
        moveCounterText.text = "Moves Left: " + gameManager.RemainingMoves;
    }
}
