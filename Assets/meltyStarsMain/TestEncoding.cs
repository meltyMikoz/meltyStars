using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

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
    }

    // Update is called once per frame
    void Update()
    {

    }
}
