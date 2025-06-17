#region

using System.Collections;
using UnityEngine;

#endregion

public class DeactivateDelay : MonoBehaviour
{
    [SerializeField] float delay = 2f;

    void OnEnable() {
        StartCoroutine(Hide_());
    }

    IEnumerator Hide_() {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}