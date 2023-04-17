require("math")

Player = {}
Player.__index = Player


--[[vars]]
local eventBus = require 'EventBus.EventBus'
local audioModule = require 'Audio.AudioModule'
local Graphics = require 'Graphics.GraphicsModule'
local graphicsModule = nil
local player = nil


--[[game specific vars]]
local gameStarted = false
local BOARD_DIMENSION = 3	-- The board will be this in both dimensions.
local PLAYER_1 = "x"	-- Player 1 is represented by this. Player 1 goes first.
local PLAYER_2 = "o"	-- Player 2 is represented by this.
local EMPTY_SPACE = " "	-- An empty space is displayed like this.
local playerTurn = 1
local gameEnd = 0
local grid = {}

local InstructionLabel = "InstructionLabel"

function Player.new()
    local instance = setmetatable({}, Player)
    graphicsModule = Graphics.new()
    return instance
end

function Player:Initialize(callback)
    player = Player.new()
    Player:CreateGrid()
    local textLabelRect = GetRect(1010, 950, 500, 100)
    print(textLabelRect)
    graphicsModule:CreateTextLabel(InstructionLabel, textLabelRect, "<color=red>Click button to start the game</color>")
    callback()
end

function GetRect(x, y, width, height) 
    rect = {}
    rect.x = x
    rect.y = y
    rect.width = width
    rect.height = height
    return rect
end

function Player:GameStart()
     graphicsModule:SetTextLabelText(InstructionLabel, "<color=black>Game started, player 1 turn</color>")
     gameStarted = true
end

function Player:CreateGrid()
    for widthI = 0, (BOARD_DIMENSION  - 1), 1 do
        grid[widthI] = {}
        for heightI = 0, (BOARD_DIMENSION - 1), 1 do
            grid[widthI][heightI] = nil
            local btnName = "BtnX" .. tostring(widthI) .. "Y" .. tostring(heightI)
            local btnRect = GetRect((graphicsModule:GetCanvasWidth() * 0.5) + (100 * (widthI - 1)) + 50, (graphicsModule:GetCanvasHeight() * 0.5 ) + (100 * heightI),  100, 100)

            graphicsModule:CreateButton(btnName, btnRect, EMPTY_SPACE, function ()
                Player:ButtonIsClicked(widthI, heightI)
            end )
            grid[widthI][heightI] = graphicsModule:GetElementByName(btnName)
        end
    end
end


function Player:ButtonIsClicked(widthIndex, heightIndex)
    if not gameStarted then
        print("game has not started yet dumb fuck")
        return
    end
    Player:SelectPlace(widthIndex, heightIndex)
end

function Player:SelectPlace(widthIndex, heightIndex)
    local button = grid[widthIndex][heightIndex]
    if playerTurn == 1 then
        graphicsModule:SetButtonText(button.name, PLAYER_1)
        playerTurn = 2
    else
        graphicsModule:SetButtonText(button.name, PLAYER_2)
        playerTurn = 1
    end

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