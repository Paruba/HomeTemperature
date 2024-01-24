using Fluxor;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Boiler.Mobile.Framework.Logging;

public class LoggingMiddleware : Middleware
{

    public LoggingMiddleware()
    {
    }

    public override void BeforeDispatch(object action)
    {
        Console.WriteLine("console.log", $"[Action]: %c{action.GetType().Name}", "font-weight: bold; text-decoration: underline", action);
    }

    private static string ObjectInfo(object obj)
        => ": " + obj.GetType().Name + " - " + JsonSerializer.Serialize(obj);
}