using UnityEngine;
using System.Collections;

public class DragAndDrop : MonoBehaviour
{
    private bool _mouseState;
    public GameObject Target;
    public Vector3 screenSpace;
    public Vector3 offset;

    public Spawner spawner;

    public Vector3 currentPosition;
    public Vector3 previousPosition;

    public int Speed = 3000;

    public float timeCounter;

    public float clicked = 0;
    public float clicktime = 0;
    public float clickdelay = 0.5f;

    //public GameObject gameDisableObjectWhenDoubleClicked1;
    //public GameObject gameDisableObjectWhenDoubleClicked2;

    //public GameObject gameEnableObjectWhenDoubleClicked1;
    //public GameObject gameEnableObjectWhenDoubleClicked2;

    public bool DisableObjectWhenDoubleClicked = false;

    //[SerializeField]
    //private Transform _player;

    public Vector3 direction = Vector3.forward;

    // Use this for initialization
    void Start()
    {
        Target = this.gameObject;
    }

    //public void DisableOrEnableWhenDoubleClicked(bool disableEnable)
    //{
    //    if (DisableObjectWhenDoubleClicked == false)
    //    {
    //        if (gameDisableObjectWhenDoubleClicked1 != null)
    //            gameDisableObjectWhenDoubleClicked1.SetActive(false);

    //        if (gameDisableObjectWhenDoubleClicked2 != null)
    //            gameDisableObjectWhenDoubleClicked2.SetActive(false);

    //        if (gameEnableObjectWhenDoubleClicked1 != null)
    //            gameEnableObjectWhenDoubleClicked1.SetActive(true);

    //        if (gameEnableObjectWhenDoubleClicked2 != null)
    //            gameEnableObjectWhenDoubleClicked2.SetActive(true);
    //    }
    //    else if (DisableObjectWhenDoubleClicked == true)
    //    {
    //        if (gameDisableObjectWhenDoubleClicked1 != null)
    //            gameDisableObjectWhenDoubleClicked1.SetActive(true);

    //        if (gameDisableObjectWhenDoubleClicked2 != null)
    //            gameDisableObjectWhenDoubleClicked2.SetActive(true);

    //        if (gameEnableObjectWhenDoubleClicked1 != null)
    //            gameEnableObjectWhenDoubleClicked1.SetActive(false);

    //        if (gameEnableObjectWhenDoubleClicked2 != null)
    //            gameEnableObjectWhenDoubleClicked2.SetActive(false);
    //    }
    //}

    // Update is called once per frame
    void Update()
    {
        //Double-Click Logic
        //if (Input.GetMouseButtonDown(0))
        //{
        //    clicked++;
        //    if (clicked == 1) clicktime = Time.time;
        //}
        //if (clicked > 1 && Time.time - clicktime < clickdelay)
        //{
        //    clicked = 0;
        //    clicktime = 0;

        //    Debug.Log("Double Clicked");

        //    DisableOrEnableWhenDoubleClicked(true);
        //}
        //else if (clicked > 2 || Time.time - clicktime > 1)
        //{
        //    //if(gameDisableObjectWhenDoubleClicked1 != null)
        //    //{
        //    //    if (gameDisableObjectWhenDoubleClicked1.activeSelf)
        //    //        gameDisableObjectWhenDoubleClicked1.SetActive(false);
        //    //    else
        //    //        gameDisableObjectWhenDoubleClicked1.SetActive(true);
        //    //}

        //    clicked = 0;

        //}

        // Debug.Log(_mouseState);
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo;
            if (Target == GetClickedObject(out hitInfo))
            {
                _mouseState = true;
                screenSpace = Camera.main.WorldToScreenPoint(Target.transform.position);
                offset = Target.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z));

                //if(Target.GetComponent<Rigidbody>() != null)
                //    Target.GetComponent<Rigidbody>().AddForce(offset * Speed, ForceMode.Impulse);  
            }

            direction = Target.transform.position - transform.position;
            direction.Normalize();

            //Debug.Log("Magnitude: " + direction.magnitude);

            //Debug.DrawRay(transform.position, direction, Color.green);

            //transform.Translate(direction * Time.deltaTime);
        }
        if (Input.GetMouseButtonUp(0))
        {
            _mouseState = false;
        }
        if (_mouseState)
        {
            if (spawner == null)
            {
                if(GameObject.FindWithTag("Managers") != null)
                    spawner = GameObject.FindWithTag("Managers").GetComponent<Spawner>();

                if (spawner != null)
                    spawner.LatestActiveInstanceID = GetInstanceID();
            }
            //keep track of the mouse position
            var curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);

            //convert the screen mouse position to world point and adjust with offset
            var curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + offset;

            //update the position of the object in the world
            Target.transform.position = curPosition;
            //Target.transform.SetPositionAndRotation(curPosition, Target.transform.rotation)); // Camera.main.transform.rotation
            //Target.transform.SetPositionAndRotation(curPosition, new Quaternion(Target.transform.rotation.x, Target.transform.rotation.y, Target.transform.rotation.z, Target.transform.rotation.w)); // Camera.main.transform.rotation

            //Quaternion qTo;
            //Vector3 vFrom;

            //vFrom = Camera.main.transform.rotation.ToEuler(); //Target.transform.rotation.ToEuler();

            //vFrom = new Vector3(vFrom.x, vFrom.y, -vFrom.z);

            //qTo = Quaternion.Euler(vFrom);

            //Target.transform.SetPositionAndRotation(curPosition, qTo);

            //currentPosition = Target.transform.position;

            //timeCounter += Time.deltaTime;

            //if(timeCounter > .5)
            //{
            //    previousPosition = currentPosition;

            //    timeCounter = 0;
            //}

            //direction = previousPosition - currentPosition;

            //gameobject.transform.Find("ChildName")

            if (Target.GetComponent<HideObjectAfter>() != null)
            {
                Target.GetComponent<HideObjectAfter>().Hide();
            }
        }
    }

    GameObject GetClickedObject(out RaycastHit hit)
    {
        GameObject target = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
        {
            target = hit.collider.gameObject;
        }

        return target;
    }
}