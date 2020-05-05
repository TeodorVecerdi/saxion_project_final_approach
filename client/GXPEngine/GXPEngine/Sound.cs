using System;
using System.Collections.Generic;

namespace GXPEngine {
    /// <summary>
    ///     The Sound Class represents a Sound resource in memory
    ///     You can load .mp3, .ogg or .wav
    /// </summary>
    public class Sound {
        private static int _system;
        private static readonly Dictionary<string, int> _soundCache = new Dictionary<string, int>();

        private readonly int _id;

        /// <summary>
        ///     Creates a new <see cref="GXPEngine.Sound" />.
        ///     This class represents a sound file.
        ///     Sound files are loaded into memory unless you set them to 'streamed'.
        ///     An optional parameter allows you to create a looping sound.
        /// </summary>
        /// <param name='filename'>
        ///     Filename, should include path and extension.
        /// </param>
        /// <param name='looping'>
        ///     If set to <c>true</c> the sound file repeats itself. (It loops)
        /// </param>
        /// <param name='streaming'>
        ///     If set to <c>true</c>, the file will be streamed rather than loaded into memory.
        /// </param>
        /// <param name='cached'>
        ///     If set to <c>true</c>, the sound will be stored in cache, preserving memory when creating the same sound multiple
        ///     times.
        /// </param>
        public Sound(string filename, bool looping = false, bool streaming = false) {
            if (_system == 0) {
                // if fmod not initialized, create system and init default
                FMOD.System_Create(out _system);
                FMOD.System_Init(_system, 32, 0, 0);
            }

            uint loop = FMOD.FMOD_LOOP_OFF; // no loop
            if (looping) loop = FMOD.FMOD_LOOP_NORMAL;
            if (streaming) {
                FMOD.System_CreateStream(_system, filename, loop, 0, out _id);
                if (_id == 0) throw new Exception("Sound file not found: " + filename);
            } else {
                if (_soundCache.ContainsKey(filename)) {
                    _id = _soundCache[filename];
                } else {
                    FMOD.System_CreateSound(_system, filename, loop, 0, out _id);
                    if (_id == 0) throw new Exception("Sound file not found: " + filename);
                    _soundCache[filename] = _id;
                }
            }
        }

        ~Sound() { }

        internal static void Step() {
            //if (_system != 0) 
            FMOD.System_Update(_system);
        }

        /// <summary>
        ///     Play the specified paused and return the newly created SoundChannel
        /// </summary>
        /// <param name='paused'>
        ///     When set to <c>true</c>, the sound is set up, but remains paused.
        ///     You can use this to set frequency, panning and volume before playing the sound.
        /// </param>
        /// <param name='channelId'>
        ///     When in range 0...31, the selected channel will be used. If it already
        ///     contains a playing sound, that sound will be stopped.
        ///     When set to -1 (the default), the next free channel will be used.
        ///     However, when all channels are in use, Sound.Play will silently fail.
        /// </param>
        public SoundChannel Play(bool paused = false, int channelId = -1) {
            var id = 0;
            FMOD.System_PlaySound(_system, channelId, _id, paused, ref id);
            var soundChannel = new SoundChannel(id);
            return soundChannel;
        }
    }
}