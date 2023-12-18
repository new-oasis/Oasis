using Oasis.ECS.BlockStates;
using Oasis.ECS.Models;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UIElements;

namespace Oasis.Game.Player.FirstPerson.Hud
{
    public class Toolbar : MonoBehaviour
    {
        private static Toolbar _instance;
        public static Toolbar Instance => _instance;
    
        public Entity ToolbarEntity;
    
        private UQueryBuilder<VisualElement> slots;
        private VisualElement toolbar;
        private VisualElement root;

        public GameObject ToolbarItemGO;
        public GameObject ToolbarItemPrefab;

        private EntityManager em;
    
        private void Start()
        {
            _instance = this;
            em = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager;
            var worldEntity = em.CreateEntityQuery(typeof(Oasis.ECS.World.World)).GetSingletonEntity();
            var worldBlockStates = em.GetBuffer<BlockStateElement>(worldEntity);
        
            // Get ToolbarDataEntity
            ToolbarEntity = em.CreateEntityQuery(typeof(ToolbarData)).GetSingletonEntity();
            var Toolbar = em.CreateEntityQuery(typeof(ToolbarData)).GetSingleton<ToolbarData>();
            var toolbarBlockStates = em.GetBuffer<BlockStateElement>(ToolbarEntity);
        
            root = gameObject.GetComponent<UIDocument>().rootVisualElement;
            toolbar = root.Q<VisualElement>("toolbar");
            slots = toolbar.Query<VisualElement>("toolbarSlot");
        
            ToolbarItemGO = Instantiate(ToolbarItemPrefab);
            var meshFilter = ToolbarItemGO.GetComponent<MeshFilter>();
            var voxels = new NativeArray<ushort>(1, Allocator.Temp);

            // Loop through toolbar paletteItems
            int slotIndex = 0;
            foreach (var toolbarBlockState in toolbarBlockStates)
            {
                var toolbarBlockStateIndex = (byte)worldBlockStates.AsNativeArray().IndexOf(toolbarBlockState);
                voxels[0] = toolbarBlockStateIndex;
            
                // Get blockState model mesh
                var blockStateEntity = toolbarBlockState.Value;
                var blockState = em.GetComponentData<BlockState>(blockStateEntity);
                var modelMesh = em.GetSharedComponentManaged<ModelMesh>(blockState.Model);
                meshFilter.mesh = modelMesh.Value;
                // meshFilter.mesh = Mesher.Mesher.Compute(new int3(1), new int3(0), new int3(1), voxels);
                
                var texture = Render3D.Render3D.Instance.Snapshot(ToolbarItemGO);
                var slot = slots.AtIndex(slotIndex);
                var image = new Image();
                image.image = texture;
                if (slot.childCount > 0) slot.RemoveAt(0);
                slot.Add(image);

                slotIndex++;
            }

            SetActiveItem(0);
        }
    
        public void SetActiveItem(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            if (((KeyControl) context.control).keyCode == Key.Digit1)
                SetActiveItem(0);
            else if (((KeyControl) context.control).keyCode == Key.Digit2)
                SetActiveItem(1);
            else if (((KeyControl) context.control).keyCode == Key.Digit3)
                SetActiveItem(2);
            else if (((KeyControl) context.control).keyCode == Key.Digit4)
                SetActiveItem(3);
            else if (((KeyControl) context.control).keyCode == Key.Digit5)
                SetActiveItem(4);
            else if (((KeyControl) context.control).keyCode == Key.Digit6)
                SetActiveItem(5);
            else if (((KeyControl) context.control).keyCode == Key.Digit7)
                SetActiveItem(6);
            else if (((KeyControl) context.control).keyCode == Key.Digit8)
                SetActiveItem(7);
            else if (((KeyControl) context.control).keyCode == Key.Digit9)
                SetActiveItem(8);
            else if (((KeyControl) context.control).keyCode == Key.Digit0)
                SetActiveItem(9);
        }
    
        public void SetActiveItem(int slotIndex)
        {
            var toolbarData = em.CreateEntityQuery(typeof(ToolbarData)).GetSingleton<ToolbarData>();
            toolbarData.selectedItem = slotIndex;
            em.SetComponentData(ToolbarEntity, toolbarData);
        
            for (var i = 0; i < 10; i++)
                slots.AtIndex(i).RemoveFromClassList("toolbarSlotActive");
            slots.AtIndex(slotIndex).AddToClassList("toolbarSlotActive");
        }
    
    
        public BlockState GetActiveBlockState()
        {
            var toolbarData = em.CreateEntityQuery(typeof(ToolbarData)).GetSingleton<ToolbarData>();
            var toolbarBlockStates = em.GetBuffer<BlockStateElement>(ToolbarEntity);
            var blockStateEntity = toolbarBlockStates[toolbarData.selectedItem].Value;
            return em.GetComponentData<BlockState>(blockStateEntity);
        }   
    }
}