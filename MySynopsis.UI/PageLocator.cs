using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySynopsis.UI
{
    public class PageLocator
    {
        private static Dictionary<Type, ObjectCreatorWithState<object>> _context = new Dictionary<Type, ObjectCreatorWithState<object>>();

        public delegate T ObjectCreatorWithState<T>(object state) where T : class;

        public static T Get<T>(object state = null) where T : class
        {
            var creator = _context[typeof(T)];
            if (creator == null)
            {
                throw new Exception(typeof(T).Name + " Not Registered");
            }
            return creator(state) as T;
        }

        public static void Register<T>(ObjectCreatorWithState<object> objectCreator) where T : class
        {
            _context.Add(typeof(T), objectCreator);
        }
    }
}
