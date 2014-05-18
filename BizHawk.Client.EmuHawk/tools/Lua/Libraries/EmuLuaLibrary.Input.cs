﻿using System.Linq;
using System.Windows.Forms;

using BizHawk.Client.Common;
using LuaInterface;

namespace BizHawk.Client.EmuHawk
{
	public class InputLuaLibrary : LuaLibraryBase
	{
		public InputLuaLibrary(Lua lua)
		{
			_lua = lua;
		}

		public override string Name { get { return "input"; } }

		private readonly Lua _lua;

		[LuaMethodAttributes(
			"get",
			"Returns a lua table of all the buttons the user is currently pressing on their keyboard and gamepads"
		)]
		public LuaTable Get()
		{
			var buttons = _lua.NewTable();
			foreach (var kvp in Global.ControllerInputCoalescer.BoolButtons().Where(kvp => kvp.Value))
			{
				buttons[kvp.Key] = true;
			}

			return buttons;
		}

		[LuaMethodAttributes(
			"getmouse",
			"Returns a lua table of the mouse X/Y coordinates and button states. Table returns the values X, Y, Left, Middle, Right, XButton1, XButton2"
		)]
		public LuaTable GetMouse()
		{
			var buttons = _lua.NewTable();
			//TODO - need to specify whether in "emu" or "native" coordinate space.
			var p = GlobalWin.DisplayManager.UntransformPoint(Control.MousePosition);
			buttons["X"] = p.X;
			buttons["Y"] = p.Y;
			buttons[MouseButtons.Left.ToString()] = (Control.MouseButtons & MouseButtons.Left) != 0;
			buttons[MouseButtons.Middle.ToString()] = (Control.MouseButtons & MouseButtons.Middle) != 0;
			buttons[MouseButtons.Right.ToString()] = (Control.MouseButtons & MouseButtons.Right) != 0;
			buttons[MouseButtons.XButton1.ToString()] = (Control.MouseButtons & MouseButtons.XButton1) != 0;
			buttons[MouseButtons.XButton2.ToString()] = (Control.MouseButtons & MouseButtons.XButton2) != 0;
			return buttons;
		}
	}
}