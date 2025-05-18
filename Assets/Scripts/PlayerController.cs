using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameManager.PlayerTurn owner;
	
    private GameManager gameManager;
    private bool hasClimbedThisTurn = false;
    private bool inTower = false;

    private void Start(){ gameManager = FindObjectOfType<GameManager>(); }

    private void Update()
    {
        
        inTower = IsStandingOnTowerFloor();
    }

    private bool IsStandingOnTowerFloor()
    {
        Collider[] hits = Physics.OverlapBox(transform.position, new Vector3(0.3f, 0.7f, 0.3f));
        foreach (var hit in hits)
        {
            if (hit.GetComponent<TowerFloor>())
                return true;
        }
        return false;
    }
	
	public void ForceUpdateTowerStatus()
	{
		inTower = IsStandingOnTowerFloor();
	}

	public void Climb(bool up)
	{
		if (!inTower || hasClimbedThisTurn || gameManager.currentTurn != owner) return;

		float step = 1.5f;
		float nextY = transform.position.y + (up ? step : -step);
		float groundY = 0.6f;

		if (!up && nextY < groundY)
		return;
		

		transform.position = new Vector3(transform.position.x, nextY, transform.position.z);
		hasClimbedThisTurn = true;

		CheckForTreasure();

		gameManager.SetRemainingMoves(1);
		gameManager.UseMove();
	}

	public void MoveTo(Vector3 targetPos)
	{
		if (gameManager.currentTurn != owner || gameManager.currentPhase != GameManager.GamePhase.Playing)
			return;

		if (inTower){return;}


		transform.position = targetPos + Vector3.up * 0.6f;
		CheckForTreasure();
		gameManager.UseMove();
	}

	
	private void CheckForTreasure()
	{
		Collider[] hits = Physics.OverlapBox(
			transform.position,
			new Vector3(0.4f, 0.3f, 0.4f)
		);

		foreach (var hit in hits)
		{
			TreasureMarker marker = hit.GetComponentInChildren<TreasureMarker>();
			if (marker != null && marker.ownedBy != (owner == GameManager.PlayerTurn.Blue ? TreasureMarker.Owner.Blue : TreasureMarker.Owner.Red))
			{
				gameManager.DeclareWinner(owner);
				return;
			}
		}
	}

    public void ResetTurn()
    {
        hasClimbedThisTurn = false;
    }
	

    public bool IsInTower() => inTower;
}
