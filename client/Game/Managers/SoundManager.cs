using System.Collections.Generic;
using System.Threading;
using GXPEngine;

namespace game {
    public class SoundManager : GameObject {
        private static SoundManager instance;
        public static SoundManager Instance => instance ?? (instance = new SoundManager());

        public bool IsLoadingDone;
        private Dictionary<string, SoundChannel> currentlyPlayingSoundChannels;
        private Dictionary<string, Sound> sounds;

        private SoundManager() {
            sounds = new Dictionary<string, Sound>();
            currentlyPlayingSoundChannels = new Dictionary<string, SoundChannel>();
            
            new Thread(() => {
                sounds.Add("bar_ambiance", new Sound("data/sounds/bar_ambiance.mp3", true));
                sounds.Add("client_joined", new Sound("data/sounds/client_joined.wav"));
                sounds.Add("client_left", new Sound("data/sounds/client_left.wav"));
                sounds.Add("new_message", new Sound("data/sounds/new_message.wav"));
                sounds.Add("Song1", new Sound("data/sounds/bensound-jazzyfrenchy.mp3", true));
                sounds.Add("Song2", new Sound("data/sounds/bensound-cute.mp3", true));
                sounds.Add("Song3", new Sound("data/sounds/bensound-summer.mp3", true));
                sounds.Add("Song4", new Sound("data/sounds/bensound-dreams.mp3", true));
                sounds.Add("Song5", new Sound("data/sounds/bensound-erf.mp3", true));
                sounds.Add("Song6", new Sound("data/sounds/alexander-nakarada-riffs-two.mp3", true));
                sounds.Add("Song7", new Sound("data/sounds/alexander-nakarada-stoned.mp3", true));
                sounds.Add("Song8", new Sound("data/sounds/le-gang-waiting-for-you.mp3", true));
                sounds.Add("Song9", new Sound("data/sounds/roa-music-puzzle.mp3", true));
                sounds.Add("Song10", new Sound("data/sounds/ron-gelinas-chillout-lounge-natural-high.mp3", true));
                sounds.Add("Song11", new Sound("data/sounds/sokolovsky-music-inspirational-indie-rock.mp3", true));
                sounds.Add("Song12", new Sound("data/sounds/the-denotes-let-us-run-for-it.mp3", true));
                sounds.Add("Song13", new Sound("data/sounds/twisterium-aspire.mp3", true));
                sounds.Add("Song14", new Sound("data/sounds/vlad-gluschenko-overseas.mp3", true));
                sounds.Add("Song15", new Sound("data/sounds/wayne-john-bradley-every-day.mp3", true));
                IsLoadingDone = true;
            }).Start();
        }

        public void PlaySound(string soundName, bool stopAlreadyPlaying = true) {
            if(!sounds.ContainsKey(soundName)) return;
            if (stopAlreadyPlaying) {
                if(currentlyPlayingSoundChannels.ContainsKey(soundName)) currentlyPlayingSoundChannels[soundName].Stop();
            }

            if (currentlyPlayingSoundChannels.ContainsKey(soundName)) currentlyPlayingSoundChannels[soundName] = sounds[soundName].Play();
            else currentlyPlayingSoundChannels.Add(soundName, sounds[soundName].Play());
        }

        public void StopPlaying(string sound) {
            if (!currentlyPlayingSoundChannels.ContainsKey(sound)) return;
            currentlyPlayingSoundChannels[sound].Stop();
            currentlyPlayingSoundChannels.Remove(sound);
        }
    }
}