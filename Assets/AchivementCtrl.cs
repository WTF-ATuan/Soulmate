using System;
using System.Collections;
using System.Collections.Generic;
using Game.Scripts;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class AchivementCtrl : MonoBehaviour
{
    [Inject] public readonly PersonalityDataSet _dataSet;
    
    public List<Text> Achivements;
    public List<GameObject> Mask;

    private void OnEnable()
    {
        for (int i = 0; i < _dataSet.endingRulesList.Count; i++)
        {
            var name = _dataSet.endingRulesList[i].name;
            var s = PlayerPrefs.GetInt(name, 0);
            Achivements[i].text = name;
            Mask[i].SetActive(s==0);
        }
    }
}
