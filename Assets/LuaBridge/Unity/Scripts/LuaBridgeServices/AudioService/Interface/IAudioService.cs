using System.Threading.Tasks;
using Services;

namespace LuaBridge.Unity.Scripts.LuaBridgeServices.AudioService.Interface
{
    public interface IAudioService
    {
        void SetVolume(int volume);
        Task PlayAudioClip(string audioClip);
        void StopPlayingAudio();
        void SetSandboxRoot(string path);
        void SetFileService(IFileService fileService);

    }
}