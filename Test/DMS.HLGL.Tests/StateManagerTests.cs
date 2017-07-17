using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DMS.HLGL.Tests
{
	[TestClass()]
	public class StateManagerTests
	{
		class StateBool : IStateBool
		{
			public bool Enabled { get; set; } = false;
		}

		class StateFloat : IStateFloat
		{
			public float Value { get; set; }
		}

		[TestMethod()]
		public void RegisterTest()
		{
			var stateManager = new StateManager();
			stateManager.Register<IStateBool, IBlending>(new StateBool());
			var blend = stateManager.GetState<IStateBool, IBlending>();
			blend.Enabled = true;
			Assert.IsTrue(stateManager.GetState<IStateBool, IBlending>().Enabled);

			blend.Enabled = false;
			Assert.IsFalse(stateManager.GetState<IStateBool, IBlending>().Enabled);
		}

		[TestMethod()]
		[ExpectedException(typeof(ArgumentException))]
		public void UnregisteredTest()
		{
			var stateManager = new StateManager();
			var blend = stateManager.GetState<IStateBool, IBlending>();
		}

		[TestMethod()]
		[ExpectedException(typeof(ArgumentException))]
		public void RegisterWrongTypeTest()
		{
			var stateManager = new StateManager();
			stateManager.Register<IStateBool, IBackfaceCulling>(new StateBool());
			var state = stateManager.GetState<IStateBool, IBlending>();
		}

		[TestMethod()]
		[ExpectedException(typeof(InvalidCastException))]
		public void RegisterWrongInterfaceTest()
		{
			var stateManager = new StateManager();
			stateManager.Register<IStateBool, IBlending>(new StateFloat());
		}

		[TestMethod()]
		[ExpectedException(typeof(InvalidCastException))]
		public void RegisterWrongInterfaceOnGetTest()
		{
			var stateManager = new StateManager();
			stateManager.Register<IStateBool, IBlending>(new StateFloat());
			var state = stateManager.GetState<IStateBool, IBlending>();
		}

		[TestMethod()]
		[ExpectedException(typeof(ArgumentException))]
		public void RegisterSameTest()
		{
			var stateManager = new StateManager();
			//not intended purpsoe but still valid
			stateManager.Register<StateBool, StateBool>(new StateBool());
			//register same again
			stateManager.Register<StateBool, StateBool>(new StateBool());
			var state = stateManager.GetState<IStateBool, StateBool>();
		}

		class StateBool2 : StateBool { };

		[TestMethod()]
		public void RegisterCustomClassTest()
		{
			var stateManager = new StateManager();
			//not intended purpose but still valid
			stateManager.Register<StateBool, StateBool>(new StateBool());
			//register same again
			stateManager.Register<StateBool, StateBool2>(new StateBool());
			var state = stateManager.GetState<IStateBool, StateBool>();
			var state2 = stateManager.GetState<IStateBool, StateBool2>();
			Assert.AreNotSame(state, state2);
			Assert.AreEqual(state.Enabled, state2.Enabled);
		}
	}
}