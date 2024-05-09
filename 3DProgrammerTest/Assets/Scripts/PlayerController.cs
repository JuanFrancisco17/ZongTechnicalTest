using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [Header("Inputs")]

    [SerializeField] InputActionAsset playerControls;
    [SerializeField, HideInInspector] InputAction clickToPickAction;
    public bool isPick { get; private set; }
    [SerializeField, HideInInspector] InputAction clickToPutAction;
    public bool isPut { get; private set; }

    [Header("Movement")]

    [SerializeField] float speed;
    [SerializeField] float sensitivity;
    [SerializeField] float maxForce;
    private Rigidbody rb;
    private Vector2 move, look;
    private float lookRotation;
    private RaycastHit hit;

    [Header("Camera")]

    [SerializeField] GameObject cameraPivot;
    private Transform cameraTransform;

    [Header("PickUp Mechanic")]

    [SerializeField] LayerMask itemLayer;
    [SerializeField] float hitRange;

    [Header("Inventory")]

    [SerializeField] GameObject inventoryObject;
    public ItemsSO weaponsSO, instrumentsSO;
    private GameObject canvasObject;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;
        clickToPickAction.Enable();
        clickToPutAction.Enable();
        clickToPickAction = playerControls.FindActionMap("Player").FindAction("Pick");
        clickToPutAction = playerControls.FindActionMap("Player").FindAction("Put");
    }

    void Update()
    {
        //Debug.DrawRay(cameraTransform.position, cameraTransform.forward * hitRange, Color.red);
        PickUpItems();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            InventorySystem.instance.OpenInventory();
        }
    }

    void FixedUpdate()
    {
        Movement();
    }

    void LateUpdate()
    {
       Rotation();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();

    }

    public void OnLook(InputAction.CallbackContext context)
    {
        look = context.ReadValue<Vector2>();
    }

    public void OnClickToPick(InputAction.CallbackContext context)
    {
        clickToPickAction.performed += context => isPick = true;
        clickToPickAction.canceled += context => isPick = false;
    }

    public void OnPressToPut(InputAction.CallbackContext context)
    {
        clickToPutAction.performed += context => isPut = true;
        clickToPutAction.canceled += context => isPut = false;
    }

    private void Movement()
    {
        
        Vector3 currentVelocity = rb.velocity;
        Vector3 targetVelocity = new Vector3(move.x, 0f, move.y);
        targetVelocity *= speed;

        targetVelocity = transform.TransformDirection(targetVelocity);

        Vector3 changeVelocity = targetVelocity - currentVelocity;

        Vector3.ClampMagnitude(changeVelocity, maxForce);

        rb.AddForce(changeVelocity, ForceMode.VelocityChange);
    }

    private void Rotation()
    {
        transform.Rotate(Vector3.up * look.x * sensitivity);
        lookRotation += (-look.y * sensitivity);
        lookRotation = Mathf.Clamp(lookRotation, -90f, 90f);
        cameraPivot.transform.eulerAngles = new Vector3(lookRotation, cameraPivot.transform.eulerAngles.y, cameraPivot.transform.eulerAngles.z);
    }

    private void PickUpItems()
    {
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, hitRange, itemLayer))
        {
            canvasObject = hit.collider.transform.GetChild(0).gameObject;
            canvasObject.SetActive(true);

            //If Player press the input to pick...
            if (isPick)
            {
                hit.collider.gameObject.transform.SetParent(inventoryObject.transform);
                hit.collider.transform.position = inventoryObject.transform.position;

                canvasObject.SetActive(false);

                if(InventorySystem.instance.CheckIfItemHave(weaponsSO, InventorySystem.instance.weapons) == false)
                    InventorySystem.instance.PutOnInventory(weaponsSO, hit.collider.gameObject, InventorySystem.instance.weapons, InventorySystem.instance.weaponsSlots);

                if(InventorySystem.instance.CheckIfItemHave(instrumentsSO, InventorySystem.instance.instruments) == false)
                    InventorySystem.instance.PutOnInventory(instrumentsSO, hit.collider.gameObject, InventorySystem.instance.instruments, InventorySystem.instance.instrumentsSlots);
            }
        }
        else if (canvasObject != null)
        {
            canvasObject.SetActive(false);
            canvasObject = null;
        }
    }
}
