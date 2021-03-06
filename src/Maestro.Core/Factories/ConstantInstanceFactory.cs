﻿using System;

namespace Maestro.Factories
{
	internal class ConstantInstanceFactory : IInstanceFactory
	{
		private readonly object _instance;

		public ConstantInstanceFactory(object instance)
		{
			_instance = instance;
		}

		public bool CanGet(IContext context)
		{
			return true;
		}

		public object Get(IContext context)
		{
			return _instance;
		}

		public IInstanceFactory MakeGeneric(Type[] types)
		{
			throw new NotSupportedException();
		}

		public IInstanceFactory Clone()
		{
			return this;
		}

		public override string ToString()
		{
			var strings = new[] { _instance.GetType().ToString(), _instance.ToString() };
			return string.Format("constant instance : {0}", string.Join(" > ", strings));
		}
	}
}