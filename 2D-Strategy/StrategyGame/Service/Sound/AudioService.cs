using System.Collections.Generic;
using StrategyGame.Model.IService;

namespace StrategyGame.Service.Sound
{
    public class AudioService : IAudioService
    {
        private IServiceProvider serviceProvider;

        private Dictionary<string, CachedSound> sounds;

        public AudioService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            sounds = new Dictionary<string, CachedSound>();
        }

        public void AddSound(string name)
        {
            if (!sounds.ContainsKey(name))
            {
                sounds.Add(name, new CachedSound(serviceProvider.GetService<IFileResourceService>().GetFileName("Sounds/" + name + ".mp3")));
            }
        }

        public void PlaySound(string name)
        {
            CachedSound sound;
            if (sounds.TryGetValue(name, out sound))
            {
                AudioPlaybackEngine.Instance.PlaySound(sound);
            }
        }
    }
}
