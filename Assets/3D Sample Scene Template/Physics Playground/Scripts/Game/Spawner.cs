using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using TMPro;

[RequireComponent(typeof(SpawnedObjectManager))]
public class Spawner : MonoBehaviour
{
    [Tooltip("Main camera of the scene.")]
    public Camera cam;
    [Tooltip("Min distance from camera of the placeholder object.")]
    public float minDistance = 1;
    [Tooltip("Max distance from camera of the placeholder object.")]
    public float maxDistance = 10;
    [Tooltip("Initial distance from camera of the placeholder object.")]
    public float initialDistance = 7;
    [Tooltip("The keyboard button to select the previous element.")]
    public KeyCode previusElementKey = KeyCode.V;
    public KeyCode previousOtherElementKey = KeyCode.DownArrow;
    [Tooltip("The keyboard button to select the next element.")]
    public KeyCode nextElementKey = KeyCode.B;
    public KeyCode nextOtherElementKey = KeyCode.UpArrow;

    public KeyCode setObjectActive0 = KeyCode.I;
    public KeyCode setObjectActive1 = KeyCode.O;
    public KeyCode setObjectActive2 = KeyCode.P;

    public GameObject[] F1HelpAndWorldGUI;

    public string World_ID = "2";

    public string Region_ID = "1";

    public GameObject goDefaultActiveInstanceObject;

    public int LatestActiveInstanceID = -1;

    public GameObject goHUD1;
    public GameObject goHUD2;
    public GameObject goHUD3;
    public GameObject goHUD4;
    public GameObject goHUD5;

    public TextMeshPro goHUD1Text;
    public TextMeshPro goHUD2Text;
    public TextMeshPro goHUD3Text;
    public TextMeshPro goHUD4Text;
    public TextMeshPro goHUD5Text;

    public string HUD1Text;
    public string HUD2Text;
    public string HUD3Text;
    public string HUD4Text;
    public string HUD5Text;

    [Space]

    [Tooltip("List of spawnables")]
    public List<GameObject> objects;

    [Space]

    [Tooltip("List of spawnables")]
    public List<GameObject> objects1;
    [Space]

    [Tooltip("List of spawnables")]
    public List<GameObject> objects2;
    [Space]

    [Tooltip("List of objects already spawned in scene")]
    List<GameObject> objectsAlreadyInScene;
    [Space]

    [Tooltip("Material assigned to the placeholder objects")]
    public Material outlineMaterial;
    [Tooltip("Particle instantiated on spawn. If not assigned it doesn't spawn a particle")]
    public GameObject spawnParticle;
    [Tooltip("Material assigned to the placeholder objects")]
    public GameObject despawnParticle;
    [Space]

    [Tooltip("Enable the marker lineRenderer below the gameobject")]
    public bool wantMarker = true;
    [Tooltip("LineRenderer's material")]
    public Material lineRendererMaterial;

    [SerializeField]
    SpawnedObjectManager spawnedObjectManager;

    public List<GameObject> outlinedObjects;
    public int cycleIndex = 0;
    float scrollSpeed = 10;
    GameObject parent;
    public float distance;
    Vector3 currentPos;
    Quaternion currentRotation;

    public bool HasRigidBody = true;

    public int objectsActive = 0;

    public GameObject goWorldRegionUp;

    public GameObject goLoadedObjects;

    private void Awake()
    {
        //Load a text file (Assets/Resources/Text/textFile01.txt)

        //if (objects[16] == null)
        {
            /////
            //GameObject instance = Instantiate(Resources.Load("Dog1", typeof(GameObject))) as GameObject;

            //objects[16] = instance;
        }

        LatestActiveInstanceID = goDefaultActiveInstanceObject.GetInstanceID();
    }

    void Start()
    {
        //Init
        distance = initialDistance;
        outlinedObjects = new List<GameObject>();
        spawnedObjectManager = GetComponent<SpawnedObjectManager>();

        //if (World_ID == "1")
        //    objectsActive = 2;
        //else if (World_ID == "2")
        //    objectsActive = 0;
        //else if (World_ID == "3")
        //    objectsActive = 1;

        SpawnInitialObjects();
        AddAlreadySpawnedObjects();

        RecalculateCurrentPosAndRot();

        LatestActiveInstanceID = goDefaultActiveInstanceObject.GetInstanceID();

        CinemachineCore.CameraUpdatedEvent.AddListener(UpdateObjectPosition);
        
        //Load User, Worlds, ObjectInstances, 
        InitializeWorlds();

        if (objectsActive == 0)
        {
            CheckHUDObjects0();

            if(objects[0] != null)
                if (objects[0].GetComponent<SpriteRenderer>() != null)
                    goHUD1.GetComponent<SpriteRenderer>().sprite = objects[0].GetComponent<SpriteRenderer>().sprite;
                else if(objects[0].GetComponentInChildren<SpriteRenderer>() != null)
                    goHUD1.GetComponent<SpriteRenderer>().sprite = objects[0].GetComponentInChildren<SpriteRenderer>().sprite;

            if (objects[1] != null)
                if (objects[1].GetComponent<SpriteRenderer>() != null)
                    goHUD2.GetComponent<SpriteRenderer>().sprite = objects[1].GetComponent<SpriteRenderer>().sprite;
                else if(objects[1].GetComponentInChildren<SpriteRenderer>() != null)
                    goHUD2.GetComponent<SpriteRenderer>().sprite = objects[1].GetComponentInChildren<SpriteRenderer>().sprite;

            if (objects[2] != null)
                if (objects[2].GetComponent<SpriteRenderer>() != null)
                    goHUD3.GetComponent<SpriteRenderer>().sprite = objects[2].GetComponent<SpriteRenderer>().sprite;
                else if(objects[2].GetComponentInChildren<SpriteRenderer>() != null)
                    goHUD3.GetComponent<SpriteRenderer>().sprite = objects[2].GetComponentInChildren<SpriteRenderer>().sprite;

            if (objects[3] != null)
                if (objects[3].GetComponent<SpriteRenderer>() != null)
                    goHUD4.GetComponent<SpriteRenderer>().sprite = objects[3].GetComponent<SpriteRenderer>().sprite;
                else if(objects[3].GetComponentInChildren<SpriteRenderer>() != null)
                    goHUD4.GetComponent<SpriteRenderer>().sprite = objects[3].GetComponentInChildren<SpriteRenderer>().sprite;
            if (objects[4] != null)
                if (objects[4].GetComponent<SpriteRenderer>() != null)
                    goHUD5.GetComponent<SpriteRenderer>().sprite = objects[4].GetComponent<SpriteRenderer>().sprite;
                else if(objects[4].GetComponentInChildren<SpriteRenderer>() != null)
                    goHUD5.GetComponent<SpriteRenderer>().sprite = objects[4].GetComponentInChildren<SpriteRenderer>().sprite;
        }
        else if (objectsActive == 1)
        {
            CheckHUDObjects1();

            if (objects1[0] != null)
                if (objects1[0].GetComponent<SpriteRenderer>() != null)
                    goHUD1.GetComponent<SpriteRenderer>().sprite = objects1[0].GetComponent<SpriteRenderer>().sprite;
                else if(objects1[0].GetComponentInChildren<SpriteRenderer>() != null)
                    goHUD1.GetComponent<SpriteRenderer>().sprite = objects1[0].GetComponentInChildren<SpriteRenderer>().sprite;
            
            if (objects1[1] != null)
                if (objects1[1].GetComponent<SpriteRenderer>() != null)
                    goHUD2.GetComponent<SpriteRenderer>().sprite = objects1[1].GetComponent<SpriteRenderer>().sprite;
                else if(objects1[1].GetComponentInChildren<SpriteRenderer>() != null)
                    goHUD2.GetComponent<SpriteRenderer>().sprite = objects1[1].GetComponentInChildren<SpriteRenderer>().sprite;

            if (objects1[2] != null)
                if (objects1[2].GetComponent<SpriteRenderer>() != null)
                    goHUD3.GetComponent<SpriteRenderer>().sprite = objects1[2].GetComponent<SpriteRenderer>().sprite;
                else if(objects1[2].GetComponentInChildren<SpriteRenderer>() != null)
                    goHUD3.GetComponent<SpriteRenderer>().sprite = objects1[2].GetComponentInChildren<SpriteRenderer>().sprite;

            if (objects1[3] != null)
                if (objects1[3].GetComponent<SpriteRenderer>() != null)
                    goHUD4.GetComponent<SpriteRenderer>().sprite = objects1[3].GetComponent<SpriteRenderer>().sprite;
                else if(objects1[3].GetComponentInChildren<SpriteRenderer>() != null)
                    goHUD4.GetComponent<SpriteRenderer>().sprite = objects1[3].GetComponentInChildren<SpriteRenderer>().sprite;
            
            if (objects1[4] != null)
                if (objects1[4].GetComponent<SpriteRenderer>() != null)
                    goHUD5.GetComponent<SpriteRenderer>().sprite = objects1[4].GetComponent<SpriteRenderer>().sprite;
                else if(objects1[4].GetComponentInChildren<SpriteRenderer>() != null)
                    goHUD5.GetComponent<SpriteRenderer>().sprite = objects1[4].GetComponentInChildren<SpriteRenderer>().sprite;
        }
        else if (objectsActive == 2)
        {
            CheckHUDObjects2();

            if (objects2[0] != null)
                if (objects2[0].GetComponent<SpriteRenderer>() != null)
                    goHUD1.GetComponent<SpriteRenderer>().sprite = objects2[0].GetComponent<SpriteRenderer>().sprite;
                else if(objects2[0].GetComponentInChildren<SpriteRenderer>() != null)
                    goHUD1.GetComponent<SpriteRenderer>().sprite = objects2[0].GetComponentInChildren<SpriteRenderer>().sprite;

            if (objects2[1] != null)
                if (objects2[1].GetComponent<SpriteRenderer>() != null)
                    goHUD2.GetComponent<SpriteRenderer>().sprite = objects2[1].GetComponent<SpriteRenderer>().sprite;
                else if(objects2[1].GetComponentInChildren<SpriteRenderer>() != null)
                    goHUD2.GetComponent<SpriteRenderer>().sprite = objects2[1].GetComponentInChildren<SpriteRenderer>().sprite;

            if (objects2[2] != null)
                if (objects2[2].GetComponent<SpriteRenderer>() != null)
                    goHUD3.GetComponent<SpriteRenderer>().sprite = objects2[2].GetComponent<SpriteRenderer>().sprite;
                else if(objects2[2].GetComponentInChildren<SpriteRenderer>() != null)
                    goHUD3.GetComponent<SpriteRenderer>().sprite = objects2[2].GetComponentInChildren<SpriteRenderer>().sprite;

            if (objects2[3] != null)
                if (objects2[3].GetComponent<SpriteRenderer>() != null)
                    goHUD4.GetComponent<SpriteRenderer>().sprite = objects2[3].GetComponent<SpriteRenderer>().sprite;
                else if(objects2[3].GetComponentInChildren<SpriteRenderer>() != null)
                    goHUD4.GetComponent<SpriteRenderer>().sprite = objects2[3].GetComponentInChildren<SpriteRenderer>().sprite;

            if (objects2[4] != null)
                if (objects2[4].GetComponent<SpriteRenderer>() != null)
                    goHUD5.GetComponent<SpriteRenderer>().sprite = objects2[4].GetComponent<SpriteRenderer>().sprite;
                else if(objects2[4].GetComponentInChildren<SpriteRenderer>() != null)
                    goHUD5.GetComponent<SpriteRenderer>().sprite = objects2[4].GetComponentInChildren<SpriteRenderer>().sprite;
        }
        ////
                
        CycleList(false);
        CycleList(true);
}

    private void CheckHUDObjects0()
    {
        //Show/Hide HUD-objects
        if (objects.Count >= 1)
            if (objects[0] != null)
            {
                goHUD1.SetActive(true);
            }
            else
            {
                goHUD1.SetActive(false);
            }

        if (objects.Count >= 2)
            if (objects[1] != null)
            {
                goHUD2.SetActive(true);
            }
            else
            {
                goHUD2.SetActive(false);
            }

        if (objects.Count >= 3)
            if (objects[2] != null)
            {
                goHUD3.SetActive(true);
            }
            else
            {
                goHUD3.SetActive(false);
            }

        if (objects.Count >= 4)
            if (objects[3] != null)
            {
                goHUD4.SetActive(true);
            }
            else
            {
                goHUD4.SetActive(false);
            }

        if (objects.Count >= 5)
            if (objects[4] != null)
            {
                goHUD5.SetActive(true);
            }
            else
            {
                goHUD5.SetActive(false);
            }
        ////
    }

    private void CheckHUDObjects1()
    {
        //
        if (objects1.Count >= 1)
            if (objects1[0] != null)
            {
                goHUD1.SetActive(true);
            }
            else
            {
                goHUD1.SetActive(false);
            }

        if (objects1.Count >= 2)
            if (objects1[1] != null)
            {
                goHUD2.SetActive(true);
            }
            else
            {
                goHUD2.SetActive(false);
            }

        if (objects1.Count >= 3)
            if (objects1[2] != null)
            {
                goHUD3.SetActive(true);
            }
            else
            {
                goHUD3.SetActive(false);
            }

        if (objects1.Count >= 4)
            if (objects1[3] != null)
            {
                goHUD4.SetActive(true);
            }
            else
            {
                goHUD4.SetActive(false);
            }

        if (objects1.Count >= 5)
            if (objects1[4] != null)
            {
                goHUD5.SetActive(true);
            }
            else
            {
                goHUD5.SetActive(false);
            }
        ////
    }

    private void CheckHUDObjects2()
    {
        //
        if (objects2.Count >= 1)
            if (objects2[0] != null)
            {
                goHUD1.SetActive(true);
            }
            else
            {
                goHUD1.SetActive(false);
            }

        if (objects2.Count >= 2)
            if (objects2[1] != null)
            {
                goHUD2.SetActive(true);
            }
            else
            {
                goHUD2.SetActive(false);
            }

        if (objects2.Count >= 3)
            if (objects2[2] != null)
            {
                goHUD3.SetActive(true);
            }
            else
            {
                goHUD3.SetActive(false);
            }

        if (objects2.Count >= 4)
            if (objects2[3] != null)
            {
                goHUD4.SetActive(true);
            }
            else
            {
                goHUD4.SetActive(false);
            }

        if (objects2.Count >= 5)
            if (objects2[4] != null)
            {
                goHUD5.SetActive(true);
            }
            else
            {
                goHUD5.SetActive(false);
            }
        ////
    }

    public void InitializeWorlds()
    {
        //Load User

        //Load ListObjectInstances

        //Load ObjectInstances

        //Object Collections 1 (), 2 (), 3 ()
        GameObject[] listOfAllTemplateObjects = new GameObject[objects.Count + objects1.Count + objects2.Count];

        objects.CopyTo(listOfAllTemplateObjects, 0);
        objects1.CopyTo(listOfAllTemplateObjects, objects.Count);
        objects2.CopyTo(listOfAllTemplateObjects, objects1.Count + objects.Count);

        if(goLoadedObjects != null)
        {
            DeleteObjects(goLoadedObjects);
        }
    }

    /// <summary>
    /// Recalculate position and rotation used to spawn objects
    /// </summary>
    void RecalculateCurrentPosAndRot()
    {
        currentPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance));
        
        Vector3 lookPos = cam.transform.position;
        lookPos.y = 0;
        //currentRotation = Quaternion.LookRotation(lookPos);

        currentRotation = Quaternion.AngleAxis(0, new Vector3(0, 0, 1));
        //currentRotation = Camera.main.transform.rotation;
    }
    public bool MouseMiddleDown = false;
    public bool MouseLeftDown = false;
    public bool MouseRightDown = false;
    public bool MouseDownOnObject = false;

    //public bool MouseDown = false;

    RaycastHit hit;

    Ray ray;

    public int counter = 0;

    Quaternion rotation = new Quaternion();
    Vector3 targetAngles = new Vector3();

    void Update()
    {
        //New feature to be evalutated - use the mouse scroll wheel to increase or decrease the distance from the camera
        var dist = Input.GetAxis("Mouse ScrollWheel");
        if(dist != 0)
            distance += Time.deltaTime * (dist > 0 ? scrollSpeed : -scrollSpeed);

        distance = Mathf.Clamp(distance, minDistance, maxDistance);

        counter++;

        if(outlinedObjects != null && outlinedObjects.Count > 0)
        { 
            outlinedObjects[cycleIndex].SetActive(true);

            //Mouse left click - Instantiate the selected object
            if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftAlt))
            {
                //MouseDown = true;

                if (MouseDownOnObject == true)
                {
                    hit.transform.gameObject.transform.SetPositionAndRotation(hit.point, hit.transform.gameObject.transform.rotation);
                }

                //tempObj.transform.SetPositionAndRotation(tempObj.transform.position, Camera.main.transform.rotation);
            }

            if (outlinedObjects[cycleIndex] != null)
            {
                if(Camera.main != null)
                { 
                    targetAngles = Camera.main.transform.eulerAngles + 180f * Vector3.up;

                    rotation.eulerAngles = targetAngles;

                    outlinedObjects[cycleIndex].transform.SetPositionAndRotation(outlinedObjects[cycleIndex].transform.position, rotation);
                }
            }
        }

        if (Input.GetMouseButtonDown(2))
        {
            MouseMiddleDown = true;
        }

        if (Input.GetMouseButtonUp(2))
        {
            MouseMiddleDown = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            MouseLeftDown = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            MouseLeftDown = false;
        }

        if (Input.GetMouseButtonDown(1))
        {
            MouseRightDown = true;
        }

        if (Input.GetMouseButtonUp(1))
        {
            MouseRightDown = false;
        }

        //Delete Object
        if (MouseMiddleDown == true)
        { 
            //ray = cam.ScreenPointToRay(Input.mousePosition);

            //Physics.Raycast(ray, out hit, 5000);

            //if (hit.transform != null)
            //{
            //    if (hit.transform.gameObject != null)
            //    {
            //        if (hit.transform.gameObject.name == "Floor")
            //            return;

            //        if (hit.transform.gameObject.tag != "characters")
            //            return;

            //        if (spawnedObjectManager.CheckAndDestroyObject(hit.transform.gameObject) && despawnParticle != null)
            //        {
            //            var particle = Instantiate(despawnParticle, currentPos, despawnParticle.transform.rotation);
            //            Destroy(particle, 3);

            //            ////Play random forward audio clip
            //            //if (spawnedObjectManager.audioController != null)
            //            //    spawnedObjectManager.audioController.PlayRandomClip(spawnedObjectManager.audioController.reverseNoteClips);
            //        }
            //    }
            //}

            MouseMiddleDown = false;
        }

        //Left Mouse Down and Not Left Shift Down And Not Left Alt Down And Left Or Right Control
        if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftAlt) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
        {
            MouseLeftDown = false;
            MouseDownOnObject = false;

            ray = cam.ScreenPointToRay(Input.mousePosition);

            Physics.Raycast(ray, out hit, 10000);
            if (hit.transform != null)
            {

                if (spawnedObjectManager.CheckAndDestroyObject(hit.transform.gameObject) && despawnParticle != null)
                {
                    var particle = Instantiate(despawnParticle, currentPos, despawnParticle.transform.rotation);
                    Destroy(particle, 3);

                    ////Play random forward audio clip
                    //if (spawnedObjectManager.audioController != null)
                    //    spawnedObjectManager.audioController.PlayRandomClip(spawnedObjectManager.audioController.reverseNoteClips);
                }
            }

            //if (Physics.Raycast(ray, out hit, 100))
            //{
            //    if (spawnedObjectManager.CheckAndDestroyObject(hit.transform.gameObject) && despawnParticle != null)
            //    {
            //        var particle = Instantiate(despawnParticle, currentPos, despawnParticle.transform.rotation);
            //        Destroy(particle, 3);

            //        //Play random forward audio clip
            //        if (spawnedObjectManager.audioController != null)
            //            spawnedObjectManager.audioController.PlayRandomClip(spawnedObjectManager.audioController.reverseNoteClips);
            //    }

            //    //hit.transform.gameObject.transform.SetPositionAndRotation(hit.point, hit.transform.gameObject.transform.rotation);
            //}
        }
        //LeftShift + Mouse left click - Raycast and if you hit a spawned object, destroy it! //
        else if (MouseRightDown) //Input.GetKey(KeyCode.LeftShift) && 
        {
            if (Input.GetMouseButtonDown(1))
            {
                GameObject spawned = null;
                Quaternion rotation = new Quaternion();
                Vector3 targetAngles = new Vector3();

                targetAngles = Camera.main.transform.eulerAngles + 180f * Vector3.up;

                if (World_ID == "4")
                {
                    rotation.eulerAngles = new Vector3(0, targetAngles.y, targetAngles.z);
                }
                else
                {
                    rotation.eulerAngles = targetAngles;
                }

                //rotation.eulerAngles = targetAngles;

                if (objectsActive == 0)
                {
                    spawned = Instantiate(objects[cycleIndex], currentPos, rotation);

                    string[] str = new string[5];

                    str[0] = "objectsActive";
                    str[1] = "0";
                }

                if (objectsActive == 1)
                {
                    spawned = Instantiate(objects1[cycleIndex], currentPos, rotation);

                    string[] str = new string[5];

                    str[0] = "objectsActive";
                    str[1] = "1";
                }

                if (objectsActive == 2)
                {
                    spawned = Instantiate(objects2[cycleIndex], currentPos, rotation);

                    string[] str = new string[5];

                    str[0] = "objectsActive";
                    str[1] = "2";
                }

                if (HasRigidBody == false)
                {
                    if (spawned.GetComponent<Rigidbody>() != null)
                        Destroy(spawned.GetComponent<Rigidbody>());
                }

                LatestActiveInstanceID = spawned.GetInstanceID();

                AddObject(spawned);

                if (spawnParticle != null)
                {
                    var particle = Instantiate(spawnParticle, currentPos, spawnParticle.transform.rotation);
                    Destroy(particle, 3);
                }

                //animation
                spawned.transform.DOScale(0, .25f).SetEase(Ease.OutBounce).From();

                ////Play random forward audio clip
                //if (spawnedObjectManager.audioController != null)
                //    spawnedObjectManager.audioController.PlayRandomClip(spawnedObjectManager.audioController.forwardNoteClips);

                MouseRightDown = false;
            }
        }

        //Cycle through the list of outlined objects
        if (Input.GetKeyDown(previusElementKey) || Input.GetKeyDown(previousOtherElementKey))
        {
            CycleList(false);

        }
        else if (Input.GetKeyDown(nextElementKey) || Input.GetKeyDown(nextOtherElementKey))
        {
            CycleList(true);
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            for (int i = 0; i < F1HelpAndWorldGUI.Length; i++)
            {
                if (F1HelpAndWorldGUI[i].activeSelf)
                    F1HelpAndWorldGUI[i].SetActive(false);
                else
                    F1HelpAndWorldGUI[i].SetActive(true);
            }
        }

        if (Input.GetKeyDown(setObjectActive0))
        {
            //CheckHUDObjects0();

            //cycleIndex = 0;

            //objectsActive = 0;
            //outlinedObjects = new List<GameObject>();
            //SpawnInitialObjects();

            //if(objects[0] != null)
            //    if(objects[0].GetComponent<SpriteRenderer>() != null)
            //        goHUD1.GetComponent<SpriteRenderer>().sprite = objects[0].GetComponent<SpriteRenderer>().sprite;
            //    else
            //        goHUD1.GetComponent<SpriteRenderer>().sprite = objects[0].GetComponentInChildren<SpriteRenderer>().sprite;

            //if (objects[1] != null)
            //    if (objects[1].GetComponent<SpriteRenderer>() != null)
            //        goHUD2.GetComponent<SpriteRenderer>().sprite = objects[1].GetComponent<SpriteRenderer>().sprite;
            //    else
            //        goHUD2.GetComponent<SpriteRenderer>().sprite = objects[1].GetComponentInChildren<SpriteRenderer>().sprite;

            //if (objects[2] != null)
            //    if (objects[2].GetComponent<SpriteRenderer>() != null)
            //        goHUD3.GetComponent<SpriteRenderer>().sprite = objects[2].GetComponent<SpriteRenderer>().sprite;
            //    else
            //        goHUD3.GetComponent<SpriteRenderer>().sprite = objects[2].GetComponentInChildren<SpriteRenderer>().sprite;

            //if (objects[3] != null)
            //    if (objects[3].GetComponent<SpriteRenderer>() != null)
            //        goHUD4.GetComponent<SpriteRenderer>().sprite = objects[3].GetComponent<SpriteRenderer>().sprite;
            //    else
            //        goHUD4.GetComponent<SpriteRenderer>().sprite = objects[3].GetComponentInChildren<SpriteRenderer>().sprite;

            //if (objects[4] != null)
            //    if (objects[4].GetComponent<SpriteRenderer>() != null)
            //        goHUD5.GetComponent<SpriteRenderer>().sprite = objects[4].GetComponent<SpriteRenderer>().sprite;
            //    else
            //        goHUD5.GetComponent<SpriteRenderer>().sprite = objects[4].GetComponentInChildren<SpriteRenderer>().sprite;
        }

        if (Input.GetKeyDown(setObjectActive1))
        {
        //    CheckHUDObjects1();

        //    cycleIndex = 0;

        //    objectsActive = 1;
        //    outlinedObjects = new List<GameObject>();
        //    SpawnInitialObjects();

        //    if (objects1[0] != null)
        //        if (objects1[0].GetComponent<SpriteRenderer>() != null)
        //            goHUD1.GetComponent<SpriteRenderer>().sprite = objects1[0].GetComponent<SpriteRenderer>().sprite;
        //        else
        //            goHUD1.GetComponent<SpriteRenderer>().sprite = objects1[0].GetComponentInChildren<SpriteRenderer>().sprite;

        //    if (objects1[1] != null)
        //        if (objects1[1].GetComponent<SpriteRenderer>() != null)
        //            goHUD2.GetComponent<SpriteRenderer>().sprite = objects1[1].GetComponent<SpriteRenderer>().sprite;
        //        else
        //            goHUD2.GetComponent<SpriteRenderer>().sprite = objects1[1].GetComponentInChildren<SpriteRenderer>().sprite;

        //    if (objects1[2] != null)
        //        if (objects1[2].GetComponent<SpriteRenderer>() != null)
        //            goHUD3.GetComponent<SpriteRenderer>().sprite = objects1[2].GetComponent<SpriteRenderer>().sprite;
        //        else
        //            goHUD3.GetComponent<SpriteRenderer>().sprite = objects1[2].GetComponentInChildren<SpriteRenderer>().sprite;

        //    if (objects1[3] != null)
        //        if (objects1[3].GetComponent<SpriteRenderer>() != null)
        //            goHUD4.GetComponent<SpriteRenderer>().sprite = objects1[3].GetComponent<SpriteRenderer>().sprite;
        //        else
        //            goHUD4.GetComponent<SpriteRenderer>().sprite = objects1[3].GetComponentInChildren<SpriteRenderer>().sprite;

        //    if (objects1[4] != null)
        //        if (objects1[4].GetComponent<SpriteRenderer>() != null)
        //            goHUD5.GetComponent<SpriteRenderer>().sprite = objects1[4].GetComponent<SpriteRenderer>().sprite;
        //        else
        //            goHUD5.GetComponent<SpriteRenderer>().sprite = objects1[4].GetComponentInChildren<SpriteRenderer>().sprite;
        }

        if (Input.GetKeyDown(setObjectActive2))
        {
            //CheckHUDObjects2();

            //cycleIndex = 0;

            //objectsActive = 2;
            //outlinedObjects = new List<GameObject>();
            //SpawnInitialObjects();

            //if (objects2[0] != null)
            //    if (objects2[0].GetComponent<SpriteRenderer>() != null)
            //        goHUD1.GetComponent<SpriteRenderer>().sprite = objects2[0].GetComponent<SpriteRenderer>().sprite;
            //    else
            //        goHUD1.GetComponent<SpriteRenderer>().sprite = objects2[0].GetComponentInChildren<SpriteRenderer>().sprite;

            //if (objects2[1] != null)
            //    if (objects2[1].GetComponent<SpriteRenderer>() != null)
            //        goHUD2.GetComponent<SpriteRenderer>().sprite = objects2[1].GetComponent<SpriteRenderer>().sprite;
            //    else
            //        goHUD2.GetComponent<SpriteRenderer>().sprite = objects2[1].GetComponent<SpriteRenderer>().sprite;

            //if (objects2[2] != null)
            //    if (objects2[2].GetComponent<SpriteRenderer>() != null)
            //        goHUD3.GetComponent<SpriteRenderer>().sprite = objects2[2].GetComponent<SpriteRenderer>().sprite;
            //    else
            //        goHUD3.GetComponent<SpriteRenderer>().sprite = objects2[2].GetComponentInChildren<SpriteRenderer>().sprite;

            //if (objects2[3] != null)
            //    if (objects2[3].GetComponent<SpriteRenderer>() != null)
            //        goHUD4.GetComponent<SpriteRenderer>().sprite = objects2[3].GetComponent<SpriteRenderer>().sprite;
            //    else
            //        goHUD4.GetComponent<SpriteRenderer>().sprite = objects2[3].GetComponentInChildren<SpriteRenderer>().sprite;

            //if (objects2[4] != null)
            //    if (objects2[4].GetComponent<SpriteRenderer>() != null)
            //        goHUD5.GetComponent<SpriteRenderer>().sprite = objects2[4].GetComponent<SpriteRenderer>().sprite;
            //    else
            //        goHUD5.GetComponent<SpriteRenderer>().sprite = objects2[4].GetComponentInChildren<SpriteRenderer>().sprite;
        }
    }

    /// <summary>
    /// Given an object, adds all children with a rigidbody to the spawned objects.
    /// </summary>
    void AddObject(GameObject spawnedObject)
    {
        var bodies = spawnedObject.GetComponentsInChildren<Rigidbody>();
        foreach(var b in bodies)
        {
            spawnedObjectManager.AddObject(b.gameObject);
        }
    }

    void DeleteObjects(GameObject goLoadedObjects)
    {
        for (int i = 0; i < goLoadedObjects.transform.childCount; i++)
        {
            GameObject.Destroy(goLoadedObjects.transform.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// Add the object in the "objectsAlreadyInScene" list to the spawned objects.
    /// </summary>
    void AddAlreadySpawnedObjects()
    {
        if(objectsAlreadyInScene != null)
        {
            foreach(var o in objectsAlreadyInScene)
            {
                AddObject(o);
            }
        }
    }

    /// <summary>
    /// Update the position of the current object.
    /// </summary>
    void UpdateObjectPosition(CinemachineBrain brain)
    {
        RecalculateCurrentPosAndRot();
        parent.transform.position = currentPos;
        //parent.transform.rotation = currentRotation;
    }

    //GameObject tempObj;

    /// <summary>
    /// Spawn the placeholder gameobjects. This is the list of gameobject to cycle across.
    /// Placeholders don't need colliders, rigidbodies and have a different material.
    /// </summary>
    void SpawnInitialObjects()
    {
        if(GameObject.Find("OutlinedPlaceholders") != null)
            Destroy(GameObject.Find("OutlinedPlaceholders"));

        parent = new GameObject();
        parent.name = "OutlinedPlaceholders";

        List<GameObject> tempObjects = new List<GameObject>();

        if (objectsActive == 0)
        {
            tempObjects = objects;
        }
        else if (objectsActive == 1)
        {
            tempObjects = objects1;
        }
        else if (objectsActive == 2)
        {
            tempObjects = objects2;
        }

        for (int i = 0; i < tempObjects.Count; i++)
        {
            GameObject tempObj = Instantiate(tempObjects[i], parent.transform);
            tempObj.SetActive(false);

            var colliders = tempObj.GetComponentsInChildren<Collider>();
            foreach (var collider in colliders)
                Destroy(collider);

            var rigidbodies = tempObj.GetComponentsInChildren<Rigidbody>();
            foreach (var rigidbody in rigidbodies)
                Destroy(rigidbody);

            var audios = tempObj.GetComponentsInChildren<AudioSource>();
            foreach (var audio in audios)
                Destroy(audio);

            var animators = tempObj.GetComponentsInChildren<Animator>();
            foreach (var animator in animators)
                Destroy(animator);

            //var meshRenderers = tempObj.GetComponentsInChildren<Renderer>();
            //foreach (var mR in meshRenderers)
            //{
            //    mR.material = outlineMaterial;
            //    mR.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            //}

            if (wantMarker)
            {
                var marker = tempObj.AddComponent<Marker>();
                if (lineRendererMaterial != null)
                    marker.lineMaterial = lineRendererMaterial;
            }

            outlinedObjects.Add(tempObj);
        }
    }

    MeshRenderer mr = new MeshRenderer();

    /// <summary>
    /// Cycle through the outlined objects list
    /// </summary>
    void CycleList(bool wantIncrement)
    {
        outlinedObjects[cycleIndex].SetActive(false);
        if (wantIncrement)
        {
            cycleIndex++;
            if (cycleIndex == outlinedObjects.Count)
                cycleIndex = 0;
        }
        else
        {
            cycleIndex--;
            if (cycleIndex < 0)
                cycleIndex = outlinedObjects.Count - 1;
        }
        outlinedObjects[cycleIndex].SetActive(true);

        if(objectsActive == 0)
        {
            //Mesh As Sprite/Screenshot

            //if(goHUD1.GetComponent<SpriteRenderer>().sprite == null)
            //{
            //if(goHUD1.GetComponentInChildren <MeshRenderer>() != null)
            //{
            //    mr = goHUD1.GetComponentInChildren<MeshRenderer>();

            //    mr = objects[cycleIndex].GetComponent<MeshRenderer>();

            //    return;
            //}
            //}
            if (objects.Count >= 1)
            {
                if ((cycleIndex) <= (objects.Count - 1))
                {
                    goHUD1Text.SetText(cycleIndex + 1 + ": " + objects[cycleIndex].name);

                    if (objects[cycleIndex].GetComponent<SpriteRenderer>() != null)
                        goHUD1.GetComponent<SpriteRenderer>().sprite = objects[cycleIndex].GetComponent<SpriteRenderer>().sprite;
                    else if (objects[cycleIndex].GetComponentInChildren<SpriteRenderer>() != null)
                        goHUD1.GetComponent<SpriteRenderer>().sprite = objects[cycleIndex].GetComponentInChildren<SpriteRenderer>().sprite;
                }
                else
                {
                    goHUD1Text.SetText((int)(cycleIndex - (objects.Count)) + 1 + ": " + objects[(int)(cycleIndex - (objects.Count))].name);

                    if (objects[(int)(cycleIndex - (objects.Count))].GetComponent<SpriteRenderer>() != null)
                        goHUD1.GetComponent<SpriteRenderer>().sprite = objects[(int)(cycleIndex - (objects.Count))].GetComponent<SpriteRenderer>().sprite;
                    else if (objects[(int)(cycleIndex - (objects.Count))].GetComponentInChildren<SpriteRenderer>() != null)
                        goHUD1.GetComponent<SpriteRenderer>().sprite = objects[(int)(cycleIndex - (objects.Count))].GetComponentInChildren<SpriteRenderer>().sprite;
                }

                HUD1Text = "1: cycleIndex: " + cycleIndex;
            }

            if (objects.Count >= 2)
            {
                if ((cycleIndex + 1) <= (objects.Count - 1))
                {
                    goHUD2Text.SetText((int)(cycleIndex + 1) + 1 + ": " + objects[cycleIndex + 1].name);

                    if (objects[cycleIndex + 1].GetComponent<SpriteRenderer>() != null)
                        goHUD2.GetComponent<SpriteRenderer>().sprite = objects[cycleIndex + 1].GetComponent<SpriteRenderer>().sprite;
                    else if (objects[cycleIndex + 1].GetComponentInChildren<SpriteRenderer>() != null)
                        goHUD2.GetComponent<SpriteRenderer>().sprite = objects[cycleIndex + 1].GetComponentInChildren<SpriteRenderer>().sprite;
                }
                else
                {
                    goHUD2Text.SetText((int)((cycleIndex + 1) - (objects.Count)) + 1 + ": " + objects[(int)((cycleIndex + 1) - (objects.Count))].name);

                    if (objects[(int)((cycleIndex + 1) - (objects.Count))].GetComponent<SpriteRenderer>() != null)
                        goHUD2.GetComponent<SpriteRenderer>().sprite = objects[(int)((cycleIndex + 1) - (objects.Count))].GetComponent<SpriteRenderer>().sprite;
                    else if (objects[(int)((cycleIndex + 1) - (objects.Count))].GetComponentInChildren<SpriteRenderer>() != null)
                        goHUD2.GetComponent<SpriteRenderer>().sprite = objects[(int)((cycleIndex + 1) - (objects.Count))].GetComponentInChildren<SpriteRenderer>().sprite;
                }
                HUD2Text = "1: cycleIndex + 1: " + cycleIndex + 1;
            }

            if (objects.Count >= 3)
            {
                if ((cycleIndex + 2) <= (objects.Count - 1))
                {
                    goHUD3Text.SetText((int)(cycleIndex + 2) + 1 + ": " + objects[cycleIndex + 2].name);

                    if (objects[cycleIndex + 2].GetComponent<SpriteRenderer>() != null)
                        goHUD3.GetComponent<SpriteRenderer>().sprite = objects[cycleIndex + 2].GetComponent<SpriteRenderer>().sprite;
                    else if (objects[cycleIndex + 2].GetComponentInChildren<SpriteRenderer>() != null)
                        goHUD3.GetComponent<SpriteRenderer>().sprite = objects[cycleIndex + 2].GetComponentInChildren<SpriteRenderer>().sprite;
                }
                else
                {
                    goHUD3Text.SetText((int)((cycleIndex + 2) - (objects.Count)) + 1 + ": " + objects[(int)((cycleIndex + 2) - (objects.Count))].name);

                    if (objects[(int)((cycleIndex + 2) - (objects.Count))].GetComponent<SpriteRenderer>() != null)
                        goHUD3.GetComponent<SpriteRenderer>().sprite = objects[(int)((cycleIndex + 2) - (objects.Count))].GetComponent<SpriteRenderer>().sprite;
                    else if (objects[(int)((cycleIndex + 2) - (objects.Count))].GetComponentInChildren<SpriteRenderer>() != null)
                        goHUD3.GetComponent<SpriteRenderer>().sprite = objects[(int)((cycleIndex + 2) - (objects.Count))].GetComponentInChildren<SpriteRenderer>().sprite;
                }
                
                HUD3Text = "1: cycleIndex + 2: " + cycleIndex + 2;
            }

            if (objects.Count >= 4)
            {
                if ((cycleIndex + 3) <= (objects.Count - 1))
                {
                    goHUD4Text.SetText((int)(cycleIndex + 3) + 1 + ": " + objects[cycleIndex + 3].name);

                    if (objects[cycleIndex + 3].GetComponent<SpriteRenderer>() != null)
                        goHUD4.GetComponent<SpriteRenderer>().sprite = objects[cycleIndex + 3].GetComponent<SpriteRenderer>().sprite;
                    else if (objects[cycleIndex + 3].GetComponentInChildren<SpriteRenderer>() != null)
                        goHUD4.GetComponent<SpriteRenderer>().sprite = objects[cycleIndex + 3].GetComponentInChildren<SpriteRenderer>().sprite;
                }
                else
                {
                    goHUD4Text.SetText((int)((cycleIndex + 3) - (objects.Count)) + 1 + ": " + objects[(int)((cycleIndex + 3) - (objects.Count))].name);

                    if (objects[(int)((cycleIndex + 3) - (objects.Count))].GetComponent<SpriteRenderer>() != null)
                        goHUD4.GetComponent<SpriteRenderer>().sprite = objects[(int)((cycleIndex + 3) - (objects.Count))].GetComponent<SpriteRenderer>().sprite;
                    else if (objects[(int)((cycleIndex + 3) - (objects.Count))].GetComponentInChildren<SpriteRenderer>() != null)
                        goHUD4.GetComponent<SpriteRenderer>().sprite = objects[(int)((cycleIndex + 3) - (objects.Count))].GetComponentInChildren<SpriteRenderer>().sprite;
                }

                HUD4Text = "1: cycleIndex + 3: " + cycleIndex + 3;
            }

            if (objects.Count >= 5)
            {
                if ((cycleIndex + 4) <= (objects.Count - 1))
                {
                    goHUD5Text.SetText((cycleIndex + 4) + 1 + ": " + objects[cycleIndex + 4].name);

                    if (objects[cycleIndex + 4].GetComponent<SpriteRenderer>() != null)
                        goHUD5.GetComponent<SpriteRenderer>().sprite = objects[cycleIndex + 4].GetComponent<SpriteRenderer>().sprite;
                    else if (objects[cycleIndex + 4].GetComponentInChildren<SpriteRenderer>() != null)
                        goHUD5.GetComponent<SpriteRenderer>().sprite = objects[cycleIndex + 4].GetComponentInChildren<SpriteRenderer>().sprite;
                }
                else
                {
                    goHUD5Text.SetText((int)((cycleIndex + 4) - (objects.Count)) + 1 + ": " + objects[(int)((cycleIndex + 4) - (objects.Count))].name);

                    if (objects[(int)((cycleIndex + 4) - (objects.Count))].GetComponent<SpriteRenderer>() != null)
                        goHUD5.GetComponent<SpriteRenderer>().sprite = objects[(int)((cycleIndex + 4) - (objects.Count))].GetComponent<SpriteRenderer>().sprite;
                    else if (objects[(int)((cycleIndex + 4) - (objects.Count))].GetComponentInChildren<SpriteRenderer>() != null)
                        goHUD5.GetComponent<SpriteRenderer>().sprite = objects[(int)((cycleIndex + 4) - (objects.Count))].GetComponentInChildren<SpriteRenderer>().sprite;
                }

                HUD5Text = "1: cycleIndex + 4: " + cycleIndex + 4;
            }
        }
        else if (objectsActive == 1)
        {
            if (objects1.Count >= 1)
            {
                if ((cycleIndex) <= (objects1.Count - 1))
                {
                    goHUD1Text.SetText(cycleIndex + 1 + ": " + objects1[cycleIndex].name);

                    if (objects1[cycleIndex].GetComponent<SpriteRenderer>() != null)
                        goHUD1.GetComponent<SpriteRenderer>().sprite = objects1[cycleIndex].GetComponent<SpriteRenderer>().sprite;
                    else if (objects1[cycleIndex].GetComponentInChildren<SpriteRenderer>() != null)
                        goHUD1.GetComponent<SpriteRenderer>().sprite = objects1[cycleIndex].GetComponentInChildren<SpriteRenderer>().sprite;
                }
                else
                {
                    goHUD1Text.SetText((int)(cycleIndex - (objects1.Count)) + 1 + ": " + objects1[(int)(cycleIndex - (objects1.Count))].name);

                    if (objects1[(int)(cycleIndex - (objects1.Count))].GetComponent<SpriteRenderer>() != null)
                        goHUD1.GetComponent<SpriteRenderer>().sprite = objects1[(int)(cycleIndex - (objects1.Count))].GetComponent<SpriteRenderer>().sprite;
                    else if (objects1[(int)(cycleIndex - (objects1.Count))].GetComponentInChildren<SpriteRenderer>() != null)
                        goHUD1.GetComponent<SpriteRenderer>().sprite = objects1[(int)(cycleIndex - (objects1.Count))].GetComponentInChildren<SpriteRenderer>().sprite;
                }

                HUD1Text = "1: cycleIndex: " + cycleIndex;
            }

            if (objects1.Count >= 2)
            {
                if ((cycleIndex + 1) <= (objects1.Count - 1))
                {
                    goHUD2Text.SetText((int)(cycleIndex + 1) + 1 + ": " + objects1[cycleIndex + 1].name);

                    if (objects1[cycleIndex + 1].GetComponent<SpriteRenderer>() != null)
                        goHUD2.GetComponent<SpriteRenderer>().sprite = objects1[cycleIndex + 1].GetComponent<SpriteRenderer>().sprite;
                    else if (objects1[cycleIndex + 1].GetComponentInChildren<SpriteRenderer>() != null)
                        goHUD2.GetComponent<SpriteRenderer>().sprite = objects1[cycleIndex + 1].GetComponentInChildren<SpriteRenderer>().sprite;
                }
                else
                {
                    goHUD2Text.SetText((int)((cycleIndex + 1) - (objects1.Count)) + 1 + ": " + objects1[(int)((cycleIndex + 1) - (objects1.Count))].name);

                    if (objects1[(int)((cycleIndex + 1) - (objects1.Count))].GetComponent<SpriteRenderer>() != null)
                        goHUD2.GetComponent<SpriteRenderer>().sprite = objects1[(int)((cycleIndex + 1) - (objects1.Count))].GetComponent<SpriteRenderer>().sprite;
                    else if (objects1[(int)((cycleIndex + 1) - (objects1.Count))].GetComponentInChildren<SpriteRenderer>() != null)
                        goHUD2.GetComponent<SpriteRenderer>().sprite = objects1[(int)((cycleIndex + 1) - (objects1.Count))].GetComponentInChildren<SpriteRenderer>().sprite;
                }

                HUD2Text = "1: cycleIndex + 1: " + cycleIndex + 1;
            }

            if (objects1.Count >= 3)
            {
                if ((cycleIndex + 2) <= (objects1.Count - 1))
                {
                    goHUD3Text.SetText((int)(cycleIndex + 2) + 1 + ": " + objects1[cycleIndex + 2].name);

                    if (objects1[cycleIndex + 2].GetComponent<SpriteRenderer>() != null)
                        goHUD3.GetComponent<SpriteRenderer>().sprite = objects1[cycleIndex + 2].GetComponent<SpriteRenderer>().sprite;
                    else if (objects1[cycleIndex + 2].GetComponentInChildren<SpriteRenderer>() != null)
                        goHUD3.GetComponent<SpriteRenderer>().sprite = objects1[cycleIndex + 2].GetComponentInChildren<SpriteRenderer>().sprite;
                }
                else
                {
                    goHUD3Text.SetText((int)((cycleIndex + 2) - (objects1.Count)) + 1 + ": " + objects1[(int)((cycleIndex + 2) - (objects1.Count))].name);

                    if (objects1[(int)((cycleIndex + 2) - (objects1.Count))].GetComponent<SpriteRenderer>() != null)
                        goHUD3.GetComponent<SpriteRenderer>().sprite = objects1[(int)((cycleIndex + 2) - (objects1.Count))].GetComponent<SpriteRenderer>().sprite;
                    else if (objects1[(int)((cycleIndex + 2) - (objects1.Count))].GetComponentInChildren<SpriteRenderer>() != null)
                        goHUD3.GetComponent<SpriteRenderer>().sprite = objects1[(int)((cycleIndex + 2) - (objects1.Count))].GetComponentInChildren<SpriteRenderer>().sprite;
                }

                HUD3Text = "1: cycleIndex + 2: " + cycleIndex + 2;
            }

            if (objects1.Count >= 4)
            {
                if ((cycleIndex + 3) <= (objects1.Count - 1))
                {
                    goHUD4Text.SetText((int)(cycleIndex + 3) + 1 + ": " + objects1[cycleIndex + 3].name);

                    if (objects1[cycleIndex + 3].GetComponent<SpriteRenderer>() != null)
                        goHUD4.GetComponent<SpriteRenderer>().sprite = objects1[cycleIndex + 3].GetComponent<SpriteRenderer>().sprite;
                    else if (objects1[cycleIndex + 3].GetComponentInChildren<SpriteRenderer>() != null)
                        goHUD4.GetComponent<SpriteRenderer>().sprite = objects1[cycleIndex + 3].GetComponentInChildren<SpriteRenderer>().sprite;
                }
                else
                {
                    goHUD4Text.SetText((int)((cycleIndex + 3) - (objects1.Count)) + 1 + ": " + objects1[(int)((cycleIndex + 3) - (objects1.Count))].name);

                    if (objects1[(int)((cycleIndex + 3) - (objects1.Count))].GetComponent<SpriteRenderer>() != null)
                        goHUD4.GetComponent<SpriteRenderer>().sprite = objects1[(int)((cycleIndex + 3) - (objects1.Count))].GetComponent<SpriteRenderer>().sprite;
                    else if (objects1[(int)((cycleIndex + 3) - (objects1.Count))].GetComponentInChildren<SpriteRenderer>() != null)
                        goHUD4.GetComponent<SpriteRenderer>().sprite = objects1[(int)((cycleIndex + 3) - (objects1.Count))].GetComponentInChildren<SpriteRenderer>().sprite;
                }

                HUD4Text = "1: cycleIndex + 3: " + cycleIndex + 3;
            }

            if (objects1.Count >= 5)
            {
                if ((cycleIndex + 4) <= (objects1.Count - 1))
                {
                    goHUD5Text.SetText((cycleIndex + 4) + 1 + ": " + objects1[cycleIndex + 4].name);

                    if (objects1[cycleIndex + 4].GetComponent<SpriteRenderer>() != null)
                        goHUD5.GetComponent<SpriteRenderer>().sprite = objects1[cycleIndex + 4].GetComponent<SpriteRenderer>().sprite;
                    else if (objects1[cycleIndex + 4].GetComponentInChildren<SpriteRenderer>() != null)
                        goHUD5.GetComponent<SpriteRenderer>().sprite = objects1[cycleIndex + 4].GetComponentInChildren<SpriteRenderer>().sprite;
                }
                else
                {
                    goHUD5Text.SetText((int)((cycleIndex + 4) - (objects1.Count)) + 1 + ": " + objects1[(int)((cycleIndex + 4) - (objects1.Count))].name);

                    if (objects1[(int)((cycleIndex + 4) - (objects1.Count))].GetComponent<SpriteRenderer>() != null)
                        goHUD5.GetComponent<SpriteRenderer>().sprite = objects1[(int)((cycleIndex + 4) - (objects1.Count))].GetComponent<SpriteRenderer>().sprite;
                    else if (objects1[(int)((cycleIndex + 4) - (objects1.Count))].GetComponentInChildren<SpriteRenderer>() != null)
                        goHUD5.GetComponent<SpriteRenderer>().sprite = objects1[(int)((cycleIndex + 4) - (objects1.Count))].GetComponentInChildren<SpriteRenderer>().sprite;
                }

                HUD5Text = "1: cycleIndex + 4: " + cycleIndex + 4;
            }
        }
        else if (objectsActive == 2)
        {
            if (objects2.Count >= 1)
            {
                if ((cycleIndex) <= (objects2.Count - 1))
                {
                    goHUD1Text.SetText(cycleIndex + 1 + ": " + objects2[cycleIndex].name);

                    if (objects2[cycleIndex].GetComponent<SpriteRenderer>() != null)
                        goHUD1.GetComponent<SpriteRenderer>().sprite = objects2[cycleIndex].GetComponent<SpriteRenderer>().sprite;
                    else if (objects2[cycleIndex].GetComponentInChildren<SpriteRenderer>() != null)
                        goHUD1.GetComponent<SpriteRenderer>().sprite = objects2[cycleIndex].GetComponentInChildren<SpriteRenderer>().sprite;
                }
                else
                {
                    goHUD1Text.SetText((int)(cycleIndex - (objects2.Count)) + 1 + ": " + objects2[(int)(cycleIndex - (objects2.Count))].name);

                    if (objects2[(int)(cycleIndex - (objects2.Count))].GetComponent<SpriteRenderer>() != null)
                        goHUD1.GetComponent<SpriteRenderer>().sprite = objects2[(int)(cycleIndex - (objects2.Count))].GetComponent<SpriteRenderer>().sprite;
                    else if (objects2[(int)(cycleIndex - (objects2.Count))].GetComponentInChildren<SpriteRenderer>() != null)
                        goHUD1.GetComponent<SpriteRenderer>().sprite = objects2[(int)(cycleIndex - (objects2.Count))].GetComponentInChildren<SpriteRenderer>().sprite;
                }

                HUD1Text = "1: cycleIndex: " + cycleIndex;
            }

            if (objects2.Count >= 2)
            {
                if ((cycleIndex + 1) <= (objects2.Count - 1))
                {
                    goHUD2Text.SetText((int)(cycleIndex + 1) + 1 + ": " + objects2[cycleIndex + 1].name);

                    if (objects2[cycleIndex + 1].GetComponent<SpriteRenderer>() != null)
                        goHUD2.GetComponent<SpriteRenderer>().sprite = objects2[cycleIndex + 1].GetComponent<SpriteRenderer>().sprite;
                    else if (objects2[cycleIndex + 1].GetComponent<SpriteRenderer>() != null)
                        goHUD2.GetComponent<SpriteRenderer>().sprite = objects2[cycleIndex + 1].GetComponent<SpriteRenderer>().sprite;
                }
                else
                {
                    goHUD2Text.SetText((int)((cycleIndex + 1) - (objects2.Count)) + 1 + ": " + objects2[(int)((cycleIndex + 1) - (objects2.Count))].name);

                    if (objects2[(int)((cycleIndex + 1) - (objects2.Count))].GetComponent<SpriteRenderer>() != null)
                        goHUD2.GetComponent<SpriteRenderer>().sprite = objects2[(int)((cycleIndex + 1) - (objects2.Count))].GetComponent<SpriteRenderer>().sprite;
                    else if (objects2[(int)((cycleIndex + 1) - (objects2.Count))].GetComponentInChildren<SpriteRenderer>() != null)
                        goHUD2.GetComponent<SpriteRenderer>().sprite = objects2[(int)((cycleIndex + 1) - (objects2.Count))].GetComponentInChildren<SpriteRenderer>().sprite;
                }

                HUD2Text = "1: cycleIndex + 1: " + cycleIndex + 1;
            }

            if (objects2.Count >= 3)
            {
                if ((cycleIndex + 2) <= (objects2.Count - 1))
                {
                    goHUD3Text.SetText((int)(cycleIndex + 2) + 1 + ": " + objects2[cycleIndex + 2].name);

                    if (objects2[cycleIndex + 2].GetComponent<SpriteRenderer>() != null)
                        goHUD3.GetComponent<SpriteRenderer>().sprite = objects2[cycleIndex + 2].GetComponent<SpriteRenderer>().sprite;
                    else if (objects2[cycleIndex + 2].GetComponentInChildren<SpriteRenderer>() != null)
                        goHUD3.GetComponent<SpriteRenderer>().sprite = objects2[cycleIndex + 2].GetComponentInChildren<SpriteRenderer>().sprite;
                }
                else
                {
                    goHUD3Text.SetText((int)((cycleIndex + 2) - (objects2.Count)) + 1 + ": " + objects2[(int)((cycleIndex + 2) - (objects2.Count))].name);

                    if (objects2[(int)((cycleIndex + 2) - (objects2.Count))].GetComponent<SpriteRenderer>() != null)
                        goHUD3.GetComponent<SpriteRenderer>().sprite = objects2[(int)((cycleIndex + 2) - (objects2.Count))].GetComponent<SpriteRenderer>().sprite;
                    else if (objects2[(int)((cycleIndex + 2) - (objects2.Count))].GetComponentInChildren<SpriteRenderer>() != null)
                        goHUD3.GetComponent<SpriteRenderer>().sprite = objects2[(int)((cycleIndex + 2) - (objects2.Count))].GetComponentInChildren<SpriteRenderer>().sprite;
                }

                HUD3Text = "1: cycleIndex + 2: " + cycleIndex + 2;
            }

            if (objects2.Count >= 4)
            {
                if ((cycleIndex + 3) <= (objects2.Count - 1))
                {
                    goHUD4Text.SetText((int)(cycleIndex + 3) + 1 + ": " + objects2[cycleIndex + 3].name);

                    if (objects2[cycleIndex + 3].GetComponent<SpriteRenderer>() != null)
                        goHUD4.GetComponent<SpriteRenderer>().sprite = objects2[cycleIndex + 3].GetComponent<SpriteRenderer>().sprite;
                    else if (objects2[cycleIndex + 3].GetComponentInChildren<SpriteRenderer>() != null)
                        goHUD4.GetComponent<SpriteRenderer>().sprite = objects2[cycleIndex + 3].GetComponentInChildren<SpriteRenderer>().sprite;
                }
                else
                {
                    goHUD4Text.SetText((int)((cycleIndex + 3) - (objects2.Count)) + 1 + ": " + objects2[(int)((cycleIndex + 3) - (objects2.Count))].name);

                    if (objects2[(int)((cycleIndex + 3) - (objects2.Count))].GetComponent<SpriteRenderer>() != null)
                        goHUD4.GetComponent<SpriteRenderer>().sprite = objects2[(int)((cycleIndex + 3) - (objects2.Count))].GetComponent<SpriteRenderer>().sprite;
                    else if (objects2[(int)((cycleIndex + 3) - (objects2.Count))].GetComponentInChildren<SpriteRenderer>() != null)
                        goHUD4.GetComponent<SpriteRenderer>().sprite = objects2[(int)((cycleIndex + 3) - (objects2.Count))].GetComponentInChildren<SpriteRenderer>().sprite;
                }

                HUD4Text = "1: cycleIndex + 3: " + cycleIndex + 3;
            }

            if (objects2.Count >= 5)
            {
                if ((cycleIndex + 4) <= (objects2.Count - 1))
                {
                    goHUD5Text.SetText((cycleIndex + 4) + 1 + ": " + objects2[cycleIndex + 4].name);

                    if (objects2[cycleIndex + 4].GetComponent<SpriteRenderer>() != null)
                        goHUD5.GetComponent<SpriteRenderer>().sprite = objects2[cycleIndex + 4].GetComponent<SpriteRenderer>().sprite;
                    else if (objects2[cycleIndex + 4].GetComponentInChildren<SpriteRenderer>() != null)
                        goHUD5.GetComponent<SpriteRenderer>().sprite = objects2[cycleIndex + 4].GetComponentInChildren<SpriteRenderer>().sprite;
                }
                else
                {
                    goHUD5Text.SetText((int)((cycleIndex + 4) - (objects2.Count)) + 1 + ": " + objects2[cycleIndex + 4].name);

                    if (objects2[(int)((cycleIndex + 4) - (objects2.Count))].GetComponent<SpriteRenderer>() != null)
                        goHUD5.GetComponent<SpriteRenderer>().sprite = objects2[(int)((cycleIndex + 4) - (objects2.Count))].GetComponent<SpriteRenderer>().sprite;
                    else if (objects2[(int)((cycleIndex + 4) - (objects2.Count))].GetComponentInChildren<SpriteRenderer>() != null)
                        goHUD5.GetComponent<SpriteRenderer>().sprite = objects2[(int)((cycleIndex + 4) - (objects2.Count))].GetComponentInChildren<SpriteRenderer>().sprite;
                }

                HUD5Text = "1: cycleIndex + 4: " + cycleIndex + 4;
            }
        }
    }

    void OnDestroy() {
        CinemachineCore.CameraUpdatedEvent.RemoveListener(UpdateObjectPosition);
    }
}
