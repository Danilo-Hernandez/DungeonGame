using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DebugCommandBase
{
	private string _commandID;
	private string _commandDesc;
	private string _commandFormat;

	public string commandID {get {return _commandID;}}
	public string commandDesc {get {return _commandDesc;}}
	public string commandFormat {get {return _commandFormat;}}

	public DebugCommandBase(string id, string desc, string format) {
		_commandID = id;
		_commandFormat = format;
		_commandDesc = desc;
	}
}

public class DebugCommand<T1> : DebugCommandBase {
	private Action<T1> command;

	public DebugCommand(string id, string desc, string format, Action<T1> command) : base(id, desc, format) {
		this.command = command;
	}

	public void Invoke(T1 value) {
		command.Invoke(value);
	}
}

public class DebugCommand : DebugCommandBase {
	private Action command;

	public DebugCommand(string id, string desc, string format, Action command) : base(id, desc, format) {
		this.command = command;
	}

	public void Invoke() {
		command.Invoke();
	}
}
