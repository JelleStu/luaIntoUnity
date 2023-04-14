require("math")

Player = {}
Player.__index = Player

local eventBus = require 'EventBus.EventBus'
local audioModule = require 'Audio.AudioModule'
local Graphics = require 'Graphics.GraphicsModule'
local graphicsModule = nil
local player = nil

BOARD_RANK = 3	-- The board will be this in both dimensions.
PLAYER_1 = "x"	-- Player 1 is represented by this. Player 1 goes first.
PLAYER_2 = "o"	-- Player 2 is represented by this.
EMPTY_SPACE = " "	-- An empty space is displayed like this.

grid = {}

local playerTurn = 1
local gameEnd = 0

function Player.new()
    local instance = setmetatable({}, Player)
    graphicsModule = Graphics.new()
    return instance
end

function Player:Initialize(callback)
    player = Player.new()
    Player:CreateGrid()
    graphicsModule:CreateTextLabel("InstructionLabel",  graphicsModule:GetCanvasWidth() * 0.5, (graphicsModule:GetCanvasHeight() * 0.5) + 250, 250, 100, "<color=red>Text from LUA</color>")
    callback()
end

function Player:SendEvent(message)
    eventBus:Publish(message)
end

function Player:GameStart()
end

function Player:CreateGrid()
    for widthI = 0, (BOARD_RANK - 1), 1 do
        for heightI = 0, (BOARD_RANK - 1), 1 do
            local btnName = "BtnX" .. tostring(widthI) .. "Y" .. tostring(heightI)
            GraphicsModule:CreateButton(btnName,  (GraphicsModule:GetCanvasWidth() * 0.5) + (100 * widthI) + 50, (GraphicsModule:GetCanvasHeight() * 0.5 ) + (100 * heightI), 100, 100, function ()
                print("clicked" .. "position x = " ..  tostring(widthI) ..  "position y =" .. tostring(heightI))
            end )
        end
    end
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