using UnityEditor;
using UnityEngine;

public static class ConvertToStandaloneAssets
{
    [MenuItem("GameObject/Convert to Standalone Assets %h")]
    private static void ChangeAssets()
    {
        if (!Selection.activeTransform) return;

        Material currentMaterial;
        Texture currentTexture;
        string currentPath;
        foreach (var transform in Selection.transforms)
        {
            //Unpack prefabs
            if (PrefabUtility.GetPrefabInstanceHandle(transform.gameObject) != null)
            {
                PrefabUtility.UnpackPrefabInstance(transform.gameObject, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
            }

            //Delete children
            for (int i = 0; i <= 5; i++)
            {
                if (transform.childCount > 0)
                {
                    foreach (Transform child in transform)
                    {
                        GameObject.DestroyImmediate(child.gameObject);
                    }
                }
            }

            //Delete non-mesh filter, non-mesh renderer components
            Component[] componentList = transform.gameObject.GetComponents<Component>() as Component[];

            foreach (var component in transform.gameObject.GetComponents<Component>())
            {

                if (component is Transform || component is MeshFilter || component is MeshRenderer)
                {
                }

                else
                {
                    Component.DestroyImmediate(component);
                }

            }
            
            //Fix materials and textures
            currentMaterial = transform.gameObject.GetComponent<MeshRenderer>().sharedMaterials[0];
            currentPath = AssetDatabase.GetAssetPath(currentMaterial);
            AssetDatabase.RenameAsset(currentPath, transform.gameObject.name + "_Mat");
            currentPath = AssetDatabase.GetAssetPath(currentMaterial);
            AssetDatabase.MoveAsset(currentPath, "Assets/Unity_Export_Post/" + currentMaterial.name + ".mat");

            currentTexture = currentMaterial.GetTexture("_MainTex");
            currentPath = AssetDatabase.GetAssetPath(currentTexture);
            AssetDatabase.RenameAsset(currentPath, transform.gameObject.name + "_AlbedoTransparency");
            currentPath = AssetDatabase.GetAssetPath(currentTexture);
            AssetDatabase.MoveAsset(currentPath, "Assets/Unity_Export_Post/" + currentTexture.name + ".png");

            currentTexture = currentMaterial.GetTexture("_BumpMap");
            currentPath = AssetDatabase.GetAssetPath(currentTexture);
            AssetDatabase.RenameAsset(currentPath, transform.gameObject.name + "_Normal");
            currentPath = AssetDatabase.GetAssetPath(currentTexture);
            AssetDatabase.MoveAsset(currentPath, "Assets/Unity_Export_Post/" + currentTexture.name + ".png");

            currentTexture = currentMaterial.GetTexture("_SpecGlossMap");
            currentPath = AssetDatabase.GetAssetPath(currentTexture);
            AssetDatabase.RenameAsset(currentPath, transform.gameObject.name + "_MetallicSmoothness");
            currentPath = AssetDatabase.GetAssetPath(currentTexture);
            AssetDatabase.MoveAsset(currentPath, "Assets/Unity_Export_Post/" + currentTexture.name + ".png");

            currentMaterial.shader = Shader.Find("Standard");
            currentMaterial.SetTexture("_MetallicGlossMap", currentTexture);

        }

        AssetDatabase.SaveAssets();

    }
}