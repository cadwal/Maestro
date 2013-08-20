﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Maestro
{
	internal static class TypeHelper
	{
		public static void AssertTypeIsSupported(Type type)
		{
			if (!IsTypeSupported(type))
				throw new NotSupportedException(string.Format("Type '{0}' is not supported.", type.FullName));
		}

		public static IEnumerable<Type> GetClasses(this Type @class)
		{
			if (!@class.IsClass)
				throw new InvalidOperationException();

			do
			{
				yield return @class;
				@class = @class.BaseType;
			} while (@class != null);
		}

		public static bool IsTypeSupported(Type type)
		{
			return !type.IsValueType && type != typeof(string);
		}

		public static bool IsConcreteClosedClass(this Type type)
		{
			return type.IsClass && type != typeof(string) && !type.IsAbstract && !type.IsGenericTypeDefinition;
		}

		public static bool IsConcreteSubClassOf<T>(this Type type)
		{
			return type.IsConcreteSubClassOf(typeof(T));
		}

		public static bool IsConcreteSubClassOf(this Type type, Type basetype)
		{
			if (type == basetype)
				return false;

			if (type.IsAbstract || !type.IsClass)
				return false;

			if (type.IsGenericTypeDefinition ^ basetype.IsGenericTypeDefinition)
				return false;

			if (!basetype.IsGenericTypeDefinition)
				return basetype.IsAssignableFrom(type);

			var args = basetype.GetGenericArguments();
			var typeArgs = type.GetGenericArguments();

			if (args.Length != typeArgs.Length)
				return false;

			var typeDefinitions =
				from t in basetype.IsClass ? type.GetClasses() : type.GetInterfaces()
				where t.IsGenericType
				let tArgs = t.GetGenericArguments().Where(x => x.FullName == null).Distinct().ToArray()
				where args.Length == tArgs.Length
				select t.GetGenericTypeDefinition();

			return typeDefinitions.Any(x => x == basetype);
		}
	}
}