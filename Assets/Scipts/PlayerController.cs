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

        if (selectedFood != null)
        {
            Vector3 cameraPosition = Camera.main.transform.position;
            Vector3 cameraForward = Camera.main.transform.forward;
            selectedFood.transform.position = cameraPosition + cameraForward * floatingOffset.z + new Vector3(floatingOffset.x, floatingOffset.y, 0);

            if (Input.GetMouseButtonDown(1)) // Botão direito para soltar o alimento
            {
                selectedFood = null;
                Debug.Log("Soltou o alimento.");
            }
        }
    }
    void HandleClick(GameObject clickedObject)
    {
        if (clickedObject.CompareTag("Food"))
        {
            selectedFood = clickedObject;
            Debug.Log($"Selecionou o alimento {clickedObject.name}");
        }
        else if (clickedObject.CompareTag("Pan"))
        {
            if (selectedFood != null)
            {
                cookingManager.AddFoodToPan(selectedFood, clickedObject);
                selectedFood = null;
            }
            else
            {
                cookingManager.RemoveFoodFromPan(clickedObject);
            }
        }
        else if (clickedObject.CompareTag("fryingPan"))
        {
            if (selectedFood != null)
            {
                cookingManager.AddFoodToFryPan(selectedFood, clickedObject);
                selectedFood = null;
            }
            else
            {
                cookingManager.RemoveFoodFromFryPan(clickedObject); // Chame o método para frigideira
            }
        }
        else
        {
            Debug.Log("O objeto clicado não é nem comida nem uma panela.");
        }
    }


}
