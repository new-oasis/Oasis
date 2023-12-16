using Oasis.World;
using Unity.Entities;
using UnityEngine;

namespace Oasis.Settings
{
    
    public class SettingsAuthoring : MonoBehaviour
    {
        public WorldType WorldType;

        public class SettingsBaker : Baker<SettingsAuthoring>
        {
            public override void Bake(SettingsAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                // AddComponent(entity, new Oasis.Settings.Settings {WorldType = authoring.WorldType});
            }
        }
    }
    
}