////
//// Copyright (c) Microsoft. All rights reserved.
//// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
////
//// <code>
//using System.Collections;
//using UnityEngine;
//using UnityEngine.UI;
//using Microsoft.CognitiveServices.Speech;
//#if PLATFORM_ANDROID
//using UnityEngine.Android;
//#endif
//#if PLATFORM_IOS
//using UnityEngine.iOS;
//using System.Collections;
//#endif
//using UnityEngine.Playables;

//public class SpeechRecognition : MonoBehaviour
//{
//    public PlayableDirector playableDirector;

//    public bool speechIsCorrect;
//    public string correctSpeech;
//    public string incorrectSpeechMessage;
//    public GameObject helpText;
//    public GameObject errorUI;

//    //DELEGATE FUNCTION TO CHECK IF ALL LETTERS ARE IN THEIR SLOTS
//    //public delegate void SpeechRecognitionDelegate(SpeechRecognition speechRecognizer);
//    //public SpeechRecognitionDelegate SpeechRecognizedCallback;

//    // Hook up the two properties below with a Text and Button object in your UI.
//    public Text outputText;
//    public Button startRecoButton;

//    private object threadLocker = new object();
//    private bool waitingForReco;
//    private string message;

//    private bool micPermissionGranted = false;

//    public bool speechRecognized = false;

//#if PLATFORM_ANDROID || PLATFORM_IOS
//    // Required to manifest microphone permission, cf.
//    // https://docs.unity3d.com/Manual/android-manifest.html
//    private Microphone mic;
//#endif

//    public async void ButtonClick()
//    {
//        var config = SpeechConfig.FromSubscription("65fcf04b1c36450aa9d9d8d62b5a6b00", "eastus");
//        config.SpeechRecognitionLanguage = "es-AR";

//        // Make sure to dispose the recognizer after use!
//        using (var recognizer = new SpeechRecognizer(config))
//        {
//            lock (threadLocker)
//            {
//                waitingForReco = true;
//            }

//            // Starts speech recognition, and returns after a single utterance is recognized. The end of a
//            // single utterance is determined by listening for silence at the end or until a maximum of 15
//            // seconds of audio is processed.  The task returns the recognition text as result.
//            // Note: Since RecognizeOnceAsync() returns only a single utterance, it is suitable only for single
//            // shot recognition like command or query.
//            // For long-running multi-utterance recognition, use StartContinuousRecognitionAsync() instead.
//            var result = await recognizer.RecognizeOnceAsync().ConfigureAwait(false);

//            //ERROR HANDLING
//            //https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/get-started-speech-to-text?tabs=windowsinstall&pivots=programming-language-csharp#error-handling

//            // Checks result.
//            string newMessage = string.Empty;
//            if (result.Reason == ResultReason.RecognizedSpeech)
//            {
//                newMessage = result.Text;
//            }
//            else if (result.Reason == ResultReason.NoMatch)
//            {
//                newMessage = "NOMATCH: La voz no pudo ser reconocida.";
//            }
//            else if (result.Reason == ResultReason.Canceled)
//            {
//                var cancellation = CancellationDetails.FromResult(result);
//                newMessage = $"CANCELED: Reason={cancellation.Reason} ErrorDetails={cancellation.ErrorDetails}";

//                //agregado por reconocimiento que no anda en iOS
//                ErrorHelper(); 
//            }

//            lock (threadLocker)
//            {
//                message = newMessage;
//                waitingForReco = false;
//            }

//        }
//        speechRecognized = true;
//    }

//    void Start()
//    {
//        StartCoroutine(OnRecognition());

//        if (outputText == null)
//        {
//            UnityEngine.Debug.LogError("outputText property is null! Assign a UI Text element to it.");
//        }
//        else if (startRecoButton == null)
//        {
//            message = "startRecoButton property is null! Assign a UI Button to it.";
//            UnityEngine.Debug.LogError(message);
//        }
//        else
//        {
//            // Continue with normal initialization, Text and Button objects are present.
//#if PLATFORM_ANDROID
//            // Request to use the microphone, cf.
//            // https://docs.unity3d.com/Manual/android-RequestingPermissions.html
//            message = "Waiting for mic permission";
//            if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
//            {
//                Permission.RequestUserPermission(Permission.Microphone);
//            }
//#elif PLATFORM_IOS
//            if (!Application.HasUserAuthorization(UserAuthorization.Microphone))
//            {
//                Application.RequestUserAuthorization(UserAuthorization.Microphone);
//            }
//#else
//            micPermissionGranted = true;
//            message = "Click button to recognize speech";
//#endif
//            startRecoButton.onClick.AddListener(ButtonClick);
//        }
//    }

//    void Update()
//    {
//#if PLATFORM_ANDROID
//        if (!micPermissionGranted && Permission.HasUserAuthorizedPermission(Permission.Microphone))
//        {
//            micPermissionGranted = true;
//            message = "Lea la palabra formada. Pulse el boton para hablar";
//        }
//#elif PLATFORM_IOS
//        if (!micPermissionGranted && Application.HasUserAuthorization(UserAuthorization.Microphone))
//        {
//            micPermissionGranted = true;
//            message = "Click button to recognize speech";
//        }
//#endif

//        lock (threadLocker)
//        {
//            if (startRecoButton != null)
//            {
//                startRecoButton.interactable = !waitingForReco && micPermissionGranted;
//            }
//            if (outputText != null)
//            {
//                outputText.text = message;
//            }
//        }
//    }

//    IEnumerator OnRecognition()
//    {
//        yield return new WaitUntil(() => speechRecognized);
//        if (message == correctSpeech)
//        {
//            outputText.color = Color.blue;
//            StopCoroutine(HelpMessage());
//            //quitar help message
//            helpText.SetActive(false);
//            //cambiar estado bool
//            speechIsCorrect = true;
//            Debug.Log("Speech result is: " + speechIsCorrect);
//            //cambiar color texto traducido a correcto
//            //Comunicar a scripts que el speech recognition fue correcto
//            //SpeechRecognizedCallback(this);

//            PlayTimeline();
//        }
//        else
//        {
//            speechRecognized = false;
//            speechIsCorrect = false;
//            Debug.Log("Speech result is: " + speechIsCorrect);
//            //cambiar color del texto traducido a incorrecto
//            outputText.color = Color.red;
//            //help message
//            incorrectSpeechMessage = "La palabra no fue leida correctamente. Intente nuevamente.";
//            helpText.GetComponentInChildren<Text>().text = incorrectSpeechMessage;
//            StartCoroutine(OnRecognition());
//        }
//    }

//    public void PlayTimeline()
//    {
//        playableDirector.Play();
//    }
    
//    public void HelpMessageStart()
//    {
//        StartCoroutine(HelpMessage());
//    }

//    IEnumerator HelpMessage()
//    {
//        yield return new WaitForSeconds(5);
//        helpText.SetActive(true);
//    }

//    void ErrorHelper()
//    {
//        HelpMessageStart();
//        incorrectSpeechMessage = "Pulse el bot?n de 'Ayuda' para continuar";
//        helpText.GetComponentInChildren<Text>().text = incorrectSpeechMessage;
//        errorUI.SetActive(true);
//    }
//}
