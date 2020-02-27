﻿//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
//

using System;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace speech
{
    class Program
    {
        // It's always a good idea to access services in an async fashion
        static async Task Main()
        {
            await RecognizeSpeechAsync();
        }

        static async Task RecognizeSpeechAsync()
        {
            // Configure the subscription information for the service to access.
            // Use either key1 or key2 from the Speech Service resource you have created
            // replace YOUR_REGION with the region in which you deployed the cognitive service
            // such as "westus"
            var config = SpeechConfig.FromSubscription("YOUR_KEY", "YOUR_REGION");

            // Setup the audio configuration, in this case, using a file that is in local storage.
            using (var audioInput = AudioConfig.FromWavFileInput("../../data/office-auto/light-on.wav"))

            // Pass the required parameters to the Speech Service which includes the configuration information
            // and the audio file name that you will use as input
            using (var recognizer = new SpeechRecognizer(config, audioInput))
            {
                Console.WriteLine("Recognizing first result...");
                var result = await recognizer.RecognizeOnceAsync();

                switch (result.Reason)
                {
                    case ResultReason.RecognizedSpeech:
                        // The file contained speech that was recognized and the transcription will be output
                        // to the terminal window
                        Console.WriteLine($"We recognized: {result.Text}");
                        break;
                    case ResultReason.NoMatch:
                        // No recognizable speech found in the audio file that was supplied.
                        // Out an informative message
                        Console.WriteLine($"NOMATCH: Speech could not be recognized.");
                        break;
                    case ResultReason.Canceled:
                        // Operation was cancelled
                        // Output the reason
                        var cancellation = CancellationDetails.FromResult(result);
                        Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                        if (cancellation.Reason == CancellationReason.Error)
                        {
                            Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                            Console.WriteLine($"CANCELED: ErrorDetails={cancellation.ErrorDetails}");
                            Console.WriteLine($"CANCELED: Did you update the subscription info?");
                        }
                        break;
                }
            }
        }
    }
}
