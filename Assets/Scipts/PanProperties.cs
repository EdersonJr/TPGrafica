using UnityEngine;
public class PanProperties : MonoBehaviour
{
    public bool hasOil = false;
    public bool hasWater = false;
    public bool hasFood = false;
    public bool hasLid = false ;
    public bool isBurning = false;
    private float timeSinceOilAdded = 0f;
    private bool oilAdded = false;

    void Update()
    {
        // Se óleo foi adicionado e ainda não há comida, inicia o temporizador
        if (hasOil && !hasFood && !isBurning)
        {
            timeSinceOilAdded += Time.deltaTime;

            // Se 4 segundos se passaram e a comida não foi adicionada, a frigideira pega fogo
            if (timeSinceOilAdded >= 4f)
            {
                isBurning = true;
                Debug.Log("A frigideira está pegando fogo!");
            }
        }
    }

    // Método para resetar o timer quando óleo é adicionado
    public void ResetOilTimer()
    {
        hasOil = true;
        timeSinceOilAdded = 0f;
        isBurning = false;
        Debug.Log("Óleo foi adicionado, iniciando o temporizador.");
    }

    // Método para adicionar comida e parar o fogo (se necessário)
    public void AddFood()
    {
        if (hasOil)
        {
            hasFood = true;
            timeSinceOilAdded = 0f; // Reseta o timer porque a comida foi adicionada
            isBurning = false;      // Certifique-se de que o fogo seja apagado ao adicionar comida
            Debug.Log("Comida foi adicionada na frigideira.");
        }
        else
        {
            Debug.Log("Você precisa adicionar óleo antes da comida!");
        }
    }
}
