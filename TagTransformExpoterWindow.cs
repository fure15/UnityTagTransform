using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;

#if UNITY_EDITOR
public class TagTransformExpoterWindow : EditorWindow
{

    private const int MaxTag = 10;
    private const int MinTag = 1;
    private const int AddSize = 32;
    private List<string> tag = new List<string>() { "None" };
    private int sizeX = 256;
    private int sizeY = 120;
    private int selectMenu = 0;

    [UnityEditor.MenuItem("Tools/TransformExpoter")]//メニューにコマンド追加と名前指定
    public static void ShowWindow()
    {
        EditorWindow window = EditorWindow.GetWindow(typeof(TagTransformExpoterWindow));
        window.titleContent = new GUIContent("TransformExpoter");//表示の名前
        window.minSize = new Vector2(256, 120);
        window.maxSize = new Vector2(256, 120);
        window.Show();
    }

    // Update is called once per frame
    void OnGUI()
    {
        EditorGUILayout.LabelField("シーン中の敵をcsvファイルで保存");

        TagShow();

        if (GUILayout.Button("AddTag"))
        {
            if (tag.Count() - 1 < MaxTag)
            {
                Add();
            }
        }

        if (GUILayout.Button("Decrease"))
        {
            if (tag.Count() - 1 > MinTag)
            {
                Decrease();
            }
        }

        //指定Tagの全てのGameobjを取得
        if (GUILayout.Button("Export"))
        {
            string fileName = EditorUtility.SaveFilePanel("Export .csv file", "", "", "csv");
            if (!string.IsNullOrEmpty(fileName))
            {
                TagTransformExpoter.SceneTagTransformToFile(fileName, tag);
            }
        }

    }

    void TagShow()
    {
        GenericMenu menu = new GenericMenu();
        for (int i = 0; i < tag.Count() - 1; i++)
        {
            using (GUILayout.ScrollViewScope scroll = new GUILayout.ScrollViewScope(new Vector2(0, 0), EditorStyles.helpBox, GUILayout.Height(30)))
            {
                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.Label("タグ名");

                    tag[i] = EditorGUILayout.TagField(tag[i]);
                    if (GUILayout.Button("▼"))
                    {
                        //add後場所を変える
                        selectMenu = i;
                        if (tag.Count() - 1 < MaxTag)
                        {
                            menu.AddItem(new GUIContent("AddTag"), false, () => SelectAdd());
                        }
                        if (tag.Count() - 1 > MinTag)
                            menu.AddItem(new GUIContent("DecreaseTag"), false, () => { SelectDecrease(); });
                    }
                }
            }
        }
        if (menu.GetItemCount() > 0)
        {
            menu.ShowAsContext();
            Event.current.Use();
        }
    }
    void SelectAdd()
    {
        sizeY += AddSize;
        EditorWindow window = EditorWindow.GetWindow(typeof(TagTransformExpoterWindow));
        window.minSize = new Vector2(sizeX, sizeY);
        window.maxSize = new Vector2(sizeX, sizeY);
        tag.Insert(selectMenu + 1, "None");
    }

    void Add()
    {
        if (tag.Count() <= selectMenu)
        {
            selectMenu++;
        }
        sizeY += AddSize;
        EditorWindow window = EditorWindow.GetWindow(typeof(TagTransformExpoterWindow));
        window.minSize = new Vector2(sizeX, sizeY);
        window.maxSize = new Vector2(sizeX, sizeY);
        tag.Add("None");
    }
    void SelectDecrease()
    {
        sizeY -= AddSize;
        EditorWindow window = EditorWindow.GetWindow(typeof(TagTransformExpoterWindow));
        window.minSize = new Vector2(sizeX, sizeY);
        window.maxSize = new Vector2(sizeX, sizeY);
        tag.RemoveAt(selectMenu);
    }

    void Decrease()
    {
        if (tag.Count() - 1 <= selectMenu)
        {
            selectMenu--;
        }
        sizeY -= AddSize;
        EditorWindow window = EditorWindow.GetWindow(typeof(TagTransformExpoterWindow));
        window.minSize = new Vector2(sizeX, sizeY);
        window.maxSize = new Vector2(sizeX, sizeY);
        tag.RemoveAt(tag.Count() - 1);
    }
}

#endif