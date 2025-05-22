#region

using UnityEngine;
#if MM_UGUI2
using TMPro;
#endif
#if MM_UI
using UnityEngine.UI;
#endif

#endregion

namespace MoreMountains.Feel
{
    [AddComponentMenu("")]
    public class FeelSpringsDemoSlider : MonoBehaviour
    {
		#if MM_UI
        [Header("Bindings")]
        public Slider TargetSlider;
		#endif
		#if MM_UGUI2
        public TMP_Text ValueText;
		#endif
		#if MM_UI
        public float value => TargetSlider.value;
		#else
		public float value => 0f;
		#endif

        void Awake() {
            UpdateText();
            TargetSlider.SetValueWithoutNotify(PlayerPrefs.HasKey("sensitivity") ? PlayerPrefs.GetFloat("sensitivity") : TargetSlider.value);
        }

        public void UpdateText() {
			#if MM_UGUI2
            ValueText.text = TargetSlider.value.ToString("F2");
			#endif
        }
    }
}