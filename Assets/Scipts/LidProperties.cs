using UnityEngine;

public class LidProperties : MonoBehaviour
{
    // Propriedade para verificar se a tampa está sendo usada
    public bool isOnPan = false;

    public void PlaceOnPan()
    {
        isOnPan = true;
        // Aqui você pode adicionar lógica para fazer a tampa se mover para a posição correta na frigideira
        Debug.Log("Tampa colocada na frigideira.");
    }

    public void RemoveFromPan()
    {
        isOnPan = false;
        // Lógica para quando a tampa é removida
        Debug.Log("Tampa removida da frigideira.");
    }
}
