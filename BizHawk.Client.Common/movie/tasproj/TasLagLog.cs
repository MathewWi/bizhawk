﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using BizHawk.Emulation.Common.IEmulatorExtensions;

namespace BizHawk.Client.Common
{
	public class TasLagLog
	{
		// TODO: Change this into a regular list.
		private readonly List<bool> LagLog = new List<bool>();

		private readonly List<bool> WasLag = new List<bool>();

		public bool? this[int frame]
		{
			get
			{
				if (frame < LagLog.Count)
				{
					if (frame < 0)
						return null;
					else
						return LagLog[frame];
				}
				else if (frame == Global.Emulator.Frame && frame == LagLog.Count)
				{
					// LagLog[frame] = Global.Emulator.AsInputPollable().IsLagFrame; // Note: Side effects!
					return Global.Emulator.AsInputPollable().IsLagFrame;
				}

				return null;
			}

			set
			{
				if (!value.HasValue)
				{
					LagLog.RemoveAt(frame);
					return;
				}
				else if (frame < 0)
				{
					return; // Nothing to do
				}

				if (frame >= LagLog.Count)
				{
					do { LagLog.Add(value.Value); } while (frame >= LagLog.Count);
				}
				else
					LagLog[frame] = value.Value;

				if (frame == WasLag.Count)
					WasLag.Add(value.Value);
				else
					WasLag[frame] = value.Value;
			}
		}

		public void Clear()
		{
			LagLog.Clear();
		}

		public void RemoveFrom(int frame)
		{
			if (LagLog.Count >= frame && frame >= 0)
				LagLog.RemoveRange(frame, LagLog.Count - frame);
		}

		public void RemoveHistoryAt(int frame)
		{
			WasLag.RemoveAt(frame);
		}
		public void InsertHistoryAt(int frame, bool isLag)
		{ // LagLog was invalidated when the frame was inserted
			LagLog.Insert(frame, isLag);
			WasLag.Insert(frame, isLag);
		}

		public void Save(BinaryWriter bw)
		{
			bw.Write((byte)1); // New saving format.
			bw.Write(LagLog.Count);
			bw.Write(WasLag.Count);
			for (int i = 0; i < LagLog.Count; i++)
			{
				bw.Write(LagLog[i]);
				bw.Write(WasLag[i]);
			}
			for (int i = LagLog.Count; i < WasLag.Count; i++)
				bw.Write(WasLag[i]);
		}

		public void Load(BinaryReader br)
		{
			LagLog.Clear();
			WasLag.Clear();
			if (br.BaseStream.Length > 0)
			{
				int formatVersion = br.ReadByte();
				if (formatVersion == 0)
				{
					int length = (br.ReadByte() << 8) | formatVersion; // The first byte should be a part of length.
					length = (br.ReadInt16() << 16) | length;
					for (int i = 0; i < length; i++)
					{
						br.ReadInt32();
						LagLog.Add(br.ReadBoolean());
						WasLag.Add(LagLog.Last());
					}
				}
				else if (formatVersion == 1)
				{
					int length = br.ReadInt32();
					int lenWas = br.ReadInt32();
					for (int i = 0; i < length; i++)
					{
						LagLog.Add(br.ReadBoolean());
						WasLag.Add(br.ReadBoolean());
					}
					for (int i = length; i < lenWas; i++)
						WasLag.Add(br.ReadBoolean());
				}
			}
		}

		public bool? History(int frame)
		{
			if (frame < WasLag.Count)
			{
				if (frame < 0)
					return null;

				return WasLag[frame];
			}

			return null;
		}
	}
}
