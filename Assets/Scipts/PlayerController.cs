using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CookingManager cookingManager;
    private GameObject selectedFood;  // Armazena a referência ao alimento selecionado

    void Start()
    {
        cookingManager = FindObjectOfType<CookingManager>();
        if (cookingManager == null)
        {
            Debug.LogError("CookingManager não encontrado na cena.");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                HandleClick(hit.collider.gameObject);
            }
        }
    }

    

    void HandleClick(GameObject clickedObject)
    {
        // Verifique se o objeto clicado é um alimento
        if (clickedObject.CompareTag("Food"))
        {
            selectedFood = clickedObject;  // Armazena o alimento selecionado
            Debug.Log($"Selecionou o alimento {clickedObject.name}");
        }
        // Verifique se o objeto clicado é uma panela
        else if (clickedObject.CompareTag("Pan"))
        {
            if (selectedFood != null)
            {
                Debug.Log($"Adicionando {selectedFood.name} à panela {clickedObject.name}");
                cookingManager.AddFoodToPan(selectedFood, clickedObject);  // Passe o alimento selecionado
                selectedFood = null;  // Limpe a seleção de alimento após adicionar
            }
            else
            {
                Debug.Log("O objeto clicado é uma panela, mas nenhum alimento foi selecionado.");
            }
        }
        else
        {
            Debug.Log("O objeto clicado não é nem comida nem uma panela.");
        }
    }
}
