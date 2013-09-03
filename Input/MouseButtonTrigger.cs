﻿using System;
using DeltaEngine.Commands;
using DeltaEngine.Extensions;

namespace DeltaEngine.Input
{
	/// <summary>
	/// Allows mouse button presses to be tracked.
	/// </summary>
	public class MouseButtonTrigger : PositionTrigger
	{
		public MouseButtonTrigger(State state)
			: this(MouseButton.Left, state) {}

		public MouseButtonTrigger(MouseButton button = MouseButton.Left, State state = State.Pressing)
		{
			Button = button;
			State = state;
			Start<Mouse>();
		}

		public MouseButton Button { get; internal set; }
		public State State { get; internal set; }

		public MouseButtonTrigger(string buttonAndState)
		{
			var parameters = buttonAndState.SplitAndTrim(new[] { ' ' });
			if (parameters.Length == 0)
				throw new CannotCreateMouseButtonTriggerWithoutButton();
			Button = parameters[0].Convert<MouseButton>();
			State = parameters.Length > 1 ? parameters[1].Convert<State>() : State.Pressing;
			Start<Mouse>();
		}

		public class CannotCreateMouseButtonTriggerWithoutButton : Exception {}
	}
}