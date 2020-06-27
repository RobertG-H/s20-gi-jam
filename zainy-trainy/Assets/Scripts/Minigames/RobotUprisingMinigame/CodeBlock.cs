using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (menuName = "RobotUprising/CodeBlock")]
public class CodeBlock : ScriptableObject
{
    [System.Serializable]
    public struct line
    {
        public string text;
        public int tabs;
        public bool isFunction;
        public bool isBad;
    }
    public List<line> lines;

}
