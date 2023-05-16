using System.Threading.Tasks;
using LuaBridge.Unity.Scripts.LuaBridgeServices.AudioService.Interface;

namespace LuaBridge.Unity.Scripts.LuaBridgeModules.AudioModule
{
    public class AudioModule
    {
        private readonly IAudioService audioService;

        public AudioModule(IAudioService audioService)
        {
            this.audioService = audioService;
        }

        public async Task PlayAudio(string audioClip)
        {
            await audioService.PlayAudioClip(audioClip);
        }
        

        public void SetVolume(int volume)
        {
            audioService.SetVolume(volume);
        }

        public void StopPlayingAudio()
        {
            audioService.StopPlayingAudio(); 
        }
    }
}