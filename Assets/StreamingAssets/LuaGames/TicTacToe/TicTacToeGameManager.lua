require("math")

Player = {}
Player.__index = Player


--[[vars]]
local audioModule = require 'howl.LuaModules.Audio.AudioModule'
local Graphics = require 'howl.LuaModules.Graphics.GraphicsModule'
local graphicsModule = nil
local player = nil


--[[game specific vars]]
local gameStarted = false
local BOARD_DIMENSION = 3	-- The board will be this in both dimensions.
local PLAYER_1 = "x"	-- Player 1 is represented by this. Player 1 goes first.
local PLAYER_2 = "o"	-- Player 2 is represented by this.
local EMPTY_SPACE = " "	-- An empty space is displayed like this.
local Turn = 1
local gameEnd = false
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
    local textLabelRect = GetRect(1010, 950, 450, 100)
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
    Player:SwitchTurn(1)
     gameStarted = true
end

function Player:ResetGame()
    for widthI = 0, (BOARD_DIMENSION  - 1), 1 do
        for heightI = 0, (BOARD_DIMENSION - 1), 1 do
            grid[widthI][heightI] = nil
            local btnName = "BtnX" .. tostring(widthI) .. "Y" .. tostring(heightI)
            graphicsModule:SetButtonText(btnName, EMPTY_SPACE)
        end
    end
    gameEnd = false;
    graphicsModule:DeleteElementByName("restartButton")
    Player:GameStart()
end

function Player:CreateGrid()
    for widthI = 0, (BOARD_DIMENSION  - 1), 1 do
        grid[widthI] = {}
        for heightI = 0, (BOARD_DIMENSION - 1), 1 do
            grid[widthI][heightI] = nil
            local btnName = "BtnX" .. tostring(widthI) .. "Y" .. tostring(heightI)
            local btnRect = GetRect((graphicsModule:GetCanvasWidth() * 0.5) + (100 * (widthI - 1)) + 50, (graphicsModule:GetCanvasHeight() * 0.5 ) + (100 * heightI),  100, 100)
            graphicsModule:CreateButton(btnName, btnRect, EMPTY_SPACE, function ()
                Player:ButtonIsClicked(widthI, heightI,btnName)
            end )
        end
    end
end


function Player:ButtonIsClicked(widthIndex, heightIndex, btnName)
    if not gameStarted then
        graphicsModule:SetTextLabelText(InstructionLabel, "<color=red>Start the game first!</color>")
        return
    elseif gameEnd then
        return
    end
    Player:SelectPlace(widthIndex, heightIndex, btnName)
end

function Player:SelectPlace(widthIndex, heightIndex, btnName)
    if not Player:IsPlaceEmpty(widthIndex, heightIndex) then
        graphicsModule:SetTextLabelText(InstructionLabel, "<color=red>Selected piece is already been set</color>")
        return
    end
    local button = graphicsModule:GetElementByName(btnName)
    if playerTurn == 1 then
        graphicsModule:SetButtonText(button.name, PLAYER_1)
        grid[widthIndex][heightIndex] = PLAYER_1
        Player:SwitchTurn(2)
    else
        graphicsModule:SetButtonText(button.name, PLAYER_2)
        grid[widthIndex][heightIndex] = PLAYER_2
        Player:SwitchTurn(1)
    end

end

function Player:IsPlaceEmpty(widthIndex, heightIndex)
    if grid[widthIndex][heightIndex] == nil then
        return true    
    else
        return false
    end
end

function Player:SwitchTurn(playerNumber)
    playerTurn = playerNumber
    if playerNumber == 1 then
        playerTurn = 1
    else
        playerTurn = 2
    end 
    graphicsModule:SetTextLabelText(InstructionLabel, "<color=black> Player " .. playerNumber .. ", your turn</color>")

    if Player:IsGameOver() == true then
        gameEnd = true;
        local btnRect =  GetRect(graphicsModule:GetCanvasWidth() * 0.5, (graphicsModule:GetCanvasHeight() * 0.5 ) + 500 ,  150, 100)
        graphicsModule:CreateButton("restartButton", btnRect, "Restart game", function ()
            Player:ResetGame()
        end )
    end
end

-------------------------------------------------
-- Create regions (I admit this is a bit ugly) --
-------------------------------------------------

-- declare region and a number to increment
local region = {}
local region_number = 0

-- vertical
for i = 0, (BOARD_DIMENSION - 1) do
	region[region_number] = {}
	for j = 0, (BOARD_DIMENSION - 1) do
		region[region_number][j] = {}
		region[region_number][j]["x"] = i
		region[region_number][j]["y"] = j
	end
	region_number = region_number + 1
end

-- horizontal
for i = 0, (BOARD_DIMENSION - 1) do
	region[region_number] = {}
	for j = 0, (BOARD_DIMENSION - 1) do
		region[region_number][j] = {}
		region[region_number][j]["x"] = j
		region[region_number][j]["y"] = i
	end
	region_number = region_number + 1
end

-- diagonal, bottom-left to top-right
region[region_number] = {}
for i = 0, (BOARD_DIMENSION - 1) do
	region[region_number][i] = {}
	region[region_number][i]["x"] = i
	region[region_number][i]["y"] = i
end
region_number = region_number + 1

-- diagonal, top-left to bottom-right
region[region_number] = {}
for i = (BOARD_DIMENSION - 1), 0, -1 do
	region[region_number][i] = {}
	region[region_number][i]["x"] = BOARD_DIMENSION - i - 1
	region[region_number][i]["y"] = i
end
region_number = region_number + 1

----------------------
-- Region functions --
----------------------

-- get a region
function Player:GetRegion(number)
	return region[number]
end

-- check for a win in a particular region.
-- returns a number representation of the region. occurrences of player 1
-- add 1, occurrences of player 2 subtract 1. so if there are two X pieces,
-- it will return 2. one O will return -1.
function Player:CheckWinInRegion(number)
	local to_return = 0
	for i, v in pairs(Player:GetRegion(number)) do
		local piece = Player:GetPiece(v["x"], v["y"])
		if piece == PLAYER_1 then to_return = to_return + 1 end
		if piece == PLAYER_2 then to_return = to_return - 1 end
	end
	return to_return
end

-- get the piece at a given spot
function Player:GetPiece(widthIndex, heightIndex)
	return grid[widthIndex][heightIndex]
end

-- check for a win in every region.
-- returns false if no winner.
-- returns the winner if there is one.
function Player:CheckWin()
	for i in pairs(region) do
		local win = Player:CheckWinInRegion(i)
		if math.abs(win) == BOARD_DIMENSION then
			if win == math.abs(win) then
                graphicsModule:SetTextLabelText(InstructionLabel, "<color=green>Player 1 has won the game.</color>")
				return PLAYER_1
			else
                graphicsModule:SetTextLabelText(InstructionLabel, "<color=green>Player 2 has won the game.</color>")
				return PLAYER_2
			end
		end
	end
	return false
end

-- is the game over?
function Player:IsGameOver()
	if Player:CheckWin() == false then	
		for i = 0, (BOARD_DIMENSION - 1) do	-- is the board empty?
			for j = 0, (BOARD_DIMENSION - 1) do
				if Player:IsPlaceEmpty(i, j) == true then 
                    return false 
                end
			end
		end
		return true
	else	
		return true
	end
end
  


function Player:Update()
    graphicsModule:Update()
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