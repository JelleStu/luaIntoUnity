Player = {}
Player.__index = Player

--[[vars]]
local Graphics = require 'howl.LuaModules.Graphics.GraphicsModule'
local graphicsModule = nil
local player = nil

--[[game specific vars]]
local gameStarted = false
local BOARD_DIMENSION = 6	-- The board will be this in both dimensions.
local Directions = {"Up", "Right", "Down", "Left"}
local currentDirection = "Up"
local currentPosition = {x, y}
local nextPosition = {x,y}
local ApplePosition = {x,y}
local score = 0
local gameEnd = false
local grid = {}
local image1 = "tile_01.png"
local image2 = "tile_02.png"
local wallImage = "wall.png"
local appleImageName = "apple.png"
local snakeSourceImageName = "snake_head.png"
local snakeImageElement
local xspeed 
local yspeed 
--

--[[Graphhics stuff]]
local ScoreTextLabel = "scoreTextLabel"

function Player.new()
    local instance = setmetatable({}, Player)
    graphicsModule = Graphics.new()
    return instance
end

function Player:Initialize(callback)
    player = Player.new()
    Player:CreateGrid()
    local scoreTextLabelRect = GetRect(800, 950, 100, 100)
    graphicsModule:CreateTextLabel(ScoreTextLabel, scoreTextLabelRect, "<color=black>0</color>")
    Player:SpawnMovementButtons()
    Player:PlaceAppleInRandomPlace()
    Player:SpawnSnake()
    callback()
end

function Player:CreateGrid()
    for widthI = 0, (BOARD_DIMENSION), 1 do
        grid[widthI] = {}
        for heightI = 0, (BOARD_DIMENSION), 1 do
            grid[widthI][heightI] = nil
            local imageElementName = "image " .. tostring(widthI) .. tostring(heightI)
            local imageRect = GetRect((graphicsModule:GetCanvasWidth() * 0.5 - 100) + (100 * (widthI - 1)), (graphicsModule:GetCanvasHeight() * 0.5 - 200) + (100 * heightI),  100, 100)
            if widthI == 0 or widthI == BOARD_DIMENSION or heightI == 0 or heightI == BOARD_DIMENSION then
                graphicsModule:CreateImage(imageElementName, imageRect, wallImage)
            else
                if (heightI % 2 == 0) then
                    if (widthI % 2 == 0) then
                        graphicsModule:CreateImage(imageElementName, imageRect, image2)
                    else
                        graphicsModule:CreateImage(imageElementName, imageRect, image1)
                    end
                else
                    if (widthI % 2 == 0) then
                        graphicsModule:CreateImage(imageElementName, imageRect, image1)
                    else
                        graphicsModule:CreateImage(imageElementName, imageRect, image2)
                    end
                end 
            end
            grid[widthI][heightI] = graphicsModule:GetElementByName(imageElementName)
        end
    end
end

function Player:SpawnMovementButtons()
    Player:CreateChangeMovementToLeftButton()
    Player:CreateChangeMovementToDownButton()
    Player:CreateChangeMovementToUpButton()
    Player:CreateChangeMovementToRightButton()
end

function Player:CreateChangeMovementToLeftButton()
    local btnLeftName = "btnLeft"
    local btnLeftRect = GetRect(895, 50, 100, 100)
    graphicsModule:CreateButton(btnLeftName, btnLeftRect, "←", function ()
        Player:SwitchDirection("Left")
    end )
end

function Player:CreateChangeMovementToDownButton()
    local btnDownName = "btnDown"
    local btnDownRect = GetRect(995, 50, 100, 100)
    graphicsModule:CreateButton(btnDownName, btnDownRect, "↓", function ()
        Player:SwitchDirection("Down")
    end )
end

function Player:CreateChangeMovementToUpButton()
    local btnUpName = "btnUp"
    local btnUpRect = GetRect(995, 150, 100, 100)
    graphicsModule:CreateButton(btnUpName, btnUpRect, "↑", function ()
        Player:SwitchDirection("Up")
    end )
end

function Player:CreateChangeMovementToRightButton()
    local btnRightName = "btnRight"
    local btnRight = GetRect(1095, 50, 100, 100)
    graphicsModule:CreateButton(btnRightName, btnRight, "→", function ()
        Player:SwitchDirection("Right")
    end )
end


function Player:GameStart()
     gameStarted = true
end

function Player:SwitchDirection(direction)
    print(direction)
    currentDirection = direction
    print(currentDirection)
end


function Player:PlaceAppleInRandomPlace()
    local randomeApplePosition = Player:GetRandomPosition()
    local image = grid[randomeApplePosition.randomx][randomeApplePosition.randomy]
    local appleRect = GetRect(image.pos.x, image.pos.y,  50, 50)
    graphicsModule:CreateImage("apple", appleRect, appleImageName)
end

function Player:SpawnSnake()
    local image = grid[BOARD_DIMENSION / 2][BOARD_DIMENSION / 2]
    currentPosition.x = BOARD_DIMENSION / 2
    currentPosition.y = BOARD_DIMENSION / 2
    nextPosition.x = currentPosition.x
    nextPosition.y = currentPosition.y

    Player:SetNewSnakePosition()
    local snakeRect = GetRect(image.pos.x, image.pos.y, 50, 50)
    graphicsModule:CreateImage("snake_head", snakeRect, snakeSourceImageName)
    snakeImageElement = graphicsModule:GetElementByName("snake_head")
end

function Player:GetRandomPosition()
    local randomPosition = {randomx, randomy}
    randomPosition.randomx = math.random(1, (BOARD_DIMENSION - 1))
    randomPosition.randomy = math.random(1, (BOARD_DIMENSION - 1))
    return randomPosition
end

function Player:MoveSnakeToPosition()
    graphicsModule:MoveElement(snakeImageElement, snakeImageElement.pos.x + xspeed, snakeImageElement.pos.y + yspeed)
    if xspeed < 0 or yspeed < 0 then
        if snakeImageElement.pos.x <= grid[nextPosition.x][nextPosition.y].pos.x and snakeImageElement.pos.y <= grid[nextPosition.x][nextPosition.y].pos.y then
            currentPosition.x = nextPosition.x
            currentPosition.y = nextPosition.y
            Player:SetNewSnakePosition()
        end
    elseif xspeed > 0 or yspeed > 0 then
        if snakeImageElement.pos.x >= grid[nextPosition.x][nextPosition.y].pos.x and snakeImageElement.pos.y >= grid[nextPosition.x][nextPosition.y].pos.y then
            currentPosition.x = nextPosition.x
            currentPosition.y = nextPosition.y
            Player:SetNewSnakePosition()
        end
    end
end

function Player:SetNewSnakePosition()
    print(currentDirection)
    if currentDirection == "Left" then
        yspeed = 0
        xspeed = -1.6
        nextPosition.x = currentPosition.x - 1
    elseif currentDirection == "Up" then
        xspeed = 0
        yspeed = 1.6
        nextPosition.y = currentPosition.y + 1
        print(nextPosition.y)
    elseif currentDirection == "Right" then
        yspeed = 0
        xspeed = 1.6
        nextPosition.x = currentPosition.x + 1
    elseif currentDirection == "Down" then
        xspeed = 0
        yspeed = -1.6
        nextPosition.y = currentPosition.y - 1
    end
    if currentPosition.x == BOARD_DIMENSION or currentPosition.x == 0 or currentPosition.y == BOARD_DIMENSION or currentPosition.y == 0 then
        gameStarted = false
        print("gameover")
    end
end


function Player:Update()
    if gameStarted then
        Player:MoveSnakeToPosition()
    end
    graphicsModule:Update()
end

function Player:ResetGame()
    gameEnd = false;
    graphicsModule:DeleteElementByName("restartButton")
    Player:GameStart()
end

function GetRect(x, y, width, height) 
    rect = {}
    rect.x = x
    rect.y = y
    rect.width = width
    rect.height = height
    return rect
end

return Player
