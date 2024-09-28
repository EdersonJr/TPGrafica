using UnityEngine;
using TMPro; // Usando TextMeshPro

public class ConsoleToScreen : MonoBehaviour
{
    public TextMeshProUGUI consoleText; // Referência ao TextMeshProUGUI
    private string logMessages = ""; // Para armazenar o histórico de mensagens

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog; // Inscreve no evento de log
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog; // Cancela a inscrição no evento de log
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        // Limpa as mensagens antigas e adiciona a nova mensagem ao histórico
        logMessages = logString + "\n"; // Apenas a nova mensagem

        if (consoleText != null) // Verifica se a referência ao TextMeshProUGUI está definida
        {
            consoleText.text = logMessages; // Atualiza o texto com a nova mensagem
        }
        else
        {
            Debug.LogError("TextMeshProUGUI não está atribuído no Inspector.");
        }
    }
}
