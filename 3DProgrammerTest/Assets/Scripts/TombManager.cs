using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering;
using UnityEngine.InputSystem;

public class TombManager : MonoBehaviour
{
    private GameObject objectPosition;

    public UnityEvent TombActivate;

    [Header("UI")]

    [SerializeField] TextMeshProUGUI text;
    private GameObject canvas;

    void Start()
    {
        objectPosition = gameObject.transform.GetChild(0).gameObject;
        canvas = gameObject.transform.GetChild(1).gameObject;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Key Grave")
        {
            canvas.SetActive(true);

            if (PlayerController.instance.isPut)
            {
                canvas.SetActive(false);
                other.gameObject.transform.position = objectPosition.transform.position;
                other.gameObject.transform.rotation = objectPosition.transform.rotation;
                other.gameObject.transform.SetParent(objectPosition.transform);

                InventorySystem.instance.RemoveFromInventory(PlayerController.instance.instrumentsSO, other.gameObject, InventorySystem.instance.instruments, InventorySystem.instance.instrumentsSlots);
                
                TombActivate.Invoke();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        canvas.SetActive(false);
    }

    IEnumerator ActivateTextCoroutine(string t)
    {
        text.text = t;
        text.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        text.gameObject.SetActive(false);
    }

    public void ActivateText(string t)
    {
        StartCoroutine(ActivateTextCoroutine(t));
    }
}
