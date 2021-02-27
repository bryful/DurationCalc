using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using BRY;

namespace DurationCalc
{
	public partial class MainPage : ContentPage
	{
		private CalcBtn[] btns = new CalcBtn[(int)CALCEXEC.COUNT];
		private DurationTimeCalc m_dt = new DurationTimeCalc();

		public MainPage()
		{
			InitializeComponent();

			for (int i = 0; i < (int)CALCEXEC.COUNT; i++) btns[i] = null;

			btns[(int)CALCEXEC.K00] = k00;
			btns[(int)CALCEXEC.K01] = k01;
			btns[(int)CALCEXEC.K02] = k02;
			btns[(int)CALCEXEC.K03] = k03;
			btns[(int)CALCEXEC.K04] = k04;
			btns[(int)CALCEXEC.K05] = k05;
			btns[(int)CALCEXEC.K06] = k06;
			btns[(int)CALCEXEC.K07] = k07;
			btns[(int)CALCEXEC.K08] = k08;
			btns[(int)CALCEXEC.K09] = k09;
			btns[(int)CALCEXEC.KEX0] = kex0;
			btns[(int)CALCEXEC.KEX1] = kex1;
			btns[(int)CALCEXEC.KEX2] = kex2;

			btns[(int)CALCEXEC.SEC] = sec;
			btns[(int)CALCEXEC.BS] = bs;
			btns[(int)CALCEXEC.UNDO] = undo;
			btns[(int)CALCEXEC.TGLSIGN] = togglesign;
			btns[(int)CALCEXEC.FPS24] = fps24;
			btns[(int)CALCEXEC.FPS30] = fps30;
			btns[(int)CALCEXEC.EQUAL] = eqaul;
			btns[(int)CALCEXEC.C] = clear;
			btns[(int)CALCEXEC.AC] = ac;
			btns[(int)CALCEXEC.PLUS] = plus;
			btns[(int)CALCEXEC.MINUS] = minus;
			btns[(int)CALCEXEC.DIV] = div;
			btns[(int)CALCEXEC.MULT] = mult;


			for (int i = 0; i < (int)CALCEXEC.COUNT; i++)
			{
				if (btns[i] != null)
					btns[i].ExecCode = (CALCEXEC)i;
			}

			for (int i = 0; i <= (int)CALCEXEC.TGLSIGN; i++)
			{
				if (btns[i] != null)
					btns[i].Clicked += MainPage_Clicked;
			}
			for (int i = (int)CALCEXEC.EQUAL; i <= (int)CALCEXEC.MULT; i++)
			{
				if (btns[i] != null)
					btns[i].Clicked += MainPage_Clicked;
			}
			for (int i = (int)CALCEXEC.FPS24; i <= (int)CALCEXEC.FPS30; i++)
			{
				if (btns[i] != null)
					btns[i].Clicked += fps_Clicked;
			}

			m_dt.CalcChanged += M_dt_CalcChanged;



			SetFps(true);
			Disp();
		}
		public void Disp()
		{
			ResultFrame.Text = m_dt.ResultFrameStr;
			ResultValue.Text = m_dt.ResultStr;
			InputFrame.Text = m_dt.InputFrameStr;
			InputValue.Text = m_dt.InputStr;
			CalcMode.Text = m_dt.CalcModeStr;
		}
		private void M_dt_CalcChanged(object sender, EventArgs e)
		{
			Disp();
		}

		private void MainPage_Clicked(object sender, EventArgs e)
		{
			CalcBtn btn = (CalcBtn)sender;
			CALCEXEC ce = btn.ExecCode;
			m_dt.Exec(ce);
		}

		private void fps_Clicked(object sender, EventArgs e)
		{
			CalcBtn btn = (CalcBtn)sender;
			bool is24fps = (btn.ExecCode == CALCEXEC.FPS24);
			if (is24fps == false)
			{
				if (btn.ExecCode != CALCEXEC.FPS30) return;
			}
			SetFps(is24fps);
		}

		private void SetFps(object sender, EventArgs e)
		{
			Button btn = (Button)sender;

			bool is24 = (btn.ClassId == "fps24");
			SetFps(is24);
		}
		public void SetFps(bool is24)
		{

			CalcBtn btn24 = btns[(int)CALCEXEC.FPS24];
			CalcBtn btn30 = btns[(int)CALCEXEC.FPS30];
			if (is24 == true)
			{
				btn24.BackgroundColor = Color.White;
				btn24.TextColor = Color.Black;
				btn30.BackgroundColor = Color.Gray;
				btn30.TextColor = Color.DarkGray;
				btns[(int)CALCEXEC.KEX0].Text = "12";
				btns[(int)CALCEXEC.KEX1].Text = "18";
				btns[(int)CALCEXEC.KEX2].Text = "21";
				m_dt.SetFps24();
			}
			else
			{
				btn24.BackgroundColor = Color.Gray;
				btn24.TextColor = Color.DarkGray;
				btn30.BackgroundColor = Color.White;
				btn30.TextColor = Color.Black;
				btns[(int)CALCEXEC.KEX0].Text = "15";
				btns[(int)CALCEXEC.KEX1].Text = "20";
				btns[(int)CALCEXEC.KEX2].Text = "25";
				m_dt.SetFps30();
			}

		}
	}
}
