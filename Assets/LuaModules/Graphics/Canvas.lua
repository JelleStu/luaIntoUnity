--require GraphicElement

local GraphicElement = require 'GraphicElement'

local Canvas = {width = 1080, height = 1920,  objectsOnCanvas = {}}

function Canvas.new(width, height)
    print("new canvas")
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
print("Added", Canvas.objectsOnCanvas[object.name].name)
end

function Canvas:Update()
    GraphicElement:Update()
end

function Canvas:GetElementByName(nametosearch)
    -- find a value in a list
    for key, value in pairs(Canvas.objectsOnCanvas) do
        print(value)
    end

    local found = nil
    if Canvas.objectsOnCanvas[nametosearch] then
        found = Canvas.objectsOnCanvas[nametosearch]
        print("this is the founded object",found.name)
    end
return found
end

function Canvas:MoveElement(object, newpositionX, newpositionY)
    objectToMove = Canvas.objectsOnCanvas[object.name]
    objectToMove:SetNewPosition(newpositionX, newpositionY)
end




return Canvas

