using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Param
{
    public List<float> parameter = new List<float>();
}

[System.Serializable]
public class StringParam
{
    public uint size=0;
    public string value = "";
}


public class AddElement : MonoBehaviour
{
    public List<Param> parameters = new List<Param>();
    public List<StringParam> stringParams = new List<StringParam>();
}
