using System.Linq;
using Oasis.BlockStates;
using Oasis.Mesher;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine.Serialization;

public class ToolbarUI : MonoBehaviour
{
    [FormerlySerializedAs("ToolbarDataEntity")] public Entity ToolbarEntity;
    
    private UQueryBuilder<VisualElement> slots;
    private VisualElement toolbar;
    private VisualElement root;

    public GameObject ToolbarItemGO;
    public GameObject ToolbarItemPrefab;

    private void Start()
    {
        var em = World.DefaultGameObjectInjectionWorld.EntityManager;
        var worldEntity = em.CreateEntityQuery(typeof(Oasis.World.World)).GetSingletonEntity();
        var worldBlockStates = em.GetBuffer<BlockStateReference>(worldEntity);
        
        // Get ToolbarDataEntity
        ToolbarEntity = em.CreateEntityQuery(typeof(Toolbar)).GetSingletonEntity();
        var Toolbar = em.CreateEntityQuery(typeof(Toolbar)).GetSingleton<Toolbar>();
        var toolbarBlockStateReferences = em.GetBuffer<BlockStateReference>(ToolbarEntity);
        
        root = gameObject.GetComponent<UIDocument>().rootVisualElement;
        toolbar = root.Q<VisualElement>("toolbar");
        slots = toolbar.Query<VisualElement>("toolbarSlot");
        
        ToolbarItemGO = Instantiate(ToolbarItemPrefab);
        var meshFilter = ToolbarItemGO.GetComponent<MeshFilter>();
        var voxels = new NativeArray<ushort>(1, Allocator.Temp);

        // Loop through toolbar blockStateReferences
        int slotIndex = 0;
        foreach (var toolbarBlockStateReference in toolbarBlockStateReferences)
        {
            var toolbarBlockStateIndex = (byte)worldBlockStates.AsNativeArray().IndexOf(toolbarBlockStateReference);
            Debug.Log("toolbarBlockStateIndex " + toolbarBlockStateIndex);
            voxels[0] = toolbarBlockStateIndex;
            
            meshFilter.mesh = Mesher.Compute(new int3(1), new int3(0), new int3(1), voxels);
            var texture = Render3D.Instance.Snapshot(ToolbarItemGO);
            var slot = slots.AtIndex(slotIndex);
            var image = new Image();
            image.image = texture;
            if (slot.childCount > 0) slot.RemoveAt(0);
                slot.Add(image);

            slotIndex++;
        }

        // SetActiveItem(0);
    }

    
    // public void SetActiveItem(int slotIndex)
    // {
    //     selectedItem = slotIndex;
    //     currentBlockState = blockStates[slotIndex];
    //     for (var i = 0; i < 10; i++)
    //         slots.AtIndex(i).RemoveFromClassList("toolbarSlotActive");
    //     slots.AtIndex(slotIndex).AddToClassList("toolbarSlotActive");
    // }
    
}