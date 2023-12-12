using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Application.Helpers
{
    public static class TreeExtensions
    {
        /// <summary> Generic interface for tree structure </summary>
        /// <typeparam name="T"></typeparam>
        public interface ITree<T>
        {
            T Value { get; set; }

            ITree<T>? Parent { get; }
            ICollection<ITree<T>> Children { get; }

            bool IsRoot { get; }
            bool IsLeaf { get; }

            IEnumerable<T> Flatten();
            void Traverse(Action<T> action);
            IEnumerable<TResult> Traverse<TResult>(Func<T, TResult> func);

            TResult ToPoco<TResult>(Func<T, ICollection<TResult>, TResult> selector);
        }



        private class Tree<T> : ITree<T>
        {
            public Tree(T value)
            {
                Value = value;
            }

            public T Value { get; set; }

            public ITree<T>? Parent { get; internal set; }

            public ICollection<ITree<T>> Children { get; } = new List<ITree<T>>();

            public bool IsRoot => Parent == null;

            public bool IsLeaf => Children.Count > 0;

            public IEnumerable<T> Flatten()
            {
                throw new NotImplementedException();
            }

            public TResult ToPoco<TResult>(Func<T, ICollection<TResult>, TResult> selector)
            {
                var childPocoNodes = new List<TResult>();

                foreach (var child in this.Children)
                {
                    TResult childPoco = child.ToPoco(selector);
                    childPocoNodes.Add(childPoco);
                }

                TResult pocoNode = selector(this.Value, childPocoNodes);

                return pocoNode;
            }

            public IEnumerable<TResult> Traverse<TResult>(Func<T, TResult> func)
            {
                var queue = new Queue<ITree<T>>();

                queue.Enqueue(this);

                while (queue.Any())
                {
                    var current = queue.Dequeue();

                    if (!current.IsRoot)
                        yield return func(current.Value);

                    foreach (var x in current.Children)
                        queue.Enqueue(x);
                }
            }

            public void Traverse(Action<T> action)
            {
                var queue = new Queue<ITree<T>>();

                queue.Enqueue(this);

                while (queue.Any())
                {
                    var current = queue.Dequeue();

                    if (!current.IsRoot)
                        action(current.Value);

                    foreach (var x in current.Children)
                        queue.Enqueue(x);
                }
            }
        }

        public static ITree<T> ToTree<T>(this IEnumerable<T> sequence, Func<T, T?> parentSelector)
        {
            var lookup = sequence.ToLookup(x => parentSelector(x));

            var root = new Tree<T>(lookup.Count == 1 ? lookup.First().Key : default);

            var queue = new Queue<Tree<T>>();
            queue.Enqueue(root);

            while(queue.Any())
            {
                var current = queue.Dequeue();

                var children = lookup[current.Value].Select(x => new Tree<T>(x) { Parent = current });

                foreach (var x in children)
                {
                    current.Children.Add(x);
                    queue.Enqueue(x);
                }
            }

            return root;
        }
    }
}
