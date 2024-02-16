using Oasis.Data;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UIElements;

namespace Oasis.Bob
{
    public class ToolbarMono : MonoBehaviour
    {
        public InputReader inputReader;

        public Entity ToolbarEntity;
    
        private UQueryBuilder<VisualElement> _slots;
        private VisualElement _toolbar;
        private VisualElement _root;

        public GameObject ToolbarItemGo;
        public GameObject ToolbarItemPrefab;

        private EntityManager _em;
    
        private void Start()
        {
            inputReader.ToolbarSelect += SetActiveItem;

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