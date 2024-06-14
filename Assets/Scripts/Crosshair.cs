#region

using UnityEngine;
using UnityEngine.UI;

#endregion

class Crosshair : MonoBehaviour
{
    [SerializeField] Image crosshairImage;
    Sprite defaultSprite;
    
    void Awake() {
        defaultSprite = crosshairImage.sprite;
    }
    
    // public void SetCrosshair(Color color, Sprite sprite = null) {
    //     if (sprite != null) {
    //         crosshairImage.sprite = sprite;
    //     }
    //     crosshairImage.color = color;
    // }
    //
    // public void ResetCrosshair() {
    //     crosshairImage.color = Color.white;
    //     crosshairImage.sprite = defaultSprite;
    // }
}