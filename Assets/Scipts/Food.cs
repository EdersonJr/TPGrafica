using UnityEngine;

public class Food : MonoBehaviour
{
    public float cookingTime; // Tempo necessário para cozinhar o alimento

    public void Cook()
    {
        // Chame o método para iniciar o cozimento
        FindObjectOfType<CookingManager>().StartCooking();
    }
}
