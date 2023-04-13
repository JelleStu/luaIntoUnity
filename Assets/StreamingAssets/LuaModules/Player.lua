require("math")

Player = {}
Player.__index = Player

local eventBus = require 'EventBus.EventBus'
local audioModule = require 'Audio.AudioModule'
local Graphics = require 'Graphics.GraphicsModule'
local graphicsModule = nil
local player = nil

local playerTurn = 1
local gameEnd = 0

function Player.new()
    local instance = setmetatable({}, Player)
    graphicsModule = Graphics.new()
    return instance
end

function Player:Initialize(callback)
    player = Player.new()
    graphicsModule:CreateGrid(3,3)
    callback()
end

function Player:SendEvent(message)
    eventBus:Publish(message)
end

function Player:GameStart()
end

function Player:CreateGrid()
    graphicsModule:CreateGrid(3,3)
end

function Player:GameStart()
    --Show message player 1 can GameStart

end

function Player:ButtonIsClicked(name)
    --Get Position from button
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
    end
    callback()
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