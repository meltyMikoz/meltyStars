using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
using UnityEditor;

public class TestEncoding : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string[] strArray_1 = File.ReadAllLines($"{Application.streamingAssetsPath}/UTF8.txt", Encoding.GetEncoding("GB2312"));
        string[] strArray_2 = File.ReadAllLines($"{Application.streamingAssetsPath}/UTF16.txt", Encoding.GetEncoding("GB18030"));
        string[] strArray_3 = File.ReadAllLines($"{Application.streamingAssetsPath}/GB2312.txt", Encoding.GetEncoding("GB18030"));

        print(strArray_1[0]);
        print(strArray_2[0]);
        print(strArray_3[0]);


        using (FileStream fs = File.OpenRead($"{Application.streamingAssetsPath}/TestFile.txt"))
        {
            byte[] arr = new byte[fs.Length];
            fs.Read(arr, 0, (int)fs.Length);
            byte[] arr2 = BitConverter.GetBytes(5);
            foreach (byte b in arr2)
            {
                Debug.Log(b);
            }
            Debug.Log(BitConverter.ToInt32(arr2, 0));
            fs.Close();
        }
        using (FileStream fs = File.OpenWrite($"{Application.streamingAssetsPath}/nk.ctf"))
        {
            byte[] bArr = BitConverter.GetBytes(5);
            fs.Write(bArr, 0, bArr.Length);
            fs.Flush();
            fs.Close();
        }
        AssetDatabase.Refresh();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
