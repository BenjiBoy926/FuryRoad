using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class CarModelSelector : MonoBehaviour
{
    [SerializeField] public Mesh[] carModels;
    [SerializeField] public Material[] carMaterials;
    private MeshFilter carMeshFilter;
    private Renderer carMeshRenderer;

    public ExitGames.Client.Photon.Hashtable playerCarModelProperty = new ExitGames.Client.Photon.Hashtable();
    
    private void Start(){
        if (SaveManager.instance){
            Debug.Log(SaveManager.instance.currentCar);
            int carFilterRenderer = SaveManager.instance.currentCar;
            playerCarModelProperty.Add("Car Model", carFilterRenderer);
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerCarModelProperty);
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
