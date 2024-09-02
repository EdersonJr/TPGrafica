using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testePick : MonoBehaviour
{
    public Transform holdParent; // O local onde o objeto será mantido (ex.: na mão do jogador)
    private Transform heldObject; // O objeto que está sendo segurado
    private bool isHolding = false;

    void Start()
    {
        // Desativando o bloqueio do cursor para facilitar o teste
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // Aperte 'E' para pegar ou soltar o objeto
        {
            if (isHolding)
            {
                DropObject();
            }
            else
            {
                TryPickupObject();
            }
        }
    }

    void TryPickupObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 20f))
        {
            Debug.Log("Raycast hit: " + hit.transform.name);
            if (hit.transform.CompareTag("Food"))
            {
                // Código para pegar o objeto
                heldObject = hit.transform;
                heldObject.SetParent(holdParent);
                heldObject.position = holdParent.position;
                isHolding = true;
                heldObject.GetComponent<Collider>().enabled = false; // Desativa o collider enquanto estiver segurando
            }
        }
        else
        {
            Debug.Log("Raycast did not hit anything");
        }
    }

    void DropObject()
    {
        if (heldObject != null)
        {
            heldObject.SetParent(null);
            heldObject.GetComponent<Collider>().enabled = true; // Reativa o collider ao soltar
            heldObject = null;
            isHolding = false;
        }
    }
}
