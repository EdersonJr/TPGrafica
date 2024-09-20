using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CookingManager : MonoBehaviour
{
    public GameObject fryingPan;
    public GameObject pot;
    public GameObject egg;
    public GameObject meat;
    public GameObject pasta;
    public GameObject water;
    public GameObject oil;

    [SerializeField] TextMeshProUGUI timerText; //mostra timer

    [SerializeField] TextMeshProUGUI foodName; //mostra nome da comida no timer
    private float cookingTime = 10f;
    private bool isCooking = false;
    private bool isStoveOn = false;  // Indica se o fogão está ligado
    private Dictionary<GameObject, float> foodStartTime = new Dictionary<GameObject, float>(); // Dicionário para armazenar o tempo de início do alimento

    void Start(){

        timerText.gameObject.SetActive(false);

        foodName.gameObject.SetActive(false);

    }

    void Update()
    {
        if (isCooking && isStoveOn)
        {
            cookingTime -= Time.deltaTime;
            if (cookingTime <= 0f)
            {
                isCooking = false;
                Debug.Log("Food is ready!");

                timerText.color = Color.red;

                foodName.color = Color.red;

                cookingTime = 0f;

                // Chama a função para substituir o Steak pela Burger
                ReplaceFoodWithBurger();
            }
             else if(cookingTime <4){

                timerText.color = Color.yellow;

                foodName.color = Color.yellow;

            }

            else if(cookingTime < 10){

                timerText.color = Color.green;

                foodName.color = Color.green;

            }

            SetTimerText();
        }

        // Verifica se algum alimento está pronto após 2 segundos
        foreach (var entry in foodStartTime)
        {
            if (Time.time - entry.Value >= 10f)
            {
                Debug.Log($"{entry.Key.name} está pronto!");

                // Substitui o Steak pela Burger, se for o Steak
                if (entry.Key.name == "Steak")
                {
                    ReplaceFoodWithBurger();
                }

                // Remove o alimento do dicionário após o aviso
                foodStartTime.Remove(entry.Key);
                break; // Importante sair do loop para evitar erros de modificação do dicionário
            }
        }
    }

    private void SetTimerText(){

        timerText.text = cookingTime.ToString("0.00");

    }

    // Função para substituir o Steak pela Burger
    void ReplaceFoodWithBurger()
    {
        // Encontre o alimento Steak na cena
        GameObject steak = GameObject.Find("Steak");

        if (steak != null)
        {
            // Pegue a posição e a rotação do Steak
            Vector3 steakPosition = steak.transform.position;
            Quaternion steakRotation = steak.transform.rotation;

            // Encontre a Burger na cena (ou prefab se preferir instanciar)
            GameObject Burger = GameObject.Find("Burger");

            if (Burger != null)
            {
                // Ajusta a posição da Burger para evitar sobreposição
                Vector3 burgerPosition = steakPosition;
                burgerPosition.y += 0.1f; // Eleva a Burger ligeiramente acima da posição do Steak

                // Posiciona a Burger na nova posição
                Burger.transform.position = burgerPosition;
                Burger.transform.rotation = steakRotation;
                Burger.SetActive(true); // Ativa a Burger, se estiver desativada

                // Destrói o Steak
                Destroy(steak);

                Debug.Log("Steak foi substituído por Burger!");
            }
            else
            {
                Debug.LogError("O Burger não foi encontrada na cena.");
            }
        }
        else
        {
            Debug.LogError("O Steak não foi encontrado na cena.");
        }
    }

    public void StartCooking()
    {
       if (isStoveOn)  // Somente inicia o cozimento se o fogão estiver ligado
        {
            isCooking = true;
            timerText.gameObject.SetActive(true);
            foodName.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("O fogão está desligado! Não pode cozinhar.");
        }
    }

    public void AddFoodToPan(GameObject foodItem, GameObject pan)
    {
        Transform foodPosition = pan.transform.Find("FoodPosition");

        if (foodPosition != null)
        {
            if (isStoveOn){
                StartCooking();}
            foodName.text = foodItem.name;
            foodItem.transform.position = foodPosition.position;
            foodItem.transform.SetParent(foodPosition); // Torna o "FoodPosition" o pai do alimento
            cookingTime = 10f; // Reinicia o tempo de cozimento
            foodStartTime[foodItem] = Time.time; // Registra o tempo de adição do alimento

            Debug.Log($"Adicionou {foodItem.name} à panela {pan.name} na posição {foodPosition.position}");
        }
        else
        {
            Debug.LogError($"O GameObject 'FoodPosition' não encontrado na panela {pan.name}.");
        }
    }

    public void AddFoodToFryPan(GameObject foodItem, GameObject fryingPan)
    {
        Transform foodPosition = fryingPan.transform.Find("FoodPosition");

        if (foodPosition != null)
        {
            if (isStoveOn){
                StartCooking();}
            foodName.text = foodItem.name;
            foodItem.transform.position = foodPosition.position;
            foodItem.transform.SetParent(foodPosition); // Torna a frigideira o pai do alimento
            cookingTime = 10f; // Reinicia o tempo de cozimento
            foodStartTime[foodItem] = Time.time; // Registra o tempo de adição do alimento

            Debug.Log($"Adicionou {foodItem.name} à frigideira {fryingPan.name} na posição {foodPosition.position}");
        }
        else
        {
            Debug.LogError($"O GameObject 'FoodPosition' não encontrado na frigideira {fryingPan.name}.");
        }
    }

    public void RemoveFoodFromPan(GameObject pan)
    {
        Transform foodPosition = pan.transform.Find("FoodPosition");

        if (foodPosition != null)
        {
            if (foodPosition.childCount > 0)
            {
                isCooking = false;

                timerText.gameObject.SetActive(false);

                foodName.gameObject.SetActive(false);
                GameObject foodItem = foodPosition.GetChild(0).gameObject;
                foodItem.transform.SetParent(null); // Remove o alimento da panela
                foodItem.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2; // Move o alimento para frente da câmera

                Debug.Log($"Removeu {foodItem.name} da panela {pan.name}");
                // Remove o alimento do dicionário
                foodStartTime.Remove(foodItem);
            }
            else
            {
                Debug.Log("Não há alimento na panela para remover.");
            }
        }
        else
        {
            Debug.LogError($"O GameObject 'FoodPosition' não encontrado na panela {pan.name}.");
        }
    }

    public void RemoveFoodFromFryPan(GameObject fryingPan)
    {
        Transform foodPosition = fryingPan.transform.Find("FoodPosition");

        if (foodPosition != null)
        {
            if (foodPosition.childCount > 0)
            {
                isCooking = false;

                timerText.gameObject.SetActive(false);

                foodName.gameObject.SetActive(false);
                GameObject foodItem = foodPosition.GetChild(0).gameObject;
                foodItem.transform.SetParent(null); // Remove o alimento da frigideira
                foodItem.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2; // Move o alimento para frente da câmera

                Debug.Log($"Removeu {foodItem.name} da frigideira {fryingPan.name}");
                // Remove o alimento do dicionário
                foodStartTime.Remove(foodItem);
            }
            else
            {
                Debug.Log("Não há alimento na frigideira para remover.");
            }
        }
        else
        {
            Debug.LogError($"O GameObject 'FoodPosition' não encontrado na frigideira {fryingPan.name}.");
        }
    }
    public void ToggleStove()  // Método para ligar/desligar o fogão
    {
        isStoveOn = !isStoveOn;
        Debug.Log(isStoveOn ? "Fogão ligado!" : "Fogão desligado!");
    }
}
