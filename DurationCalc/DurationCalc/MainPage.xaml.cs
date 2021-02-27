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

		private Label m_ResultFrame = null;
		private Label m_ResultValue = null;
		private Label m_InputFrame = null;
		private Label m_InputValue = null;
		private Label m_CalcMode = null;

		public MainPage()
		{
			InitializeComponent();

			m_ResultFrame = (Label)FindByName("ResultFrame");
			m_ResultValue = (Label)FindByName("ResultValue");
			m_InputFrame = (Label)FindByName("InputFrame");
			m_InputValue = (Label)FindByName("InputValue");
			m_CalcMode = (Label)FindByName("CalcMode");

			for (int i = 0; i < (int)CALCEXEC.COUNT; i++) btns[i] = null;

			btns[(int)CALCEXEC.K00] = (CalcBtn)FindByName("k00");
			btns[(int)CALCEXEC.K01] = (CalcBtn)FindByName("k01");
			btns[(int)CALCEXEC.K02] = (CalcBtn)FindByName("k02");
			btns[(int)CALCEXEC.K03] = (CalcBtn)FindByName("k03");
			btns[(int)CALCEXEC.K04] = (CalcBtn)FindByName("k04");
			btns[(int)CALCEXEC.K05] = (CalcBtn)FindByName("k05");
			btns[(int)CALCEXEC.K06] = (CalcBtn)FindByName("k06");
			btns[(int)CALCEXEC.K07] = (CalcBtn)FindByName("k07");
			btns[(int)CALCEXEC.K08] = (CalcBtn)FindByName("k08");
			btns[(int)CALCEXEC.K09] = (CalcBtn)FindByName("k09");
			btns[(int)CALCEXEC.KEX0] = (CalcBtn)FindByName("kex0");
			btns[(int)CALCEXEC.KEX1] = (CalcBtn)FindByName("kex1");
			btns[(int)CALCEXEC.KEX2] = (CalcBtn)FindByName("kex2");

			btns[(int)CALCEXEC.SEC] = (CalcBtn)FindByName("sec");
			btns[(int)CALCEXEC.BS] = (CalcBtn)FindByName("bs");
			btns[(int)CALCEXEC.UNDO] = (CalcBtn)FindByName("undo");
			btns[(int)CALCEXEC.TGLSIGN] = (CalcBtn)FindByName("togglesign");
			btns[(int)CALCEXEC.FPS24] = (CalcBtn)FindByName("fps24");
			btns[(int)CALCEXEC.FPS30] = (CalcBtn)FindByName("fps30");
			btns[(int)CALCEXEC.EQUAL] = (CalcBtn)FindByName("eqaul");
			btns[(int)CALCEXEC.C] = (CalcBtn)FindByName("c");
			btns[(int)CALCEXEC.AC] = (CalcBtn)FindByName("ac");
			btns[(int)CALCEXEC.PLUS] = (CalcBtn)FindByName("plus");
			btns[(int)CALCEXEC.MINUS] = (CalcBtn)FindByName("minus");
			btns[(int)CALCEXEC.DIV] = (CalcBtn)FindByName("div");
			btns[(int)CALCEXEC.MULT] = (CalcBtn)FindByName("mult");


			for (int i = 0; i < (int)CALCEXEC.COUNT; i++)
			{
				if (btns[i] != null)
					btns[i].exec = (CALCEXEC)i;
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
			m_ResultFrame.Text = m_dt.ResultFrameStr;
			m_ResultValue.Text = m_dt.ResultStr;
			m_InputFrame.Text = m_dt.InputFrameStr;
			m_InputValue.Text = m_dt.InputStr;
			m_CalcMode.Text = m_dt.CalcModeStr;
		}
		private void M_dt_CalcChanged(object sender, EventArgs e)
		{
			Disp();
		}

		private void MainPage_Clicked(object sender, EventArgs e)
		{
			CalcBtn btn = (CalcBtn)sender;
			CALCEXEC ce = btn.exec;
			m_dt.Exec(ce);
		}

		private void fps_Clicked(object sender, EventArgs e)
		{
			CalcBtn btn = (CalcBtn)sender;
			bool is24fps = (btn.exec == CALCEXEC.FPS24);
			if (is24fps == false)
			{
				if (btn.exec != CALCEXEC.FPS30) return;
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
