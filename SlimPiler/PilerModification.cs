using UnityEngine;
using HarmonyLib;

namespace DysonSphereProgram.Modding.SlimPiler;

class PilerPatch
{
  [HarmonyPatch(typeof(VFPreload), nameof(VFPreload.InvokeOnLoadWorkEnded))]
  [HarmonyPostfix]
  public static void AfterLDBLoad()
  {
    PilerModification.Apply();
  }
}

public class PilerModification
{
  private static readonly Vector3 colliderScale = new Vector3(0.5f, 1f, 0.8f);
  private static readonly Vector3 meshScale = new Vector3(0.7f, 1f, 0.95f);
  private static readonly Vector3 selectOffset = new Vector3(0f, 0.35f, 0f);
  private static readonly Vector3 selectScale = meshScale;

  public static void Apply()
  {
    PilerOriginal.InitData();
    var pilerProto = LDB.items.Select(PilerOriginal.itemId);
    var pilerPrefab = pilerProto.prefabDesc;

    ColliderData tmpCollider;

    for (int i = 0; i < pilerPrefab.colliders.Length; i++)
    {
      tmpCollider = PilerOriginal.colliders[i];
      tmpCollider.ext.Scale(colliderScale);
      pilerPrefab.colliders[i] = tmpCollider;
    }

    for (int i = 0; i < pilerPrefab.buildColliders.Length; i++)
    {
      tmpCollider = PilerOriginal.buildColliders[i];
      tmpCollider.ext.Scale(colliderScale);
      pilerPrefab.buildColliders[i] = tmpCollider;
    }

    tmpCollider = PilerOriginal.buildCollider;
    tmpCollider.ext.Scale(colliderScale);
    pilerPrefab.buildCollider = tmpCollider;

    for (int i = 0; i < pilerPrefab.lodCount; i++)
    {
      var mesh = pilerPrefab.lodMeshes[i];
      var originalVerts = PilerOriginal.meshVertices[i];
      var vertices = mesh.vertices;
      for (int j = 0; j < vertices.Length; j++)
      {
        Vector3 vert = originalVerts[j];
        vert.x *= meshScale.x;
        vert.y *= meshScale.y;
        vert.z *= meshScale.z;
        vertices[j] = vert;
      }
      mesh.vertices = vertices;
    }

    pilerPrefab.selectCenter = PilerOriginal.selectCenter + selectOffset;
    pilerPrefab.selectSize = PilerOriginal.selectSize;
    pilerPrefab.selectSize.Scale(selectScale);
    pilerPrefab.selectSize += 2 * selectOffset;
    
    Plugin.Log.LogDebug("SlimPiler modification applied");
  }
}