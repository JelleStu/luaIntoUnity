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

function GraphicsModule:GetCanvasWidth()
    return canvas:GetCanvasWidth()
end

function GraphicsModule:CreateGrid(width, height)
    for widthI = 0, (width - 1), 1 do
        for heightI = 0, (height - 1), 1 do
            print(heightI)
            GraphicsModule:SpawnButton("GridButton X = " .. tostring(widthI) .. tostring(heightI),  (canvas:GetCanvasWidth() * 0.5) + (100 * widthI), (canvas:GetCanvasHeight() * 0.5 ) + (100 * heightI), 100, 100, function ()
                print("clicked" .. "position x = " ..  tostring(widthI) ..  "position y =" .. tostring(heightI))
            end )
        end
    end
end

function GraphicsModule:GetCanvasHeight()
    return canvas:GetCanvasHeight()
end

function GraphicsModule:CalculateDistance(PositionAX, PositionAY, PositionBX, PositionBY)
    return canvas:CalculateDistance(PositionAX, PositionAY, PositionBX, PositionBY)
end

function GraphicsModule:Update()
    canvas:Update()
end

function GraphicsModule:MoveButtonToLocationWithDoTween(name, endPositionX, endPositionY, time, func)
    GraphicsModuleProxy.MoveButtonToLocationWithDoTween(name, endPositionX, endPositionY, time, func)

end

return GraphicsModule