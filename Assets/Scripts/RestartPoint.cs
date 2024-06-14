//-------------------------------
//--- Prototype FPC
//--- Version 1.0
//--- © The Famous Mouse™
//-------------------------------

using UnityEngine;
using PrototypeFPC;

namespace PrototypeFPC
{
    public class RestartPoint : MonoBehaviour
    {
        [SerializeField] Collider player;
        [SerializeField] Transform restartPosition;

        void OnTriggerEnter(Collider col)
        {
            if(col.GetComponent<Collider>() == player)
            {
                player.gameObject.transform.position = restartPosition.position;
            }
        }
    }
}
