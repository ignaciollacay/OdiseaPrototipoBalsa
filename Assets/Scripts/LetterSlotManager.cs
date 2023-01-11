using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

//FMOD DISABED DUE TO INCOMPATIBILITY WITH AZURE SPEECH RECOGNITION

public class LetterSlotManager : MonoBehaviour
{
    [Header("Linked GameObjects")]
    public LetterDrag[] draggableLetters;
    public GameObject slotWireframe;
    public GameObject phraseRecognition;
    public ParticleSystem cloudsFront;
    /*
    //SOUND EVENT EMITTERS
    [Header("Sound Events")]
    public string allLettersInSlots = "FMOD Event Path";
    */
    [SerializeField]int maxScore = 6;
    
    //MATERIALS FOR GLOW
    Material letterMat;
    Material letterMatGlow;

    //Play Win Sound only once
    bool allLettersInSlotsPlayed = false;

    //CHECK IF ALL LETTERS ARE IN SLOTS ON EACH LETTER IN SLOT CALLBACK
    private void Start()
    {
        foreach (LetterDrag draggableLetter in draggableLetters)
        {
            draggableLetter.letterInSlotCallback = AllLettersInSlotsCheck;
            
            if (draggableLetter.correctLetter)
            {
                letterMat = draggableLetter.gameObject.GetComponentInChildren<TextMeshProUGUI>().fontSharedMaterial;
            }
        }
        letterMatGlow = new Material(letterMat);
        letterMatGlow.SetFloat(ShaderUtilities.ID_GlowPower, 1f);
    }

    void AllLettersInSlotsCheck(LetterDrag LetterinSlot)
    {
        int currentScore = 0;

        //ADD POINT TO SCORE FOR EACH LETTER IN SLOT
        foreach (LetterDrag draggableLetters in draggableLetters)
        {
            if (draggableLetters.letterSnappedToSlot)
            {
                currentScore++;
            }
        }
        //CHECK IF ALL LETTERS ARE IN SLOTS (IF MAX SCORE IS REACHED)
        if (currentScore == maxScore)
        {
            AllLettersInSlots();
        }
        //IF MAX SCORE IS NOT REACHED, RESET SCORE FOR THE NEXT CALLBACK
        else
        {
            currentScore = 0;
        }
    }

    //DO THINGS ONCE ALL LETTERS ARE IN SLOTS (MAX SCORE IS REACHED)
    void AllLettersInSlots()
    {
        //PLAY WORD FORMED SOUND
        if (!allLettersInSlotsPlayed)
        {
            /*
            FMODUnity.RuntimeManager.PlayOneShot(allLettersInSlots);
            */
            allLettersInSlotsPlayed = true;
        }
        
        //DESACTIVAR LOS SLOTS
        slotWireframe.SetActive(false);
        //ANIMACIONES DE LAS LETRAS
        foreach (LetterDrag draggableLetter in draggableLetters)
        {
            //DESACTIVAR LETRAS INCORRECTAS
            if (!draggableLetter.correctLetter)
            {
                StartCoroutine(draggableLetter.IncorrectFade());
            }
            //GLOW LETRAS CORRECTAS
            else
            {
                draggableLetter.gameObject.GetComponentInChildren<TextMeshProUGUI>().fontSharedMaterial = letterMatGlow;
            }
            //DISABLE COROUTINES FROM LETTER DRAG. -- TBD POLISH -- Si limpio algunas coroutines podria no ejecutarlo aca, medio desprolijo
            //principalmente molestaba el random pos.
            draggableLetter.StopAllCoroutines();
        }

        //DESACTIVAR LAS NUBES DEL FRENTE -- faltaria destruir o parar el particle system TBD
        int cloudFadeMaxParticles = 0;
        int cloudFadeSimSpeed = 3;
        ParticleSystem.MainModule cloudFade = cloudsFront.GetComponent<ParticleSystem>().main;
        cloudFade.maxParticles = cloudFadeMaxParticles;
        cloudFade.simulationSpeed = cloudFadeSimSpeed;

        //ACTIVATE PHRASE RECOGNITION
        //SpeechRecognition speechRecognizer = phraseRecognition.GetComponent<SpeechRecognition>();
        //phraseRecognition.SetActive(true);
        //speechRecognizer.enabled = true;
        //Debug.Log("Phrase recognition state: " + speechRecognizer.enabled);
        //speechRecognizer.correctSpeech = "Odisea.";
        //Debug.Log("Phrase to be recognized: " + speechRecognizer.correctSpeech);

        //speechRecognizer.HelpMessageStart();
    }
}
