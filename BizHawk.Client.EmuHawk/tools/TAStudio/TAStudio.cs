﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using BizHawk.Client.Common;

namespace BizHawk.Client.EmuHawk
{
	public partial class TAStudio : Form, IToolForm
	{
		private int _defaultWidth;
		private int _defaultHeight;

		#region API

		public TAStudio()
		{
			InitializeComponent();
			TASView.QueryItemText += TASView_QueryItemText;
			TASView.QueryItemBkColor += TASView_QueryItemBkColor;
			TASView.VirtualMode = true;
			Closing += (o, e) =>
			{
				if (AskSave())
				{
					SaveConfigSettings();
				}
				else
				{
					e.Cancel = true;
				}

				GlobalWin.OSD.AddMessage("TAStudio Disengaged");
			};

			TopMost = Global.Config.TAStudioTopMost;
		}

		public bool AskSave()
		{
			// TODO: eventually we want to do this
			return true;
		}

		public bool UpdateBefore
		{
			get { return false; }
		}

		public void UpdateValues()
		{
			if (!IsHandleCreated || IsDisposed) return;
		}

		public void Restart()
		{
			if (!IsHandleCreated || IsDisposed) return;
		}

		#endregion

		private void TASView_QueryItemBkColor(int index, int column, ref Color color)
		{
			
		}

		private void TASView_QueryItemText(int index, int column, out string text)
		{
			text = String.Empty;
		}

		private void TAStudio_Load(object sender, EventArgs e)
		{
			GlobalWin.OSD.AddMessage("TAStudio engaged");
			LoadConfigSettings();
		}

		private void LoadConfigSettings()
		{
			_defaultWidth = Size.Width;
			_defaultHeight = Size.Height;

			if (Global.Config.TAStudioSaveWindowPosition && Global.Config.TASWndx >= 0 && Global.Config.TASWndy >= 0)
			{
				Location = new Point(Global.Config.TASWndx, Global.Config.TASWndy);
			}

			if (Global.Config.TASWidth >= 0 && Global.Config.TASHeight >= 0)
			{
				Size = new Size(Global.Config.TASWidth, Global.Config.TASHeight);
			}
		}

		private void SaveConfigSettings()
		{
			Global.Config.TASWndx = Location.X;
			Global.Config.TASWndy = Location.Y;
			Global.Config.TASWidth = Right - Left;
			Global.Config.TASHeight = Bottom - Top;
		}

		#region Events

		#region File Menu

		private void ExitMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}

		#endregion

		#region Settings Menu
		
		private void SettingsSubMenu_DropDownOpened(object sender, EventArgs e)
		{
			SaveWindowPositionMenuItem.Checked = Global.Config.TAStudioSaveWindowPosition;
			AutoloadMenuItem.Checked = Global.Config.AutoloadTAStudio;
			AlwaysOnTopMenuItem.Checked = Global.Config.TAStudioTopMost;
		}

		private void AutoloadMenuItem_Click(object sender, EventArgs e)
		{
			Global.Config.AutoloadTAStudio ^= true;
		}
		private void SaveWindowPositionMenuItem_Click(object sender, EventArgs e)
		{
			Global.Config.TAStudioSaveWindowPosition ^= true;
		}

		private void AlwaysOnTopMenuItem_Click(object sender, EventArgs e)
		{
			Global.Config.TAStudioTopMost ^= true;
		}

		#endregion

		#endregion
	}
}
