using UnityEngine;

public class CarModelSelector : MonoBehaviour
{
    [SerializeField] private Mesh[] carModels;
    [SerializeField] private MeshFilter carMesh;

    private void Awake()
    {
        ChooseCarModel(SaveManager.instance.currentCar);
    }
    private void ChooseCarModel(int _index)
    {
        carMesh = this.gameObject.GetComponent<MeshFilter>();
        carMesh.sharedMesh = Resources.Load<Mesh>(carModels[_index].name); 
    }
}
