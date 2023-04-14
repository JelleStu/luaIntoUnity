--require GraphicElement

local GraphicElement = require 'Graphics.GraphicElement'

local Canvas = {width = 1920, height = 1080,  objectsOnCanvas = {}}

local Grid = {}


function Canvas.new(width, height)
    Canvas.width = width
    Canvas.height = height
    Canvas.objectsOnCanvas = {}
    return Canvas
end

function Canvas:CreateElement(name)
    local element = GraphicElement:new(name,positionX, positionY, width, height)
    Canvas:addObjectToCanvas(element)
    element = nil
end

function Canvas:CreateButton(name, positionx, positiony, width, height, onclick)
    local button = GraphicElement:CreateButton(name, positionx, positiony, width, height, onclick)
    Canvas:addObjectToCanvas(button)
end

function Canvas:CreateTextLabel(name, positionx, positiony, width, height, text)
    print("textlabel canvas")

    local textlabel = GraphicElement:CreateTextLabel(name, positionx, positiony, width, height, text)
    print(textlabel.Text)
    Canvas:addObjectToCanvas(textlabel)
end

function Canvas:addObjectToCanvas(object)
Canvas.objectsOnCanvas[object.name] = object
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




return Canvas

