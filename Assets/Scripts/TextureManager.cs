
// using Oasis.Common;
// using UnityEngine;
//
// namespace Oasis.Game.Textures
// {
//     public class TextureManager : MonoBehaviour
//     {
//         
//         public int maxTextures = 32;
//         private static TextureManager _instance;
//         public static TextureManager Instance => _instance;
//     
//         [Header("Texture Arrays")]
//         public Texture2DArray opaqueTexture2DArray;
//         public Texture2DArray cutoutTexture2DArray;
//         public Texture2DArray transTexture2DArray;
//
//         [Header("Materials")]
//         public Material[] LitMaterials;
//         public Material[] UnlitMaterials;
//         
//         public int opaqueCount;
//         public int cutoutCount;
//         public int transCount;
//         
//         void Awake()
//         {
//             _instance = this;
//             opaqueTexture2DArray = new Texture2DArray(16, 16, maxTextures, TextureFormat.DXT1, false);
//             cutoutTexture2DArray = new Texture2DArray(16, 16, maxTextures, TextureFormat.DXT5, false);
//             transTexture2DArray = new Texture2DArray(16, 16, maxTextures, TextureFormat.DXT5, false);
//
//             LitMaterials[0].SetTexture("_TextureArray", opaqueTexture2DArray);
//             LitMaterials[1].SetTexture("_TextureArray", cutoutTexture2DArray);
//             LitMaterials[2].SetTexture("_TextureArray", transTexture2DArray);
//
//             UnlitMaterials[0].SetTexture("_TextureArray", opaqueTexture2DArray);
//             UnlitMaterials[1].SetTexture("_TextureArray", cutoutTexture2DArray);
//             UnlitMaterials[2].SetTexture("_TextureArray", transTexture2DArray);
//         }
//         
//         public int LoadTexture(Texture2D texture, TextureType type)
//         {
//             switch (type)
//             {
//                 case TextureType.Opaque:
//                 {
//                     Graphics.CopyTexture(texture, 0, 0, opaqueTexture2DArray, opaqueCount, 0);
//                     opaqueCount++;
//                     return opaqueCount - 1;
//                 }
//                 case TextureType.Cutout:
//                 {
//                     Graphics.CopyTexture(texture, 0, 0, cutoutTexture2DArray, cutoutCount, 0);
//                     cutoutCount++;
//                     return cutoutCount - 1;
//                 }
//                 case TextureType.Transparent:
//                 {
//                     Graphics.CopyTexture(texture, 0, 0, transTexture2DArray, cutoutCount, 0);
//                     cutoutCount++;
//                     return cutoutCount - 1;
//                 }
//                 default:
//                     return -1;
//             }
//         }
//     }
// }
//
