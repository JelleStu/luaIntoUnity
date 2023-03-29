AudioModule = {}

function AudioModule:PlayAudio(c)
    print("trying to play a sound")
    AudioModuleProxy.Play(c)
end

return AudioModule

