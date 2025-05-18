using UnityEngine;
using TMPro;

public class PreGameManager : MonoBehaviour
{
    public GameObject towerFloorPrefab;
    public TextMeshProUGUI statusText;
    public int maxFloorsPerPlayer = 10;

    private int blueFloorsPlaced = 0;
    private int redFloorsPlaced = 0;
    private bool bluePlacedTreasure = false;
    private bool redPlacedTreasure = false;

    public GameManager gameManager;
    public Material blueMat;
    public Material redMat;

    public enum PlayerTurn { Blue, Red }
    public PlayerTurn currentTurn = PlayerTurn.Blue;

    public Camera blueCamera;
    public Camera redCamera;

    public TextMeshProUGUI phaseLabel;
    public TextMeshProUGUI towerCountText;
    public UnityEngine.UI.Button skipTurnButton;

    private bool gameStarted = false;

    private void Start()
    {
        UpdateUI();
        UpdateCamera();


    }

    void UpdateUI()
    {
        statusText.text = currentTurn + "'s Turn - Place floors or your treasure";
        int towersLeft = (currentTurn == PlayerTurn.Blue)
            ? maxFloorsPerPlayer - blueFloorsPlaced
            : maxFloorsPerPlayer - redFloorsPlaced;

        towerCountText.text = "Towers Left: " + towersLeft;
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

    void SwitchTurn()
    {
        currentTurn = (currentTurn == PlayerTurn.Blue) ? PlayerTurn.Red : PlayerTurn.Blue;
        UpdateUI();
        UpdateCamera();

        if (!gameStarted && IsSetupComplete())
        {
            phaseLabel.gameObject.SetActive(false);
            towerCountText.gameObject.SetActive(false);
			statusText.gameObject.SetActive(false);
            gameManager.BeginGame();
            gameStarted = true;
        }
    }

    void Update()
    {
        if (gameStarted) return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Tile tile = hit.collider.GetComponent<Tile>();
                if (tile == null) return;

                if ((currentTurn == PlayerTurn.Blue && blueFloorsPlaced >= maxFloorsPerPlayer) ||
                    (currentTurn == PlayerTurn.Red && redFloorsPlaced >= maxFloorsPerPlayer))
                {return;}

                int stackCount = tile.GetStackCount();

                if (stackCount > 0)
                {
                    Collider[] hits = Physics.OverlapBox(
                        tile.transform.position + Vector3.up * tile.towerHeight * 0.5f,
                        new Vector3(0.45f, tile.towerHeight * 0.5f, 0.45f));

                    foreach (var col in hits)
                    {
                        TowerFloor tfCheck = col.GetComponent<TowerFloor>();
                        if (tfCheck != null)
                        {
                            bool isOwner = (currentTurn == PlayerTurn.Blue && tfCheck.placedBy == TowerFloor.Owner.Blue)
                                        || (currentTurn == PlayerTurn.Red && tfCheck.placedBy == TowerFloor.Owner.Red);
                            if (!isOwner){return;}
                            break;
                        }
                    }
                }

                Vector3 spawnPos = tile.GetNextStackPosition();
                GameObject floor = Instantiate(towerFloorPrefab, spawnPos, Quaternion.identity);

                if (floor != null)
                {
                    floor.transform.rotation = (currentTurn == PlayerTurn.Blue)
                        ? Quaternion.Euler(0, 0, 0)
                        : Quaternion.Euler(0, 180, 0);

                    TowerFloor tf = floor.GetComponent<TowerFloor>();

                    Renderer renderer = floor.GetComponent<Renderer>();
                    if (renderer != null)
                        renderer.material = (currentTurn == PlayerTurn.Blue) ? blueMat : redMat;

                    tf.placedBy = (currentTurn == PlayerTurn.Blue) ? TowerFloor.Owner.Blue : TowerFloor.Owner.Red;

                    if (currentTurn == PlayerTurn.Blue)
                    {
                        blueFloorsPlaced++;
                        if (blueFloorsPlaced > maxFloorsPerPlayer)
                        {	Destroy(floor);
                            return;
                        }
                    }
                    else
                    {
                        redFloorsPlaced++;
                        if (redFloorsPlaced > maxFloorsPerPlayer)
                        {	Destroy(floor);
                            return;
                        }
                    }

                    SwitchTurn();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                TowerFloor tf = hit.collider.GetComponent<TowerFloor>();
                if (tf != null && tf.placedBy.ToString() == currentTurn.ToString())
                {
                    if ((currentTurn == PlayerTurn.Blue && bluePlacedTreasure) ||
                        (currentTurn == PlayerTurn.Red && redPlacedTreasure))
                    {return;}

                    tf.hasTreasure = true;

                    GameObject nugget = Instantiate(Resources.Load<GameObject>("TreasureNugget"), tf.transform);
                    nugget.transform.localPosition = new Vector3(0, 0.4f, 0);

                    TreasureMarker marker = nugget.GetComponent<TreasureMarker>();
                    marker.ownedBy = (currentTurn == PlayerTurn.Blue) ? TreasureMarker.Owner.Blue : TreasureMarker.Owner.Red;
                    marker.SetVisible(false);

                    if (currentTurn == PlayerTurn.Blue) bluePlacedTreasure = true;
                    else redPlacedTreasure = true;

                    SwitchTurn();
                }
            }
        }
    }

    public bool IsSetupComplete()
    {
        return bluePlacedTreasure && redPlacedTreasure &&
               blueFloorsPlaced >= maxFloorsPerPlayer &&
               redFloorsPlaced >= maxFloorsPerPlayer;
    }

    public void SkipTurn()
    {
        if (!gameStarted) SwitchTurn();
    }
}
