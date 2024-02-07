using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField]
    TextAsset defaultJson;
    //Не хотел это сюда вставлять но названия для другого скрипта не придумал
    [SerializeField]
    Transform scrollContentObj;
    [SerializeField]
    LeaderboardMonoEntry monoEntryPref;

    string filePath;// Application.dataPath + "/Leaderboard.txt";
    public LeaderboardList leaderboardList = new LeaderboardList();
    private void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, "Leaderboard.txt");
    }
    private void Start()
    {
        leaderboardList = GetListFromJson();
    }
    LeaderboardList GetListFromJson()
    {
        if (!File.Exists(filePath))
            File.WriteAllText(filePath, defaultJson.text);

        return JsonUtility.FromJson<LeaderboardList>(File.ReadAllText(filePath));
    }
    void PutListToJson(LeaderboardList ls)
    {
        leaderboardList = ls;
        File.WriteAllText(filePath, JsonUtility.ToJson(leaderboardList, true));
    }
    bool CheckPlayerNewRecord(int currScore, string userName)
    {
        LeaderboardEntry entry = leaderboardList.entries.Where(x => x.userName == userName).First();
        if (entry != null)
        {
            if (currScore > entry.record)
            {
                return true;
            }
            else
                return false;
        }
        return true;
    }
    LeaderboardEntry GetPlayerIfExists(string userName)
    {
        foreach (var i in leaderboardList.entries)
        {
            if (i.userName == userName)
                return i;
        }
        return null;
        //leaderboardList.entries.Where(x => x.userName == userName).First() != null;
    }
    public bool HandleEntry(int currScore, string userName)
    {
        bool isNewRecord = false;
        LeaderboardEntry entry = GetPlayerIfExists(userName);
        if (entry != null)
        {
            if (CheckPlayerNewRecord(currScore, userName))
            {
                entry.record = currScore;
                isNewRecord = true;
            }
        }
        else
        {
            entry = new LeaderboardEntry(userName, currScore);

            List<LeaderboardEntry> list = leaderboardList.entries.ToList();
            list.Add(entry);
            leaderboardList.entries = list.ToArray();
            isNewRecord = true;
        }
        PutListToJson(leaderboardList);
        return isNewRecord;
    }
    public void GenerateLeaderboard()
    {
        foreach(Transform i in scrollContentObj)
        {
            Destroy(i.gameObject);
        }
        foreach(var i in leaderboardList.entries.OrderByDescending(x => x.record).ToList())
        {
            LeaderboardMonoEntry entry = Instantiate(monoEntryPref,scrollContentObj);
            entry.Init(i);
        }
    }
}
