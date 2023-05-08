Player = {}
Player.__index = Player

--[[vars]]
local Graphics = require 'howl.LuaModules.Graphics.GraphicsModule'
local graphicsModule = nil
local player = nil

--[[game specific vars]]
local grid = {}
local gameStarted = false
local gameEnd = false
local BOARD_DIMENSION = 6	-- The board will be this in both dimensions.

local currentDirection = "Up"
local previousDirection = currentDirection
local directionUp = "Up"
local directionDown = "Down"
local directionRight = "Right"
local directionLeft = "Left"

local currentPosition = {x, y}
local nextPosition = {x,y}
local ApplePosition = {x,y}
local score = 0
local SnakeElements = {}
SnakeElement = {parent, image, xspeed, yspeed, direction}
SnakeElement.__index = SnakeElement

local image1 = "tile_01.png"
local image2 = "tile_02.png"
local wallImage = "wall.png"
local appleImageName = "apple.png"
local snakeSourceImageName = "snake_head.png"
local bodySourceImageName = "snake_body.png"
local snakesize = 75
local applesize = 75


local appleElementName = "apple"
local speed = 2
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
    Player:CreateApple()
    Player:CreateSnake()
    callback()
end

function SnakeElement.new(parent)
    local instance = setmetatable({}, SnakeElement)
    instance.parent = parent
    instance.image = nil
    instance.xspeed = 0
    instance.yspeed = 0
    instance.direction = ""
    return instance
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

function Player:CreateSnake()
    local image = grid[BOARD_DIMENSION / 2][BOARD_DIMENSION / 2]
    currentPosition.x = BOARD_DIMENSION / 2
    currentPosition.y = BOARD_DIMENSION / 2
    nextPosition.x = currentPosition.x
    nextPosition.y = currentPosition.y

    local snakeHead = SnakeElement.new(nil)
    local snakeRect = GetRect(image.pos.x, image.pos.y, snakesize, snakesize)
    graphicsModule:CreateImage("snake_head", snakeRect, snakeSourceImageName)
    snakeHead.image = graphicsModule:GetElementByName("snake_head")
    table.insert(SnakeElements, snakeHead)
    print(tostring(#SnakeElements))
    Player:SetNewSnakePosition()
end

function Player:CreateApple()
    local appleRect = GetRect(-100 ,0, applesize, applesize)
    graphicsModule:CreateImage(appleElementName, appleRect, appleImageName)
end

function Player:GameStart()
     gameStarted = true
     Player:PlaceAppleInRandomPlace()
end

function Player:PlaceAppleInRandomPlace()
    local randomeApplePosition = Player:GetRandomPosition()
    ApplePosition.x = randomeApplePosition.randomx
    ApplePosition.y = randomeApplePosition.randomy
    local image = grid[ApplePosition.x][ApplePosition.y]
    local appleImage = graphicsModule:GetElementByName(appleElementName)
    graphicsModule:MoveElement(appleImage, image.pos.x, image.pos.y)
end


function Player:SwitchDirection(direction)
    if Player:CanSwitchToDirection(direction) then
        previousDirection = currentDirection
        currentDirection = direction    
    end
end

function Player:CanSwitchToDirection(direction)
    if direction == directionUp or direction == directionDown then
        return previousDirection == directionLeft or previousDirection == directionRight
    elseif direction == directionLeft or directionRight then
        return previousDirection == directionUp or previousDirection == directionDown
    end
end



function Player:MoveSnakeToPosition()
    for index, value in ipairs(SnakeElements) do
        if value.xspeed == nil or value.yspeed == nil then
            GetSpeedByDirection(value)
        end
        print("x = " .. value.xspeed)
        graphicsModule:MoveElement(value.image, value.image.pos.x + value.xspeed, value.image.pos.y + value.yspeed)
    end
    local headElement = SnakeElements[1]
    if headElement.xspeed < 0 or headElement.yspeed < 0 then
        if headElement.image.pos.x <= grid[nextPosition.x][nextPosition.y].pos.x and headElement.image.pos.y <= grid[nextPosition.x][nextPosition.y].pos.y then
            currentPosition.x = nextPosition.x
            currentPosition.y = nextPosition.y
            Player:SetNewSnakePosition()
        end
    elseif headElement.xspeed > 0 or headElement.yspeed > 0 then
        if headElement.image.pos.x >= grid[nextPosition.x][nextPosition.y].pos.x and headElement.image.pos.y >= grid[nextPosition.x][nextPosition.y].pos.y then
            currentPosition.x = nextPosition.x
            currentPosition.y = nextPosition.y
            Player:SetNewSnakePosition()
        end
    end
    if currentPosition.x == ApplePosition.x and currentPosition.y == ApplePosition.y then
        Player:AddBodyToSnake()
        Player:PlaceAppleInRandomPlace()
        Player:IncreaseScore()
    end
end

function Player:AddBodyToSnake()
    local tailElement = nil
    tailElement = SnakeElement.new(SnakeElements[#SnakeElements])
    local previousPositionelement = CalculateBodyCreatePosition()
    local bodyrect = GetRect(previousPositionelement.pos.x, previousPositionelement.pos.y, 80, 80)
    local bodyname = "body" .. tostring(#SnakeElements)
    graphicsModule:CreateImage(bodyname, bodyrect, bodySourceImageName)
    tailElement.image = graphicsModule:GetElementByName(bodyname)
    tailElement.direction = SnakeElements[#SnakeElements].direction
    tailElement.xspeed = 0
    tailElement.yspeed = 0
    GetSpeedByDirection(tailElement)
    table.insert(SnakeElements, tailElement)
end

function Player:SetNewSnakePosition()
    local headElement = SnakeElements[1]
    if currentDirection == "Left" then
        headElement.yspeed = 0
        headElement.xspeed = -speed
        nextPosition.x = currentPosition.x - 1
    elseif currentDirection == "Up" then
        headElement.xspeed = 0
        headElement.yspeed = speed
        nextPosition.y = currentPosition.y + 1
    elseif currentDirection == "Right" then
        headElement.yspeed = 0
        headElement.xspeed = speed
        nextPosition.x = currentPosition.x + 1
    elseif currentDirection == "Down" then
        headElement.xspeed = 0
        headElement.yspeed = -speed
        nextPosition.y = currentPosition.y - 1
    end
    headElement.direction = currentDirection
    if currentPosition.x == BOARD_DIMENSION or currentPosition.x == 0 or currentPosition.y == BOARD_DIMENSION or currentPosition.y == 0 then
        gameStarted = false
        gameEnd = true
    end
    for key, value in pairs(SnakeElements) do
        if value ~= SnakeElements[1] then
            GetSpeedByDirectionPreviousElement(value)
        end
    end
end

function GetSpeedByDirection(object)
    if object.direction == "Left" then
        object.yspeed = 0
        object.xspeed = -speed
    elseif object.direction == "Up" then
        object.xspeed = 0
        object.yspeed = speed
    elseif object.direction == "Right" then
        object.yspeed = 0
        object.xspeed = speed
    elseif object.direction == "Down" then
        object.xspeed = 0
        object.yspeed = -speed
    end
    print("Setting to " .. object.xspeed)
    print("Setting to " .. object.yspeed)

end

function GetSpeedByDirectionPreviousElement(object)
    if object.parent.direction == "Left" then
        object.yspeed = 0
        object.xspeed = -speed
    elseif object.parent.direction == "Up" then
        object.xspeed = 0
        object.yspeed = speed
    elseif object.parent.direction== "Right" then
        object.yspeed = 0
        object.xspeed = speed
    elseif object.parent.direction == "Down" then
        object.xspeed = 0
        object.yspeed = -speed
    end
end

function CalculateBodyCreatePosition()
    local spawnpos = {x,y}
    if previousDirection == "Left" then
        spawnpos.x = currentPosition.x + 1
        spawnpos.y = currentPosition.y
        return grid[spawnpos.x][spawnpos.y]
    elseif currentDirection == "Up" then
        spawnpos.x = currentPosition.x
        spawnpos.y = currentPosition.y -1
        return grid[spawnpos.x][spawnpos.y]
    elseif currentDirection == "Right" then
        spawnpos.x = currentPosition.x - 1
        spawnpos.y = currentPosition.y
        return grid[spawnpos.x][spawnpos.y]
    elseif currentDirection == "Down" then
        spawnpos.x = currentPosition.x
        spawnpos.y = currentPosition.y + 1
        return grid[spawnpos.x][spawnpos.y]
    end
end

function Player:IncreaseScore()
    score = score + 1
    Player:UpdateScoreLabel()
end

function Player:UpdateScoreLabel()
    graphicsModule:SetTextLabelText(ScoreTextLabel, "<color=green>" .. tostring(score) .. "</color>")
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

function Player:GetRandomPosition()
    local randomPosition = {randomx, randomy}
    randomPosition.randomx = math.random(1, (BOARD_DIMENSION - 1))
    randomPosition.randomy = math.random(1, (BOARD_DIMENSION - 1))
    return randomPosition
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
