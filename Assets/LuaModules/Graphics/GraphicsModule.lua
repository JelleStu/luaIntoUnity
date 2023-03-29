--require LuaCanvas
GraphicsModule = {}
local canvasModule = require 'Canvas'
local canvas = nil
function GraphicsModule.new()
    print("new graphicsmodule")
    canvas = canvasModule:new(9999, 5)
    return GraphicsModule
end

function GraphicsModule:SpawnElement(name)
    print("graphicsmopdule spawnbutton")
    canvas:SpawnElement(canvas, name)
end

function GraphicsModule:SpawnButton(name, positionx, positiony, width, height, onclick)
    print("graphicsmopdule spawnbutton")
    canvas:SpawnButton(canvas, name, positionx, positiony, width, height, onclick)
    GraphicsModuleProxy.SpawnButton(name, positionx, positiony, width, height, onclick)
end

function GraphicsModule:GetElementByName(name)
    return canvas:GetElementByName(canvas, name)
end

function GraphicsModule:MoveElement(object, newpositionX, newpositionY)
    canvas:MoveElement(canvas, object, newpositionX, newpositionY)
    GraphicsModuleProxy.MoveElement(object.name, newpositionX, newpositionY)
end


return GraphicsModule