using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CookingManager : MonoBehaviour
{
    public GameObject fryingPan;
    public GameObject pot;
    public GameObject egg;
    public GameObject meat;
    public GameObject pasta;
    public GameObject water;
    public GameObject oil;

    private float cookingTime = 0f;
    private bool isCooking = false;

    void Update()
    {
        if (isCooking)
        {
            cookingTime -= Time.deltaTime;
            if (cookingTime <= 0f)
            {
                isCooking = false;
                Debug.Log("Food is ready!");
                // Handle food completion logic
            }
        }
    }

    public void StartCooking(float time)
    {
        cookingTime = time;
        isCooking = true;
    }

    // Método para adicionar alimento à panela
    public void AddFoodToPan(GameObject foodItem, GameObject pan)
    {
        Transform foodPosition = pan.transform.Find("FoodPosition");

        if (foodPosition != null)
        {
            // Mova o alimento para a posição dentro da panela
            foodItem.transform.position = foodPosition.position;
            foodItem.transform.parent = foodPosition; // Torna o "FoodPosition" o pai do alimento
            Debug.Log($"Adicionou {foodItem.name} à panela {pan.name} na posição {foodPosition.position}");
        }
        else
        {
            Debug.LogError("O GameObject 'FoodPosition' não encontrado na panela.");
        }
    }

    public void AddFoodToFryPan(GameObject foodItem, GameObject fryingPan)
    {
        Transform foodPosition = fryingPan.transform.Find("FoodPosition"); // Encontre o GameObject "FoodPosition" dentro da panela
        
        if (foodPosition != null)
        {
            // Mova o alimento para a posição dentro da panela
            foodItem.transform.position = foodPosition.position;
            foodItem.transform.parent = fryingPan.transform; // Torna a panela o pai do alimento
            Debug.Log($"Adicionou {foodItem.name} à frigideira {fryingPan.name} na posição {foodPosition.position}");
        }
        else
        {
            Debug.LogError("O GameObject 'FoodPosition' não encontrado na panela.");
        }
    }
    public void RemoveFoodFromPan(GameObject pan)
    {
        Transform foodPosition = pan.transform.Find("FoodPosition");

        if (foodPosition != null)
        {
            Debug.Log($"foodPosition encontrado: {foodPosition.name}, filhos: {foodPosition.childCount}");

            if (foodPosition.childCount > 0)
            {
                GameObject foodItem = foodPosition.GetChild(0).gameObject;
                Debug.Log($"Alimento encontrado: {foodItem.name}");

                foodItem.transform.parent = null; // Remove o alimento da panela
                foodItem.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2; // Move o alimento para frente da câmera

                Debug.Log($"Removeu {foodItem.name} da panela {pan.name}");
            }
            else
            {
                Debug.Log("Não há alimento na panela para remover.");
            }
        }
        else
        {
            Debug.LogError("O GameObject 'FoodPosition' não encontrado na panela.");
        }
    }
    public void RemoveFoodFromFryPan(GameObject fryingPan)
    {
        Transform foodPosition = fryingPan.transform.Find("FoodPosition");

        if (foodPosition != null)
        {
            Debug.Log($"foodPosition encontrado: {foodPosition.name}, filhos: {foodPosition.childCount}");

            if (foodPosition.childCount > 0)
            {
                GameObject foodItem = foodPosition.GetChild(0).gameObject;
                Debug.Log($"Alimento encontrado: {foodItem.name}");

                foodItem.transform.parent = null; // Remove o alimento da frigideira
                foodItem.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2; // Move o alimento para frente da câmera

                Debug.Log($"Removeu {foodItem.name} da frigideira {fryingPan.name}");
            }
            else
            {
                Debug.Log("Não há alimento na frigideira para remover.");
            }
        }
        else
        {
            Debug.LogError("O GameObject 'FoodPosition' não encontrado na frigideira.");
        }
    }






}
