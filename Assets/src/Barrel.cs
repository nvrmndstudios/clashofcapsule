using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour, IBlastListener
{
    public event Action<GameObject> OnBarrelBlasted;

    public void OnBlast()
    {
        OnBarrelBlasted?.Invoke(gameObject);
    }
}
