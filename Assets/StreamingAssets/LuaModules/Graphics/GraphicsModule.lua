--require LuaCanvas
GraphicsModule = {}
local canvasModule = require 'Graphics.Canvas'
local canvas = nil

function GraphicsModule.new()
    canvas = canvasModule.new(1920, 1080)
    return GraphicsModule
end

function GraphicsModule:SpawnElement(name)
    canvas:SpawnElement(canvas, name)
end

function GraphicsModule:CreateButton(name, positionx, positiony, width, height, text, onclick)
    canvas:CreateButton(name, positionx, positiony, width, height, text, onclick)
    GraphicsServiceProxy.CreateButton(name, positionx, positiony, width, height, text, onclick)
end

function GraphicsModule:GetElementByName(name)
    return canvas:GetElementByName(name)
end

function GraphicsModule:MoveElement(object, newpositionX, newpositionY)
    canvas:MoveElement(object, newpositionX, newpositionY)
    GraphicsServiceProxy.MoveElement(object.name, newpositionX, newpositionY)
end

function GraphicsModule:CreateTextLabel(name, positionx, positiony, width, height, text)
    canvas:CreateTextLabel(name, positionx, positiony, width, height, text)
    GraphicsServiceProxy.CreateTextLabel(name, positionx, positiony, width, height, text)
end

function GraphicsModule:SetTextLabelText(name, newtext)
    GraphicsServiceProxy.SetTextLabelText(name, newtext)
end

function GraphicsModule:GetCanvasHeight()
    return canvas:GetCanvasHeight()
end

function GraphicsModule:GetCanvasWidth()
    return canvas:GetCanvasWidth()
end

function GraphicsModule:CalculateDistance(PositionAX, PositionAY, PositionBX, PositionBY)
    return canvas:CalculateDistance(PositionAX, PositionAY, PositionBX, PositionBY)
end

function GraphicsModule:Update()
    canvas:Update()
end

return GraphicsModule