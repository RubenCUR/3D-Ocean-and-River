using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideObjectAfter : MonoBehaviour
{
    // Start is called before the first frame update
    public void Hide()
    {
        StartCoroutine(ActivationRoutine(this.gameObject.transform.GetChild(0).gameObject));
    }
    private IEnumerator ActivationRoutine(GameObject objectToActivate)
    {
        //Wait for 2 secs.
        //yield return new WaitForSeconds(2);

        //Turn My game object that is set to false(off) to True(on).
        objectToActivate.SetActive(true);

        //Turn the Game Oject back off after 5 sec.
        yield return new WaitForSeconds(5);

        //Game object will turn off
        objectToActivate.SetActive(false);
    }
}
