using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HullPart : MonoBehaviour
{
    public int partID;

    public enum PartType
    {
        FrontPart,
        RearPart,
        Cabine,
        Wing
    }

    public PartType partType;
}
