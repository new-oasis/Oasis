using Unity.Entities;
using Unity.Entities.UniversalDelegates;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using World = Oasis.Mono.World;

namespace Oasis
{
    public class Crosshair : MonoBehaviour
    {
        public Target target;
        public int range = 8;

        private EntityManager _em;

        private void Start()
        {
            _em = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager;
            // World.Instance.OnVoxelChanged += (_, _) => MoveHighlight();
        }

        private void Update()
        {
            // Test if the mouse is moving
            // if (Mouse.current != null && (Mouse.current.delta.ReadValue().x != 0 || Mouse.current.delta.ReadValue().x != 0))
                // UpdateTarget();

            if (Mouse.current != null && Time.frameCount % 10 == 0)
                UpdateTarget();
        }

        public void UpdateTarget()
        {
            if (Camera.main == null) return;
            if (Mouse.current == null) return;
            var ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, range))
            {
                if (hit.ToVoxel().Equals(target.Value)) return; 
                target.Value = hit.ToVoxel();
                target.InvokeChangedEvent();
            }
        }


    }
}
