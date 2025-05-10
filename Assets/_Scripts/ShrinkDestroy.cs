#region

using UnityEngine;

#endregion

namespace PrototypeFPC
{
    public class ShrinkDestroy : MonoBehaviour
    {
        [SerializeField] float lifeTime = 2f;
        [SerializeField] float shrinkSpeed = 0.5f;
        
        //Helpers
        float time;
        
        //-----------------------
        
        void Start() {
            //Record desired time
            time = lifeTime;
        }
        
        void Update() {
            //Countdown
            lifeTime = lifeTime - Time.deltaTime;
            
            //Shrink object if still visible
            if (lifeTime < time / 2) {
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, shrinkSpeed * Time.deltaTime);
            }
            
            //Destroy object when no longer visible
            if (lifeTime < 0) {
                Destroy(gameObject);
            }
        }
    }
}