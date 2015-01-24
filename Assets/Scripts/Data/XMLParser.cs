﻿using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;

public class XMLParser : MonoBehaviour
{
    public TextMesh fileDataTextbox;
    private string path;
    private string fileInfo;
    private XmlDocument xmlDoc;
    private WWW www;
    private TextAsset textXml;
    private List<LevelData> LevelsData;
    private string fileName;

    public struct AnimalData
    {
        public int AnimalId;
        public int AnimalAmount;
    }

    public struct ObstacleData
    {
        public int ObstacleId;
        public int ObstAmount;
    }

    // Structure for mainitaing the Level information
    public struct LevelData
    {
        public int PlayerId;
        public string name;
        public int score;
        public List<AnimalData> Animals;
        public int FarmerAmount;
        public List<ObstacleData> Obstacles;
        public string LevelName;

    };


    //void Awake()
    //{
    //    fileName = "LevelDataFile";
    //    Players = new List<LevelData>(); // initalize player list
    //    fileDataTextbox.text = "";
    //}

    //void Start()
    //{
    //    loadXMLFromAssest();
    //    readXml();
    //}
    // Following method load xml file from resouces folder under Assets
    private void loadXMLFromAssest()
    {
        xmlDoc = new XmlDocument();
        if (System.IO.File.Exists(getPath()))
        {
            xmlDoc.LoadXml(System.IO.File.ReadAllText(getPath()));
        }
        else
        {
            textXml = (TextAsset)Resources.Load(fileName, typeof(TextAsset));
            xmlDoc.LoadXml(textXml.text);
        }
    }

    //void Update()
    //{
    //    // Following code just check whether any button is touched on mouse down
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //        RaycastHit hit;
    //        if (Physics.Raycast(ray, out hit) && hit.collider != null)
    //        {
    //            if (hit.collider.name.Equals("ModifyXmlButton"))
    //                modifyXml();
    //            else if (hit.collider.name.Equals("CreateElementButton"))
    //                createElement();
    //        }
    //    }
    //}
    // Following method reads the xml file and display its content 
    private void readXml()
    {
        foreach (XmlElement node in xmlDoc.SelectNodes(Defines.NodeLevelData))
        {
            
            LevelData tempLvlData = new LevelData();
            foreach( XmlElement nPlayer in xmlDoc.SelectNodes(Defines.NodePlayerData))
            {
                tempLvlData.PlayerId = int.Parse(nPlayer.GetAttribute(Defines.Id));
                tempLvlData.name = nPlayer.SelectSingleNode(Defines.Name).InnerText;
                tempLvlData.score = int.Parse(nPlayer.SelectSingleNode(Defines.Score).InnerText);

                //displayPlayeData(tempLvlData);
            }
            foreach (XmlElement nFarmer in xmlDoc.SelectNodes(Defines.NodeAnimalData))
            {
                tempLvlData.FarmerAmount = int.Parse(nFarmer.GetAttribute(Defines.Amount));

            }
            foreach (XmlElement nAnimal in xmlDoc.SelectNodes(Defines.NodeAnimalData))
            {
                AnimalData tempAnimal = new AnimalData();
                tempAnimal.AnimalId = int.Parse(nAnimal.GetAttribute(Defines.Id));
                tempAnimal.AnimalAmount =int.Parse(nAnimal.GetAttribute(Defines.Amount));
                tempLvlData.Animals.Add(tempAnimal);
            }
            foreach (XmlElement nAnimal in xmlDoc.SelectNodes(Defines.NodeObstacleData))
            {
                ObstacleData tempObstacle = new ObstacleData();
                tempObstacle.ObstacleId = int.Parse(nAnimal.GetAttribute(Defines.Id));
                tempObstacle.ObstAmount =int.Parse(nAnimal.GetAttribute(Defines.Amount));
                tempLvlData.Obstacles.Add(tempObstacle);
            }
            foreach (XmlElement nPrefab in xmlDoc.SelectNodes(Defines.NodeAnimalData))
            {
                tempLvlData.LevelName = nPrefab.SelectSingleNode(Defines.Name).InnerText;
            }
            LevelsData.Add(tempLvlData);
        }
    }

    /// <summary>
    /// Get method for return XML level data file.
    /// </summary>
    /// <returns>List<LevelData></returns>
    public List<LevelData> GetLevelData()
    {
        fileName = "LevelDataFile";
        LevelsData = new List<LevelData>();
        loadXMLFromAssest();
        readXml();
        return LevelsData;
    }

    //private void displayPlayeData(LevelData tempPlayer)
    //{
    //    fileDataTextbox.text += tempPlayer.PlayerId + "\t\t" + tempPlayer.name + "\t\t" + tempPlayer.score + "\n";
    //}

    //// Following method will be called by tapping ModifyXml button
    //// It just multiply player's score by 10
    //private void modifyXml()
    //{
    //    fileDataTextbox.text = "";
    //    foreach (XmlElement node in xmlDoc.SelectNodes(Defines.NodeLevelData))
    //    {
    //        int nodeId = int.Parse(node.GetAttribute(Defines.Id));
    //        LevelData tempPlayer = Players[nodeId - 1];
    //        tempPlayer.score *= 10;
    //        node.SelectSingleNode(Defines.Score).InnerText = tempPlayer.score + "";
    //        displayPlayeData(tempPlayer);
    //    }
    //    xmlDoc.Save(getPath() + Defines.FormatFile);
    //}

    //Comentado - No se va a usar
    // Following method create new element of player
    //private void createElement()
    //{
    //    LevelData tempPlayer = new LevelData();
    //    tempPlayer.PlayerId = Players.Count + 1;
    //    tempPlayer.name = "Player" + tempPlayer.PlayerId;
    //    tempPlayer.score = tempPlayer.PlayerId * 10;
    //    Players.Add(tempPlayer);
    //    displayPlayeData(tempPlayer);

    //    XmlNode parentNode = xmlDoc.SelectSingleNode("Players");
    //    XmlElement element = xmlDoc.CreateElement("Player");
    //    element.SetAttribute("id", tempPlayer.PlayerId.ToString());
    //    element.AppendChild(createNodeByName("name", tempPlayer.name));
    //    element.AppendChild(createNodeByName("score", tempPlayer.score.ToString()));
    //    parentNode.AppendChild(element);
    //    xmlDoc.Save(getPath() + ".xml");
    //}
    //private XmlNode createNodeByName(string name, string innerText)
    //{
    //    XmlNode node = xmlDoc.CreateElement(name);
    //    node.InnerText = innerText;
    //    return node;
    //}

    // Following method is used to retrive the relative path as device platform
    private string getPath()
    {
#if UNITY_EDITOR
        return Application.dataPath + "/Resources/" + fileName;
#elif UNITY_ANDROID
		return Application.persistentDataPath+fileName;
#elif UNITY_IPHONE
		return GetiPhoneDocumentsPath()+"/"+fileName;
#else
		return Application.dataPath +"/"+ fileName;
#endif
    }
    private string GetiPhoneDocumentsPath()
    {
        // Strip "/Data" from path 
        string path = Application.dataPath.Substring(0, Application.dataPath.Length - 5);
        // Strip application name 
        path = path.Substring(0, path.LastIndexOf('/'));
        return path + "/Documents";
    }
}