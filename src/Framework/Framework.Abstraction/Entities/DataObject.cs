using System;

namespace Framework.Abstraction.Entities
{
    public abstract class EqualableDataObject
    {                       
        public override bool Equals(object obj)
        {
            return TryCast<EqualableDataObject>(obj, Equals, false);
        }

        protected bool Equals(EqualableDataObject other)
        {
            return true;
        }        

        protected bool TryCast<T>(object value, Func<T, bool> func, bool standard)
            where T : class
        {
            if (value is T)
            {
                return func((T)value);
            }
            return standard;
        }

        protected void TryCast<T>(object value, Action<T> func)
            where T : class
        {
            if (value is T)
            {
                func((T)value);
            }
        }

        protected void TryCast<T,TP1>(object value, Action<T,TP1> func, TP1 parameter1)
            where T : class
        {
            if (value is T)
            {
                func((T)value, parameter1);
            }
        }        
    }
}
