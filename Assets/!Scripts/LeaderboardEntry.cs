using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LeaderboardEntry
{
    public string userName;
    public int record;
    public LeaderboardEntry(string userName, int record)
    {
        this.userName = userName;
        this.record = record;
    }
}
