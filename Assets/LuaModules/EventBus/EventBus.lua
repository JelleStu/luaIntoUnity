EventBus = {}

function EventBus:Publish(message)
   EventBusProxy.Publish(message)
end

return EventBus