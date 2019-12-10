using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GetAllScenesInstanceCount : MonoBehaviour
{
    [MenuItem("GameObject/Get All Scene Instances %j")]
    private static void GetInstanceCount()
    {
        int currentInstanceCount = 0;
        int totalInstanceCount = 0;

        for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings; i++)
        {
            UnityEditor.SceneManagement.EditorSceneManager.OpenScene(SceneUtility.GetScenePathByBuildIndex(i), OpenSceneMode.Single);

            //yield return new WaitForSeconds(0.5f);

            currentInstanceCount = GameObject.Find("Objects").transform.childCount;
            totalInstanceCount = totalInstanceCount + currentInstanceCount;

            print(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + " has " + currentInstanceCount + " objects.");
        }
        print("The total number of objects is " + totalInstanceCount);
    }
}
//UnityEditor.SceneManagement.EditorSceneManager.GetSceneByBuildIndex(i).path