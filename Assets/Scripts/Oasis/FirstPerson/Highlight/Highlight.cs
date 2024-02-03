using Oasis.Common;
using Oasis.Data;
using Oasis.FirstPerson;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using World = Oasis.Mono.World;
using UnityEngine.iOS;
using System.Collections;

namespace Oasis.FirstPerson
{
    public class Highlight : MonoBehaviour
    {
        private static Highlight _instance;
        public static Highlight Instance => _instance;

        private EntityManager _em;
        public int3 HighlightXYZ;
        public Mesh cubeMesh;

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
            // Test if the mouse is moving
            if (Mouse.current != null && (Mouse.current.delta.ReadValue().x != 0 || Mouse.current.delta.ReadValue().x != 0))
                MoveHighlight();

            // if (Mouse.current != null && Time.frameCount % 10 == 0)
                // MoveHighlight();
        }

        public void MoveHighlight()
        {
            if (Camera.main == null) return;
            if (Mouse.current == null) return;
            var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                var voxel = hit.ToVoxel();
                var offset = new float3(0.5f, 0.5f, 0.5f);
                if (!transform.position.Equals(voxel + offset))
                    transform.position = voxel + offset;
            }
        }


    }
}
