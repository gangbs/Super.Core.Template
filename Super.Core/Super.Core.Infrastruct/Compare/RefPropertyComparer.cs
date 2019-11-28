using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Super.Core.Infrastruct.Compare
{
    public class RefPropertyComparer<T> : IEqualityComparer<T>
    {

        readonly PropertyInfo _propertyInfo;

        public RefPropertyComparer(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentException("parameter can not null！");
            }
            this._propertyInfo = propertyInfo;
        }

        public RefPropertyComparer(string propertyName)
        {
            this._propertyInfo = typeof(T).GetProperty(propertyName, BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public);
            if (_propertyInfo == null)
            {
                throw new ArgumentException(string.Format("{0} is not a property of type {1}.",
                    propertyName, typeof(T)));
            }
        }


        public bool Equals(T x, T y)
        {
            object xValue = _propertyInfo.GetValue(x, null);
            object yValue = _propertyInfo.GetValue(y, null);

            if (xValue == null)
                return yValue == null;

            return xValue.Equals(yValue);
        }

        public int GetHashCode(T obj)
        {
            object propertyValue = _propertyInfo.GetValue(obj, null);

            if (propertyValue == null)
                return 0;
            else
                return propertyValue.GetHashCode();
        }
    }
}
