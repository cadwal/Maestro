﻿using FluentAssertions;
using System;
using System.Collections.Generic;
using Maestro.Interceptors;
using Xunit;

namespace Maestro.Tests
{
	public class intercept_instance
	{
		[Fact]
		public void provided_OnCreate_interceptor_should_be_executed()
		{
			var interceptor = new Interceptor();

			new Container(x => x.For<object>().Use<object>().OnCreate.InterceptUsing(interceptor)).Get<object>();

			interceptor.IsExecuted.Should().BeTrue();
		}

		[Fact]
		public void provided_OnActivate_interceptor_should_be_executed()
		{
			var interceptor = new Interceptor();

			new Container(x => x.For<object>().Use<object>().OnActivate.InterceptUsing(interceptor)).Get<object>();

			interceptor.IsExecuted.Should().BeTrue();
		}

		[Fact]
		public void OnCreate_interceptors_should_be_executed_before_OnActivate_interceptors()
		{
			var list = new List<string>();

			new Container(x => x.For<object>().Use<object>()
				.OnCreate.InterceptUsing(new Interceptor(() => list.Add("create")))
				.OnActivate.InterceptUsing(new Interceptor(() => list.Add("activate"))))
				.Get<object>();

			list.Should().ContainInOrder(new[] { "create", "activate" });
		}

		[Fact]
		public void interceptors_should_be_executed_in_the_same_order_they_are_configured()
		{
			var list = new List<string>();

			new Container(x => x.For<object>().Use<object>()
				.OnCreate.InterceptUsing(new Interceptor(() => list.Add("create1")))
				.OnCreate.InterceptUsing(new Interceptor(() => list.Add("create2")))
				.OnActivate.InterceptUsing(new Interceptor(() => list.Add("activate1")))
				.OnActivate.InterceptUsing(new Interceptor(() => list.Add("activate2"))))
			.Get<object>();

			list.Should().ContainInOrder(new[] { "create1", "create2", "activate1", "activate2" });
		}

		[Fact]
		public void OnCreate_insterceptors_should_not_be_executed_if_instance_is_cached()
		{
			var interceptor = new Interceptor();

			new Container(x => x.For<object>().Use<object>()
				.OnActivate.InterceptUsing(interceptor)
				.Lifecycle.Singleton()).Get<object>();

			interceptor.ExecuteCount.Should().Be(1);
		}

		[Fact]
		public void OnActivate_interceptors_should_be_executed_even_when_instance_is_chached()
		{
			var interceptor = new Interceptor();

			var container = new Container(x => x.For<object>().Use<object>()
				.OnActivate.InterceptUsing(interceptor)
				.Lifecycle.Singleton());
			container.Get<object>();
			container.Get<object>();

			interceptor.ExecuteCount.Should().Be(2);
		}

		private class Interceptor : InterceptorBase
		{
			private readonly Action _action;

			public Interceptor() : this(() => { }) { }
			public Interceptor(Action action)
			{
				_action = action;
			}

			public int ExecuteCount { get; private set; }

			public bool IsExecuted
			{
				get { return ExecuteCount != 0; }
			}

			public override object Execute(object instance, IContext context)
			{
				_action();
				ExecuteCount++;
				return instance;
			}
		}
	}
}