﻿using System;

namespace Maestro.Fluent
{
	internal class LifecycleSelector<TParent> : ILifecycleSelector<TParent>
	{
		private readonly TParent _parent;
		private readonly Action<ILifecycle> _action;

		public LifecycleSelector(TParent parent, Action<ILifecycle> action)
		{
			_parent = parent;
			_action = action;
		}

		public TParent Transient()
		{
			throw new System.NotImplementedException();
		}

		public TParent Request()
		{
			throw new System.NotImplementedException();
		}

		public TParent Singleton()
		{
			throw new System.NotImplementedException();
		}

		public TParent Custom<TLifecycle>() where TLifecycle : ILifecycle, new()
		{
			return Custom(new TLifecycle());
		}

		public TParent Custom(ILifecycle lifecycle)
		{
			_action(lifecycle);
			return _parent;
		}
	}
}