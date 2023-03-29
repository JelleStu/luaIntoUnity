using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;
using NUnit.Framework;
using UnityEngine;

namespace LuaTesting
{
    public class LuaTester
    {
        private Script _luatestScript;
        private FileSystemScriptLoader _fileSystemScriptLoader = null;
        private Table functionTable = null;

        public List<string> LoadTestScript()
        {
            _fileSystemScriptLoader = CreateFileSystemScriptLoader();
            _luatestScript = new Script();
            _luatestScript.Options.DebugPrint = Debug.Log;
            _luatestScript.Options.ScriptLoader = CreateFileSystemScriptLoader();
            _luatestScript.DoFile("Assets/LuaModules/testing/luatest.lua");
            functionTable = _luatestScript.Globals["testlua"] as Table;
            List<string> _functionList = new List<string>();
            foreach (var tablePair in functionTable.Pairs)
            {
                _functionList.Add(tablePair.Key.String);
            }
            return _functionList;

        }
        
        public void LogAllFunctions()
        {
            // Create an instance of XmlSerializer for Person class
            XmlSerializer serializer = new XmlSerializer(typeof(testsuites));

            // Read the XML file into a StreamReader
            StreamReader reader = new StreamReader($"E:/UnityProjects/moonsharp/Assets/LuaModules/test2.xml");

            // Deserialize the XML into a Person object
            testsuites _output = (testsuites)serializer.Deserialize(reader);

            // Close the StreamReader
            reader.Close();
            
            foreach (var test in _output.testsuite.testcase)
            {
                if (test.failure != null)
                {
                    Debug.LogError(test.failure.Value);
                }
            }
        }
        private FileSystemScriptLoader CreateFileSystemScriptLoader()
        {
            return new FileSystemScriptLoader()
            {
                ModulePaths = new string[] {Path.Combine($"{Application.dataPath}/LuaModules/testing", "?.lua")}
            };
        }

        public void Reset()
        {
            functionTable = null;
            _fileSystemScriptLoader = null;
        }

        public void RunTestByFunctionName(string functionName)
        {
            var testFunction = functionTable.Get(functionName);
            if (testFunction != DynValue.Nil)
                _luatestScript.Call(testFunction);
        }
    }
}