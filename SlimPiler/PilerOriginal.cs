using UnityEngine;

namespace DysonSphereProgram.Modding.SlimPiler;

public static class PilerOriginal
{
  public const int itemId = 2040;
  private static bool initialized = false;

  public static ColliderData[] colliders { get; private set; }
  public static ColliderData buildCollider { get; private set; }
  public static ColliderData[] buildColliders { get; private set; }
  public static Vector3[][] meshVertices { get; private set; }
  public static Vector3 selectCenter { get; private set; }
  public static Vector3 selectSize { get; private set; }

  public static void InitData()
  {
    if (initialized)
      return;

    var pilerProto = LDB.items.Select(itemId);
    var pilerPrefab = pilerProto.prefabDesc;

    var originalColliders = new ColliderData[pilerPrefab.colliders.Length];
    var originalBuildColliders = new ColliderData[pilerPrefab.buildColliders.Length];
    var originalMeshVertices = new Vector3[pilerPrefab.lodCount][];
    for (int i = 0; i < pilerPrefab.lodCount; i++)
    {
      var vertices = pilerPrefab.lodMeshes[i].vertices;
      originalMeshVertices[i] = new Vector3[vertices.Length];
      for (int j = 0; j < vertices.Length; j++)
        originalMeshVertices[i][j] = vertices[j];
    }

    for (int i = 0; i < originalColliders.Length; i++)
      originalColliders[i] = pilerPrefab.colliders[i];
    for (int i = 0; i < originalBuildColliders.Length; i++)
      originalBuildColliders[i] = pilerPrefab.buildColliders[i];

    buildCollider = pilerPrefab.buildCollider;
    colliders = originalColliders;
    buildColliders = originalBuildColliders;
    meshVertices = originalMeshVertices;
    selectCenter = pilerPrefab.selectCenter;
    selectSize = pilerPrefab.selectSize;

    initialized = true;
  }
}