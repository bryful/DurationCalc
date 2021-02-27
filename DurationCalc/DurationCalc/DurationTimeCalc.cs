using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRY
{
	public enum CALCEXEC
	{
		NONE = -1,
		K00,
		K01,
		K02,
		K03,
		K04,
		K05,
		K06,
		K07,
		K08,
		K09,
		KEX0,
		KEX1,
		KEX2,
		SEC,
		BS,
		UNDO,
		TGLSIGN,
		FPS24,
		FPS30,
		C,
		AC,
		EQUAL,
		PLUS,
		MINUS,
		DIV,
		MULT,
		COUNT
	}

	public class DurationTimeCalc
	{
		// ***********************************************************************
		#region Event
		public event EventHandler CalcChanged;
		protected virtual void OnCalcChanged(EventArgs e)
		{
			if (CalcChanged != null)
			{
				CalcChanged(this, e);
			}
		}

		public event EventHandler InputChanged;
		protected virtual void OnInputChanged(EventArgs e)
		{
			if (InputChanged != null)
			{
				InputChanged(this, e);
			}
		}
		#endregion
		// ***********************************************************************
		#region Undo
		private List<double> m_undoBuf = new List<double>();
		#endregion

		// ***********************************************************************

		private DurationTime m_Result = new DurationTime();
		private DurationTime m_Input = new DurationTime();

		// ***********************************************************************
		public string InputStr
		{
			get { return m_Input.InputStr; }
		}
		public string InputFrameStr
		{
			get { return m_Input.Frame; }
		}
		public string ResultStr
		{
			get { return m_Result.InputStr; }
		}
		public string ResultFrameStr
		{
			get { return m_Result.Frame; }
		}
		public string CalcModeStr
		{
			get { return m_Input.CalcModeStr; }
		}
		// ***********************************************************************
		public DurationTimeCalc()
		{
			m_Result.Init();
			m_Input.Init();
		}
		// ***********************************************************************
		public void AllClear()
		{
			if (m_Result.IsEmpty == false) PushUndoBuf();
			m_Result.Init();
			m_Input.Init();
			OnCalcChanged(new EventArgs());
		}
		// ***********************************************************************
		public void Clear()
		{
			m_Input.Init();
			OnCalcChanged(new EventArgs());
		}
		// ***********************************************************************
		public void PushUndoBuf()
		{
			if (m_Result.IsEmpty == false)
			{
				m_undoBuf.Add(m_Result.Time);
				if(m_undoBuf.Count>=100)
				{
					m_undoBuf.RemoveAt(0);
				}
			}

		}
		
		// ***********************************************************************
		public void PopUndoBuf()
		{
			int cnt = m_undoBuf.Count;
			if (cnt>0)
			{
				double tm = m_undoBuf[cnt - 1];
				m_undoBuf.RemoveAt(cnt - 1);
				m_Result.Init();
				m_Input.Init();
				m_Result.Time = tm;
				OnCalcChanged(new EventArgs());
			}
		}
		// ***********************************************************************
		public void InputNumber(CALCEXEC ce)
		{
			bool ret = m_Input.InputNum(ce);
			if(ret)
			{
				if (m_Input.CalcMode == CALCMODE.NONE)
				{
					if (m_Result.IsEmpty == false) PushUndoBuf();
					m_Result.Init();
				}
				OnCalcChanged(new EventArgs());
			}
		}
		// ***********************************************************************
		public void ToggleSign()
		{
			m_Input.ToggleSign();
			OnCalcChanged(new EventArgs());
		}
		// ***********************************************************************
		public void InputEqaul()
		{
			bool ret = false;
			switch (m_Input.CalcMode)
			{
				case CALCMODE.PLUS:
				case CALCMODE.MINUS:
				case CALCMODE.DIV:
				case CALCMODE.MULT:
					PushUndoBuf();
					m_Result.Calc(m_Input);
					m_Input.Init();
					ret = true;
					break;
				default:
					if ((m_Input.IsEmpty == false))
					{
						m_Result.CopyFrom(m_Input);
						m_Input.Init();
						ret = true;
					}
					break;
			}
			if(ret)
			{
				OnCalcChanged(new EventArgs());
			}
		}
		
		// ***********************************************************************
		public void Exec(CALCEXEC ce)
		{
			if (ce == CALCEXEC.NONE) return;
			if ((ce >= CALCEXEC.K00) && (ce <= CALCEXEC.KEX2))
			{
				InputNumber(ce);
			}
			else
			{
				bool ret = false;
				switch (ce)
				{
					case CALCEXEC.SEC:
						ret = m_Input.InputKomaMode();
						break;
					case CALCEXEC.BS:
						ret = m_Input.InputBackSpace();
						break;
					case CALCEXEC.C:
						Clear();
						break;
					case CALCEXEC.AC:
						AllClear();
						break;
					case CALCEXEC.UNDO:
						PopUndoBuf();
						break;
					case CALCEXEC.TGLSIGN:
						ToggleSign();
						break;
					case CALCEXEC.FPS24:
						SetFps24();
						break;
					case CALCEXEC.FPS30:
						SetFps30();
						break;
					case CALCEXEC.EQUAL:
						InputEqaul();
						break;
					case CALCEXEC.PLUS:
					case CALCEXEC.MINUS:
					case CALCEXEC.DIV:
					case CALCEXEC.MULT:
						if (m_Input.IsEmpty == false)
						{
							if (m_Input.CalcMode == CALCMODE.NONE)
							{
								PushUndoBuf();
								m_Result.CopyFrom(m_Input);
								m_Input.Init();

							}
							else
							{
								PushUndoBuf();
								m_Result.Calc(m_Input);
								m_Input.Init();
							}
						}
						m_Input.SetCalcMode((CALCMODE)ce);
						ret = true;
						break;
				}
				if (ret == true)
				{
					OnCalcChanged(new EventArgs());
				}
			}
		}
		// ***********************************************************************

		public double Fps
		{
			get { return m_Input.Fps; }
			set
			{
				SetFps(value);

			}

		}
		// ***********************************************************************
		public void SetFps24()
		{
			SetFps(24);
		}
		// ***********************************************************************
		public void SetFps30()
		{
			SetFps(30);
		}
		// ***********************************************************************
		public void SetFps(double fps)
		{
			int [] ex24 = new int[3] { 12, 18, 21 };
			int [] ex30 = new int[3] { 15, 20, 25 };
			if (m_Input.Fps != fps)
			{
				m_Input.Fps = fps;
				m_Result.Fps = fps;
				if(fps==24)
				{
					m_Input.Ex_tbl = ex24;
				}else if(fps==30)
				{
					m_Input.Ex_tbl = ex30;
				}
				OnCalcChanged(new EventArgs());
			}
		}
	}
}
