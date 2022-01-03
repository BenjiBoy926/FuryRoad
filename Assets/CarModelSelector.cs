using UnityEngine;

public class CarModelSelector : MonoBehaviour
{
    [SerializeField] private Mesh[] carModels;
    [SerializeField] private Material[] carMaterials;
    private MeshFilter carMeshFilter;
    private Renderer carMeshRenderer;
    

    private void Awake()
    {
        // If the save manager exists then use it to select a car model
        if (SaveManager.instance) ChooseCarModel(SaveManager.instance.currentCar);
    }
    private void ChooseCarModel(int _index)
    {
        carMeshFilter = this.gameObject.GetComponent<MeshFilter>();
        carMeshRenderer = this.gameObject.GetComponent<Renderer>();
        carMeshFilter.sharedMesh = Resources.Load<Mesh>(carModels[_index].name); 
        carMeshRenderer.sharedMaterial = carMaterials[_index];
    }
}
