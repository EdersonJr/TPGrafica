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
            isDragging = true;  // Começa a arrastar o óleo
            Debug.Log($"Selecionou o óleo");
        }
        else if (clickedObject.CompareTag("Lid"))
        {
        selectedFood = clickedObject;
        isDragging = true;  // Começa a arrastar a tampa
        Debug.Log("Selecionou a tampa.");
         }
        else if (clickedObject.CompareTag("waterBottle"))
        {
            selectedFood = clickedObject;
            isDragging = true;  // Começa a arrastar a água
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
                else
                {
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

            if (panProperties != null)
            {   
                if (selectedFood != null && selectedFood.CompareTag("oil"))
                {
                    Debug.Log("Adicionando óleo à frigideira.");
                    panProperties.hasOil = true; // Adiciona óleo à frigideira
                    panProperties.ResetOilTimer(); 
                    gameManager.AddPoints(10);  // Adiciona 10 pontos
                    Debug.Log("10 pontos adicionados por colocar comida no fogão!");
                    selectedFood = null;
                    isDragging = false; // Para de arrastar o óleo
                }
                if (selectedFood != null && selectedFood.CompareTag("waterBottle"))
                {
                    Debug.Log("Adicionando água à panela.");
                    panProperties.hasWater = true; // Adiciona água à panela
                    panProperties.ResetWaterTimer(); 
                    gameManager.AddPoints(10);  // Adiciona 10 pontos
                    Debug.Log("10 pontos adicionados por colocar comida no fogão!");
                    selectedFood = null;
                    isDragging = false; // Para de arrastar o óleo
                }
                else if (selectedFood != null && selectedFood.CompareTag("Lid"))
                {
                    Debug.Log("Colocando a tampa na frigideira.");
                    cookingManager.AddLidToFryPan(selectedFood, clickedObject);
                    panProperties.hasLid = true; // Coloca a tampa na frigideira
                    gameManager.AddPoints(10);  // Adiciona 10 pontos
                    selectedFood = null;
                    isDragging = false; // Para de arrastar a tampa
                }
                else if (selectedFood != null && selectedFood.CompareTag("Food"))
                {
                    // Lógica de adicionar comida à frigideira
                    if (panProperties.hasOil)
                    {
                        cookingManager.AddFoodToFryPan(selectedFood, clickedObject);
                        gameManager.AddPoints(10);  // Adiciona 10 pontos
                        selectedFood = null;
                        panProperties.AddFood();
                        gameManager.AddPoints(10);
                        isDragging = false;
                    }
                    if (panProperties.hasWater)
                    {
                        cookingManager.AddFoodToFryPan(selectedFood, clickedObject);
                        gameManager.AddPoints(10);  // Adiciona 10 pontos
                        selectedFood = null;
                        panProperties.AddFood();
                        gameManager.AddPoints(10);
                        isDragging = false;
                    }
                    else
                    {
                        Debug.Log("A Frigideira não tem óleo. Você perdeu pontos.");
                        gameManager.RemovePoints(10); // Perde 10 pontos
                    }
                }
                else
                {
                    cookingManager.RemoveFoodFromFryPan(clickedObject);
                }
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
                if (clickedObject.CompareTag("Food"))
                {
                    Debug.Log("Soltou o alimento em qualquer lugar.");
                }
                else
                {
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
            if (selectedFood.CompareTag("Lid"))
            {
                // Se for a tampa, define a posição específica
                Vector3 tablePosition = new Vector3(8.3f, 0.8f, 3.0f); // Ajuste a altura conforme necessário
                selectedFood.transform.position = tablePosition;
                Debug.Log("Tampa colocada na posição específica.");
            }
            else
            {
                // Coloque o alimento na posição onde o raio atingiu
                selectedFood.transform.position = hit.point;
            }
        }
    }
}
