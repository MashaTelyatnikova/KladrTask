using System.Collections.Generic;

namespace PrefixTree
{
    public class Node
    {
        public string Index { get; set; }
        public string Code { get; set; }
        public string FullName { get; set; }
        public Node Parent { get; set; }
        public bool IsFinal { get; set; }
        public IEnumerable<Node> Childs { get { return childs.Values; } }
        private Dictionary<string, Node> childs;

        public Node()
        {
            childs = new Dictionary<string, Node>();
        }

        public void AddChild(Node child, string key)
        {
            childs[key] = child;
        }

        public Node GetChild(string code)
        {
            Node result = null;
            return childs.TryGetValue(code, out result) ? result : null;
        }

        public Node Clone()
        {
            var copy = new Node();
            copy.Index = Index;
            copy.Code = Code;
            copy.FullName = FullName;
            copy.Parent = Parent;
            copy.IsFinal = IsFinal;
            copy.childs = childs;

            return copy;
        }
    }
}