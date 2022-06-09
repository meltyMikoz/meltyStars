using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class OnGenerateCSProjectProcessor : AssetPostprocessor
{
    public static string OnGeneratedCSProjectProcessor(string path, string content)
    {
        return content;
    }
}
