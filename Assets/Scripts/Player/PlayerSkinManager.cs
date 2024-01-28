using UnityEngine;

public class PlayerSkinManager : MonoBehaviour
{
    [SerializeField] private MeshRenderer[] meshes = null;

    public void ChangeMaterial(Material material)
    {
        for (int i = 0; i < meshes.Length; i++)
        {
            meshes[i].material = material;
        }        
    }
}
