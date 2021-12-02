using UnityEngine;

public class CarModelSelector : MonoBehaviour
{
    [SerializeField] private Mesh[] carModels;
    [SerializeField] private MeshFilter carMesh;

    private void Awake()
    {
        // If the save manager exists then use it to select a car model
        if (SaveManager.instance) ChooseCarModel(SaveManager.instance.currentCar);
    }
    private void ChooseCarModel(int _index)
    {
        carMesh = this.gameObject.GetComponent<MeshFilter>();
        carMesh.sharedMesh = Resources.Load<Mesh>(carModels[_index].name); 
    }
}
