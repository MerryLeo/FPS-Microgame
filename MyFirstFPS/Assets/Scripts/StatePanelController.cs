using UnityEngine;
using UnityEngine.UI;

public class StatePanelController : MonoBehaviour
{
    public Image idleImage;
    public Image walkingImage;
    public Image sprintingImage;
    public Image jumpingImage;
    public Image reloadingImage;

    public Color releasedColor;
    public Color pressedColor;

    private PlayerController_FSM player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController_FSM>();

        
    }

    // Update is called once per frame
    void Update()
    {
        if (player.CurrentState is PlayerIdleState)
        {
            SetButtonDown(idleImage);
            SetButtonUp(walkingImage);
            SetButtonUp(sprintingImage);
            SetButtonUp(jumpingImage);
            SetButtonUp(reloadingImage);
        }
        else if (player.CurrentState is PlayerWalkingState)
        {
            SetButtonUp(idleImage);
            SetButtonDown(walkingImage);
            SetButtonUp(sprintingImage);
            SetButtonUp(jumpingImage);
            SetButtonUp(reloadingImage);
        }
        else if (player.CurrentState is PlayerRunningState)
        {
            SetButtonUp(idleImage);
            SetButtonUp(walkingImage);
            SetButtonDown(sprintingImage);
            SetButtonUp(jumpingImage);
            SetButtonUp(reloadingImage);
        }
        else if (player.CurrentState is PlayerJumpingState)
        {
            SetButtonUp(idleImage);
            SetButtonUp(walkingImage);
            SetButtonUp(sprintingImage);
            SetButtonDown(jumpingImage);
            SetButtonUp(reloadingImage);
        }
        else if (player.CurrentState is PlayerReloadingState)
        {
            SetButtonUp(idleImage);
            SetButtonUp(walkingImage);
            SetButtonUp(sprintingImage);
            SetButtonUp(jumpingImage);
            SetButtonDown(reloadingImage);
        }
    }

    void SetButtonUp(Image buttonImage)
    {
        buttonImage.color = releasedColor;
    }

    void SetButtonDown(Image buttonImage)
    {
        buttonImage.color = pressedColor;
    }
}
