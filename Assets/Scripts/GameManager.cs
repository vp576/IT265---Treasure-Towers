using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public enum GamePhase { Pregame, Playing, Ended }
    public GamePhase currentPhase = GamePhase.Pregame;

    public GameObject bluePlayerPrefab;
    public GameObject redPlayerPrefab;

    public GameObject bluePlayer;
    public GameObject redPlayer;
	
	public Camera blueCamera;
    public Camera redCamera;

    public Transform blueStartPos;
    public Transform redStartPos;

    public TextMeshProUGUI turnText;
	public UnityEngine.UI.Button skipTurnButton;

    public enum PlayerTurn { Blue, Red }
    public PlayerTurn currentTurn = PlayerTurn.Blue;
	
	private int remainingMoves;
	public int RemainingMoves => remainingMoves;
	
	public TextMeshProUGUI winText;

	
	

   public void BeginGame()
	{
		if (currentPhase != GamePhase.Pregame) return; 

		currentPhase = GamePhase.Playing;
		
		remainingMoves = 0;

		if (bluePlayer == null && redPlayer == null)
			SpawnPlayers();

		UpdateTurnText();

		DiceRoller dice = FindObjectOfType<DiceRoller>();
		if (dice != null)
			dice.ShowDiceUI();
	}


	void UpdateCamera()
	{
		bool isBlueTurn = currentTurn == PlayerTurn.Blue;

		if (blueCamera != null) blueCamera.enabled = isBlueTurn;
		if (redCamera != null) redCamera.enabled = !isBlueTurn;

		var blueListener = blueCamera?.GetComponent<AudioListener>();
		var redListener = redCamera?.GetComponent<AudioListener>();
		if (blueListener) blueListener.enabled = isBlueTurn;
		if (redListener) redListener.enabled = !isBlueTurn;
	}

   void SpawnPlayers()
	{
		bluePlayer = Instantiate(bluePlayerPrefab, blueStartPos.position, Quaternion.identity);
		bluePlayer.GetComponent<PlayerController>().owner = PlayerTurn.Blue;

		redPlayer = Instantiate(redPlayerPrefab, redStartPos.position, Quaternion.identity);
		redPlayer.GetComponent<PlayerController>().owner = PlayerTurn.Red;
	}



    void UpdateTurnText()
    {
        turnText.text = currentTurn.ToString() + "'s Turn";
    }

	public void EndTurn()
	{
		remainingMoves = 0;

		currentTurn = (currentTurn == PlayerTurn.Blue) ? PlayerTurn.Red : PlayerTurn.Blue;
		UpdateTurnText();
		UpdateCamera();
		
		bluePlayer?.GetComponent<PlayerController>()?.ResetTurn();
		redPlayer?.GetComponent<PlayerController>()?.ResetTurn();

		DiceRoller dice = FindObjectOfType<DiceRoller>();
		if (dice != null)
		{
			dice.ResetRoll();
			dice.ShowDiceUI();
		}
	}


	public void SkipCurrentTurn()
	{
		EndTurn(); 
	}

	
	
	public void DeclareWinner(PlayerTurn winner)
	{
		currentPhase = GamePhase.Ended;

		if (winText != null)
		{
			winText.text = winner + " WINS!";
			winText.gameObject.SetActive(true);
		}

		
		FindObjectOfType<DiceRoller>()?.ShowDiceUI(false);
		FindObjectOfType<ClimbUIManager>()?.ShowClimbButtons(false);
	}

		
	public void SetRemainingMoves(int moves)
	{ remainingMoves = moves; }

	public void UseMove()
	{
		remainingMoves--;

		FindObjectOfType<DiceRoller>().UpdateMoveCounter();

		if (remainingMoves <= 0)
		{
			EndTurn();}
	}

	
	public GameObject GetCurrentPlayerObject()
	{
		return (currentTurn == PlayerTurn.Blue) ? bluePlayer : redPlayer;
	}

}
