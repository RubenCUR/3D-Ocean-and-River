using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterData
{
    public string characterName;
    public int strength;
    public int intelligence;
    public int agility;

    public string[] nos;

    public SerializableVector3[] gos;

    public SerializableQuaternion[] ros;
    
    public CharacterData(Character character)
    {
        characterName = character.name;
        strength = character.strength;
        intelligence = character.intelligence;
        agility = character.agility;

        nos = character.nos;

        gos = character.gos;
        ros = character.ros;
    }
}