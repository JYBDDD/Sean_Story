using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum State
    {
        Idle,
        Run,
        Attack,
        Hit,
        Die,
    }
    public enum ItemType
    {
        Equipment,
        Consumption,
        Coin,
        Etc,
    }
}
