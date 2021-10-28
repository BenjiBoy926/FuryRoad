using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarSelection : MonoBehaviour
{
    private int currentCar;
    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;

    private void Awake()
    {
        selectCar(0);
    }

    private void selectCar(int index)
    {
        previousButton.interactable = (index != 0);
        nextButton.interactable = (index != transform.childCount-1);

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i == index);
        }
    }

    public void ChangeCar(int change)
    {
        currentCar += change;
        selectCar(currentCar);
    }
}
