﻿using System;
using DeltaEngine.Core;
using DeltaEngine.Entities;
using DeltaEngine.Extensions;

namespace DeltaEngine.Logging
{
	/// <summary>
	/// Very simple logger just spaming out all log events into the console. Used by default.
	/// </summary>
	public class ConsoleLogger : Logger
	{
		public ConsoleLogger()
			: base(true) {}

		/// <summary>
		/// Write all messages to the console, but limit to 100 per second and 10 in debug mode as the
		/// console is very slow and will cause the app to freeze if too many messages are processed.
		/// </summary>
		public override void Write(MessageType messageType, string message)
		{
			if (numberOfMessagesWritten >
				(Time.Total + 1) * (ExceptionExtensions.IsDebugMode ? 10 : 100))
			{
				if (!warnTooManyMessages)
					Console.WriteLine("Too many messages " + numberOfMessagesWritten +
						" written to the console, ignoring until time passes on ..");
				warnTooManyMessages = true;
				return;
			}
			Console.WriteLine(CreateMessageTypePrefix(messageType) + message);
			numberOfMessagesWritten++;
		}

		private int numberOfMessagesWritten;
		private bool warnTooManyMessages;

		public override void Dispose()
		{
			base.Dispose();
			if (NumberOfRepeatedMessagesIgnored > 0)
				Console.WriteLine("NumberOfRepeatedMessagesIgnored=" + NumberOfRepeatedMessagesIgnored);
		}
	}
}