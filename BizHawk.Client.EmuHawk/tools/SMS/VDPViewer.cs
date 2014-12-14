﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BizHawk.Emulation.Cores.Sega.MasterSystem;
using BizHawk.Common;
using BizHawk.Client.Common;
using System.Drawing.Imaging;

namespace BizHawk.Client.EmuHawk
{
	[RequiredServices(typeof(SMS))]
	public partial class SmsVDPViewer : Form, IToolForm
	{
		public IDictionary<Type, object> EmulatorServices { private get; set; }
		private VDP vdp { get { return (EmulatorServices[typeof(SMS)] as SMS).Vdp; } }

		int palindex = 0;

		public SmsVDPViewer()
		{
			InitializeComponent();

			bmpViewTiles.ChangeBitmapSize(256, 128);
			bmpViewPalette.ChangeBitmapSize(16, 2);
			bmpViewBG.ChangeBitmapSize(256, 256);
		}

		unsafe static void Draw8x8(byte* src, int* dest, int pitch, int* pal)
		{
			int inc = pitch - 8;
			dest -= inc;
			for (int i = 0; i < 64; i++)
			{
				if ((i & 7) == 0)
					dest += inc;
				*dest++ = pal[*src++];
			}
		}

		unsafe static void Draw8x8hv(byte* src, int* dest, int pitch, int* pal, bool hflip, bool vflip)
		{
			int incx = hflip ? -1 : 1;
			int incy = vflip ? -pitch : pitch;
			if (hflip)
				dest -= incx * 7;
			if (vflip)
				dest -= incy * 7;
			incy -= incx * 8;
			for (int j = 0; j < 8; j++)
			{
				for (int i = 0; i < 8; i++)
				{
					*dest = pal[*src++];
					dest += incx;
				}
				dest += incy;
			}
		}

		unsafe void DrawTiles(int *pal)
		{
			var lockdata = bmpViewTiles.bmp.LockBits(new Rectangle(0, 0, 256, 128), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
			int* dest = (int*)lockdata.Scan0;
			int pitch = lockdata.Stride / sizeof(int);

			fixed (byte* src = vdp.PatternBuffer)
			{
				for (int tile = 0; tile < 512; tile++)
				{
					int srcaddr = tile * 64;
					int tx = tile & 31;
					int ty = tile >> 5;
					int destaddr = ty * 8 * pitch + tx * 8;
					Draw8x8(src + srcaddr, dest + destaddr, pitch, pal);
				}
			}
			bmpViewTiles.bmp.UnlockBits(lockdata);
			bmpViewTiles.Refresh();
		}

		unsafe void DrawBG(int* pal)
		{
			int bgheight = vdp.FrameHeight == 192 ? 224 : 256;
			int maxtile = bgheight * 4;
			if (bgheight != bmpViewBG.bmp.Height)
			{
				bmpViewBG.Height = bgheight;
				bmpViewBG.ChangeBitmapSize(256, bgheight);
			}

			var lockdata = bmpViewBG.bmp.LockBits(new Rectangle(0, 0, 256, bgheight), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
			int* dest = (int*)lockdata.Scan0;
			int pitch = lockdata.Stride / sizeof(int);

			fixed (byte* src = vdp.PatternBuffer)
			fixed (byte* vram = vdp.VRAM)
			{
				short* map = (short*)(vram + vdp.CalcNameTableBase());

				for (int tile = 0; tile < maxtile; tile++)
				{
					short bgent = *map++;
					bool hflip = (bgent & 1 << 9) != 0;
					bool vflip = (bgent & 1 << 10) != 0;
					int* tpal = pal + ((bgent & 1 << 11) >> 7);
					int srcaddr = (bgent & 511) * 64;
					int tx = tile & 31;
					int ty = tile >> 5;
					int destaddr = ty * 8 * pitch + tx * 8;
					Draw8x8hv(src + srcaddr, dest + destaddr, pitch, tpal, hflip, vflip);
				}
			}
			bmpViewBG.bmp.UnlockBits(lockdata);
			bmpViewBG.Refresh();
		}

		unsafe void DrawPal(int* pal)
		{
			var lockdata = bmpViewPalette.bmp.LockBits(new Rectangle(0, 0, 16, 2), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
			int* dest = (int*)lockdata.Scan0;
			int pitch = lockdata.Stride / sizeof(int);

			for (int j = 0; j < 2; j++)
			{
				for (int i = 0; i < 16; i++)
				{
					*dest++ = *pal++;
				}
				dest -= 16;
				dest += pitch;
			}
			bmpViewPalette.bmp.UnlockBits(lockdata);
			bmpViewPalette.Refresh();
		}

		public void UpdateValues()
		{
			unsafe
			{
				fixed (int* pal = vdp.Palette)
				{
					DrawTiles(pal + palindex * 16);
					DrawBG(pal);
					DrawPal(pal);
				}
			}
		}

		public void FastUpdate()
		{
			// Do nothing
		}

		public void Restart()
		{
			UpdateValues();
		}

		public bool AskSaveChanges()
		{
			return true;
		}

		public bool UpdateBefore
		{
			get { return true; }
		}

		private void bmpViewPalette_MouseClick(object sender, MouseEventArgs e)
		{
			int p = Math.Min(Math.Max(e.Y / 16, 0), 1);
			palindex = p;
			unsafe
			{
				fixed (int* pal = vdp.Palette)
				{
					DrawTiles(pal + palindex * 16);
				}
			}
		}

		private void VDPViewer_KeyDown(object sender, KeyEventArgs e)
		{
			if (ModifierKeys.HasFlag(Keys.Control) && e.KeyCode == Keys.C)
			{
				// find the control under the mouse
				Point m = Cursor.Position;
				Control top = this;
				Control found;
				do
				{
					found = top.GetChildAtPoint(top.PointToClient(m));
					top = found;
				} while (found != null && found.HasChildren);

				if (found is BmpView)
				{
					var bv = found as BmpView;
					Clipboard.SetImage(bv.bmp);
				}
			}
		}

		private void RefreshFloatingWindowControl()
		{
			Owner = Global.Config.SmsVdpSettings.FloatingWindow ? null : GlobalWin.MainForm;
		}

		protected override void OnShown(EventArgs e)
		{
			RefreshFloatingWindowControl();
			base.OnShown(e);
		}

		private void CloseMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void OptionsSubMenu_DropDownOpened(object sender, EventArgs e)
		{
			AutoloadMenuItem.Checked = Global.Config.SmsVdpAutoLoad;
			SaveWindowPositionMenuItem.Checked = Global.Config.SmsVdpSettings.SaveWindowPosition;
			AlwaysOnTopMenuItem.Checked = Global.Config.SmsVdpSettings.TopMost;
			FloatingWindowMenuItem.Checked = Global.Config.SmsVdpSettings.FloatingWindow;
		}

		private void SaveWindowPositionMenuItem_Click(object sender, EventArgs e)
		{
			Global.Config.SmsVdpSettings.SaveWindowPosition ^= true;
		}

		private void AlwaysOnTopMenuItem_Click(object sender, EventArgs e)
		{
			TopMost = Global.Config.SmsVdpSettings.TopMost ^= true;
		}

		private void FloatingWindowMenuItem_Click(object sender, EventArgs e)
		{
			Global.Config.SmsVdpSettings.FloatingWindow ^= true;
			RefreshFloatingWindowControl();
		}

		private void VDPViewer_Load(object sender, EventArgs e)
		{
			TopMost = Global.Config.SmsVdpSettings.TopMost;
			if (Global.Config.SmsVdpSettings.UseWindowPosition)
			{
				Location = Global.Config.SmsVdpSettings.WindowPosition;
			}

			Restart();
		}

		private void AutoloadMenuItem_Click(object sender, EventArgs e)
		{
			Global.Config.SmsVdpAutoLoad ^= true;
		}

		private void saveTilesScreenshotToolStripMenuItem_Click(object sender, EventArgs e)
		{
			bmpViewTiles.SaveFile();
		}

		private void savePalettesScrenshotToolStripMenuItem_Click(object sender, EventArgs e)
		{
			bmpViewPalette.SaveFile();
		}

		private void saveBGScreenshotToolStripMenuItem_Click(object sender, EventArgs e)
		{
			bmpViewBG.SaveFile();
		}
	}
}
