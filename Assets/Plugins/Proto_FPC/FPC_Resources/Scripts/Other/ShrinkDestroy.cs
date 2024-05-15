//-------------------------------
//--- Prototype FPC
//--- Version 1.0
//--- © The Famous Mouse™
//-------------------------------

using UnityEngine;
using PrototypeFPC;

namespace PrototypeFPC
{
    public class ShrinkDestroy : MonoBehaviour
    {
        [SerializeField] float lifeTime = 2f;
        [SerializeField] float shrinkSpeed = 0.5f;

        //Helpers
        float time;


        //-----------------------


        //Functions
        ///////////////

        void Start()
        {
            //Record desired time
            time = lifeTime;
        }

        void Update()
        {
            //Countdown
            lifeTime = lifeTime - Time.deltaTime;

            //Shrink object if still visible
            if(lifeTime < (time / 2))
            {
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, shrinkSpeed * Time.deltaTime);
            }

            //Destroy object when no longer visible
            if(lifeTime < 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
