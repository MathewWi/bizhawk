﻿using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BizHawk.Client.Common
{
	/// <summary>
	/// Represents a TasMovie Marker
	/// A marker is a tagged frame with a message
	/// </summary>
	public class TasMovieMarker
	{
		private int _frame;

		public TasMovieMarker(int frame, string message = "")
		{
			_frame = frame;
			Message = message;
		}

		/// <summary>
		/// Parses a Marker from a line of text
		/// </summary>
		public TasMovieMarker(string line)
		{
			var split = line.Split('\t');
			_frame = int.Parse(split[0]);
			Message = split[1];
		}

		public virtual int Frame 
		{
			get { return _frame; }
		}

		public virtual string Message { get; set; }

		public override string ToString()
		{
			return Frame.ToString() + '\t' + Message;
		}

		public override int GetHashCode()
		{
			return this.Frame.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			else if (obj is TasMovieMarker)
			{
				return this.Frame == (obj as TasMovieMarker).Frame;
			}
			else
			{
				return false;
			}
		}

		public static bool operator ==(TasMovieMarker a, TasMovieMarker b)
		{
			return a.Frame == b.Frame;
		}

		public static bool operator !=(TasMovieMarker a, TasMovieMarker b)
		{
			return a.Frame != b.Frame;
		}

		public static bool operator ==(TasMovieMarker marker, int frame)
		{
			return marker.Frame == frame;
		}

		public static bool operator !=(TasMovieMarker marker, int frame)
		{
			return marker.Frame != frame;
		}
	}

	public class TasMovieMarkerList : List<TasMovieMarker>
	{
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			foreach (var marker in this)
			{
				sb.AppendLine(marker.ToString());
			}

			return sb.ToString();
		}

		public new void Add(TasMovieMarker item)
		{
			var existingItem = this.FirstOrDefault(m => m.Frame == item.Frame);
			if (existingItem != null)
			{
				existingItem.Message = item.Message;
			}
			else
			{
				base.Add(item);
			}
		}

		public void Add(int frame, string message)
		{
			Add(new TasMovieMarker(frame, message));
		}

		public void Remove(int frame)
		{
			var existingItem = this.FirstOrDefault(m => m.Frame == frame);
			if (existingItem != null)
			{
				this.Remove(existingItem);
			}
		}
	}
}