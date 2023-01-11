////
//// Copyright (c) Microsoft. All rights reserved.
//// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
////
//// <code>
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

//public class HelloWorld : MonoBehaviour
//{
//    public SpeechRecognition speechRecognition;
//    //public bool speechCorrect;
//    // Hook up the two properties below with a Text and Button object in your UI.
//    public Text outputText;
//    public Button startRecoButton;

//    private object threadLocker = new object();
//    private bool waitingForReco;
//    private string message;

//    private bool micPermissionGranted = false;

//#if PLATFORM_ANDROID || PLATFORM_IOS
//    // Required to manifest microphone permission, cf.
//    // https://docs.unity3d.com/Manual/android-manifest.html
//    private Microphone mic;
//#endif

//    public async void ButtonClick()
//    {
//        //RECOGNIZE FROM MICROPHONE 
//        //https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/get-started-speech-to-text?tabs=windowsinstall&pivots=programming-language-csharp#recognize-from-microphone
//        //habria de probar tambien desde audio files de las pruebas online de les chiques

//        // Creates an instance of a speech config with specified subscription key and service region.
//        // Replace with your own subscription key and service region (e.g., "westus").
//        var config = SpeechConfig.FromSubscription("65fcf04b1c36450aa9d9d8d62b5a6b00", "eastus");

//        //AGREGO ESTO PARA PROBAR CON OTRO IDIOMA
//        //https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/get-started-speech-to-text?tabs=windowsinstall&pivots=programming-language-csharp#change-source-language
//        //AVAILABLE LANGUAGES
//        //https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/language-support
//        config.SpeechRecognitionLanguage = "es-AR";
//        //funciona pero igual no parece ser la mejor manera
//        //https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/how-to-specify-source-language
//        //Creo que la mejor manera es esta. Es algo que habria de hacer para que solo traduzca en el idioma indicado, sino pierde eficiencia
//        //solo habria de traducir si el idioma detectado es espanol
//        //igual no se si cuenta como texto traducido esta lectura, y es cobrado. Por lo pronto, lee pero no traduce
//        //https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/language-identification?pivots=programming-language-csharp

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
//                newMessage = "NOMATCH: Speech could not be recognized.";
//            }
//            else if (result.Reason == ResultReason.Canceled)
//            {
//                var cancellation = CancellationDetails.FromResult(result);
//                newMessage = $"CANCELED: Reason={cancellation.Reason} ErrorDetails={cancellation.ErrorDetails}";
//            }

//            lock (threadLocker)
//            {
//                message = newMessage;
//                waitingForReco = false;
//            }
//        }
//    }

//    void Start()
//    {
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
//            message = "Click button to recognize speech";
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
//                CompareSpeech();
//            }
//        }
//    }

//    void CompareSpeech()
//    {
//        string correctMessage = "Odisea.";
//        if (message == correctMessage)
//        {
//            //Debug.Log("Speech recognition correct");
//            speechRecognition.PlayTimeline();
//            //speechCorrect = true;
//        }
//        else
//        {
//            //Debug.Log("Speech recognition incorrect");
//        }
//    }
//}
//// </code>
