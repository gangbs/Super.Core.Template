using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Super.Core.Infrastruct.AOP
{
    public class DecoratorProxy<T> : DispatchProxy
    {
        public T Wrapped;
        public Action<MethodInfo, object[]> BeforeAction;   //动作之前执行
        public Action<MethodInfo, object[], object> AfterAction;   //动作之后执行

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            if (this.BeforeAction != null)
                this.BeforeAction(targetMethod, args);

            var result = targetMethod.Invoke(this.Wrapped, args);

            if (this.AfterAction != null)
                this.AfterAction(targetMethod, args, result);

            return result;
        }


    }
}
