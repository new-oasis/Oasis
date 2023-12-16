using System;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Highlight : MonoBehaviour
{
    
    private static Highlight _instance;
    public static Highlight Instance => _instance;
    
    public int3 highlightXyz;
    public int3 blueprintStartXyz;

    private void Awake()
    {
        _instance = this;
    }
    private void Start()
    {
        // World.Instance.OnVoxelChanged += (_, _) => MoveHighlight();
    }

    private void Update()
    {
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    public void MoveHighlight()
    {
        var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var hitVector3 = hit.point - hit.normal * 0.05f;
            var floor = new float3(Mathf.Floor(hitVector3.x), Mathf.Floor(hitVector3.y), Mathf.Floor(hitVector3.z));
            var offset = new float3(0.5f, 0.5f, 0.5f);
            highlightXyz = new int3(floor + offset);
            if (!transform.position.Equals(floor + offset))
                transform.position = floor + offset;
        }
    }

    public void Place(InputAction.CallbackContext context)
    {
//         if (!context.started) return;
//         var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
//         RaycastHit hit;
//         if (!Physics.Raycast(ray, out hit)) return;
//         var VoxelAdjacent = (hit.point + hit.normal * 0.05f).ToInt3();
//         
//         // Adjust facing
//         var currentBlockHasFacingState = Toolbar.Instance.currentBlockState.States.FirstOrDefault(s => s.Key == "facing");
//         if (currentBlockHasFacingState != null)
//         {
//             Debug.Log("Facing " + Character.Instance.Facing());
//             
//             // Clone the current block state
//             var modifiedBlockState = new BlockState
//             {
//                 Block = Toolbar.Instance.currentBlockState.Block,
//                 States = Toolbar.Instance.currentBlockState.States.ToArray(), // Create a copy of the states array
//             };
//
// // Update facing in the clone
//             var facingState = modifiedBlockState.States.FirstOrDefault(s => s.Key == "facing");
//             if (facingState != null)
//             {
//                 facingState.Value = Character.Instance.Facing();
//             }
//
// // Find the index of the modified block state
//             int modifiedBlockStateIndex = World.Instance.BlockStates.FindIndex(b =>
//                 b.Block == modifiedBlockState.Block &&
//                 b.States.SequenceEqual(modifiedBlockState.States));
//
//             Debug.Log("modifiedBlockStateIndex: " + modifiedBlockStateIndex);
//
//             
//             
//             
//             
//             
//             Debug.Log("actualBlockState: " + actualBlockState);
//             Toolbar.Instance.currentBlockState = actualBlockState;
//         }
//         
//         var blockStateIndex = (ushort) World.Instance.BlockStates.FindIndex(b => b.Equals(Toolbar.Instance.currentBlockState));
//         World.Instance.Place(VoxelAdjacent.ToIndex(World.Instance.dims), blockStateIndex);
//         Debug.Log($"Placing {blockStateIndex}");
//         Debug.Log($"Placing {Toolbar.Instance.currentBlockState}");
    }

    public void Remove(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit)) return;
        var Voxel = (hit.point - hit.normal * 0.05f).ToInt3();
        // World.Instance.Remove(Voxel.ToIndex(World.Instance.dims));
    }

    public void Blueprint(InputAction.CallbackContext context)
    {
        // if (!context.performed) return;
        // if (blueprintStartXyz.Equals(default))
        //     blueprintStartXyz = highlightXyz;
        // else
        // {
        //     var blueprint = ScriptableObject.CreateInstance<Blueprint>();
        //     blueprint.name = "blueprint";
        //     var startXyz = new int3(math.min(blueprintStartXyz.x, highlightXyz.x), math.min(blueprintStartXyz.y, highlightXyz.y), math.min(blueprintStartXyz.z, highlightXyz.z));
        //     var endXyz = new int3(math.max(blueprintStartXyz.x, highlightXyz.x), math.max(blueprintStartXyz.y, highlightXyz.y), math.max(blueprintStartXyz.z, highlightXyz.z));
        //     blueprint.dims = (endXyz - startXyz) + new int3(1,1,1);
        //     var blueprintSize = (blueprint.dims.x) * (blueprint.dims.y) * (blueprint.dims.z);
        //     blueprint.blocks = new Block[blueprintSize];
        //     
        //     for (var z = 0; z < blueprint.dims.z; z++)
        //     for (var y = 0; y < blueprint.dims.y; y++)
        //     for (var x = 0; x < blueprint.dims.x; x++)
        //     {
        //         var xyz = new int3(x, y, z);
        //         var index = xyz.ToIndex(blueprint.dims);
        //         var worldIndex = (startXyz + xyz).ToIndex(World.Instance.dims);
        //         var blockIndex = World.Instance.voxels[worldIndex];
        //         // blueprint.blocks[index] = Blocks.Instance.blocks[blockIndex];  // TODO will need to update for rotation/palette
        //     }
        //     blueprintStartXyz = default;
        //     
        //     var datetime = System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
        //     var path = $"Assets/Blueprints/blueprint-{datetime}.asset";
        //     UnityEditor.AssetDatabase.CreateAsset(blueprint, path);
        //     Debug.Log($"Created blueprint at {path}");
        // }
    }

}