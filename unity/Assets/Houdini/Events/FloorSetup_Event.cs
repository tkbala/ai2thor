using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Linq;

/// <summary>
/// Test class for simple HDA events (build, cook, bake)
/// </summary>
public class FloorSetup_Event : MonoBehaviour
{

	public void ReloadCallback(HoudiniEngineUnity.HEU_HoudiniAsset asset, bool success, List<GameObject> outputList)
	{
		// When reloaded

        // Most straightforward our scripts should mostly go here over Cooked
        // But could be slow so we can disbale "Auto-Cook On Parameter Change" and move this to CookedCallback with it's quirks
        // outputList.ForEach( (gameObject) => {
        //             Debug.Log("Creating sim obj");
        //             // gameObject.GetComponent<MeshRenderer>();
        //             var c = gameObject.AddComponent<MeshCollider>();
        //             var simObj = gameObject.AddComponent<SimObjPhysics>();
        //             simObj.Type = SimObjType.Floor;
        //     });

        Debug.LogFormat("Reloaded! Asset={0}, Success={1}, Outputs={2}", asset.AssetName, success, outputList.Count);
	}

	public void CookedCallback(HoudiniEngineUnity.HEU_HoudiniAsset asset, bool success, List<GameObject> outputList)
	{
		// After Cook
        // Having this here is a little tricky, uncomment this block and disable the ReloadCallback callback.
        // if (success) {
        //     outputList.ForEach( (gameObject) => {
        //         // If you need this here you need to check if it already has a SimObjPhysics
        //         // if not it will add more every cook
        //         if (gameObject.GetComponent<SimObjPhysics>() == null) {
        //             Debug.Log("Creating sim obj");
        //             // gameObject.GetComponent<MeshRenderer>();
        //             var c = gameObject.AddComponent<MeshCollider>();
        //             var simObj = gameObject.AddComponent<SimObjPhysics>();
        //             simObj.Type = SimObjType.Floor;
        //         }
        //     });
        // }
        
        Debug.LogFormat("Cooked! Asset={0}, Success={1}, Outputs={2}", asset.AssetName, success, outputList.Count);
	}

	public void BakedCallback(HoudiniEngineUnity.HEU_HoudiniAsset asset, bool success, List<GameObject> outputList)
	{
         Debug.Log("Baking...");
		// After Bake Could be an option
        outputList.ForEach( (gameObject) => {
            // var go = GameObject.Find(gameObject.name);
            var simObj = gameObject.AddComponent<SimObjPhysics>();
            simObj.Type = SimObjType.Floor;
        });
        
        Debug.LogFormat("Baked! Asset={0}, Success={1}, Outputs={2}", asset.AssetName, success, outputList.Count);
	}
}
