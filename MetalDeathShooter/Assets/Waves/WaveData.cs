using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/WaveDataScriptableObject", order = 1)]
public class WaveData : ScriptableObject
{
    public string levelStartText;
    public int genNumber;
    public int wallThreshold;
    public int mineThreshold;
    public int arrowThreshold;
    public int laserThreshold;
    public int sniperThreshold;
    public int rerollChance;
    public int startMoney;
    public bool spawnInitial;
    public bool spawnFinal;
    public bool ignorePlacementRules;
    public bool allowBombers;
    public bool allowDestroyers;
    public bool allowSubs;
    
}
