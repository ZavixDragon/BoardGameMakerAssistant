using System;
using UnityEngine;

public class InitCurrentOnAwake : MonoBehaviour
{
    private void Awake() => Current.Init();
}