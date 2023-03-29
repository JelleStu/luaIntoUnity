require("math")
require("os")

--require "EventBus"
-- require "AudioModule"
-- require "GraphicsModule"

Player = {}

local eventBus = require 'EventBus'
local audioModule = require 'AudioModule'
local graphicsModule = require 'GraphicsModule'

function Player:Initialize()
    graphicsModule = graphicsModule:new()
end

function Player:SendEvent(message)
    eventBus:Publish(message)
end

function Player:GameStart()
    eventBus:Publish("Sound! pu")
    audioModule:PlayAudio(function (s) Player:SendEvent(s)  end)
end

function Player:SpawnButton()
--    graphicsModule:canvas:SpawnButton(x,y, function()print 'clicked' end, {radius = 10})
    print("spawn button function")
    graphicsModule:SpawnButton("buttonFromLua", 100,100, 250,250, function (s) Player:SetButtonToRandomLocation()  end)
end
function Player:SetButtonToRandomLocation(name)
        button = graphicsModule:GetElementByName(name)
        graphicsModule:MoveElement(button, math.random(100, 800), math.random(200, 900))
end

function Player:SpawnMultipleButtons(amountOfButtons)
    print(amountOfButtons)
    for i = 1, 10, -1 do
        buttonName = "buttonFromLua" .. tostring(i)
        graphicsModule:SpawnButton("buttonFromLua" .. tostring(i),  math.random(100, 1080), math.random(200, 1920), 100, 100,  function (s) Player:SetButtonToRandomLocation(buttonName) end)
    end

end
return Player