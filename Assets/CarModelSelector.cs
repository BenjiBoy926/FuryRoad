using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

using Photon.Pun;
using Photon.Realtime;

public class CarModelSelector : MonoBehaviour
{
    [SerializeField] public Mesh[] carModels;
    [SerializeField] public Material[] carMaterials;
    public MeshFilter carMeshFilter;
    public Renderer carMeshRenderer;
    
    private void Start(){
        if (SaveManager.instance){
            int carFilterRenderer = SaveManager.instance.currentCar;
            Hashtable hash = new Hashtable();
            hash.Add("Car Model", carFilterRenderer);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
        
    }

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
