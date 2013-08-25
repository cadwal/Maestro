﻿using System.Linq;

namespace Maestro
{
	public abstract class LifecycleBase : ILifecycle
	{
		public virtual ILifecycle Clone()
		{
			var defaultCtor = GetType().GetConstructors().FirstOrDefault(x => x.GetParameters().Length == 0);
			return defaultCtor == null
				? this
				: (ILifecycle)defaultCtor.Invoke(null);
		}

		public abstract object Process(IContext context, IPipeline pipeline);
	}
}