using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboardMonoEntry : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI textField;

    public void Init(LeaderboardEntry entry)
    {
        textField.text = entry.userName + " : " + entry.record;
    }
}
