// using UnityEngine;
// using UnityEngine.UIElements;
//
// namespace Oasis.Bob.Look
// {
//     public class LookController : MonoBehaviour
//     {
//         public Camera cameraToRotate;
//         public Transform characterToRotate;
//         public float sensitivityX = 100f;
//         public float sensitivityY = 100f;
//         public float smoothingFactor = 5f;
//         
//         Vector3 currentCameraRotation = Vector3.zero;
//         Vector3 targetCameraRotation = Vector3.zero;
//
//         Vector3 currentCharacterRotation = Vector3.zero;
//         Vector3 targetCharacterRotation = Vector3.zero;
//         
//         
//         void OnEnable()
//         {
//             var root = GetComponent<UIDocument>().rootVisualElement;
//             var lookArea = root.Q<VisualElement>("LookArea");
//             lookArea.RegisterCallback<PointerMoveEvent>((ev) => OnPointerMove(ev));
//         }
//         
//         void OnPointerMove(PointerMoveEvent ev)
//         {
//             var deltaX = Mathf.Clamp(ev.deltaPosition.x, -40f, 40f);
//             var deltaY = Mathf.Clamp(ev.deltaPosition.y, -40f, 40f);
//
//             // Update target rotations
//             targetCameraRotation = new Vector3(currentCameraRotation.x + deltaY * sensitivityY, 0, 0);
//             targetCharacterRotation = new Vector3(0f, currentCharacterRotation.y + deltaX * sensitivityX, 0);
//         }
//
//         void Update()
//         {
//             // Smoothly blend camera rotation
//             currentCameraRotation = Vector3.Lerp(currentCameraRotation, targetCameraRotation, Time.deltaTime * smoothingFactor);
//             cameraToRotate.transform.localRotation = Quaternion.Euler(currentCameraRotation);
//
//             // Smoothly blend character rotation
//             currentCharacterRotation = Vector3.Lerp(currentCharacterRotation, targetCharacterRotation, Time.deltaTime * smoothingFactor);
//             characterToRotate.transform.localRotation = Quaternion.Euler(currentCharacterRotation);
//         }
//         
//     }
//     
// }
