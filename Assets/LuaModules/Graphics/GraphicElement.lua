require("math")

Position = {x, y}
GraphicElement =  {uuid, name, Position, width, height, onUpdate}

local Updatables = {}

Position.__index = Position
GraphicElement.__index = GraphicElement

function GraphicElement.new(name, positionx, positiony, width, height)
    local instance = setmetatable({}, GraphicElement)
    instance.uuid = GraphicElement:createUuid(name)
    instance.name = name
    instance.Position = Position.new(positionx, positiony)
    instance.width = width
    instance.height = height
    instance.onUpdate = nil
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

function GraphicElement:Update()
    if not Updatables.maxn == 0 then
        for key, value in pairs(Updatables) do
            value.onUpdate() 
         end
    end
end

function Position.new(positionx, positiony)
    local instance = setmetatable({}, Position)
    instance.x = positionx
    instance.y = positiony
    return instance
end

function GraphicElement:CreateButton(name, positionx, positiony, width, height, onclick)
    local Button = GraphicElement.new(name, positionx, positiony, width, height)
    Button.onclick=onclick
    return Button;
end

function GraphicElement:CreateTextLabel(name, positionx, positiony, width, height, labeltext)
    local Textlabel = GraphicElement.new(name, positionx, positiony, width, height)
    Textlabel.Text = labeltext
    return Textlabel;
end

function GraphicElement:SetNewPosition(newpositionX, newpositionY)
    GraphicElement.Position = Position.new(newpositionX, newpositionY)
end


local random = math.random
function GraphicElement:createUuid(name)
    local template ='xxxxxxxx-xxxx-xxxx-yxxx-xxxxxxxxxxxx'
    return string.gsub(template, '[xy]', function (c)
        local v = (c == 'x') and random(0, 0xf) or random(8, 0xb)
        return string.format('%x', v)
    end)
end


return GraphicElement