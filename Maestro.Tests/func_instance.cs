﻿using System;
using FluentAssertions;
using Xunit;

namespace Maestro.Tests
{
	public class func_instance
	{
		[Fact]
		public void should_delegate_instantiation_to_provided_func()
		{
			var name = "foo";
			var container = new Container(x =>
			{
				x.Default<object>().Is(() => new EventArgs());
				x.Add<object>(name).As(_ => new Exception());
			});

			var @default = container.Get<object>();
			var named = container.Get<object>(name);

			@default.Should().BeOfType<EventArgs>();
			named.Should().BeOfType<Exception>();
		}
	}
}