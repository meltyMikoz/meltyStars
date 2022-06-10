using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class UpdateTest : MonoBehaviour
{
    private void Start()
    {
        print("Create By Hotfix");
    }
    public async static Task<string> TestTask()
    {
        await Task.Delay(5000);

        Debug.Log("nmsl");

        return "g";
    }
}
