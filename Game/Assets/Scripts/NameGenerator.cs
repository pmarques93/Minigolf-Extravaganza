using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NameGenerator
{
    private string[] names =
    {
        "Harry Potter",
        "Doug Dimmadome",
        "Robert California",
        "Gwyn Lord of Cinder",
        "Legolas",
        "Kayle",
        "Bernie Sanders",
        "The concept of time",
        "Alfred",
        "The Sun",
        "Mini Golf World Champion",
        "Champion Cynthia",
        "Lady Maria",
        "Ring Fit",
        "Joseph Guilotin",
        "Google",
        "Isabelle",
    };

    private System.Random rng;
    private List<int> usedNames;

    public NameGenerator()
    {
        rng = new System.Random();
        usedNames = new List<int>();
    }

    public string GetRandomName()
    {
        int i;
        do { i = rng.Next(names.Length); } while (usedNames.Contains(i));

        usedNames.Add(i);

        return names[i];
    }

    public void Reset()
    {
        usedNames = new List<int>();
    }
}
