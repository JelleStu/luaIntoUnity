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
local BOARD_RANK = 3	-- The board will be this in both dimensions.
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
    graphicsModule:CreateTextLabel(InstructionLabel,  graphicsModule:GetCanvasWidth() * 0.5, (graphicsModule:GetCanvasHeight() * 0.5) + 250, 500, 100, "<color=red>Click button to start the</color>")
    graphicsModule:CreateButton("GameStartButton", graphicsModule:GetCanvasWidth() * 0.5, graphicsModule:GetCanvasHeight() * 0.5 + 500, 250, 100, "Start Game", function ()
        Player:GameStart() end)
    callback()
end

function Player:GameStart()
     graphicsModule:SetTextLabelText(InstructionLabel, "<color=black>Game started, player 1 turn</color>")
     gameStarted = true
end

function Player:CreateGrid()
    for widthI = 0, (BOARD_RANK - 1), 1 do
        grid[widthI] = {}
        for heightI = 0, (BOARD_RANK - 1), 1 do
            local btnName = "BtnX" .. tostring(widthI) .. "Y" .. tostring(heightI)
            graphicsModule:CreateButton(btnName,  (graphicsModule:GetCanvasWidth() * 0.5) + (100 * widthI) + 50, (graphicsModule:GetCanvasHeight() * 0.5 ) + (100 * heightI),
             100, 100, EMPTY_SPACE, function ()
                print("clicked" .. "position x = " ..  tostring(widthI) ..  "position y =" .. tostring(heightI))
            end )
            grid[widthI][heightI] = graphicsModule:GetElementByName(name)

        end
    end
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