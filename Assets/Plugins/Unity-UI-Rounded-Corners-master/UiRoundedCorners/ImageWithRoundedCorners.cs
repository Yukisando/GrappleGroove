using UnityEngine;
using UnityEngine.UI;

namespace Nobi.UiRoundedCorners {
    [ExecuteInEditMode, RequireComponent(typeof(RectTransform))]
    public class ImageWithRoundedCorners : MonoBehaviour {
        static readonly int Props = Shader.PropertyToID("_WidthHeightRadius");

        public float radius = 10f;
        Material material;

        [HideInInspector, SerializeField]
        Image image;
        [HideInInspector, SerializeField]
        RawImage rawImage;

        void OnValidate() {
            Validate();
            Refresh();
        }

        void OnDestroy() {
            DestroyHelper.Destroy(material);
            image = null;
            rawImage = null;
            material = null;
        }

        void OnEnable() {
            Validate();
            Refresh();
        }

        void OnRectTransformDimensionsChange() {
            if (enabled && material != null) Refresh();
        }

        void Validate() {
            if (material == null) material = new Material(Shader.Find("UI/RoundedCorners/RoundedCorners"));

            if (image == null) TryGetComponent(out image);

            if (image != null) image.material = material;

            if (rawImage == null) TryGetComponent(out rawImage);

            if (rawImage != null) rawImage.material = material;
        }

        void Refresh() {
            var rect = ((RectTransform) transform).rect;
            material.SetVector(Props, new Vector4(rect.width, rect.height, radius, 0));
        }
    }
}