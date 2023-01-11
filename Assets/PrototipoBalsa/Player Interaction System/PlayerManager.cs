using UnityEngine;
using StarterAssets;
using Cinemachine;

/// <summary>
/// Player Manager Singleton
/// Creates an Instance of the Player to get its properties in InteractableItems
/// </summary>

[DefaultExecutionOrder(-1)]
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; set; }

    [Header("Player Attributes")]
    //get
    public Animator playerAnimator;
    public ThirdPersonController playerController;
    public StarterAssetsInputs playerInputs;
    //set
    [SerializeField] private Transform playerTransform; //used in CharacterAimRotation() by InteractableItem Button hold => Raycast
    //TODO No sé si termino updateando al player o solo el valor de acá
    public Transform PlayerTransform
    {
        get
        {
            return playerTransform;
        }
        private set
        {
            playerTransform = value;
        }
    }

    public RayCast raycast;

    [Header("Aim Attributes")]
    public CinemachineVirtualCamera playerAimCamera;
    public Transform projectileSpawnPosition;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            Debug.Log("more than one instance of player manager");
        }
        
    }
}