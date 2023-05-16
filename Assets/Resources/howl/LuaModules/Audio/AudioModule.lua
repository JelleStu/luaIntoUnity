AudioModule = {}
local test = nil


function AudioModule.new()
    return AudioModule
end

function AudioModule:PlayAudio(clipname)
    AudioServiceProxy.PlayAudioClip(clipname)
end

function AudioModule:SetVolume(volume)
    AudioServiceProxy.SetVolume(volume)
end

function AudioModule:StopPlayingAudio()
    AudioServiceProxy.StopPlayingAudio()
end

return AudioModule