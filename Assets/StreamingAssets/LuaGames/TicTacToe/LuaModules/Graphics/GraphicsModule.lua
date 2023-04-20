--require LuaCanvas
GraphicsModule = {}
local canvasModule = require 'LuaModules.Graphics.Canvas'
local canvas = nil

function GraphicsModule.new()
    canvas = canvasModule.new(1920, 1080)
    return GraphicsModule
end

function GraphicsModule:SpawnElement(name)
    canvas:SpawnElement(canvas, name)
end

function GraphicsModule:CreateButton(name, rect, text, onclick)
    canvas:CreateButton(name, rect, text, onclick)
    GraphicsServiceProxy.CreateButton(name, rect, text, onclick)
end

function GraphicsModule:SetButtonText(name, text)
    canvas:SetButtonText(name, text)
    GraphicsServiceProxy.SetButtonText(name, text)
end

function GraphicsModule:DeleteElementByName(name)
    canvas:DeleteElementByName(name)
    GraphicsServiceProxy.DeleteElement(name)
end
function GraphicsModule:GetElementByName(name)
    return canvas:GetElementByName(name)
end

function GraphicsModule:MoveElement(object, newpositionX, newpositionY)
    canvas:MoveElement(object, newpositionX, newpositionY)
    GraphicsServiceProxy.MoveElement(object.name, newpositionX, newpositionY)
end

function GraphicsModule:CreateTextLabel(name, rect, text)
    canvas:CreateTextLabel(name, rect, text)
    GraphicsServiceProxy.CreateTextLabel(name, rect, text)
end

function GraphicsModule:SetTextLabelText(name, newtext)
    canvas:SetTextLabelText(name, newtext)
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

function GetRect(x, y, width, height) 
    rect = {}
    rect.x = x
    rect.y = y
    rect.width = width
    rect.height = height
    return rect
end

return GraphicsModule