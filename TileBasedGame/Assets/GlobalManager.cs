using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalManager
{
    private static GlobalManager _instance;
    public static GlobalManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GlobalManager();
            }
            return _instance;
        }
    }

    private GlobalManager()
    {

    }

    public List<ClassFactory> PartyFactories = new List<ClassFactory>();

    public List<Unit> GeneratePartyForStage()
    {
        List < Unit > units = new List<Unit>();
        foreach(ClassFactory f in PartyFactories)
        {
            units.Add(f.Generate());
        }
        return units;
    }
}