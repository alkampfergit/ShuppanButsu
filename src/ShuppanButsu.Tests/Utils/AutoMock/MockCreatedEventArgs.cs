using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShuppanButsu.Tests.Utils.AutoMock
{
	public class MockCreatedEventArgs : EventArgs
	{
		public Object Mock { get; set; }

		public String DependencyName { get; set; }

		public MockCreatedEventArgs(object mock, string dependencyName)
		{
			Mock = mock;
			DependencyName = dependencyName;
		}

		public T Get<T>()
		{
			return (T) Mock;
		}
	}
}
