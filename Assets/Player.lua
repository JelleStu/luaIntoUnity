require("math")
require("os")

--require "EventBus"
-- require "AudioModule"
-- require "GraphicsModule"

Player = {}

local eventBus = require 'EventBus'
local audioModule = require 'AudioModule'
local Graphics = require 'GraphicsModule'
local graphicsModule = nil
function Player:Initialize()
    graphicsModule = Graphics.new()
    return true
end

function Player:SendEvent(message)
    eventBus:Publish(message)
end

function Player:GameStart()
    eventBus:Publish("Sound! pu")
    audioModule:PlayAudio(function (s) Player:SendEvent(s)  end)
end

function Player:Update()
    graphicsModule:Update()
end

function Player:SetButtonToRandomLocation(buttonName)
        local button = graphicsModule:GetElementByName(buttonName)
        if button == nil then
            print("BUTTON IS NOT FOUND", buttonName)
            return
        end
        graphicsModule:MoveElement(button, math.random(100, 800), math.random(200, 900))
end

function Player:SpawnMultipleButtons(amountOfButtons, fn)
     for i = amountOfButtons, 1, -1 do
        local name = "buttonFromLua" .. tostring(i)
        graphicsModule:SpawnButton(name,  math.random(100, 1520), math.random(100, 750), 100, 100,  function (s) Player:SetButtonToRandomLocation(name) end)
    end
    return AllButtonsSpawned();
end
return Player