Position = {x, y}
GraphicElement =  {name, Position, width, height, onUpdate}

Updatables = {}

Position.__index = Position
GraphicElement.__index = GraphicElement

function GraphicElement.new(name, positionx, positiony, width, height)
    local instance = setmetatable({}, GraphicElement)
    instance.name = name
    instance.pos = Position.new(positionx, positiony)
    instance.width = width
    instance.height = height
    instance.onUpdate = nil
    return instance
end

function Position.new(positionx, positiony)
    local instance = setmetatable({}, Position)
    instance.x = positionx
    instance.y = positiony
    return instance
end

function GraphicElement:AddOnUpdate(fn)
    self.onUpdate = fn
    Updatables[self.name] = self
end

function GraphicElement:RemoveOnUpdate()
    self.onUpdate = nil
    Updatables[self.name] = nil
end

function GraphicElement:onUpdate()
    self.onUpdate()
end

function GraphicElement:Update()
    if GraphicElement:tablelength() > 0 then
        for key, value in pairs(Updatables) do
            value.onUpdate()
        end
    end
end


function GraphicElement:CreateButton(name, positionx, positiony, width, height, text, onclick)
    local Button = GraphicElement.new(name, positionx, positiony, width, height)
    Button.text = text
    Button.onclick=onclick
    return Button;
end

function GraphicElement:CreateTextLabel(name, positionx, positiony, width, height, text)
    local Textlabel = GraphicElement.new(name, positionx, positiony, width, height)
    Textlabel.text = text
    return Textlabel;
end

function GraphicElement:SetNewPosition(newpositionX, newpositionY)
    self.pos.x = newpositionX
    self.pos.y = newpositionY
end

function GraphicElement:GetCurrentPosition()
return self.pos
end

function GraphicElement:tablelength()
    local count = 0
    for _ in pairs(Updatables) do count = count + 1 end
    return count
 end

return GraphicElement