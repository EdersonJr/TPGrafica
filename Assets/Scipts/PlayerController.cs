using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CookingManager cookingManager;
    private GameObject selectedFood;  // Armazena a referência ao alimento selecionado
    public Vector3 floatingOffset = new Vector3(0, 0, 2); // Offset da posição flutuante em relação à câmera
    public GameManager gameManager;
    private bool isDragging = false;  // Para rastrear se está arrastando o alimento

    void Start()
    {
        cookingManager = FindObjectOfType<CookingManager>();
        gameManager = FindObjectOfType<GameManager>(); 

        if (cookingManager == null)
        {
            Debug.LogError("CookingManager não encontrado na cena.");
        }

        if (gameManager == null)
        {
            Debug.LogError("GameManager não encontrado na cena.");
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

        // Se um alimento está selecionado e sendo arrastado, mantenha-o flutuando em frente à câmera
        if (selectedFood != null)
        {
            Vector3 cameraPosition = Camera.main.transform.position;
            Vector3 cameraForward = Camera.main.transform.forward;
            selectedFood.transform.position = cameraPosition + cameraForward * floatingOffset.z + new Vector3(floatingOffset.x, floatingOffset.y, 0);

            if (Input.GetMouseButtonDown(1)) // Botão direito para soltar o alimento
            {
                selectedFood = null;
                isDragging = false;  // Parar de arrastar o alimento
                Debug.Log("Soltou o alimento.");
            }
        }
    }

    void HandleClick(GameObject clickedObject)
    {
        if (clickedObject.CompareTag("Stove"))  // Adiciona lógica para o fogão
        {
            cookingManager.ToggleStove();  // Liga ou desliga o fogão
            Debug.Log("Clicou no fogão.");
        }

        if (clickedObject.CompareTag("Food"))
        {
            selectedFood = clickedObject;
            isDragging = true;  // Começa a arrastar o alimento
            Debug.Log($"Selecionou o alimento {clickedObject.name}");
        }
        else if (clickedObject.CompareTag("oil"))
        {
            selectedFood = clickedObject;
            isDragging = true;  // Começa a arrastar o alimento
            Debug.Log($"Selecionou o óleo");
        }
        else if (clickedObject.CompareTag("waterBottle"))
        {
            selectedFood = clickedObject;
            isDragging = true;  // Começa a arrastar o alimento
            Debug.Log($"Selecionou a água");
        }
        else if (clickedObject.CompareTag("Pan"))
        {
            PanProperties panProperties = clickedObject.GetComponent<PanProperties>();

            if (selectedFood != null)
            {
                if (panProperties == null)
                {
                    Debug.LogWarning("O objeto clicado não tem o componente PanProperties.");
                    return;
                }
                
		if (selectedFood.name == "Water")
            	{
                	Debug.Log("Adicionando água à panela.");
                	panProperties.hasWater = true;
                	selectedFood = null;
                	isDragging = false;
            	}
                else if (!panProperties.hasWater)
                {
                    Debug.Log("A Panela não tem água. Você perdeu pontos.");
                    gameManager.RemovePoints(10); // Perde 10 pontos
                }
                else {
                cookingManager.AddFoodToPan(selectedFood, clickedObject);
                selectedFood = null;
                isDragging = false;
                }
                
            }
            else
            {
                cookingManager.RemoveFoodFromPan(clickedObject);
            }
        }
        else if (clickedObject.CompareTag("fryingPan"))
        {
            PanProperties panProperties = clickedObject.GetComponent<PanProperties>();

            if (selectedFood != null)
            {
                if (panProperties == null)
                {
                    Debug.LogWarning("O objeto clicado não tem o componente PanProperties.");
                    return;
                }
		if (selectedFood.name == "Oil"){
			Debug.Log("Adicionando óleo à frigideira.");
                	panProperties.hasOil = true;
                	selectedFood = null;
                	isDragging = false;
		}
                else if (!panProperties.hasOil)
                {
                    Debug.Log("A Frigideira não tem óleo. Você perdeu pontos.");
                    gameManager.RemovePoints(10); // Perde 10 pontos
                }
                else{
                cookingManager.AddFoodToFryPan(selectedFood, clickedObject);
                selectedFood = null;
                isDragging = false;
                }
                
            }
            else
            {
                cookingManager.RemoveFoodFromFryPan(clickedObject);
            }
        }
        else
        {
            if (isDragging && selectedFood != null)
            {
                // Soltar o alimento em qualquer lugar da cena
                PlaceFood(clickedObject);
                selectedFood = null;
                isDragging = false;
                if (clickedObject.CompareTag("Food")){
                	Debug.Log("Soltou o alimento em qualquer lugar.");
                }
                else {
                	Debug.Log("Soltou o objeto em qualquer lugar.");
                }
            }
            else
            {
                Debug.Log("O objeto clicado não é nem comida nem uma panela.");
            }
        }
    }

    void PlaceFood(GameObject target)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Coloque o alimento na posição onde o raio atingiu
            selectedFood.transform.position = hit.point;
        }
    }
}
