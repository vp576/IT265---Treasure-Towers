using UnityEngine;
using UnityEngine.UI;

public class ClimbUIManager : MonoBehaviour
{
    public Button upButton;
    public Button downButton;

    private void Start()
    {
        upButton.onClick.AddListener(() => Climb(true));
        downButton.onClick.AddListener(() => Climb(false));
        ShowClimbButtons(false);
    }

    public void ShowClimbButtons(bool show)
    {
        upButton.gameObject.SetActive(show);
        downButton.gameObject.SetActive(show);
    }

    void Climb(bool up)
    {
        GameObject player = FindObjectOfType<GameManager>().GetCurrentPlayerObject();
        if (player != null)
            player.GetComponent<PlayerController>().Climb(up);
    }
}
