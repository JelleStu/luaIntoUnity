--require LuaCanvas
GraphicsModule = {}
local canvasModule = require 'Canvas'
local canvas = nil
function GraphicsModule.new()
    canvas = canvasModule.new(9999, 5)
    return GraphicsModule
end

function GraphicsModule:SpawnElement(name)
    canvas:SpawnElement(canvas, name)
end

function GraphicsModule:SpawnButton(name, positionx, positiony, width, height, onclick)
    canvas:SpawnButton(name, positionx, positiony, width, height, onclick)
    GraphicsModuleProxy.SpawnButton(name, positionx, positiony, width, height, onclick)
end

function GraphicsModule:GetElementByName(name)
    return canvas:GetElementByName(name)
end

function GraphicsModule:MoveElement(object, newpositionX, newpositionY)
    canvas:MoveElement(object, newpositionX, newpositionY)
    GraphicsModuleProxy.MoveElement(object.name, newpositionX, newpositionY)
end

function GraphicsModule:Update()
    canvas:Update()
end


return GraphicsModule