using UnityEngine;
using UnityEditor;

public class MaterialReplacer : EditorWindow
{
    public Material oldMaterial;
    public Material newMaterial;

    [MenuItem("Tools/Material Replacer")]
    public static void ShowWindow()
    {
        GetWindow<MaterialReplacer>("Material Replacer");
    }

    void OnGUI()
    {
        GUILayout.Label("Replace Material In Scene", EditorStyles.boldLabel);

        oldMaterial = (Material)EditorGUILayout.ObjectField("Old Material:", oldMaterial, typeof(Material), false);
        newMaterial = (Material)EditorGUILayout.ObjectField("New Material:", newMaterial, typeof(Material), false);

        if (GUILayout.Button("Replace"))
        {
            int count = 0;
            foreach (Renderer r in FindObjectsOfType<Renderer>())
            {
                var mats = r.sharedMaterials;
                for (int i = 0; i < mats.Length; i++)
                {
                    if (mats[i] == oldMaterial)
                    {
                        mats[i] = newMaterial;
                        count++;
                    }
                }
                r.sharedMaterials = mats;
            }
            Debug.Log($"Replaced {count} materials in scene.");
        }
    }
}
