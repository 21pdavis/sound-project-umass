﻿//
// Copyright 2017-2023 Valve Corporation. Subject to the following license:
// https://valvesoftware.github.io/steam-audio/license.html
//
#if STEAMAUDIO_ENABLED

using System;
using UnityEngine;

namespace SteamAudio
{
    public sealed class UnityAudioEngineState : AudioEngineState
    {
        public override void Initialize(IntPtr context, IntPtr defaultHRTF, SimulationSettings simulationSettings, PerspectiveCorrection correction)
        {
            API.iplUnityInitialize(context);
            API.iplUnitySetHRTF(defaultHRTF);
            API.iplUnitySetSimulationSettings(simulationSettings);
            API.iplUnitySetPerspectiveCorrection(correction);
        }

        public override void Destroy()
        {
            API.iplUnityTerminate();
        }

        public override void SetHRTF(IntPtr hrtf)
        {
            API.iplUnitySetHRTF(hrtf);
        }

        public override void SetPerspectiveCorrection(PerspectiveCorrection correction)
        {
            API.iplUnitySetPerspectiveCorrection(correction);
        }

        public override void SetReverbSource(Source reverbSource)
        {
            API.iplUnitySetReverbSource(reverbSource.Get());
        }
    }

    public sealed class UnityAudioEngineStateHelpers : AudioEngineStateHelpers
    {
        public override Transform GetListenerTransform()
        {
            var audioListener = GameObject.FindObjectOfType<AudioListener>();
            return (audioListener != null) ? audioListener.transform : null;
        }

        public override AudioSettings GetAudioSettings()
        {
            var audioSettings = new AudioSettings { };

            audioSettings.samplingRate = UnityEngine.AudioSettings.outputSampleRate;

            var numBuffers = 0;
            UnityEngine.AudioSettings.GetDSPBufferSize(out audioSettings.frameSize, out numBuffers);

            return audioSettings;
        }
    }
}

#endif
