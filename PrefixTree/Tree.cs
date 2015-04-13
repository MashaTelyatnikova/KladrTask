using System.Collections.Generic;
using System.Linq;
using KladrTask.Domain.Entities;

namespace PrefixTree
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

        public IEnumerable<Region> GetChilds(string code)
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
                    yield return new Region() { Code = node.Code, Name = node.FullName };
                }

                foreach (var child in node.Childs)
                {
                    var copyChild = child.Clone();
    
                    if (node.FullName != "")
                    {
                        if (copyChild.FullName != "")
                            copyChild.FullName += ", ";
                        copyChild.FullName += node.FullName;
                    }
                    queue.Enqueue(copyChild);
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

        public void InsertRoad(Road road)
        {
            if (!road.Code.EndsWith("00"))
                return;

            var town = road.Code.Substring(0, 11);
            var parent = GetNode(town);
            parent.AddChild(new Node() { Code = road.Code, FullName = road.Name }, road.Code.Substring(11, 4));
        }

        public IEnumerable<Road> GetRoads(string code)
        {
            var parent = GetNode(code);
            return parent.Childs.Select(ch => new Road() { Name = ch.FullName, Code = ch.Code });
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