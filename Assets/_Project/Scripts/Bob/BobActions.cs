// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.InputSystem;
// using Unity.Collections;
// using Unity.Entities;
// using Unity.Mathematics;
// using System.Linq;
// using UnityEditor.Rendering;
// using Oasis.Data;

// namespace Oasis.Bob
// {

//     public class BobActions : MonoBehaviour
//     {
//         private static BobActions _instance;
//         public static BobActions Instance { get { return _instance; } }

//         public PlayerInput playerInput;
//         public Transform godCameraContainer;
//         public int removeBlockTime = 1; // How long...
//         private float removeStartTime;
//         private bool hasLongPress;
//         private bool hasMoved;
//         private Vector2 removeScreenPoint;
//         private int3 removeXYZ;
//         private bool isRemoving;

//         private EntityManager _em;
//         private int3 blockToRemove;
//         private int removeCount;

//         void Awake()
//         {
//             _instance = this;
//         }

//         void Start()
//         {
//             _em = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager;
//         }

//         void Update()
//         {
//             if (isRemoving) UpdateRemove();
//         }


//         public void Place(InputAction.CallbackContext context)
//         {
//             if (!context.started) return;
//             if (Camera.main == null) return;
//             Place(new Vector2(Screen.width / 2, Screen.height / 2));
//         }
//         public void Place(Vector2 screenPoint)
//         {
//             var ray = Camera.main.ScreenPointToRay(screenPoint);
//             RaycastHit hit;

//             if (!Physics.Raycast(ray, out hit)) return;
//             var voxelAdjacent = hit.ToAdjacentVoxel();

//             // Get current block
//             var toolbarEntity = _em.CreateEntityQuery(typeof(ToolbarData)).GetSingletonEntity();
//             var toolbarData = _em.GetComponentData<ToolbarData>(toolbarEntity);
//             var toolbarBlockStateRefs = _em.GetBuffer<BlockStateRef>(toolbarEntity);
//             var block = _em.GetComponentData<Block>(toolbarBlockStateRefs[ toolbarData.SelectedItem ].Block);
//             var blockStates = _em.GetBuffer<BlockState>(toolbarBlockStateRefs[ toolbarData.SelectedItem ].Block);

//             var blockStateRef = toolbarBlockStateRefs[ toolbarData.SelectedItem ];

//             // Get blockStateRef to attach
//             bool attachable = false;
//             for (int i = 0; i < blockStates.Length; i++)
//                 for (int j = 0; j < blockStates[i].States.Length; j++)
//                     if (blockStates[i].States[j].Key == "attach") 
//                         attachable = true;

//             if (attachable)
//             {
//                 Debug.Log("Block is attachable.  (has attach blockStates)");
//                 var blockStateWithAttach = new State { Key = "attach", Value = hit.normal.ToSide()};
//                 int blockStateIndex = -1; // Default to -1 if not found
//                 for (int i = 0; i < blockStates.Length; i++)
//                     for (int j = 0; j < blockStates[i].States.Length; j++)
//                         if (blockStates[i].States[j].Equals(blockStateWithAttach))
//                             blockStateIndex = i;
//                 if (blockStateIndex == -1) Debug.LogError("Could not find blockState with attach key " + blockStateWithAttach.Value);

//                 blockStateRef = new BlockStateRef{
//                     Block = blockStateRef.Block,
//                     BlockStatesIndex = blockStateIndex
//                 };
//             }

//             Oasis.Mono.World.Instance.Place(voxelAdjacent, blockStateRef);
//         }


//         // RMB down fires Started and Performed, RMB up fires Cancelled
//         public void Remove(InputAction.CallbackContext context)
//         {
//             if (Camera.main == null) return;

//             if (context.started)
//                 RemoveBlockStart(new Vector2(Screen.width / 2, Screen.height / 2));
//             else if (context.canceled)
//                 RemoveBlockEnd(new Vector2(Screen.width / 2, Screen.height / 2));
//         }
//         public void RemoveBlockStart(Vector2 screenPoint)
//         {
//             Debug.Log("RemoveBlock start");
//             var ray = Camera.main.ScreenPointToRay(screenPoint);
//             RaycastHit hit;
//             if (!Physics.Raycast(ray, out hit)) return;

//             removeScreenPoint = screenPoint;
//             removeStartTime = Time.time;
//             removeXYZ = (hit.point - hit.normal * 0.05f).ToInt3();
//             isRemoving = true;
//         }
//         public void RemoveBlockEnd(Vector2 screenPoint)
//         {
//             Debug.Log("RemoveBlock canceled");
//             isRemoving = false;
//         }
//         public void UpdateRemove()
//         {
//             var ray = Camera.main.ScreenPointToRay(removeScreenPoint);
//             RaycastHit hit;

//             // if no voxel hit...reset removeStartTime and removeXYZ
//             if (!Physics.Raycast(ray, out hit))
//                 isRemoving = false;

//             // if voxel changes...reset removeStartTime and removeXYZ
//             if (!removeXYZ.Equals((hit.point - hit.normal * 0.05f).ToInt3()))
//             {
//                 removeStartTime = Time.time;
//                 removeXYZ = (hit.point - hit.normal * 0.05f).ToInt3();
//                 return;
//             }

//             // reset removeStartTime if target voxel changed
//             if (Time.time - removeStartTime > removeBlockTime)
//             {
//                 Debug.Log("RemoveBlock firing");
//                 // removeXYZ = (hit.point - hit.normal * 0.05f).ToInt3();
//                 Mono.World.Instance.Remove(removeXYZ);
//                 removeStartTime = Time.time;
//             }
//         }


//         public void God(InputAction.CallbackContext context)
//         {
//             if (context.performed)
//             {
//                 Debug.Log("Switching to God");
//                 playerInput.SwitchCurrentActionMap("God");
//                 Camera.main.transform.parent = godCameraContainer;
//                 Camera.main.transform.localPosition = Vector3.zero;
//                 Camera.main.transform.localRotation = Quaternion.identity;
//             }
//         }    

//     }
// }





//         // public void Blueprint(InputAction.CallbackContext context)
//         // {
//         //     if (!context.performed) return;
//         //     if (blueprintStartXyz.Equals(default))
//         //         blueprintStartXyz = highlightXyz;
//         //     else
//         //     {
//         //         var blueprint = ScriptableObject.CreateInstance<Blueprint>();
//         //         blueprint.name = "blueprint";
//         //         var startXyz = new int3(math.min(blueprintStartXyz.x, highlightXyz.x), math.min(blueprintStartXyz.y, highlightXyz.y), math.min(blueprintStartXyz.z, highlightXyz.z));
//         //         var endXyz = new int3(math.max(blueprintStartXyz.x, highlightXyz.x), math.max(blueprintStartXyz.y, highlightXyz.y), math.max(blueprintStartXyz.z, highlightXyz.z));
//         //         blueprint.dims = (endXyz - startXyz) + new int3(1,1,1);
//         //         var blueprintSize = (blueprint.dims.x) * (blueprint.dims.y) * (blueprint.dims.z);
//         //         blueprint.blocks = new Block[blueprintSize];
                
//         //         for (var z = 0; z < blueprint.dims.z; z++)
//         //         for (var y = 0; y < blueprint.dims.y; y++)
//         //         for (var x = 0; x < blueprint.dims.x; x++)
//         //         {
//         //             var xyz = new int3(x, y, z);
//         //             var index = xyz.ToIndex(blueprint.dims);
//         //             var worldIndex = (startXyz + xyz).ToIndex(World.Instance.dims);
//         //             var blockIndex = World.Instance.voxels[worldIndex];
//         //             // blueprint.blocks[index] = Blocks.Instance.blocks[blockIndex];  // TODO will need to update for rotation/palette
//         //         }
//         //         blueprintStartXyz = default;
                
//         //         var datetime = System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
//         //         var path = $"Assets/Blueprints/blueprint-{datetime}.asset";
//         //         UnityEditor.AssetDatabase.CreateAsset(blueprint, path);
//         //         Debug.Log($"Created blueprint at {path}");
//         //     }
//         // }




