Position = {x, y}
GraphicElement =  {name, Position, width, height, onUpdate}

Updatables = {}

Position.__index = Position
GraphicElement.__index = GraphicElement

function GraphicElement.new(name,rect)
    local instance = setmetatable({}, GraphicElement)
    instance.name = name
    instance.pos = Position.new(rect.x, rect.y)
    instance.width = rect.width
    instance.height = rect.height
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

function GraphicElement:CreateButton(name,rect, text, onclick)
    local Button = GraphicElement.new(name, rect)
    Button.text = text
    Button.onclick=onclick
    return Button;
end

function GraphicElement:SetButtonText(text)
    self.text = text
end

function GraphicElement:CreateImage(name, rect, sourceImage)
    local Image = GraphicElement.new(name, rect)
    Image.image = sourceImage
    return Image;
end

function GraphicElement:CreateTextLabel(name, rect, text)
    local Textlabel = GraphicElement.new(name, rect)
    Textlabel.text = text
    return Textlabel;
end

function GraphicElement:SetTextLabelText(text)
    self.text = text
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

 function GetRect(x, y, width, height) 
    rect = {}
    rect.x = x
    rect.y = y
    rect.width = width
    rect.height = height
    return rect
end
return GraphicElement