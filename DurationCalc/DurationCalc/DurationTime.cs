using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRY
{
	// ************************************************************************
	/// <summary>
	/// 計算モード
	/// </summary>
	public enum CALCMODE
	{
		/// <summary>
		/// 何もしていない
		/// </summary>
		NONE=0,
		/// <summary>
		/// イコール
		/// </summary>
		EQUAL = CALCEXEC.EQUAL,
		PLUS = CALCEXEC.PLUS,
		/// <summary>
		/// 引き算
		/// </summary>
		MINUS = CALCEXEC.MINUS,
		/// <summary>
		/// 割り算
		/// </summary>
		DIV = CALCEXEC.DIV,
		/// <summary>
		/// 掛け算
		/// </summary>
		MULT = CALCEXEC.MULT,

	}
	// ************************************************************************
	/// <summary>
	/// 秒コマを管理するクラス
	/// </summary>
	public class DurationTime
	{
		#region Property
		private double m_Time = 0;
		/// <summary>
		/// 秒数
		/// </summary>
		public double Time
		{
			get { return m_Time * m_IsPlus; }
			set
			{
				m_Time = value;
				if (m_Time<0)
				{
					m_IsPlus = -1;
					m_Time *= -1;
				}
				else
				{
					m_IsPlus = 1;
				}
				FromTime();
			}
		}
		/// <summary>
		/// 負数
		/// </summary>
		private double m_IsPlus = 1;
		/// <summary>
		/// (+/-)を入れ替える
		/// </summary>
		public void ToggleSign() { m_IsPlus *= -1; }
		
		private double m_Fps = 24;
		/// <summary>
		/// フレームレート
		/// </summary>
		public double Fps
		{
			get { return m_Fps; }
			set
			{
				double f = value;
				if (f < 24) f = 24;
				if(m_Fps !=value)
				{
					m_Fps = value;
					FromTime();
				}
			}
		}
		/// <summary>
		/// 秒の文字列
		/// </summary>
		private string m_LineSec = "";
		/// <summary>
		/// コマの文字列
		/// </summary>
		private string m_LineKoma = "";
		
		private CALCMODE m_md = CALCMODE.NONE;
		/// <summary>
		/// 計算モード
		/// </summary>
		public CALCMODE CalcMode
		{
			get { return m_md; }
		}
		public string CalcModeStr
		{
			get
			{
				string ret = "";
				switch(m_md)
				{
					case CALCMODE.DIV:
						ret = "÷";
						break;
					case CALCMODE.MULT: 
						ret = "×"; 
						break;
					case CALCMODE.PLUS:
						ret = "＋";
						break;
					case CALCMODE.MINUS:
						ret = "－";
						break;
				}
				return ret;
			}
		}
		public void SetCalcMode(CALCMODE md) { m_md = md; }

		/// <summary>
		/// 秒数とコマ数の入力フラグ
		/// trueなら秒数がターゲット
		/// </summary>
		private bool m_InputSec = true;

		private string m_SecSepa = "+";
		/// <summary>
		/// 入力された秒数とコマ数の文字列
		/// </summary>
		public string InputStr
		{
			get
			{
				string ret = "";
				if (m_IsPlus < 0)
				{
					ret += "-";
				}
				if (m_LineSec == "")
				{
					ret += "0";
				}
				else
				{
					ret += m_LineSec;
				}
				if ((m_LineKoma != "")||(m_InputSec==false))
				{
					ret += m_SecSepa;
				}
				if ((m_LineKoma == "") && (m_InputSec == false))
				{
					ret += "0";
				}
				else
				{
					ret += m_LineKoma;
				}

				return ret;
			}
		}
		
		/// <summary>
		/// 何も入力されていないならtrue
		/// </summary>
		public bool IsEmpty
		{
			get { return ((m_LineSec == "") && (m_LineKoma == "")); }
		}
		/// <summary>
		/// フレーム数
		/// </summary>
		public string Frame
		{
			get
			{
				return string.Format("{0}", (int)(m_Time * m_Fps *m_IsPlus + 0.5));
			}
		}
		private int[] m_Ex_tbl = new int[3];
		public int[] Ex_tbl
		{
			get { return m_Ex_tbl; }
			set
			{
				for (int i = 0; i < 3; i++) m_Ex_tbl[i] = -1;
				int cnt = value.Length;
				if (cnt <= 0) return;
				else if (cnt > 3) cnt = 3;
				for (int i = 0; i < cnt; i++) m_Ex_tbl[i] = value[i];
			}
		}

		#endregion

		// ***********************************************************************
		public DurationTime()
		{
			Init();
			m_Ex_tbl[0] = 12;
			m_Ex_tbl[1] = 18;
			m_Ex_tbl[2] = 21;
		}
		// ***********************************************************************
		/// <summary>
		/// 初期化。fpsは変更しない
		/// </summary>
		public void Init(CALCMODE Mode =  CALCMODE.NONE)
		{
			m_Time = 0;
			m_IsPlus = 1;
			m_LineSec = "";
			m_LineKoma = "";
			m_InputSec = true;
			m_md = Mode;
		}
		// ***********************************************************************
		/// <summary>
		/// 値を複写する
		/// </summary>
		/// <param name="dt"></param>
		public void CopyFrom(DurationTime dt)
		{
			m_Time = dt.m_Time;
			m_IsPlus = dt.m_IsPlus;
			m_Fps = dt.m_Fps;
			m_LineSec = dt.m_LineSec;
			m_LineKoma = dt.m_LineKoma;
			m_InputSec = dt.m_InputSec;
			FromTime();
		}
		// ***********************************************************************
		/// <summary>
		/// 計算の実態。
		/// m_mdによって計算を変える
		/// </summary>
		/// <param name="dt"></param>
		public void Calc(DurationTime dt)
		{
			if (IsEmpty)
			{
				CopyFrom(dt);
				return;
			}
			double src = m_Time * m_IsPlus;
			double dst = dt.m_Time * dt.m_IsPlus;

			switch (dt.m_md)
			{
				case CALCMODE.PLUS:
					src += dst;
					break;
				case CALCMODE.MINUS:
					src -= dst;
					break;
				case CALCMODE.DIV:
					if (dst != 0)
					{
						src /= dst;
					}
					break;
				case CALCMODE.MULT:
					src *= dst;
					break;
			}
			if (src < 0)
			{
				m_Time = src *-1;
				m_IsPlus = -1;
			}
			else
			{
				m_Time = src;
				m_IsPlus = 1;
			}
			FromTime();
		}
		// ***********************************************************************
		public bool InputNum(CALCEXEC ce)
		{
			bool ret = false;
			if ( (ce>=CALCEXEC.K00)&& (ce <= CALCEXEC.KEX2))
			{
				int  v = -1;
				switch (ce)
				{
					case CALCEXEC.KEX0:
						v = m_Ex_tbl[0];
						break;
					case CALCEXEC.KEX1:
						v = m_Ex_tbl[1];
						break;
					case CALCEXEC.KEX2:
						v = m_Ex_tbl[2];
						break;
					default:
						v = (int)ce;
						break;
				}
				if (v < 0) return ret;
				string c = string.Format("{0}", v);

				if (m_InputSec)
				{
					if (m_LineSec == "0")
					{
						m_LineSec = c;
					}
					else
					{
						m_LineSec += c;
					}
				}
				else
				{
					if ((m_LineKoma == "") && (c == "0"))
					{

					}
					else
					{
						m_LineKoma += c;
					}
				}
				ToTime();
				ret = true;
			}
			return ret;

		}
		// ***********************************************************************
		/// <summary>
		/// 実行するとコマ入力モードへ
		/// </summary>
		/// <returns></returns>
		public bool InputKomaMode()
		{
			bool ret = false;
			if (m_InputSec == true)
			{
				m_InputSec = false;
				ToTime();
				ret = true;
			}
			return ret;
		}
		// ***********************************************************************
		/// <summary>
		/// 入力文字を削除
		/// </summary>
		/// <returns></returns>
		public bool InputBackSpace()
		{
			bool ret = false;
			if ((m_LineSec == "") && (m_LineKoma == ""))
			{
				m_InputSec = true;
				m_IsPlus = 1;
				ret = true;
			}
			else if (m_InputSec == true)
			{
				m_LineSec = m_LineSec.Substring(0, m_LineSec.Length - 1);
				if (m_LineSec == "") m_IsPlus = 1;
				ret = true;
			}
			else
			{
				m_LineKoma = m_LineKoma.Substring(0, m_LineKoma.Length - 1);
				if (m_LineKoma == "")
				{
					m_InputSec = true;
				}
				ret = true;
			}
			ToTime();
			return ret;
		}
		// ***********************************************************************
		/// <summary>
		/// 入力文字からTimeを計算
		/// </summary>
		public void ToTime()
		{
			double ret = 0;
			if (m_LineSec != "") {
				ret += (double)(int.Parse(m_LineSec));
			}
			if(m_LineKoma!="")
			{
				ret += (double)(int.Parse(m_LineKoma)) / m_Fps;
			}
			m_Time = ret;
		}
		// ***********************************************************************
		/// <summary>
		/// Timeから入力文字を計算
		/// </summary>
		public void FromTime()
		{
			m_LineSec = m_LineKoma = "";
			int sec = (int)m_Time;
			int koma = (int) ( (m_Time - (double)sec) * m_Fps + 0.5);
			if (sec > 0)
			{
				m_LineSec = string.Format("{0}", sec);
			}
			if (koma>0)
			{
				m_LineKoma = string.Format("{0}", koma);
			}
		}
	}
}
