using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//FMOD DISABLED BECAUSE OF PROBLEM WITH AZURE SPEECH RECOGNITION

public class LetterDrag : MonoBehaviour
{
    //agrego cam para poder usar perspectiva en display, y retener el drag que requiere una orthographic cam
    public Camera OrthoCam;

    public LetterSlot assignedSlot; //lo paso a public. Quiero sacar el for loop que no corre bien
    public LetterSlot[] letterSlots;

    //INCORRECT LETTER FADE VARIABLES
    public float incorrectLetterFadeDuration = 1;
    public bool correctLetter = false; //para desaparecer la letra si no es correcta
    Button button;

    //DRAG VARIABLES
    bool dragEnabled = true; //para dejar de mover la letra una vez posicionada
    bool isDragged = false;
    Vector3 mouseDragStartPosition; //para mover la letra

    //CORRECT LETTER IN INCORRECT SLOT VARIABLE
    Vector3 letterDragStartPosition; // para resetear la posicion de la letra a su posicion antes de arrastrar
    Vector3 letterPositionBeforeDrag;
    [SerializeField]float snapRange = 0.3f;

    public bool letterSnappedToSlot = false;

    //COROUTINES FOR MOUSE UP CONSEQUENCES -- WIP
    public bool enableSnapLetterToSlot = false;
    public bool enableResetLetterPosition = false;

    //DELEGATE FUNCTION TO CHECK IF ALL LETTERS ARE IN THEIR SLOTS
    public delegate void LetterInSlotDelegate(LetterDrag draggableLetter);
    public LetterInSlotDelegate letterInSlotCallback;

    /* FMOD DISABLED BECAUSE OF PROBLEM WITH AZURE SPEECH RECOGNITION
    //SOUND EVENT EMITTERS
    [Header("Sound Events")]
    public string LetterMouseOver = "FMOD Event Path";
    public string LetterPressedCorrect = "FMOD Event Path";
    public string LetterPressedIncorrect = "FMOD Event Path";
    public string letterPlacedCorrect = "FMOD Event Path";
    public string letterPlacedIncorrect = "FMOD Event Path";
    */

    bool letterPlacedCorrectPlayed = false;

    //LETTER MOVEMENT
    public float maxSpeed = 1;
    private Vector3 movement;
    public float xMin = -1.5f;
    public float xMax = -1;
    public float yMin;
    public float yMax;
    float yPos;
    float xPos;
    float startYPos;
    //public int interpolationFramesCount = 100; // Number of frames to completely interpolate between the 2 positions
    //int elapsedFrames = 0;
    int frames = 0;
    public int maxFrames = 90;
    //RANDOM LETTERS
    string randomLetter;
    string[] alphabet;


    private void Start()
    {
        /* quiero sacar estos for loops.
        foreach (LetterSlot letterSlot in letterSlots)
        {
            if(letterSlot != null)
            {
                if (this.GetComponentInChildren<TextMeshProUGUI>().text == letterSlot.letterSlot)
                {
                    assignedSlot = letterSlot;
                    correctLetter = true;
                    //Debug.Log("Letter is Correct" + gameObject.name,gameObject);
                }
            }
        }
        */
        StartCoroutine(SnapLetterToSlot());
        StartCoroutine(ResetLetterPosition());
        startYPos = transform.position.y;
        StartCoroutine(RandomPos());

        //RANDOM LETTER
        alphabet = new string[20]
        {
            "Q", "W", "R", "T", "Y", "U", "P", "F", "G", "H", "J", "K", "L", "Z", "X", "C", "V", "B", "N", "M"
        };
    }
    /*
    private void OnMouseEnter()
    {
        if ((!letterSnappedToSlot) && (!isDragged))
        {
            FMODUnity.RuntimeManager.PlayOneShot(LetterMouseOver);
        }
    }
    */
    private void OnMouseDown()
    {
        StartCoroutine(SnapLetterToSlot());
        //StartCoroutine(ResetLetterPosition());
        /*
        //PLAY SOUND
        if ((!letterSnappedToSlot) && (correctLetter))
        {
            FMODUnity.RuntimeManager.PlayOneShot(LetterPressedCorrect);
        }
        */
        //LETTER DRAG
        if (dragEnabled)
        {
            letterPositionBeforeDrag = this.transform.position;
            isDragged = true;
            mouseDragStartPosition = OrthoCam.ScreenToWorldPoint(Input.mousePosition);
            letterDragStartPosition = transform.localPosition; //POSITION BEFORE DRAG
        }
        //INCORRECT LETTER FADE
        if (!correctLetter)
        {
            /*
            FMODUnity.RuntimeManager.PlayOneShot(LetterPressedIncorrect);
            */
            //colorblock();
            StartCoroutine(IncorrectFade());
        }
    }
    private void OnMouseDrag()
    {
        if (isDragged)
        {
            transform.localPosition = letterDragStartPosition + (OrthoCam.ScreenToWorldPoint(Input.mousePosition) - mouseDragStartPosition);
        }
    }
    private void OnMouseUp()
    {
        isDragged = false;

        //TBD - POLISH
        //pase a bools y coroutines en vez de funciones, para que no se ejecute la funcion de snap o reset por cada iteracion de los slots en el loop.
        //pero creo que sigue habiendo un problema. El debug muestra el mismo valor en todos los casos. No creo poder sacar el loop. No entiendo bien como funciona, pero funciona.
        /*
        foreach(LetterSlot letterSlot in letterSlots)
        {
            if (this.GetComponentInChildren<TextMeshProUGUI>().text == assignedSlot.letterSlot)
            {
                if (assignedSlot.correctLetterInCorrectSlotTrigger)
                {
                    enableSnapLetterToSlot = true;
                    Debug.Log("Letter slot: " + letterSlot + ". Snap state: " + enableSnapLetterToSlot + gameObject.name, gameObject);
                }
            }
            if(this.GetComponentInChildren<TextMeshProUGUI>().text != letterSlot.letterSlot)
            {
                if (!letterSlot.correctLetterInCorrectSlotTrigger)
                {
                    enableResetLetterPosition = true;
                }
            }
            //esta funcionando mal cuando suelto y ya estaba en un casillero otro. lo pruebo aca
            StartCoroutine(ResetLetterPosition());
        }*/
        if (assignedSlot.correctLetterInCorrectSlotTrigger)
        {
            enableSnapLetterToSlot = true;
        }
        //aca falta la parte de que resete la position. Pero el for causa problemas con las otras letras. Ya arregle el otro script para implementarlo?

    }

    IEnumerator SnapLetterToSlot()
    {
        yield return new WaitUntil(() => enableSnapLetterToSlot);
        float closestDistance = -1;
        Transform SlotSnapPoint = assignedSlot.transform;

        float currentDistance = Vector2.Distance(this.transform.localPosition, SlotSnapPoint.localPosition); //aca calcula la distancia entre el objeto y el casillero
        if (currentDistance <= snapRange) // aca snappea
        {
            this.transform.localPosition = SlotSnapPoint.localPosition;
            if (enabled)
            {
                LetterSnappedToSlot();
            }
        }
        //StartCoroutine(SnapLetterToSlot());
    }
    //DO THINGS WHEN LETTER IS IN SLOT
    void LetterSnappedToSlot()
    {
        //desactivar interactable boton
        this.gameObject.GetComponent<Button>().interactable = false;
        //desactivar draggableLetter
        dragEnabled = false;
        //LETTER IS IN ASSIGNED SLOT - TO CHECK IF THEY ALL ARE IN LETTER SLOT MANAGER SCRIPT
        letterSnappedToSlot = true;

        Debug.Log("Letter " + gameObject.name + " dragged to " + assignedSlot + gameObject.name,gameObject);
        /*
        //PLAY SOUND EVENT: LetterPlacedCorrectSlot
        if (!letterPlacedCorrectPlayed)
        {
            FMODUnity.RuntimeManager.PlayOneShot(letterPlacedCorrect);
        }
        */
        //llamar al LetterSlotManager para correr la funcion AllLettersInSlots
        letterInSlotCallback(this);

        //frenar el script. Para frenar el sonido al clickear cuando ya esta armada la palabra.
        StopCoroutine(SnapLetterToSlot());
        enabled = false;
    }

    //RESET POSITION IF CORRECT LETTER IS PLACED ON INCORRECT SLOT
    IEnumerator ResetLetterPosition()
    {
        yield return new WaitUntil(()=> enableResetLetterPosition);
        /*
        //PLAY SOUND EVENT: LetterPlacedCorrectSlot
        FMODUnity.RuntimeManager.PlayOneShot(letterPlacedIncorrect);
        */
        //reset position
        this.transform.position = letterPositionBeforeDrag;

        enableResetLetterPosition = false;
    }

    //INCORRECT LETTER FADE -- TBD -- Polish -- podria ser una funcion con una coroutine helper function con parametro de tiempo para reciclarlo en las otras coroutines
    public IEnumerator IncorrectFade()
    {
        //cambio la duracion del fade duration a 1s y espero a que termine para desactivar el boton

        button = GetComponent<Button>();

        ColorBlock cBlock = button.colors;
        button.colors = cBlock;
        yield return new WaitForSeconds(cBlock.fadeDuration); //espero que termine el highlight fade duration -- highlight color fade duration = 0.1s
        cBlock.fadeDuration = incorrectLetterFadeDuration;
        button.colors = cBlock;
        yield return new WaitForSeconds(cBlock.fadeDuration); //espero a que termine el fade duration

        GetComponent<Button>().interactable = !GetComponent<Button>().interactable; //desactivo la interaccion (disabled color)

        this.gameObject.SetActive(false);
    }

    //LETTER MOVEMENT
    void LetterMovement()
    {
        if (!isDragged && !enableSnapLetterToSlot && !correctLetter)
        {
            movement = new Vector3(transform.position.x - 1, transform.position.y + yPos, 0);
            //transform.position = transform.position + movement * (maxSpeed * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, movement, (maxSpeed * Time.deltaTime));

            //LIMITES PANTALLA
            if (transform.position.x < -4.42f) //limite izquiero
            {
                Vector3 resetPosX = new Vector3(4.61f, transform.position.y, transform.position.z);
                transform.position = resetPosX;
                //CHANGE LETTER TO A RANDOM LETTER
                RandomLetter();

            }
            if (transform.position.y > 2.6f) //limite superior
            {
                Vector3 resetPosY = new Vector3(transform.position.x, -2.97f, transform.position.z);
                transform.position = resetPosY;
                //
            }
            
        }
        if (!isDragged && !enableSnapLetterToSlot && correctLetter)
        {
            transform.position = Vector3.MoveTowards(transform.position, assignedSlot.transform.position, (maxSpeed * Time.deltaTime));
            if (transform.position == assignedSlot.transform.position)
            {
                enableSnapLetterToSlot = true;
            }
        }
    }
    //GENERAR POSICIONES ALEATORIAS PARA MOVIMIENTO LETRAS INCORRECTAS
    IEnumerator RandomPos()
    {
        yield return new WaitForSeconds(1);
        yPos = Random.Range(yMin, yMax);
        //xPos = Random.Range(transform.position.x + xMin, transform.position.x + xMax);
        StartCoroutine(RandomPos());
    }
    //GENERAR LETRAS ALEATORIAS AL VOLVER A APARECER DEL OTRO LADO
    void RandomLetter()
    {
        if (!gameObject.activeInHierarchy) return;

        randomLetter = alphabet[Random.Range(0, alphabet.Length)];
        this.GetComponentInChildren<TextMeshProUGUI>().text = randomLetter;
    }

    private void Update()
    {
        LetterMovement();
    }
}
