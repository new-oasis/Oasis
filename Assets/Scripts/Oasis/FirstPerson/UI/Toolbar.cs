using Oasis.Data;
using Oasis.FirstPerson;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UIElements;

namespace Oasis.FirstPerson
{
    public class Toolbar : MonoBehaviour
    {
        private static Toolbar _instance;
        public static Toolbar Instance => _instance;
    
        public Entity ToolbarEntity;
    
        private UQueryBuilder<VisualElement> _slots;
        private VisualElement _toolbar;
        private VisualElement _root;

        public GameObject ToolbarItemGo;
        public GameObject ToolbarItemPrefab;

        private EntityManager _em;
    
        private void Start()
        {
            _instance = this;
            _em = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager;
            // var worldEntity = _em.CreateEntityQuery(typeof(Data.World)).GetSingletonEntity();
            // var bockStates = em.GetBuffer<BlockState>(worldEntity);
        
            // Get ToolbarDataEntity
            ToolbarEntity = _em.CreateEntityQuery(typeof(ToolbarData)).GetSingletonEntity();
            var toolbar = _em.CreateEntityQuery(typeof(ToolbarData)).GetSingleton<ToolbarData>();
            var toolbarBlockStates = _em.GetBuffer<BlockStateRef>(ToolbarEntity);
        
            _root = gameObject.GetComponent<UIDocument>().rootVisualElement;
            _toolbar = _root.Q<VisualElement>("toolbar");
            _slots = _toolbar.Query<VisualElement>("slot");
        
            ToolbarItemGo = Instantiate(ToolbarItemPrefab);
            var meshFilter = ToolbarItemGo.GetComponent<MeshFilter>();
            var voxels = new NativeArray<ushort>(1, Allocator.Temp);

            // Loop through toolbar paletteItems
            int slotIndex = 0;
            foreach (var toolbarBlockState in toolbarBlockStates)
            {
                var blockBlockStates = _em.GetBuffer<BlockState>(toolbarBlockState.Block);
                var blockState = blockBlockStates[toolbarBlockState.BlockStatesIndex];
                var modelMesh = _em.GetSharedComponentManaged<ModelMesh>(blockState.Model);
                meshFilter.mesh = modelMesh.Value;
                
                var texture = Game.Render3D.Render3D.Instance.Snapshot(ToolbarItemGo);
                var slot = _slots.AtIndex(slotIndex);
                var image = new Image();
                image.image = texture;
                if (slot.childCount > 0) slot.RemoveAt(0);
                slot.Add(image);
                var index = slotIndex;
                slot.RegisterCallback<ClickEvent>((ev) => SetActiveItem(index));
                
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
            Debug.Log($"Setting slotIndex to {slotIndex}");
            var toolbarData = _em.CreateEntityQuery(typeof(ToolbarData)).GetSingleton<ToolbarData>();
            toolbarData.SelectedItem = slotIndex;
            _em.SetComponentData(ToolbarEntity, toolbarData);
        
            for (var i = 0; i < 10; i++)
                _slots.AtIndex(i).RemoveFromClassList("slotActive");
            _slots.AtIndex(slotIndex).AddToClassList("slotActive");
        }
    
    }
}