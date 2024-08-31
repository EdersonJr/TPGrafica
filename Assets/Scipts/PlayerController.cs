using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CookingManager cookingManager;
    private GameObject selectedFood;  // Armazena a referência ao alimento selecionado
    public Vector3 floatingOffset = new Vector3(0, 0, 2); // Offset da posição flutuante em relação à câmera

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

        // Se um alimento está selecionado, mantê-lo flutuando em frente à câmera
        if (selectedFood != null)
        {
            Vector3 cameraPosition = Camera.main.transform.position;
            Vector3 cameraForward = Camera.main.transform.forward;
            selectedFood.transform.position = cameraPosition + cameraForward * floatingOffset.z + new Vector3(floatingOffset.x, floatingOffset.y, 0);
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
        else if (clickedObject.CompareTag("fryingPan"))
        {
            if (selectedFood != null)
            {
                Debug.Log($"Adicionando {selectedFood.name} à frigideira {clickedObject.name}");
                cookingManager.AddFoodToFryPan(selectedFood, clickedObject);  // Passe o alimento selecionado
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
