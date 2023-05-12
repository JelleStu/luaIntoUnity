Player = {}
Player.__index = Player

SnakeElement = {parent, image, xspeed, yspeed, previousDirection, currentDirection, currentPosition, previousPosition, nextPosition}
SnakeElement.__index = SnakeElement

Pos = {x,y}
Pos.__index = Pos


--[[vars]]
local Graphics = require 'howl.LuaModules.Graphics.GraphicsModule'
local graphicsModule = nil
local player = nil

--[[game specific vars]]
local grid = {}
local gameStarted = false
local gameEnd = false
local BOARD_DIMENSION = 6	-- The board will be this in both dimensions.


local directionUp = "Up"
local directionDown = "Down"
local directionRight = "Right"
local directionLeft = "Left"

local snakeHeadElement
local ApplePosition = {x,y}
local score = 0
local SnakeElements = {}


local image1 = "tile_01.png"
local image2 = "tile_02.png"
local wallImage = "wall.png"
local appleImageName = "apple.png"
local snakeSourceImageName = "snake_head.png"
local bodySourceImageName = "snake_body.png"
local snakesize = 75
local applesize = 75


local appleElementName = "apple"
local speed = 5
--

--[[Graphhics stuff]]
local ScoreTextLabel = "scoreTextLabel"

function Player.new()
    local instance = setmetatable({}, Player)
    graphicsModule = Graphics.new()
    return instance
end

function SnakeElement.new(parent)
    local instance = setmetatable({}, SnakeElement)
    instance.parent = parent
    instance.image = nil
    instance.xspeed = 0
    instance.yspeed = 0
    instance.previousDirection = ""
    instance.currentDirection = ""
    instance.currentPosition = Pos.new()
    instance.previousPosition = Pos.new()
    instance.nextPosition = Pos.new()
    return instance
end

function Pos.new()
    local instance = setmetatable({}, Pos)
    instance.x = 0
    instance.y = 0
    return instance
end

function Player:Initialize(callback)
    player = Player.new()
   Player:CreateGrid()
    local scoreTextLabelRect = GetRect(800, 950, 100, 100)
    graphicsModule:CreateTextLabel(ScoreTextLabel, scoreTextLabelRect, "<color=black>0</color>")
    Player:SpawnMovementButtons()
    Player:CreateApple()
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
    Player:CreateSnake()
end

function Player:CreateSnake()
    local image = grid[BOARD_DIMENSION / 2][BOARD_DIMENSION / 2]
    local snakeHead = SnakeElement.new(nil)

    local snakeRect = GetRect(image.pos.x, image.pos.y, snakesize, snakesize)
    snakeHead.currentPosition.x = BOARD_DIMENSION / 2
    snakeHead.currentPosition.y = BOARD_DIMENSION / 2
    snakeHead.nextPosition.x = snakeHead.currentPosition.x
    snakeHead.nextPosition.y = snakeHead.currentPosition.y


    graphicsModule:CreateImage("snake_head", snakeRect, snakeSourceImageName)

    snakeHead.image = graphicsModule:GetElementByName("snake_head")
    snakeHead.currentDirection = directionUp
    snakeHead.previousDirection = snakeHead.currentDirection
    snakeHeadElement = snakeHead
    table.insert(SnakeElements, snakeHead)
    SetNewPositionSnakeElement(snakeHead)
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
        snakeHeadElement.previousDirection = snakeHeadElement.currentDirection
        snakeHeadElement.currentDirection = direction    
    end
end

function Player:CanSwitchToDirection(directionSwitchTo)
    if directionSwitchTo == directionUp or directionSwitchTo == directionDown then
        if snakeHeadElement.currentDirection == directionLeft or snakeHeadElement.currentDirection == directionRight then
            return true
        else
            return false
        end
    end
    if directionSwitchTo == directionLeft or directionSwitchTo == directionRight then
        if snakeHeadElement.currentDirection == directionUp or snakeHeadElement.currentDirection == directionDown then
            return true
        else
            return false
        end
    end
end

function Player:MoveSnake()

    if snakeHeadElement.currentPosition.x == BOARD_DIMENSION or snakeHeadElement.currentPosition.x == 0 or snakeHeadElement.currentPosition.y == BOARD_DIMENSION or snakeHeadElement.currentPosition.y == 0 then
        gameStarted = false
        gameEnd = true
    end


    for key, value in pairs(SnakeElements) do
        graphicsModule:MoveElement(value.image, value.image.pos.x + value.xspeed, value.image.pos.y + value.yspeed)
    end
    if snakeHeadElement.xspeed < 0 or snakeHeadElement.yspeed < 0  then
        if snakeHeadElement.image.pos.x <= grid[snakeHeadElement.nextPosition.x][snakeHeadElement.nextPosition.y].pos.x and snakeHeadElement.image.pos.y <= grid[snakeHeadElement.nextPosition.x][snakeHeadElement.nextPosition.y].pos.y then
            graphicsModule:MoveElement(snakeHeadElement.image, grid[snakeHeadElement.nextPosition.x][snakeHeadElement.nextPosition.y].pos.x, grid[snakeHeadElement.nextPosition.x][snakeHeadElement.nextPosition.y].pos.y)
            for key, value in pairs(SnakeElements) do
                value.currentPosition.x = value.nextPosition.x
                value.currentPosition.y = value.nextPosition.y
                value.previousDirection = value.currentDirection
                SetNewPositionSnakeElement(value)
            end
        end
    elseif snakeHeadElement.xspeed > 0 or snakeHeadElement.yspeed > 0 then
        if snakeHeadElement.image.pos.x >= grid[snakeHeadElement.nextPosition.x][snakeHeadElement.nextPosition.y].pos.x and snakeHeadElement.image.pos.y >= grid[snakeHeadElement.nextPosition.x][snakeHeadElement.nextPosition.y].pos.y then
            graphicsModule:MoveElement(snakeHeadElement.image, grid[snakeHeadElement.nextPosition.x][snakeHeadElement.nextPosition.y].pos.x, grid[snakeHeadElement.nextPosition.x][snakeHeadElement.nextPosition.y].pos.y)
            for key, value in pairs(SnakeElements) do
                value.currentPosition.x = value.nextPosition.x
                value.currentPosition.y = value.nextPosition.y
                value.previousDirection = value.currentDirection
                SetNewPositionSnakeElement(value)
            end
        end
    end
    if snakeHeadElement.currentPosition.x == ApplePosition.x and snakeHeadElement.currentPosition.y == ApplePosition.y then
        Player:PlaceAppleInRandomPlace()
        Player:AddBodyToSnake()
        Player:IncreaseScore()
    end 
end

function SetNewPositionSnakeElement(object)
    if object.parent ~= nil then
        object.nextPosition.x = object.parent.currentPosition.x
        object.nextPosition.y = object.parent.currentPosition.y

        if object.nextPosition.x > object.currentPosition.x then
            object.xspeed = speed
            object.yspeed = 0
            object.currentDirection = directionRight
            object.nextPosition.x = object.currentPosition.x + 1
        elseif object.nextPosition.x < object.currentPosition.x then
            object.xspeed = -speed
            object.yspeed = 0
            object.currentDirection = directionLeft
            object.nextPosition.x = object.currentPosition.x - 1
        elseif object.nextPosition.y > object.currentPosition.y then
            object.xspeed = 0
            object.yspeed = speed
            object.nextPosition.y = object.currentPosition.y + 1
            object.currentDirection = directionUp
        elseif object.nextPosition.y < object.currentPosition.y then
            object.xspeed = 0
            object.yspeed = -speed
            object.currentDirection = directionDown
            object.nextPosition.y = object.currentPosition.y - 1
        end
    else
        object.previousDirection = object.currentDirection
        if object.currentDirection == directionUp then
            object.xspeed = 0
            object.yspeed = speed
            object.nextPosition.y = object.currentPosition.y + 1
        elseif object.currentDirection == directionDown then
            object.xspeed = 0
            object.yspeed = -speed
            object.nextPosition.y = object.currentPosition.y - 1
        elseif object.currentDirection == directionLeft then
            object.xspeed = -speed
            object.yspeed = 0
            object.nextPosition.x = object.currentPosition.x - 1
        elseif object.currentDirection == directionRight then
            object.xspeed = speed
            object.yspeed = 0
            object.nextPosition.x = object.currentPosition.x + 1
        end
    end
end

function Player:AddBodyToSnake()
    local tailElement = nil
    tailElement = SnakeElement.new(SnakeElements[#SnakeElements])
    local previousPositionelement = CalculateBodyCreateWorldPosition()
    local bodyrect = GetRect(previousPositionelement.pos.x, previousPositionelement.pos.y, 100, 100)
    local bodyname = "body" .. tostring(#SnakeElements)
    graphicsModule:CreateImage(bodyname, bodyrect, bodySourceImageName)
    tailElement.image = graphicsModule:GetElementByName(bodyname)

    if #SnakeElements == 1 then
        tailElement.currentDirection = snakeHeadElement.currentDirection
    else
        tailElement.currentDirection = SnakeElements[#SnakeElements].parent.currentDirection
    end
    local spanwpositiongrid = CalculateBodyCreateGridPosition()
    tailElement.currentPosition.x = spanwpositiongrid.x
    tailElement.currentPosition.y = spanwpositiongrid.y
    tailElement.parent = SnakeElements[#SnakeElements]
    SetNewPositionSnakeElement(tailElement)
    table.insert(SnakeElements, #SnakeElements + 1, tailElement)
end

function CalculateBodyCreateWorldPosition()
    local lastElement = SnakeElements[#SnakeElements]
    local spawnpos = {x,y}
    if lastElement.currentDirection == directionLeft then
        spawnpos.x = lastElement.currentPosition.x + 1
        spawnpos.y = lastElement.currentPosition.y
        return grid[spawnpos.x][spawnpos.y]
    elseif lastElement.currentDirection == directionUp then
        spawnpos.x = lastElement.currentPosition.x
        spawnpos.y = lastElement.currentPosition.y - 1
        return grid[spawnpos.x][spawnpos.y]
    elseif lastElement.currentDirection == directionRight then
        spawnpos.x = lastElement.currentPosition.x - 1
        spawnpos.y = lastElement.currentPosition.y
        return grid[spawnpos.x][spawnpos.y]
    elseif lastElement.currentDirection == directionDown then
        spawnpos.x = lastElement.currentPosition.x
        spawnpos.y = lastElement.currentPosition.y + 1
        return grid[spawnpos.x][spawnpos.y]
    end
end

function CalculateBodyCreateGridPosition()
    local lastElement = SnakeElements[#SnakeElements]
    local spawnpos = {x,y}
    if lastElement.currentDirection == directionLeft then
        spawnpos.x = lastElement.currentPosition.x + 1
        spawnpos.y = lastElement.currentPosition.y
        return spawnpos
    elseif lastElement.currentDirection == directionUp then
        spawnpos.x = lastElement.currentPosition.x
        spawnpos.y = lastElement.currentPosition.y - 1
        return spawnpos
    elseif lastElement.currentDirection == directionRight then
        spawnpos.x = lastElement.currentPosition.x - 1
        spawnpos.y = lastElement.currentPosition.y
        return spawnpos
    elseif lastElement.currentDirection == directionDown then
        spawnpos.x = lastElement.currentPosition.x
        spawnpos.y = lastElement.currentPosition.y + 1
        return spawnpos
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
        Player:MoveSnake()
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

function Player:OnUpArrow()
    Player:SwitchDirection(directionUp)
end

function Player:OnDownArrow()
    Player:SwitchDirection(directionDown)
end
function Player:OnRightArrow()
    Player:SwitchDirection(directionRight)
end
function Player:OnLeftArrow()
    Player:SwitchDirection(directionLeft)
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
        Player:SwitchDirection(directionLeft)
    end )
end

function Player:CreateChangeMovementToDownButton()
    local btnDownName = "btnDown"
    local btnDownRect = GetRect(995, 50, 100, 100)
    graphicsModule:CreateButton(btnDownName, btnDownRect, "↓", function ()
        Player:SwitchDirection(directionDown)
    end )
end

function Player:CreateChangeMovementToUpButton()
    local btnUpName = "btnUp"
    local btnUpRect = GetRect(995, 150, 100, 100)
    graphicsModule:CreateButton(btnUpName, btnUpRect, "↑", function ()
        Player:SwitchDirection(directionUp)
    end )
end

function Player:CreateChangeMovementToRightButton()
    local btnRightName = "btnRight"
    local btnRight = GetRect(1095, 50, 100, 100)
    graphicsModule:CreateButton(btnRightName, btnRight, "→", function ()
        Player:SwitchDirection(directionRight)
    end )
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
