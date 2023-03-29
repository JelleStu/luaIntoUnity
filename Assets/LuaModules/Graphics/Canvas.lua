--require GraphicElement

local GraphicElement = require 'GraphicElement'

local Canvas = {width = 1080, height = 1920,  objectsOnCanvas = {}}

function Canvas.new(width, height)
    Canvas.height = height
    Canvas.width = width
    Canvas.objectsOnCanvas = {}
    return Canvas
end

function Canvas:SpawnElement(name)
    local element = GraphicElement:new(name,positionX, positionY, width, height)
    Canvas:addObjectToCanvas(element)
    element = nil
end

function Canvas:SpawnButton(name, positionx, positiony, width, height, onclick)
    local button = GraphicElement:CreateButton(name, positionx, positiony, width, height, onclick)
    Canvas:addObjectToCanvas(button)
end

function Canvas:SpawnTextlabel(name, textlabel)
    local textlabel = GraphicElement:CreateTextLabel(name, textlabel)
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

function Canvas:MoveElement(object, newpositionX, newpositionY)
    local objectToMove = Canvas.objectsOnCanvas[object.name]
    objectToMove:SetNewPosition(newpositionX, newpositionY)
end




return Canvas

