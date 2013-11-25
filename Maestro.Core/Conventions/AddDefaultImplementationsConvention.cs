﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Maestro.Conventions
{
	internal class AddDefaultImplementationsConvention : IConvention
	{
		public void Process(IEnumerable<Type> types, IContainerExpression containerExpression)
		{
			types = types as IList<Type> ?? types.ToList();

			var interfaces = types.Where(x => x.IsInterface);
			var classes = types.Where(x => x.IsConcreteClass()).GroupBy(x => x.Namespace ?? string.Empty).ToDictionary(x => x.Key, x => x.ToList());

			foreach (var @interface in interfaces)
			{
				List<Type> list;
				if (!classes.TryGetValue(@interface.Namespace ?? string.Empty, out list)) continue;
				var @class = list.SingleOrDefault(x => x.Name == @interface.Name.Substring(1));
				if (@class == null) continue;
				containerExpression.For(@interface).Use(@class);
			}
		}
	}
}