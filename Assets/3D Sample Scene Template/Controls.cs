using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using UnityEngine;
using UnityTemplateProjects;

public class Settings
{
    public Settings() { }
    
    public int Setting1 { get; set; }
    public bool Setting2 { get; set; }
    public double Setting3 { get; set; } = 7.6;
    public System.DateTime Setting4 { get; set; }
    public string Setting5 { get; set; } = "test";
    public List<string> Setting6 { get; set; } = new List<string>()
                                       { "test1", "test2" };

}

public class Controls : MonoBehaviour
{
    public GameObject go1stGO1;
    public GameObject go1stGO2;
    public GameObject go1stGO3;
    public GameObject go1stGO4;
    public GameObject go1stGO5;

    public GameObject go2ndGO1;
    public GameObject go2ndGO2;
    public GameObject go2ndGO3;
    
    public GameObject go3rdGO1;
    public GameObject go3rdGO2;
    public GameObject go3rdGO3;
    
    public GameObject go4thGO1;
    public GameObject go4thGO2;
    public GameObject go4thGO3;

    public bool bGo1stEditModeGOs;
    public bool bGo2ndThirdPersonGOs;
    public bool bGo3rdFirstPersonModeGOs;
    public bool bGo4thDemoModeGOs;

    public GameObject BackgroundSphereOcean;
    public GameObject BackgroundSphereRiver;

    public MeshRenderer BackgroundSphereMeshRenderer;
    public Material[] BackgroundSphereMaterials = new Material[7];

    public Vector3 delta = Vector3.zero;
    private Vector3 lastPos = Vector3.zero;

    public string magnitude = "";

    public bool IsKeyDownC = false;
    public bool IsKeyDownF = false;

    public bool IsKeyDownL = false;
    public bool IsKeyDownK = false;
    
    public SpawnedObjectManager spwn;

    public Character character;

    // Start is called before the first frame update
    void Start()
    {
        #region Settings

        //var mySettings = (MySettingsClass)GetSettings("MySettingsClass"); // here, the class
                                               // is used is named MySettingsClass

        ////var settings = (Settings)GetSettings("Settings"); // name of class, from above
        ////settings.Setting1 = settings.Setting1 + 1;        // change some settings
        ////settings.Setting4 = DateTime.Now;
        ////SaveSettings(settings);

        #endregion
    }

    public void SaveSettings(object settings, string filename = "settings.config")
    {
        // create storage folder and file name
        //string storageFolder = Path.Combine(
        //    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        //    Path.GetFileNameWithoutExtension(Application.persistentDataPath));
        //if (!Directory.Exists(storageFolder))
        //    Directory.CreateDirectory(storageFolder);
        //if (!Directory.Exists(storageFolder))
        //    throw new Exception($"Could not create folder {storageFolder}");
        //string settingsFileName = Path.Combine(storageFolder, filename);

        //// create file and process class, save properties one by one
        //XmlWriterSettings xmlWriterSettings = new XmlWriterSettings()
        //{
        //    Indent = true,
        //};
        //XmlWriter writer = XmlWriter.Create(settingsFileName, xmlWriterSettings);
        //using (writer)
        //{
        //    writer.WriteStartElement("settings");
        //    var classType = settings.GetType();
        //    var props = classType.GetProperties();
        //    for (int i = 0; i < props.Length; i++)
        //    {
        //        string fileSetting = char.ToLower(props[i].Name[0]) + props[i].Name.Substring(1);

        //        if (props[i].PropertyType == typeof(string))
        //        {
        //            var sett = classType.GetProperty(props[i].Name).GetValue(settings);
        //            if (sett != null)
        //            {
        //                string s = sett.ToString();
        //                if (!string.IsNullOrEmpty(s))
        //                    writer.WriteElementString(fileSetting, s);
        //            }
        //        }
        //        else if (props[i].PropertyType == typeof(int))
        //        {
        //            writer.WriteElementString(fileSetting,
        //            classType.GetProperty(props[i].Name).GetValue(settings).ToString());
        //        }
        //        else if (props[i].PropertyType == typeof(double))
        //        {
        //            writer.WriteElementString(fileSetting,
        //            classType.GetProperty(props[i].Name).GetValue(settings).ToString());
        //        }
        //        else if (props[i].PropertyType == typeof(DateTime))
        //        {
        //            var dt = (DateTime)(classType.GetProperty(props[i].Name).GetValue(settings));
        //            writer.WriteElementString(fileSetting, dt.ToOADate().ToString());
        //        }
        //        else if (props[i].PropertyType == typeof(bool))
        //        {
        //            writer.WriteElementString(fileSetting,
        //            classType.GetProperty(props[i].Name).GetValue(settings).ToString());
        //        }
        //        else if (props[i].PropertyType == typeof(List<string>))
        //        {
        //            List<string> values =
        //            classType.GetProperty(props[i].Name).GetValue(settings) as List<string>;
        //            string val = string.Join(",", values.ToArray());
        //            writer.WriteElementString(fileSetting, val);
        //        }
        //        else
        //            throw new Exception($"Unknown setting type found: {props[0].PropertyType}");
        //    }

        //    writer.WriteEndElement();
        //    writer.Flush();
        //}
    }

    public object GetSettings(string settingsClassName, string filename = "settings.config")
    {
        // get class type and create default class
        var settingsType = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                            from t in assembly.GetTypes()
                            where t.Name == settingsClassName
                            select t).FirstOrDefault();
        object settings = Activator.CreateInstance(settingsType);

        // create storage folder and file name
        string storageFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            Path.GetFileNameWithoutExtension(Application.persistentDataPath));
        if (!Directory.Exists(storageFolder))
            Directory.CreateDirectory(storageFolder);
        if (!Directory.Exists(storageFolder))
            throw new Exception($"Could not create folder {storageFolder}");
        string settingsFileName = Path.Combine(storageFolder, filename);

        // recreate file if missing and return default settings
        if (!File.Exists(settingsFileName))
        {
            SaveSettings(settings);
            return settings;
        }

        // read and process file
        XmlDocument settingsFile = new XmlDocument();
        try
        {
            settingsFile.Load(settingsFileName);
        }
        catch (Exception ex)
        {
            
            // option to return default data
            SaveSettings(settings);
            return settings;

            throw ex;
        }
        XmlNode settingsNode = null;
        int n = 0;
        while (n < settingsFile.ChildNodes.Count && settingsNode == null)
        {
            if (settingsFile.ChildNodes[n].Name.ToLower() == "settings")
                settingsNode = settingsFile.ChildNodes[n];
            n++;
        }
        if (settingsNode == null)
        {
            
            // option to return default data
            return settings;

            throw new Exception($"Settings section is not found in settings file { settingsFileName }");
        }
        var classType = settings.GetType();
        var props = classType.GetProperties();
        foreach (XmlNode setting in settingsNode.ChildNodes)
        {
            if (setting.ParentNode.Name.ToLower() != "settings")
                break;
            if (setting.NodeType != XmlNodeType.Element)
                continue;

            var settingName = props.Where
            (w => string.Compare(w.Name, setting.Name, true) == 0).ToList();
            if (settingName.Count == 0)
                continue;
            if (string.IsNullOrEmpty(settingName[0].Name))
                continue;

            // parse setting as type defines
            if (settingName[0].PropertyType == typeof(string))
                classType.GetProperty(settingName[0].Name).SetValue(settings, setting.InnerText);
            else if (settingName[0].PropertyType == typeof(int))
            {
                int val = 0;
                if (int.TryParse(setting.InnerText, out val))
                    classType.GetProperty(settingName[0].Name).SetValue(settings, val);
            }
            else if (settingName[0].PropertyType == typeof(double))
            {
                double val = 0;
                if (double.TryParse(setting.InnerText, out val))
                    classType.GetProperty(settingName[0].Name).SetValue(settings, val);
            }
            else if (settingName[0].PropertyType == typeof(DateTime))
            {
                double val = 0;
                if (double.TryParse(setting.InnerText, out val))
                    classType.GetProperty(settingName[0].Name).SetValue
                    (settings, DateTime.FromOADate(val));
            }
            else if (settingName[0].PropertyType == typeof(bool))
            {
                bool val = (string.Compare("true",
                setting.InnerText, true) == 0) || setting.InnerText == "1";
                classType.GetProperty(settingName[0].Name).SetValue(settings, val);
            }
            else if (settingName[0].PropertyType == typeof(List<string>))
            {
                string val = setting.InnerText.Trim();
                if (string.IsNullOrEmpty(val))
                {
                    classType.GetProperty(settingName[0].Name).SetValue
                    (settings, new List<string>());
                    continue;
                }
                List<string> values = val.Split(',').ToList();
                classType.GetProperty(settingName[0].Name).SetValue(settings, values);
            }
            else
                throw new Exception
                ($"Unknown setting type found: {settingName[0].PropertyType}");
        }
        return settings;
    }

    int backgroundSphereCounter = 0;

    // Update is called once per frame
    void Update()
    {
        if (IsKeyDownC == true)
        {
            if (spwn != null)
            {
                foreach (var item in spwn.spawnedBodies)
                {
                    if(item.GetComponent<Rigidbody>() != null)
                        item.GetComponent<Rigidbody>().isKinematic = true;
                }
            }
        }

        if (IsKeyDownC == false)
        {
            if (spwn != null)
            {
                foreach (var item in spwn.spawnedBodies)
                {
                    if(item != null)
                        if(item.GetComponent<Rigidbody>() != null)
                            item.GetComponent<Rigidbody>().isKinematic = false;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("space key was pressed");
        }

        if (Input.GetKeyDown(KeyCode.C) == true)
        {
            IsKeyDownC = true;
            IsKeyDownF = false;
        }

        if (Input.GetKeyDown(KeyCode.F) == true)
        {
            IsKeyDownF = true;
            IsKeyDownC = false;
        }

        if (Input.GetKeyDown(KeyCode.L) == true)
        {
            IsKeyDownL = true;
            IsKeyDownK = false;
        }

        if (Input.GetKeyDown(KeyCode.K) == true)
        {
            IsKeyDownK = true;
            IsKeyDownL = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            lastPos = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            delta = Input.mousePosition - lastPos;

            // Do Stuff here

            //Debug.Log("delta X : " + delta.x);
            //Debug.Log("delta Y : " + delta.y);

            //Debug.Log("delta distance : " + delta.magnitude);

            magnitude = "delta distance : " + delta.magnitude;

            // End do stuff

            lastPos = Input.mousePosition;
        }


        if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
        {
            //if (bGo1stEditModeGOs == true)
            //{
            //    //bGo1stEditModeGOs = false;
            //}
            //else
            //{
            //    bGo1stEditModeGOs = true;

            //    bGo2ndThirdPersonGOs = false;
            //    bGo3rdFirstPersonModeGOs = false;
            //    bGo4thDemoModeGOs = false;
            //}
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ////character.SaveGame();

            //SaveLoad.SaveData(character);
        }

        if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
        {
            //if (bGo2ndThirdPersonGOs == true)
            //{
            //    bGo2ndThirdPersonGOs = false;
            //}
            //else
            //{
            //    bGo2ndThirdPersonGOs = true;

            //    bGo1stEditModeGOs = false;
            //    bGo3rdFirstPersonModeGOs = false;
            //    bGo4thDemoModeGOs = false;
            //}
        }

        //if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    if (bGo3rdFirstPersonModeGOs == true)
        //    {
        //        bGo3rdFirstPersonModeGOs = false;
        //    }
        //    else
        //    {
        //        bGo3rdFirstPersonModeGOs = true;

        //        bGo1stEditModeGOs = false;
        //        bGo2ndThirdPersonGOs = false;
        //        bGo4thDemoModeGOs = false;
        //    }
        //}

        if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Alpha4))
        {
            //if (bGo4thDemoModeGOs == true)
            //{
            //    bGo4thDemoModeGOs = false;
            //}
            //else
            //{
            //    bGo4thDemoModeGOs = true;

            //    bGo1stEditModeGOs = false;
            //    bGo2ndThirdPersonGOs = false;
            //    bGo3rdFirstPersonModeGOs = false;
            //}
        }

        if (Input.GetKeyDown(KeyCode.Keypad5) || Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (BackgroundSphereOcean.activeSelf)
            {
                BackgroundSphereOcean.SetActive(false);
                BackgroundSphereRiver.SetActive(true);
            }
            else
            {
                BackgroundSphereOcean.SetActive(true);
                BackgroundSphereRiver.SetActive(false);
            }


            //Material Change
            //if (backgroundSphereCounter >= 7)
            //    backgroundSphereCounter = 0;

                //BackgroundSphereMeshRenderer.material = BackgroundSphereMaterials[backgroundSphereCounter];

                //backgroundSphereCounter++;
        }

        //------
        if (bGo1stEditModeGOs == true)
        {
            if(go1stGO1 != null)
                go1stGO1.SetActive(bGo1stEditModeGOs);

            if (go1stGO2 != null)
            {
                go1stGO2.SetActive(bGo1stEditModeGOs);
            }
            if (go1stGO3 != null)
                go1stGO3.SetActive(bGo1stEditModeGOs);

            if (go1stGO4 != null)
                go1stGO4.SetActive(bGo1stEditModeGOs);

            if (go1stGO5 != null)
                go1stGO5.SetActive(bGo1stEditModeGOs);
        }
        else
        {
            if (go1stGO1 != null)
                go1stGO1.SetActive(false);

            if (go1stGO2 != null)
                go1stGO2.SetActive(false);

            if (go1stGO3 != null)
                go1stGO3.SetActive(false);

            if (go1stGO4 != null)
                go1stGO4.SetActive(false);
        }

        if (bGo2ndThirdPersonGOs == true)
        {
            if (go2ndGO1 != null)
            {
                go2ndGO1.SetActive(bGo2ndThirdPersonGOs);

            }
            if (go2ndGO2 != null)
            {
                go2ndGO2.SetActive(bGo2ndThirdPersonGOs);



            }

            if (go2ndGO3 != null)
                go2ndGO3.SetActive(bGo2ndThirdPersonGOs);
        }
        else
        {
            if (go2ndGO1 != null)
                go2ndGO1.SetActive(false);

            if (go2ndGO2 != null)
            {
                go2ndGO2.SetActive(false);

                
            }

            if (go2ndGO3 != null)
                go2ndGO3.SetActive(false);
        }

        if (bGo3rdFirstPersonModeGOs == true)
        {
            if (go3rdGO1 != null)
                go3rdGO1.SetActive(bGo3rdFirstPersonModeGOs);

            if (go3rdGO2 != null)
                go3rdGO2.SetActive(bGo3rdFirstPersonModeGOs);

            if (go3rdGO3 != null)
                go3rdGO3.SetActive(bGo3rdFirstPersonModeGOs);
        }
        else
        {
            if (go3rdGO1 != null)
                go3rdGO1.SetActive(false);

            if (go3rdGO2 != null)
                go3rdGO2.SetActive(false);

            if (go3rdGO3 != null)
                go3rdGO3.SetActive(false);
        }

        if (bGo4thDemoModeGOs == true)
        {
            if (go4thGO1 != null)
                go4thGO1.SetActive(bGo4thDemoModeGOs);

            if (go4thGO2 != null)
                go4thGO2.SetActive(bGo4thDemoModeGOs);

            if (go4thGO3 != null)
                go4thGO3.SetActive(bGo4thDemoModeGOs);
        }
        else
        {
            if (go4thGO1 != null)
                go4thGO1.SetActive(false);

            if (go4thGO2 != null)
                go4thGO2.SetActive(false);

            if (go4thGO3 != null)
                go4thGO3.SetActive(false);
        }
    }
}
