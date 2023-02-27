using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;

public class TagTransformExpoter
{
    //指定されたタグのオブジェクトをcsvに出力する
    public static void SceneTagTransformToFile(string fileName, List<string> tagName)
    {
        //transformの値を入れる
        TransformCsvExe tag = CreateTagSetTransform(tagName);

        //if (tag.IsEmpty) return;

        //オブジェクトデータの書き込み
        var tagFileName = Path.ChangeExtension(fileName, "csv");
        try
        {
            using (StreamWriter sw = new StreamWriter(tagFileName))
            {
                tag.WriteObject(sw);
            }
        }
        catch(IOException error)
        {
            Debug.LogError("File is open and cannot be saved.\n"+ error.Message);
        }

    }
    public static TransformCsvExe CreateTagSetTransform(List<string> tag)
    {
        //エネミーを作成
        TransformCsvExe tagTransform = new TransformCsvExe();

        //シーン中のゲームオブジェクトをイテレーション
        foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
        {
            if (obj.activeInHierarchy && obj.activeSelf)
            {
                if (tagCheack(obj, tag)) tagTransform.Add(obj);

            }
        }
        return tagTransform;
    }
    //指定されたタグを同じか調べる
    public static bool tagCheack(GameObject obj, List<string> tag)
    {
        for (int i = 0; i < tag.Count; i++)
        {
            if (obj.tag == tag[i])
            {
                return true;
            }
        }
        return false;
    }

}
