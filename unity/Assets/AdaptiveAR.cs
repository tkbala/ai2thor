using Boo.Lang;
using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

public static class AdaptiveAR {
    static Scene scene;
    static List<string> prefabPathsList = new List<string>();
    static string newScenePath;
    static string newSceneName;
    [MenuItem("Kubblammo/Do Data Hacks")]
    static void Do() {
        var changes = 0;
        GameObject arUserView = null;
        var originalScene = EditorSceneManager.GetActiveScene().path;





        List<string> objectsToDelete = new List<string> { "PhysicsSceneManager", "DebugCanvasPhysics" };

        Action<GameObject> removeComponents = null;
        removeComponents = (obj) => {
            var tfm = obj.transform;
            EditorUtility.SetDirty(obj);

            //Remove Components
            RemoveComponents(obj);

            GameObject prefab = PrefabUtility.GetCorrespondingObjectFromOriginalSource(obj);
            
            if (prefab == null) {

                


            } else {

                string prefabPath = AssetDatabase.GetAssetPath(prefab);
                if (!prefabPathsList.Contains(prefabPath) && !prefabPath.Contains(".fbx")) {
                    prefabPathsList.Add(prefabPath);

                }


            }


            if (obj.name == "Ceiling") {
                UnityEngine.Object.DestroyImmediate(obj);
            } else {
                for (var i = tfm.childCount - 1; i != -1; --i)
                    removeComponents(tfm.GetChild(i).gameObject);
            }

        };

        foreach (var sceneGUID in AssetDatabase.FindAssets("t:Scene", new string[] { "Assets/Scenes" })) {
            var scenePath = AssetDatabase.GUIDToAssetPath(sceneGUID);

            if (!scenePath.Contains("Miniworld") && scenePath.Contains("FloorPlan")) {
                Debug.Log(scenePath);
                EditorSceneManager.OpenScene(scenePath);
                scene = EditorSceneManager.GetActiveScene();


                //Unparent "FirstPersonCharacter"
                GameObject[] goArray = scene.GetRootGameObjects();
                for (int i = 0; i < goArray.Length; i++) {
                    GameObject obj = goArray[i];
                    EditorUtility.SetDirty(obj);
                    if (obj.name == "FPSController") {

                        arUserView = UnityEngine.Object.Instantiate(obj.transform.Find("FirstPersonCharacter").gameObject);
                        EditorUtility.SetDirty(arUserView);
                        arUserView.name = "AR User View";
                        CharacterController oldcl = obj.GetComponent<CharacterController>();
                        CharacterController newcl = arUserView.AddComponent<CharacterController>();
                        newcl.slopeLimit = oldcl.slopeLimit;
                        newcl.stepOffset = oldcl.stepOffset;
                        newcl.skinWidth = oldcl.skinWidth;
                        newcl.minMoveDistance = oldcl.minMoveDistance;
                        newcl.center = oldcl.center;
                        newcl.radius = oldcl.radius;
                        newcl.height = oldcl.height;
                        arUserView.transform.SetParent(obj.transform.parent);
                        UnityEngine.Object.DestroyImmediate(obj);
                        break;
                    }
                }

                //Delete irrelavnt objects
                goArray = scene.GetRootGameObjects();
                for (int i = 0; i < goArray.Length; i++) {
                    GameObject obj = goArray[i];

                    if (objectsToDelete.Contains(obj.name)) {

                        UnityEngine.Object.DestroyImmediate(obj);

                    }
                }

                //Update array and //Remove components
                goArray = scene.GetRootGameObjects();
                for (int i = 0; i < goArray.Length; i++) {
                    GameObject obj = goArray[i];

                    removeComponents(obj);

                }


                //Organize substructre
                GameObject simEnv = new GameObject();
                simEnv.name = "Simulated Environment";
                for (int i = 0; i < goArray.Length; i++) {
                    GameObject obj = goArray[i];
                    obj.transform.parent = simEnv.transform;
                }
                arUserView.transform.parent = null;
                simEnv.transform.parent = null;


                GameObject miniWorldEnv = new GameObject();
                miniWorldEnv.name = "Miniworld_" + scene.name;
                goArray = scene.GetRootGameObjects();
                for (int i = 0; i < goArray.Length; i++) {
                    GameObject obj = goArray[i];
                    obj.transform.parent = miniWorldEnv.transform;
                }

                ClearChildren(arUserView.transform);
                newScenePath = "Assets/Miniworlds/";
                newSceneName = "Miniworld_" + scene.name + ".unity";


                //CLose and open the new scene scene
                EditorSceneManager.SaveScene(scene, newScenePath + newSceneName, true);
                EditorSceneManager.CloseScene(scene, true);


            }
        }

        foreach (string prefabPath in prefabPathsList) {



            Debug.Log("Start - modfying prefab - " + prefabPath);
            // Load the contents of the Prefab Asset.
            GameObject contentsRoot = PrefabUtility.LoadPrefabContents(prefabPath);

            // Modify Prefab contents.
            RemoveComponents(contentsRoot);

            // Save contents back to Prefab Asset and unload contents.
            PrefabUtility.SaveAsPrefabAsset(contentsRoot, prefabPath);
            PrefabUtility.UnloadPrefabContents(contentsRoot);

            Debug.Log("End - modfying prefab - " + prefabPath);
        }

        EditorSceneManager.OpenScene(newScenePath + newSceneName);
        Debug.Log(String.Format("changes made"));
    }
    static void RemoveComponent<T>(GameObject go) {
        T tComponent = go.GetComponent<T>();
        if (tComponent != null) {
            UnityEngine.Object.DestroyImmediate(tComponent as UnityEngine.Object, false);
        }
    }

    static void RemoveComponents(GameObject obj) {
        RemoveComponent<SimObjPhysics>(obj);
        RemoveComponent<SimObj>(obj);
        RemoveComponent<Rigidbody>(obj);
        
        RemoveComponent<SliceObject>(obj);
        
        
        
        
        RemoveComponent<UsedUp>(obj);
        RemoveComponent<PhysicsRemoteFPSAgentController>(obj);

        RemoveComponent<Bathtub>(obj);
        RemoveComponent<Bed>(obj);
        RemoveComponent<Blinds>(obj);
        RemoveComponent<Box>(obj);
        RemoveComponent<Break>(obj);
        RemoveComponent<Breakdown>(obj);
        RemoveComponent<Cabinet>(obj);
        RemoveComponent<CanOpen_Object>(obj);
        RemoveComponent<CanToggleOnOff>(obj);
        RemoveComponent<coffeemachine>(obj);
        RemoveComponent<ColdZone>(obj);
        RemoveComponent<CollisionListener>(obj);
        RemoveComponent<childcollider>(obj);
        RemoveComponent<Contains>(obj);
        RemoveComponent<ContinuousMovement>(obj);
        RemoveComponent<CookObject>(obj);
        RemoveComponent<Contains>(obj);
        RemoveComponent<DecalCollision>(obj);
        RemoveComponent<DeferredDecal>(obj);
        RemoveComponent<Dirty>(obj);
        RemoveComponent<Door>(obj);
        RemoveComponent<DroneBasket>(obj);
        RemoveComponent<DroneObjectLauncher>(obj);
        RemoveComponent<Flame>(obj);
        RemoveComponent<Fill>(obj);
        RemoveComponent<FrameCollider>(obj);
        RemoveComponent<FreezeObject>(obj);
        RemoveComponent<Fridge>(obj);
        RemoveComponent<GenerateRoboTHOR>(obj);

        //RemoveComponent<>(obj);

    }
    static void ClearChildren(Transform transform) {
        //Debug.Log(transform.childCount);
        int i = 0;

        //Array to hold all child obj
        GameObject[] allChildren = new GameObject[transform.childCount];

        //Find all child obj and store to that array
        foreach (Transform child in transform) {
            allChildren[i] = child.gameObject;
            i += 1;
        }

        //Now destroy them
        foreach (GameObject child in allChildren) {
            UnityEngine.Object.DestroyImmediate(child.gameObject);
        }


    }
}