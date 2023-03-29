using System;
using System.IO;
using UnityEngine;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;

public class test : MonoBehaviour
{
	public string stringTest = "";
	public int number1CSharp = 0;
	public int number2CSharp = 0;
	public string LuaCode;

	void Update()
	{
		LuaCodeExecuteTest();
	}

    private void LuaCodeExecuteTest()
    {
	    string scriptCode = LuaCode;
	    Script script = new Script();
	    script.Options.DebugPrint = Debug.Log;

	    ((ScriptLoaderBase)script.Options.ScriptLoader).ModulePaths = new string[] { $"{Application.dataPath}/MoonSharp/Scripts/?", $"{Application.dataPath}/MoonSharp/Scripts/?.lua" };
	    script.RequireModule($"testfile");
	    scriptCode = @"
		local t = require 'testfile'
		t:printHello()
		";
	    script.DoString(scriptCode);
	    
	    Table functionTable = script.Globals["Test"] as Table;
	    DynValue function = functionTable.Get("printHello");

	    DynValue result = script.Call(function);
    }
    
    void OnGUI ()
    {
	    GUILayout.BeginArea (new Rect (0.0f, 0.0f, Screen.width, Screen.height));
	    GUILayout.FlexibleSpace ();
	    GUILayout.BeginHorizontal ();
	    GUILayout.FlexibleSpace ();
	    GUILayout.Box (stringTest);
	    GUILayout.FlexibleSpace ();
	    GUILayout.EndHorizontal ();
	    GUILayout.FlexibleSpace ();
	    GUILayout.EndArea ();
    }
   double MoonSharpFactorial()
    {
        string scriptCode = @"    
		-- defines a factorial function
		function fact (n)
			if (n == 0) then
				return 1
			else
				return n*fact(n - 1)
			end
		end

		return fact(mynumber)";

        Script script = new Script();
        script.Globals["mynumber"] = 10;
        DynValue res = script.DoString(scriptCode);
        return res.Number;
    }

    private static int Mul(int a, int b)
    {
	    return a * b;
    }

    private static double CallBackTest()
    {
	    string scriptCode = @"    
        -- defines a factorial function
        function fact (n)
            if (n == 0) then
                return 1
            else
                return Mul(n, fact(n - 1));
            end
        end";

	    Script script = new Script();
	    script.Globals["Mul"] = (Func<int, int, int>) Mul;
	    script.DoString(scriptCode);
	    DynValue res = script.Call(script.Globals["fact"], 4);
	    return res.Number;

    }

    private static double MoonSharpFactorial2()
    {
	    string scriptCode = @"    
		-- defines a factorial function
		function fact (n)
			if (n == 0) then
				return 1
			else
				return n*fact(n - 1)
			end
		end";

	    Script script = new Script();
	    script.DoString(scriptCode);
	    DynValue luaFactFunction = script.Globals.Get("fact");
	    DynValue res = script.Call(luaFactFunction, DynValue.NewNumber(4));
	    return res.Number;
    }


}
