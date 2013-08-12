﻿using System;

namespace Maestro
{
	internal interface ICustomDictionary<T>
	{
		T GetOrAdd(Type type);
		bool TryGet(Type type, out T value);
		void Clear();
	}
}