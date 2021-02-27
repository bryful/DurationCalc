using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BRY
{
	public  class CalcBtn : Xamarin.Forms.Button
	{
		public CALCEXEC m_exec = CALCEXEC.NONE;
		public CALCEXEC exec
		{
			get
			{
				return m_exec;
			}
			set
			{
				m_exec = value;
			}
		}
	}
}
