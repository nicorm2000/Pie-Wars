using UnityEngine;

public class PlayerSkinManager : MonoBehaviour
{
    [SerializeField] private MeshRenderer mesh = null;

    public void ChangeMaterial(Material material)
    {
        mesh.material = material;
    }
}
