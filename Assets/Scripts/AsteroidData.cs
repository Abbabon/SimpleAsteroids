using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AsteroidsData", menuName = "ScriptableObjects/AsteroidsData")]
public class AsteroidData : ScriptableObject
{
    public List<AsteroidLevelData> AsteroidLevels;
}

[Serializable]
public class AsteroidLevelData
{
    public int Level;
    public Asteroid Prefab;
}