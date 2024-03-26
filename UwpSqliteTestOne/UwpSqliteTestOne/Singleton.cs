using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UwpSqliteTestOne
{
    public static class Singleton<T> where T : class
    {
        private static readonly Lazy<T> instance = new Lazy<T>(() =>
        {
            var constructor = typeof(T).GetConstructor(
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
                null, new Type[0], null);
            return (T)constructor.Invoke(null);
        });

        public static T Instance => instance.Value;
    }
}
