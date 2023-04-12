require("math")

Player = {}
Player.__index = Player

local eventBus = require 'EventBus.EventBus'
local audioModule = require 'Audio.AudioModule'
local Graphics = require 'Graphics.GraphicsModule'
local graphicsModule = nil
local player = nil

function Player.new()
    local instance = setmetatable({}, Player)
    graphicsModule = Graphics.new()
    return instance
end

function Player:Initialize()
       player = Player.new()
    return true
end

function Player:SendEvent(message)
    eventBus:Publish(message)
end

function Player:GameStart()
    eventBus:Publish("Sound! pu")
    audioModule:PlayAudio(function(s)
        Player:SendEvent(s)
    end)
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

function Player:MoveButtonToLocation(button, positionX, positionY)
    graphicsModule:MoveElement(button, positionX, positionY)

end

function Player:SpawnMultipleButtons(amountOfButtons, callback)
    for i = amountOfButtons, 1, -1 do
        local name = "buttonFromLua" .. tostring(i)
        graphicsModule:SpawnButton(name, math.random(100, 900), math.random(100, 900), 100, 100)
        Player:MoveButtonToMaxXOfCanvas(name)
    end
    callback()
end

function Player:MoveButtonToMaxOfCanvasWithDotween(name)
    local button = graphicsModule:GetElementByName(name)
    local buttonY = button.pos.y
    graphicsModule:MoveButtonToLocationWithDoTween(button.name, graphicsModule:GetCanvasWidth() - button.width, buttonY, math.random(1, 20), function()
        Player:MoveButtonToMinOfCanvasWithDotween(button.name)
    end)

end

function Player:MoveButtonToMinOfCanvasWithDotween(name)
    local button = graphicsModule:GetElementByName(name)
    local buttonY = button.pos.y
    graphicsModule:MoveButtonToLocationWithDoTween(button.name, button.width, buttonY, math.random(1, 20), function()
        Player:MoveButtonToMaxOfCanvasWithDotween(button.name)
    end)
end

function Player:MoveButtonToMaxXOfCanvas(name)
    local button = graphicsModule:GetElementByName(name)
    local buttonY = button.pos.y
    local distance = graphicsModule:CalculateDistance(button.pos.x, buttonY, graphicsModule:GetCanvasWidth() - 100, buttonY)
    local time = math.random(500, 5000)
    local speed = distance / time

    button:AddOnUpdate(function(s)
        local currentPositon = button:GetCurrentPosition()
        if currentPositon.x < (graphicsModule:GetCanvasWidth() - button.width) then
            Player:MoveButtonToLocation(button, (currentPositon.x + 5 * speed), currentPositon.y)
        else
            button:RemoveOnUpdate()
            Player:MoveButtonToMinXOfCanvas(button.name)
        end
    end)
end

function Player:MoveButtonToMinXOfCanvas(name)
    local button = graphicsModule:GetElementByName(name)
    local buttonY = button.pos.y
    local distance = graphicsModule:CalculateDistance(button.pos.x, buttonY, button.width, buttonY)
    local time = math.random(500, 5000)
    local speed = distance / time

    button:AddOnUpdate(function(s)
        local currentPositon = button:GetCurrentPosition()
        if currentPositon.x > button.width then
            Player:MoveButtonToLocation(button, (currentPositon.x - 5 *  speed), (currentPositon.y))
        else
            button:RemoveOnUpdate()
            Player:MoveButtonToMaxXOfCanvas(button.name)
        end
    end)
end

return Player