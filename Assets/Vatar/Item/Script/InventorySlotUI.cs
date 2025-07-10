using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    public Image backgroundImage;
    public Image iconImage;
    public Image borderImage; 

    Color normalColor = new Color(0f, 0f, 0f, 0.66f); 

    void Start()
    {
        if (borderImage != null)
            borderImage.enabled = false;
    }

    public void SetIcon(Sprite icon)
    {
        iconImage.sprite = icon;
        iconImage.enabled = true;
        iconImage.color = Color.white;
        backgroundImage.color = normalColor;

        if (borderImage != null)
            borderImage.enabled = false;
    }

    public void Highlight(bool isSelected)
    {
        if (borderImage != null)
            borderImage.enabled = isSelected;
    }

    public void Clear()
    {
        iconImage.sprite = null;
        iconImage.enabled = false;
        iconImage.color = new Color(1, 1, 1, 0); 

        backgroundImage.color = normalColor;

        if (borderImage != null)
            borderImage.enabled = false;
    }
}
