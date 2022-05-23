using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public new string name;
    public int strength;
    public int intelligence;
    public int agility;

    public Spawner spw;

    public GameObject[] spwObjectsClone;

    public SpawnedObjectManager spwnom;

    [SerializeField]
    public string[] audioFilenamesAndPathsForGuitar;

    [SerializeField]
    public string[] audioFilenamesAndPathsForDrums;

    [SerializeField]
    public string[] audioFilenamesAndPathsForVoiceOther;

    [SerializeField]
    public string[] audioFilenamesAndPathsForBass;

    [SerializeField]
    public string[] nos;

    [SerializeField]
    public SerializableVector3[] gos;

    [SerializeField]
    public SerializableQuaternion[] ros;

    [SerializeField]
    private InputField _name;

    [SerializeField]
    private InputField _strength;

    [SerializeField]
    private InputField _intelligence;

    [SerializeField]
    private InputField _agility;

    GameObject gameObject;

    void Start()
    {
        ///First Time Only!
        //SaveLoad.SaveData(this);

        //Load
        //spwObjectsClone = spw.objects.ToArray();
                     
        //CharacterData data = SaveLoad.LoadData();

        //name = data.characterName;
        //strength = data.strength;
        //intelligence = data.intelligence;
        //agility = data.agility;

        //nos = data.nos;

        //gos = data.gos;

        //ros = data.ros;

        ////Load Characters
        //for (int i = 0; i < nos.Length; i++)
        //{
        //    foreach (GameObject item in spwObjectsClone)
        //    {
        //        if (item.name == nos[i].Substring(0, nos[i].Length - 7))
        //        {
        //            gameObject = new GameObject();

        //            gameObject = item;

        //            gameObject = Instantiate(gameObject, gos[i], ros[i]);

        //            if (gameObject.GetComponent<Rigidbody>() != null)
        //            {
        //                gameObject.GetComponent<Rigidbody>().isKinematic = true;

        //                //gameObject.GetComponent<Rigidbody>().isKinematic = true;

        //                Destroy(gameObject.GetComponent<Rigidbody>());
        //            }

        //            gameObject.transform.SetPositionAndRotation(gos[i], ros[i]);

        //            spwnom.AddObject(gameObject);
        //        }
        //    }
        //}
        //////

        //_name.text = name;
        //_strength.text = strength.ToString();
        //_intelligence.text = intelligence.ToString();
        //_agility.text = agility.ToString();

        ////_gos.text = gos.ToString() + gos.Length;

        ////Load
    }

    public void UpdateName(InputField inputField)
    {
        name = inputField.text;
    }
    
    public void UpdateNameDirect(string text)
    {
        name = text;
    }

    public void UpdateStrength(InputField inputField)
    {
        strength = int.Parse(inputField.text);
    }

    public void UpdateIntelligence(InputField inputField)
    {
        intelligence = int.Parse(inputField.text);
    }

    public void UpdateAgility(InputField inputField)
    {
        agility = int.Parse(inputField.text);
    }

    private void OnDestroy()
    {
        //SaveGame();
    }

    public void SaveGame()
    {
        UpdateName(_name);

        //Save Positions

        nos = new string[spwnom.spawnedBodies.Count];

        gos = new SerializableVector3[spwnom.spawnedBodies.Count];

        ros = new SerializableQuaternion[spwnom.spawnedBodies.Count];

        for (int i = 0; i < spwnom.spawnedBodies.Count; i++)
        {
            nos[i] = spwnom.spawnedBodies[i].name;

            gos[i] = spwnom.spawnedBodies[i].transform.position;

            ros[i] = spwnom.spawnedBodies[i].transform.rotation;
        }
        
        ///

        SaveLoad.SaveData(this);
    }

}