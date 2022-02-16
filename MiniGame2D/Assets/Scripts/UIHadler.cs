using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHadler : MonoBehaviour
{
    [SerializeField] private GameObject WinPanel;

    public void Win()
    {
        WinPanel.SetActive(true);
    }
}
