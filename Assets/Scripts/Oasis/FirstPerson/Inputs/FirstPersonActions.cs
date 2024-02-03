using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace Oasis.FirstPerson
{

    public class FirstPersonActions : MonoBehaviour
    {
        private static FirstPersonActions _instance;
        public static FirstPersonActions Instance { get { return _instance; } }

        public int removeBlockTime = 1; // How long...
        private float removeStartTime;
        private bool hasLongPress;
        private bool hasMoved;
        private Vector2 removeScreenPoint;
        private int3 removeXYZ;
        private bool isRemoving;

        private EntityManager _em;
        private int3 blockToRemove;
        private int removeCount;

        void Awake()
        {
            _instance = this;
        }

        void Start()
        {
            _em = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager;
        }

        void Update()
        {
            if (isRemoving) UpdateRemove();
        }


        public void Place(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            if (Camera.main == null) return;
            Place(new Vector2(Screen.width / 2, Screen.height / 2));
        }
        public void Place(Vector2 screenPoint)
        {
            var ray = Camera.main.ScreenPointToRay(screenPoint);
            RaycastHit hit;

            if (!Physics.Raycast(ray, out hit)) return;
            var voxelAdjacent = hit.ToAdjacentVoxel();

            // Get Toolbar data
            var toolbarEntity = _em.CreateEntityQuery(typeof(ToolbarData)).GetSingletonEntity();
            var toolbarData = _em.GetComponentData<ToolbarData>(toolbarEntity);
            var toolbarBlockStateRefs = _em.GetBuffer<Data.BlockStateRef>(toolbarEntity);
            
            // Debug.Log($"Toolbar selected Item is : {toolbarData.SelectedItem}");
            Oasis.Mono.World.Instance.Place(voxelAdjacent, toolbarBlockStateRefs[ toolbarData.SelectedItem ]);
        }


        // RMB down fires Started and Performed, RMB up fires Cancelled
        public void Remove(InputAction.CallbackContext context)
        {
            if (Camera.main == null) return;

            if (context.started)
                RemoveBlockStart(new Vector2(Screen.width / 2, Screen.height / 2));
            else if (context.canceled)
                RemoveBlockEnd(new Vector2(Screen.width / 2, Screen.height / 2));
        }
        public void RemoveBlockStart(Vector2 screenPoint)
        {
            Debug.Log("RemoveBlock start");
            var ray = Camera.main.ScreenPointToRay(screenPoint);
            RaycastHit hit;
            if (!Physics.Raycast(ray, out hit)) return;

            removeScreenPoint = screenPoint;
            removeStartTime = Time.time;
            removeXYZ = (hit.point - hit.normal * 0.05f).ToInt3();
            isRemoving = true;
        }

        public void RemoveBlockEnd(Vector2 screenPoint)
        {
            Debug.Log("RemoveBlock canceled");
            isRemoving = false;
        }

        public void UpdateRemove()
        {
            var ray = Camera.main.ScreenPointToRay(removeScreenPoint);
            RaycastHit hit;

            // if no voxel hit...reset removeStartTime and removeXYZ
            if (!Physics.Raycast(ray, out hit))
                isRemoving = false;

            // if voxel changes...reset removeStartTime and removeXYZ
            if (!removeXYZ.Equals((hit.point - hit.normal * 0.05f).ToInt3()))
            {
                removeStartTime = Time.time;
                removeXYZ = (hit.point - hit.normal * 0.05f).ToInt3();
                return;
            }

            // reset removeStartTime if target voxel changed
            if (Time.time - removeStartTime > removeBlockTime)
            {
                Debug.Log("RemoveBlock firing");
                // removeXYZ = (hit.point - hit.normal * 0.05f).ToInt3();
                Mono.World.Instance.Remove(removeXYZ);
                removeStartTime = Time.time;
            }
        }

    }
}





        // public void Blueprint(InputAction.CallbackContext context)
        // {
        //     if (!context.performed) return;
        //     if (blueprintStartXyz.Equals(default))
        //         blueprintStartXyz = highlightXyz;
        //     else
        //     {
        //         var blueprint = ScriptableObject.CreateInstance<Blueprint>();
        //         blueprint.name = "blueprint";
        //         var startXyz = new int3(math.min(blueprintStartXyz.x, highlightXyz.x), math.min(blueprintStartXyz.y, highlightXyz.y), math.min(blueprintStartXyz.z, highlightXyz.z));
        //         var endXyz = new int3(math.max(blueprintStartXyz.x, highlightXyz.x), math.max(blueprintStartXyz.y, highlightXyz.y), math.max(blueprintStartXyz.z, highlightXyz.z));
        //         blueprint.dims = (endXyz - startXyz) + new int3(1,1,1);
        //         var blueprintSize = (blueprint.dims.x) * (blueprint.dims.y) * (blueprint.dims.z);
        //         blueprint.blocks = new Block[blueprintSize];
                
        //         for (var z = 0; z < blueprint.dims.z; z++)
        //         for (var y = 0; y < blueprint.dims.y; y++)
        //         for (var x = 0; x < blueprint.dims.x; x++)
        //         {
        //             var xyz = new int3(x, y, z);
        //             var index = xyz.ToIndex(blueprint.dims);
        //             var worldIndex = (startXyz + xyz).ToIndex(World.Instance.dims);
        //             var blockIndex = World.Instance.voxels[worldIndex];
        //             // blueprint.blocks[index] = Blocks.Instance.blocks[blockIndex];  // TODO will need to update for rotation/palette
        //         }
        //         blueprintStartXyz = default;
                
        //         var datetime = System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
        //         var path = $"Assets/Blueprints/blueprint-{datetime}.asset";
        //         UnityEditor.AssetDatabase.CreateAsset(blueprint, path);
        //         Debug.Log($"Created blueprint at {path}");
        //     }
        // }




