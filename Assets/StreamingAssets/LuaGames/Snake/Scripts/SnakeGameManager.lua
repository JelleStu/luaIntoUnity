Player = {}
Player.__index = Player

--[[vars]]
local Graphics = require 'LuaModules.Graphics.GraphicsModule'
local graphicsModule = nil
local player = nil

--[[game specific vars]]
local gameStarted = false
local BOARD_DIMENSION = 5	-- The board will be this in both dimensions.
local Direction = {"Up", "Right", "Down", "Left"}
local previousDirection
local ApplePosition = {x,y}
local score
local gameEnd = false
local grid = {}
local image1= "tile_01.png"
local image2= "tile_02.png"
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
    callback()
end

function Player:CreateGrid()
    for widthI = 0, (BOARD_DIMENSION  - 1), 1 do
        grid[widthI] = {}
        for heightI = 0, (BOARD_DIMENSION - 1), 1 do
            grid[widthI][heightI] = nil
            local imageElementName = "image " .. tostring(widthI) .. tostring(heightI)
            local imageRect = GetRect((graphicsModule:GetCanvasWidth() * 0.5 - 100) + (100 * (widthI - 1)) + 50, (graphicsModule:GetCanvasHeight() * 0.5 - 200) + (100 * heightI),  100, 100)
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
    end
end

function Player:GameStart()
    Player:SwitchTurn(1)
     gameStarted = true
end

function Player:ResetGame()
    gameEnd = false;
    graphicsModule:DeleteElementByName("restartButton")
    Player:GameStart()
end




function Player:PlaceAppleInRandomPlace()
    local randomPosition = {randomx, randomy}
    randomPosition.randomx = math.random(0, (BOARD_DIMENSION - 1))
    randomPosition.randomy = math.random(0, (BOARD_DIMENSION - 1))
    return randomPosition
end
function Player:Update()
    graphicsModule:Update()
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
