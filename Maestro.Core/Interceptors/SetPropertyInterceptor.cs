﻿using System;

namespace Maestro.Interceptors
{
	internal class SetPropertyInterceptor : IInterceptor
	{
		private readonly string _propertyName;
		private Func<IContext, object> _factory;
		private Action<object, object> _setter;

		public SetPropertyInterceptor(string propertyName, Func<IContext, object> factory = null)
		{
			_propertyName = propertyName;
			_factory = factory;
		}

		public IInterceptor Clone()
		{
			return new SetPropertyInterceptor(_propertyName);
		}

		public object Execute(object instance, IContext context)
		{
			var instanceType = instance.GetType();
			var propertyType = instanceType.GetProperty(_propertyName).PropertyType;

			if (_factory == null)
				_factory = Reflector.GetPropertyValueProvider(propertyType, context);

			if (_setter == null)
				_setter = Reflector.GetPropertySetter(instanceType, _propertyName);

			var value = _factory(context);
			_setter(instance, value);
			return instance;
		}

		public override string ToString()
		{
			return string.Format("set property {0}", _propertyName);
		}
	}
}