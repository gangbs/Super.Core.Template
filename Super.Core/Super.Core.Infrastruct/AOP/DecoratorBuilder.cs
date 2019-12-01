using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Super.Core.Infrastruct.AOP
{
    public class DecoratorBuilder<T> where T : class//T必须为一个抽象的接口
    {
        private T _component;
        private object _proxy;

        public DecoratorBuilder(T component)
        {
            this._component = component;
        }

        public T BuilderDecorator()
        {
            return this.BuilderDecorator(null, null);
        }

        public T BuilderDecorator(Action<MethodInfo, object[]> beforeAction, Action<MethodInfo, object[], object> afterAction)
        {
            _proxy = DispatchProxy.Create<T, DecoratorProxy<T>>();
            ((DecoratorProxy<T>)_proxy).Wrapped = this._component;
            ((DecoratorProxy<T>)_proxy).BeforeAction = beforeAction;
            ((DecoratorProxy<T>)_proxy).AfterAction = afterAction;
            return (T)_proxy;
        }

        public void AddBeforeAction(Action<MethodInfo, object[]> action)
        {
            ((DecoratorProxy<T>)_proxy).BeforeAction += action;
        }
        public void RemoveBeforeAction(Action<MethodInfo, object[]> action)
        {
            ((DecoratorProxy<T>)_proxy).BeforeAction -= action;
        }

        public void AddAfterAction(Action<MethodInfo, object[], object> action)
        {
            ((DecoratorProxy<T>)_proxy).AfterAction += action;
        }
        public void RemoveAfterAction(Action<MethodInfo, object[], object> action)
        {
            ((DecoratorProxy<T>)_proxy).AfterAction -= action;
        }

    }
}
