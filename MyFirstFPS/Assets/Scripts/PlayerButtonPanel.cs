using UnityEngine;
using UnityEngine.UI;

public class PlayerButtonPanel : MonoBehaviour
{
    public Image walkImage;
    public Image sprintImage;
    public Image jumpImage;
    public Image reloadImage;

    public Color releasedColor;
    public Color pressedColor;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            GetButtonDown(walkImage);
        }
        else
        {
            GetButtonUp(walkImage);
        }

        if (Input.GetButton("Sprint"))
        {
            GetButtonDown(sprintImage);
        }
        else
        {
            GetButtonUp(sprintImage);
        }

        if (Input.GetButton("Jump"))
        {
            GetButtonDown(jumpImage);
        }
        else
        {
            GetButtonUp(jumpImage);
        }

        if (Input.GetButton("Reload"))
        {
            GetButtonDown(reloadImage);
        }
        else
        {
            GetButtonUp(reloadImage);
        }
    }

    void GetButtonUp(Image buttonImage)
    {
        buttonImage.color = releasedColor;
    }

    void GetButtonDown(Image buttonImage)
    {
        buttonImage.color = pressedColor;
    }
}
