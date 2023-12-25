// using Oasis.Data;
// using Unity.Entities;
// using UnityEngine;
// using UnityEngine.UIElements;
//
// namespace Oasis.Game.Player.FirstPerson.Debug
// {
//     public class Debug : MonoBehaviour
//     {
//         private static Debug _instance;
//         public static Debug Instance => _instance;
//     
//         private VisualElement root;
//
//         private EntityManager em;
//     
//         private void Start()
//         {
//             _instance = this;
//             em = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager;
//             var worldEntity = em.CreateEntityQuery(typeof(Data.World)).GetSingletonEntity();
//             var worldBlockStates = em.GetBuffer<BlockStateElement>(worldEntity);
//         
//             root = gameObject.GetComponent<UIDocument>().rootVisualElement;
//         }
//         
//     }
// }