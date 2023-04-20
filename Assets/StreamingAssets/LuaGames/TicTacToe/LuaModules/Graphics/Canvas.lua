--require GraphicElement

local GraphicElement = require 'LuaModules.Graphics.GraphicElement'

local Canvas = {width = 1920, height = 1080,  objectsOnCanvas = {}}

local Grid = {}


function Canvas.new(width, height)
    Canvas.width = width
    Canvas.height = height
    Canvas.objectsOnCanvas = {}
    return Canvas
end

function Canvas:CreateElement(name)
    local element = GraphicElement:new(name,rect)
    Canvas:addObjectToCanvas(element)
    element = nil
end

function Canvas:CreateButton(name, rect, text, onclick)
    local button = GraphicElement:CreateButton(name, rect, text, onclick)
    Canvas:addObjectToCanvas(button)
end

function Canvas:SetButtonText(name, newtext)
    local button = Canvas:GetElementByName(name)
    button:SetButtonText(newtext)
end


function Canvas:CreateTextLabel(name, rect, text)
    local textlabel = GraphicElement:CreateTextLabel(name,rect, text)
    Canvas:addObjectToCanvas(textlabel)
end

function Canvas:SetTextLabelText(name, newtext)
    local textlabel = Canvas:GetElementByName(name)
    textlabel:SetTextLabelText(newtext)
end

function Canvas:addObjectToCanvas(object)
    Canvas.objectsOnCanvas[object.name] = object
end

function Canvas:DeleteElementByName(name)
    Canvas.objectsOnCanvas[name] = nil
end

function Canvas:Update()
    GraphicElement:Update()
end

function Canvas:GetElementByName(nametosearch)
    -- find a value in a list
    local found = nil
    if Canvas.objectsOnCanvas[nametosearch] then
        found = Canvas.objectsOnCanvas[nametosearch]
    end
return found
end

function Canvas:GetCanvasHeight()
    return Canvas.height
end

function Canvas:GetCanvasWidth()
    return Canvas.width
end

function Canvas:CalculateDistance(PositionAX,PositionAY, PositionBX, PositionBY)
    local num1 = PositionAX - PositionBX
    local num2 = PositionAY - PositionBY

    return math.sqrt(num1 * num1 + num2 * num2)
end

function Canvas:MoveElement(object, newpositionX, newpositionY)
    local objectToMove = Canvas.objectsOnCanvas[object.name]
    if newpositionY == nil then
        newpositionY = objectToMove.y
    end
    if newpositionX == nil then
        newpositionX = objectToMove.x
    end
    objectToMove:SetNewPosition(newpositionX, newpositionY)
end



function GetRect(x, y, width, height) 
    rect = {}
    rect.x = x
    rect.y = y
    rect.width = width
    rect.height = height
    return rect
end

return Canvas

