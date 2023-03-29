--require GraphicElement

local GraphicElement = require 'GraphicElement'

Canvas = {width = 1080, height = 1920,  objectsOnCanvas = {}}

function Canvas.new(width, height)
    print("new canvas")
    Canvas.height = height
    Canvas.width = width
    return Canvas
end

function Canvas:SpawnElement(self, name)
    print("Spawn element")
    local element = GraphicElement:new(name,positionX, positionY, width, height)
    self:addObjectToCanvas(self, element)
    element = nil
end

function Canvas:SpawnButton(self, name, positionx, positiony, width, height, onclick)
    print("Spawn a button")
    local button = GraphicElement:CreateButton(name, positionx, positiony, width, height, onclick)
    print("button created in canvas")
    print(button.onclick)
    self:addObjectToCanvas(self, button)
end

function Canvas:SpawnTextlabel(self, name, textlabel)
    print("Spawn a textlabel")
    local textlabel = GraphicElement:CreateTextLabel(name, textlabel)
    print("textlabel created in canvas")
    print(textlabel.Text)
    self:addObjectToCanvas(self, textlabel)
end

function Canvas:addObjectToCanvas(self, object)
print(object.name, " is added to canvas")
self.objectsOnCanvas[object.uuid] = object
end

function Canvas:GetElementByName(self, nametosearch)
    -- find a value in a list
    print('kak')
    local found = nil
    for key, value in pairs(self.objectsOnCanvas) do
        if self.objectsOnCanvas[key].name == nametosearch then
            found = self.objectsOnCanvas[key]
        end
    end
    print(found.uuid)
    return found
end

function Canvas:MoveElement(self, object, newpositionX, newpositionY)
    objectToMove = self.objectsOnCanvas[object.uuid]
    objectToMove:SetNewPosition(newpositionX, newpositionY)
end




return Canvas

