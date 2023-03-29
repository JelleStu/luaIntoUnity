using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace LuaTesting.UI
{
    public class LuaTestWindow : EditorWindow
    {
        private static LuaTester _luaTester = null;

        private string[] _fromLua;
        
        [MenuItem("Lunacy/LuaTester")]
        public static void ShowWindow()
        {
            GetWindow<LuaTestWindow>("Lua tester");
            _luaTester = new LuaTester();
        }
        
        private void OnGUI()
        {
            EditorGUILayout.LabelField("This is the lua tester", EditorStyles.boldLabel);

            if (GUILayout.Button("Load all tests from luascript"))
            {
                _fromLua = _luaTester.LoadTestScript().ToArray();
            }
            
            if (_fromLua != null)
                foreach (var functionName in  _fromLua)
                {
                    EditorGUILayout.Space();
                    if (GUILayout.Button(functionName))
                    {
                        _luaTester.RunTestByFunctionName(functionName);
                    }
                }
            
            EditorGUILayout.Space();
            
            if (EditorGUILayout.LinkButton("Reset SCRIPTS"))
            {
                _luaTester.Reset();
            }
            
            EditorGUILayout.Space();
            
            if (EditorGUILayout.LinkButton("Run tests"))
            {
                _luaTester.LogAllFunctions();
            }
            Repaint();
        }
    }
}