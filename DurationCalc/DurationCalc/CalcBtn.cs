using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BRY
{
	public enum ColorType
	{
		White,
		GrayL,
		GrayM,
		GrayD,
		RedL,
		Black,
		BlackL
	};
	public  class CalcBtn : Xamarin.Forms.Button
	{
		public CALCEXEC m_ExecCodec = CALCEXEC.NONE;
		public CALCEXEC ExecCode
		{
			get
			{
				return m_ExecCodec;
			}
			set
			{
				m_ExecCodec = value;
			}
		}
		// *********************************************************************
		public CalcBtn()
		{
			this.Margin = 1;
			this.FontSize = 24;
		}
		// *********************************************************************
		public void SetBGColor(ColorType ct )
		{
			switch(ct)
			{
				case ColorType.GrayL:
					this.BackgroundColor = Color.FromRgb(0xF0, 0xF0, 0xF0);
					break;
				case ColorType.GrayM:
					this.BackgroundColor = Color.FromRgb(0xE8, 0xE8, 0xE8);
					break;
				case ColorType.GrayD:
					this.BackgroundColor = Color.FromRgb(0xD0, 0xD0, 0xD0);
					break;
				case ColorType.RedL:
					this.BackgroundColor = Color.FromRgb(0xF0, 0xA0, 0xA0);
					break;
				case ColorType.White:
					this.BackgroundColor = Color.FromRgb(0xFF, 0xFF, 0xFF);
					break;
				case ColorType.Black:
					this.BackgroundColor = Color.FromRgb(0x00, 0x00, 0x00);
					break;
				case ColorType.BlackL:
					this.BackgroundColor = Color.FromRgb(0x20, 0x20, 0x20);
					break;
			}
		}
	}
}
