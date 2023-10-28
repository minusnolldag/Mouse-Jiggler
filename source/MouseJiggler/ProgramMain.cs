using System;
using System.Windows.Forms;

namespace MouseJiggler {
	public static class ProgramMain {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		public static void Main() {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MouseJigglerForm());
		}
	}
}