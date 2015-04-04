using System.Collections.Generic;
using System.Linq;

namespace MvcApplication9.Infrastructure
{
    public class Tree
    {
        private Node root;
        public Tree()
        {
            root = new Node() { Code = string.Empty, FullName = string.Empty };
        }

        public void Insert(string code, string fullName)
        {
            var i = 0;
            var size = 2;
            var node = root;

            while (i < code.Length - 2)
            {
                var k = code.Substring(i, size);
                var n = node.GetChild(k);
                if (n != null)
                {
                    node = n;
                    i += size;
                    size = 3;
                }
                else
                {
                    break;
                }
            }

            Node lastNotZero = null;
            while (i < code.Length - 2)
            {
                var k = code.Substring(i, size);

                var child = new Node() { Code = code, FullName = string.Empty };
                if (k.Any(c => c != '0'))
                    lastNotZero = child;

                node.AddChild(child, k);
                node = child;
                i += size;
                size = 3;
            }

            lastNotZero.Code = code;
            lastNotZero.FullName = fullName;
            node.IsFinal = true;
        }

        public IEnumerable<MyRegion> GetChilds(string code)
        {
            var parent = GetNode(code.Substring(0, 2));
            parent.FullName = "";
            var queue = new Queue<Node>();
            queue.Enqueue(parent);

            while (queue.Count != 0)
            {
                var node = queue.Dequeue();

                if (!node.Childs.Any() && node.FullName != "")
                {
                    yield return new MyRegion() { Code = node.Code, FulName = node.FullName };
                }

                foreach (var child in node.Childs)
                {
                    if (node.FullName != "")
                    {
                        if (child.FullName != "")
                            child.FullName += ", ";
                        child.FullName += node.FullName;
                    }
                    queue.Enqueue(child);
                }
            }
        }

        public Node GetNode(string code)
        {
            var i = 0;
            var size = 2;
            var node = root;

            while (i < code.Length)
            {
                var k = code.Substring(i, size);
                var n = node.GetChild(k);
                if (n != null)
                {
                    node = n;
                    i += size;
                    size = 3;
                }
                else
                {
                    break;
                }
            }

            return node;
        }

        public void InsertStreet(StreetTable street)
        {
            if (!street.Code.EndsWith("00"))
                return;

            var town = street.Code.Substring(0, 11);
            var parent = GetNode(town);
            parent.AddChild(new Node() { Code = street.Code, FullName = street.Name, Index = street.Index }, street.Code.Substring(11, 4));
        }

        public IEnumerable<StreetTable> GetStreets(string code)
        {
            var parent = GetNode(code);
            return parent.Childs.Select(ch => new StreetTable() { Name = ch.FullName, Index = ch.Index, Code = ch.Code });
        }


        public static int GetLevel(string code)
        {
            if (code == "")
                return 0;
            if (code.Substring(0, 1) != "00" && code.Substring(2, 9) == "000000000")
                return 1;
            if (code.Substring(2, 3) != "000" && code.Substring(5, 6) == "000000")
                return 2;
            if (code.Substring(5, 3) != "000" && code.Substring(8, 3) == "000")
                return 3;
            return 4;
        }
    }
}