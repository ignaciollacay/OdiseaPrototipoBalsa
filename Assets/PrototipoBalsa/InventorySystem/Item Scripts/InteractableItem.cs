using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using StarterAssets;
using Cinemachine;

/// <summary>
/// Interaction Controller.
/// 
/// Convendrá attachear los controllers al Player?
/// Quizás incluso unir al PlayerManager y quedar con un interaction controller, que es lo que parece terminar siendo.
/// Una extensión del Character Controller, pero para Interactions.
/// 
/// La idea era tenerlos separados para que no tenga que crear nuevas subclases en el player controller
/// cada vez que agregue un item que altere el controller,
/// pero creo que el Interaction behaviour es igual?
/// Al menos las animaciones sé que van a variar.
/// Si es solo eso, quizás conviene que sea una variable privada en el player,
/// Y cambiarla al equipar un interactable item
///
/// Porque quizás es raro andar cambiando todas estas cosas en runtime al equipar objetos,
/// cuando las variables del comportamiento son fijas,
/// y solo varía el comportamiento según el Interaction type
///
/// Podría comunicarme directamente entre equipment y controllers:
///     Equipment inventory
///     Interaction Controller.
///     Inputs Controller
/// Sacando del medio los item gameobjects.
/// </summary>
public abstract class InteractableItem : MonoBehaviour
{
    //Player Instance static variables
    protected Animator playerAnimator;
    protected ThirdPersonController playerController;
    protected RayCast playerRaycast;
    protected StarterAssetsInputs playerInputs;

    //public GameEvent_GameObject OnEquip;
    //public GameEvent_GameObject OnUnequip;

    public virtual void Awake()
    {
        playerAnimator = PlayerManager.Instance.playerAnimator;
        playerController = PlayerManager.Instance.playerController;
        playerRaycast = PlayerManager.Instance.raycast;
        playerInputs = PlayerManager.Instance.playerInputs;

        //TODO Disable collider if equipped, else Enable
        //lo prendería solo en remove?
        //porque sino quizas joda en el momento que lo equipa?
        //lo pickupeará apenas se instancea?
    }

    //private void Start()
    //{
    //    //OnEquip.RaiseEvent(this.gameObject);
    //    //GetComponent<ItemSO>().OnEquip.RaiseEvent(this.gameObject); //sería algo así para que el vínculo del evento este guardado en el SO y usarlo.
    //}
    //private void OnDestroy()
    //{
    //    //OnUnequip.RaiseEvent(this.gameObject);
    //}

    public virtual void OnButtonDown()
    {
        PlayerManager.Instance.raycast.enabled = true;
    }
    public virtual void OnButtonUp()
    {
        playerRaycast.enabled = false;
    }
}