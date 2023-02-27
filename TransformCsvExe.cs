using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TransformCsvExe
{
    List<string> name = new List<string>();
    List<Vector3> position = new List<Vector3>();
    List<Vector3> rotation = new List<Vector3>();
    List<Vector3> scale = new List<Vector3>();

    List<AddElement> elements = new List<AddElement>();

    public void Add(GameObject gameObject)
    {
        name.Add(nameRetouch(gameObject.name));
        position.Add(gameObject.transform.position);
        rotation.Add(gameObject.transform.rotation.eulerAngles);
        scale.Add(gameObject.transform.localScale);

        if (gameObject.GetComponent<AddElement>())
        {
            elements.Add(gameObject.GetComponent<AddElement>());
        }
        else
        {
            elements.Add(new AddElement());
        }

    }
    public StreamWriter WriteObject(StreamWriter writer)
    {
        writer.WriteLine("Name,posX,posY,posZ,rotateX,rotateY,rotateZ,scaleX,scaleY,scaleZ,ParamsCount,ParamCount,Param");

        if (IsEmpty)
        {
            Debug.Log("Tag is not attached.");
            return writer;
        }

        for (int i = 0; i < name.Count; i++)
        {
            int elementSize = elements[i].parameters.Count;

            string str = $"{name[i]}," +
                $"{position[i].x},{position[i].y},{position[i].z}," +
                $"{rotation[i].x},{rotation[i].y},{rotation[i].z}," +
                $"{scale[i].x},{scale[i].y},{scale[i].z}," +
                $"{elementSize}";

            str = ElementValue(str, elements[i], elementSize);

            writer.WriteLine(string.Format(
              str
                  ));
        }
        return writer;

    }

    string ElementValue(string str, AddElement element, int elementSize)
    {
        //Paramの数だけ追加の設定値を入れる
        for (int i = 0; i < elementSize; i++)
        {
            // 追加設定値の一つのクラスに入れる数を教える
            int paramCount = element.parameters[i].parameter.Count;

            str += $",{paramCount}";
            //設定値を入れる
            for (int j = 0; j < paramCount; j++)
            {
                str += $",{element.parameters[i].parameter[j]}";
            }
        }
        return str;
    }

    //複数作った場合に出てくる数字を無くす
    public string NameNoberOut(string name)
    {
        return name;
    }
    //名前の後ろについている()を消す
    public string nameRetouch(string name)
    {
        string[] split_name = name.Split('(');
        int split_decrease = 1;
        if (split_name.Length == 1)
        {
            split_decrease = 0;
        }
        //最後に(がないか調べる
        if (split_decrease <= 0) return name;
        //あった場合最後の(の後に文字があるか調べる
        if (!splitInNumber(split_name[split_name.Length - 1]))
        {//無い場合
            split_decrease = 0;
            //一番最後に空白があった場合無くす
            if (Is_LastMoji(split_name[split_name.Length - 1], ' '))
                split_name[split_name.Length - 1] = LastDelete(split_name[split_name.Length - 1]);
            name = Join(split_name, '(', split_name.Length);
        }
        else
        {//(1)があった
            if (Is_LastMoji(split_name[split_name.Length - 2], ' '))
                split_name[split_name.Length - 2] = LastDelete(split_name[split_name.Length - 2]);
            //切り離した( を入れる
            name = Join(split_name, '(', split_name.Length - 1);
        }

        return name;
    }
    //最後の(の後にある文字を調べる
    bool splitInNumber(string name)
    {
        bool NumberOnly = true;
        for (int i = 0; i < name.Length; i++)
        {
            if (!IsNumber(name[i])) NumberOnly = false;

        }

        if (NumberOnly) return true;
        return false;
    }
    //それは数字かまたは)か
    bool IsNumber(char name)
    {
        if (name == ')') return true;
        if (name < '0') return false;
        if (name > '9') return false;
        return true;
    }
    //切り離した文字に指定した文字を結合する
    string Join(string[] name, char pulas, int time)
    {
        string new_name = name[0];
        //nameの(を置く
        for (int i = 1; i < time; i++)
        {
            new_name += pulas + name[i];
        }
        return new_name;
    }
    //最後の文字だけ消す
    string LastDelete(string name)
    {
        return name.Remove(name.Length - 1);
    }
    //指定の文字が最後にあるか
    bool Is_LastMoji(string name, char moji)
    {
        if (name[name.Length - 1] == moji)
        {
            return true;
        }
        return false;
    }

    public bool IsEmpty => name.Count == 0;

}
