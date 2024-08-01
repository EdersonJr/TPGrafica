using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : MonoBehaviour
{
    public GameObject myHands; // Referência às suas mãos/a posição onde você quer que o objeto vá
    bool canpickup; // Verifica se você pode ou não pegar o item
    GameObject ObjectIwantToPickUp; // O GameObject com o qual você colidiu
    bool hasItem; // Verifica se você tem um item na mão
    
    void Start()
    {
        canpickup = false;    // Inicializando ambos como falso
        hasItem = false;
    }

    void Update()
    {
        if (canpickup == true) // Se você entrar no colisor do objeto
        {
            if (Input.GetKey(KeyCode.I)) // Pode ser "e" ou qualquer tecla
            {
                Debug.Log("pressed I");
                PickupItem(); // Separado em um método
            }
        }
        if (Input.GetKey(KeyCode.O) && hasItem == true) // Se você tem um item e pressiona a tecla para soltar o objeto, pode ser qualquer tecla
        {
            Debug.Log("pressed O");
            DropItem(); // Separado em um método
        }
    }

    private void PickupItem()
    {
        Rigidbody objectRb = ObjectIwantToPickUp.GetComponent<Rigidbody>();
        objectRb.isKinematic = true; // Faz com que o Rigidbody não seja afetado por forças
        objectRb.detectCollisions = false; // Desativa colisões temporariamente

        // Define a posição e a rotação do objeto para corresponder à mão
        ObjectIwantToPickUp.transform.position = myHands.transform.position;
        ObjectIwantToPickUp.transform.rotation = myHands.transform.rotation;
        ObjectIwantToPickUp.transform.parent = myHands.transform; // Faz o objeto se tornar um filho da mão

        // Zera a velocidade do personagem
        Rigidbody characterRb = GetComponent<Rigidbody>();
        characterRb.velocity = Vector3.zero;
        characterRb.angularVelocity = Vector3.zero;

        hasItem = true;
    }

    private void DropItem()
    {
        Rigidbody objectRb = ObjectIwantToPickUp.GetComponent<Rigidbody>();
        objectRb.isKinematic = false; // Faz com que o Rigidbody volte a funcionar
        objectRb.detectCollisions = true; // Reativa as colisões
        ObjectIwantToPickUp.transform.parent = null; // Faz o objeto deixar de ser um filho da mão
        objectRb.velocity = Vector3.zero; // Zera a velocidade do objeto
        objectRb.angularVelocity = Vector3.zero; // Zera a velocidade angular do objeto
        hasItem = false;
    }

    private void OnTriggerEnter(Collider other) // Para ver quando o player entra no colisor
    {
        Debug.Log("can pick up object");
        if (other.gameObject.tag == "PickUp") // No objeto que você quer pegar, defina a tag como "PickUp"
        {
            canpickup = true;  // Define a bool de pegar como true
            ObjectIwantToPickUp = other.gameObject; // Define o GameObject com o qual você colidiu como um que você pode referenciar
        }
    }

    private void OnTriggerExit(Collider other)
    {
        canpickup = false; // Quando você sai do colisor, define a bool de pegar como false
    }
}
