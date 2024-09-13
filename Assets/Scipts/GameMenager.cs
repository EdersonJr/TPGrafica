using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int points = 0; // Pontuação inicial
    public Text pointsText; // Campo de texto na UI para exibir a pontuação

    void Start()
    {
        UpdatePointsText(); // Atualiza o texto com a pontuação inicial
    }

    // Função para adicionar pontos
    public void AddPoints(int amount)
    {
        points += amount;
        UpdatePointsText();
    }

    // Função para remover pontos
    public void RemovePoints(int amount)
    {
        points -= amount;
        UpdatePointsText();
    }

    // Função que atualiza o campo de texto da pontuação
    void UpdatePointsText()
    {
        pointsText.text = "Points: " + points.ToString();
    }
}
