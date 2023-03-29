require("math")

local Position = {x, y}
GraphicElement =  {uuid, name, Position, width, height}



function GraphicElement:new(name, positionx, positiony, width, height)
    print("CREATING GRAPHICELEMENT")
    GraphicElement.uuid = GraphicElement:createUuid(name)
    GraphicElement.name = name
    GraphicElement.Position = {positionx, positiony}
    GraphicElement.width = width
    GraphicElement.height = height
    return GraphicElement
end

function GraphicElement:CreateButton(name, positionx, positiony, width, height, onclick)
    local Button = GraphicElement:new(name, positionx, positiony, width, height)
    Button.onclick=onclick
    return Button;
end

function GraphicElement:CreateTextLabel(name, positionx, positiony, width, height, labeltext)
    local Textlabel = GraphicElement:new(name, positionx, positiony, width, height)
    Textlabel.Text = labeltext
    return Textlabel;
end

function GraphicElement:SetNewPosition(newpositionX, newpositionY)
    GraphicElement.Position = {newpositionX, newpositionY}  
end


local random = math.random
function GraphicElement:createUuid(name)
    print("generating for", name )
    local template ='xxxxxxxx-xxxx-xxxx-yxxx-xxxxxxxxxxxx'
    return string.gsub(template, '[xy]', function (c)
        local v = (c == 'x') and random(0, 0xf) or random(8, 0xb)
        return string.format('%x', v)
    end)
end


return GraphicElement