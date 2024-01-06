using Oasis.Data;
using Oasis.Player.FirstPerson.Hud;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using World = Oasis.Mono.World;

namespace Oasis.Player.FirstPerson.Highlight
{
    public class Highlight : MonoBehaviour
    {
        private static Highlight _instance;
        public static Highlight Instance => _instance;

        private EntityManager _em;
        public int3 HighlightXYZ;
        public int3 BlueprintStartXYZ;

        private void Awake()
        {
            _instance = this;
        }
        private void Start()
        {
            _em = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager;
            World.Instance.OnVoxelChanged += (_, _) => MoveHighlight();
        }

        private void Update()
        {
            if (Time.frameCount % 10 == 0)
                MoveHighlight();
        }

        public void MoveHighlight()
        {
            if (Camera.main == null) return;
            var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                var hitVector3 = hit.point - hit.normal * 0.05f;
                var floor = new float3(Mathf.Floor(hitVector3.x), Mathf.Floor(hitVector3.y), Mathf.Floor(hitVector3.z));
                var offset = new float3(0.5f, 0.5f, 0.5f);
                HighlightXYZ = new int3(floor + offset);
                if (!transform.position.Equals(floor + offset))
                    transform.position = floor + offset;
            }
        }

        public void Place(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            if (Camera.main == null) return;
            var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;
            if (!Physics.Raycast(ray, out hit)) return;
            var voxelAdjacent = (hit.point + hit.normal * 0.05f).ToInt3();

            // Get Toolbar data
            var toolbarEntity = _em.CreateEntityQuery(typeof(ToolbarData)).GetSingletonEntity();
            var toolbarData = _em.GetComponentData<ToolbarData>(toolbarEntity);
            var toolbarBlockStateRefs = _em.GetBuffer<BlockStateRef>(toolbarEntity);
            
            // Debug.Log($"Toolbar selected Item is : {toolbarData.SelectedItem}");
            World.Instance.Place(voxelAdjacent, toolbarBlockStateRefs[ toolbarData.SelectedItem ]);
        }

        public void Remove(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            if (Camera.main == null) return;
            var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;
            if (!Physics.Raycast(ray, out hit)) return;
            var voxel = (hit.point - hit.normal * 0.05f).ToInt3();
            World.Instance.Remove(voxel);
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
}