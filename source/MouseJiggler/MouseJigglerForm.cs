using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows.Forms;

namespace MouseJiggler {
	public class MouseJigglerForm : Form {
		private const int WM_KEYPRESS_HOTKEY_CTRL_SHIFT_ALT_M_ID = 1;
		private const int WM_SYSTEM_MENU_ABOUT_ID = 2;
		private const int WM_HOTKEY_MSG_ID = 0x0312;
		private const int WM_SYSCOMMAND = 0x112;
		private const int MF_STRING = 0x0;
		private const int MF_SEPARATOR = 0x800;

		[DllImport("user32.dll")]
		private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

		[DllImport("user32.dll")]
		private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool AppendMenu(IntPtr hMenu, int uFlags, int uIDNewItem, string lpNewItem);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool InsertMenu(IntPtr hMenu, int uPosition, int uFlags, int uIDNewItem, string lpNewItem);

		private readonly string startMouseJigglerText = "Start mouse jiggler";
		private readonly string stopMouseJigglerText = "Stop mouse jiggler";
		private System.Timers.Timer mouseMovedTimer = new System.Timers.Timer(100);
		private bool started = false;
		private Point currentMousePosition;
		private int maxIdleTimeBeforeMouseJiggleInMilliseconds = 0;
		private double currentIdleTimeInMilliseconds = 0;
		private Random random = new Random();

		private Button startStopMouseJigglerButton;
		private TextBox whenUserIsIdleMoveMouseEveryXSecondsTextBox;
		private Label whenUserIsIdleMoveMouseEveryXSecondsLabel;
		private TextBox userHasBeenIdleForXSecondsTextBox;
		private Label userHasBeenIdleForXSecondsLabel;
		private Label startStopMouseJigglerKeyCombinationLabel;
		private Button ctrlButton;
		private Button shiftButton;
		private Button altButton;
		private Button mButton;
		private Label plusLabel1;
		private Label plusLabel2;
		private Label plusLabel3;
		private IContainer components = null;

		public MouseJigglerForm() {
			currentMousePosition = Cursor.Position;
			mouseMovedTimer.Elapsed += MouseMovedTimer_Elapsed;
			mouseMovedTimer.AutoReset = true;
			mouseMovedTimer.Enabled = true;
			RegisterHotKey(this.Handle, WM_KEYPRESS_HOTKEY_CTRL_SHIFT_ALT_M_ID, 7, (int)'M');
			InitializeComponent();
		}

		private void InitializeComponent() {
			ComponentResourceManager resources = new ComponentResourceManager(typeof(MouseJigglerForm));
			this.startStopMouseJigglerButton = new Button();
			this.whenUserIsIdleMoveMouseEveryXSecondsTextBox = new TextBox();
			this.whenUserIsIdleMoveMouseEveryXSecondsLabel = new Label();
			this.userHasBeenIdleForXSecondsTextBox = new TextBox();
			this.userHasBeenIdleForXSecondsLabel = new Label();
			this.startStopMouseJigglerKeyCombinationLabel = new Label();
			this.ctrlButton = new Button();
			this.shiftButton = new Button();
			this.altButton = new Button();
			this.mButton = new Button();
			this.plusLabel1 = new Label();
			this.plusLabel2 = new Label();
			this.plusLabel3 = new Label();
			this.SuspendLayout();
			// 
			// startStopMouseJigglerButton
			// 
			this.startStopMouseJigglerButton.Location = new Point(12, 12);
			this.startStopMouseJigglerButton.Name = "startStopMouseJigglerButton";
			this.startStopMouseJigglerButton.Size = new Size(236, 33);
			this.startStopMouseJigglerButton.TabIndex = 0;
			this.startStopMouseJigglerButton.Text = "Start mouse jiggler";
			this.startStopMouseJigglerButton.UseVisualStyleBackColor = true;
			this.startStopMouseJigglerButton.Click += new EventHandler(this.StartStopMouseJigglerButton_Click);
			// 
			// whenUserIsIdleMoveMouseEveryXSecondsTextBox
			// 
			this.whenUserIsIdleMoveMouseEveryXSecondsTextBox.Location = new Point(15, 76);
			this.whenUserIsIdleMoveMouseEveryXSecondsTextBox.Name = "whenUserIsIdleMoveMouseEveryXSecondsTextBox";
			this.whenUserIsIdleMoveMouseEveryXSecondsTextBox.Size = new Size(80, 20);
			this.whenUserIsIdleMoveMouseEveryXSecondsTextBox.TabIndex = 1;
			this.whenUserIsIdleMoveMouseEveryXSecondsTextBox.Text = "300";
			// 
			// whenUserIsIdleMoveMouseEveryXSecondsLabel
			// 
			this.whenUserIsIdleMoveMouseEveryXSecondsLabel.AutoSize = true;
			this.whenUserIsIdleMoveMouseEveryXSecondsLabel.Location = new Point(12, 60);
			this.whenUserIsIdleMoveMouseEveryXSecondsLabel.Name = "whenUserIsIdleMoveMouseEveryXSecondsLabel";
			this.whenUserIsIdleMoveMouseEveryXSecondsLabel.Size = new Size(236, 13);
			this.whenUserIsIdleMoveMouseEveryXSecondsLabel.TabIndex = 2;
			this.whenUserIsIdleMoveMouseEveryXSecondsLabel.Text = "When user is idle move mouse every X seconds:";
			// 
			// userHasBeenIdleForXSecondsTextBox
			// 
			this.userHasBeenIdleForXSecondsTextBox.Location = new Point(15, 128);
			this.userHasBeenIdleForXSecondsTextBox.Name = "userHasBeenIdleForXSecondsTextBox";
			this.userHasBeenIdleForXSecondsTextBox.ReadOnly = true;
			this.userHasBeenIdleForXSecondsTextBox.Size = new Size(100, 20);
			this.userHasBeenIdleForXSecondsTextBox.TabIndex = 3;
			this.userHasBeenIdleForXSecondsTextBox.Text = "0";
			// 
			// userHasBeenIdleForXSecondsLabel
			// 
			this.userHasBeenIdleForXSecondsLabel.AutoSize = true;
			this.userHasBeenIdleForXSecondsLabel.Location = new Point(12, 112);
			this.userHasBeenIdleForXSecondsLabel.Name = "userHasBeenIdleForXSecondsLabel";
			this.userHasBeenIdleForXSecondsLabel.Size = new Size(166, 13);
			this.userHasBeenIdleForXSecondsLabel.TabIndex = 4;
			this.userHasBeenIdleForXSecondsLabel.Text = "User has been idle for X seconds:";
			// 
			// startStopMouseJigglerKeyCombinationLabel
			// 
			this.startStopMouseJigglerKeyCombinationLabel.AutoSize = true;
			this.startStopMouseJigglerKeyCombinationLabel.Location = new Point(12, 161);
			this.startStopMouseJigglerKeyCombinationLabel.Name = "startStopMouseJigglerKeyCombinationLabel";
			this.startStopMouseJigglerKeyCombinationLabel.Size = new Size(201, 13);
			this.startStopMouseJigglerKeyCombinationLabel.TabIndex = 12;
			this.startStopMouseJigglerKeyCombinationLabel.Text = "Start/stop mouse jiggler key combination:";
			// 
			// ctrlButton
			// 
			this.ctrlButton.Location = new Point(15, 177);
			this.ctrlButton.Name = "ctrlButton";
			this.ctrlButton.Size = new Size(47, 23);
			this.ctrlButton.TabIndex = 5;
			this.ctrlButton.Text = "CTRL";
			this.ctrlButton.UseVisualStyleBackColor = true;
			// 
			// shiftButton
			// 
			this.shiftButton.Location = new Point(87, 177);
			this.shiftButton.Name = "shiftButton";
			this.shiftButton.Size = new Size(47, 23);
			this.shiftButton.TabIndex = 6;
			this.shiftButton.Text = "SHIFT";
			this.shiftButton.UseVisualStyleBackColor = true;
			// 
			// altButton
			// 
			this.altButton.Location = new Point(159, 177);
			this.altButton.Name = "altButton";
			this.altButton.Size = new Size(38, 23);
			this.altButton.TabIndex = 7;
			this.altButton.Text = "ALT";
			this.altButton.UseVisualStyleBackColor = true;
			// 
			// mButton
			// 
			this.mButton.Location = new Point(222, 177);
			this.mButton.Name = "mButton";
			this.mButton.Size = new Size(22, 23);
			this.mButton.TabIndex = 8;
			this.mButton.Text = "M";
			this.mButton.UseVisualStyleBackColor = true;
			// 
			// plusLabel1
			// 
			this.plusLabel1.AutoSize = true;
			this.plusLabel1.Location = new Point(68, 182);
			this.plusLabel1.Name = "plusLabel1";
			this.plusLabel1.Size = new Size(13, 13);
			this.plusLabel1.TabIndex = 9;
			this.plusLabel1.Text = "+";
			// 
			// plusLabel2
			// 
			this.plusLabel2.AutoSize = true;
			this.plusLabel2.Location = new Point(140, 182);
			this.plusLabel2.Name = "plusLabel2";
			this.plusLabel2.Size = new Size(13, 13);
			this.plusLabel2.TabIndex = 10;
			this.plusLabel2.Text = "+";
			// 
			// plusLabel3
			// 
			this.plusLabel3.AutoSize = true;
			this.plusLabel3.Location = new Point(203, 182);
			this.plusLabel3.Name = "plusLabel3";
			this.plusLabel3.Size = new Size(13, 13);
			this.plusLabel3.TabIndex = 11;
			this.plusLabel3.Text = "+";
			// 
			// mouseJigglerForm
			// 
			this.AutoScaleDimensions = new SizeF(6F, 13F);
			this.AutoScaleMode = AutoScaleMode.Font;
			this.ClientSize = new Size(259, 212);
			this.Controls.Add(this.startStopMouseJigglerKeyCombinationLabel);
			this.Controls.Add(this.plusLabel3);
			this.Controls.Add(this.plusLabel2);
			this.Controls.Add(this.plusLabel1);
			this.Controls.Add(this.mButton);
			this.Controls.Add(this.altButton);
			this.Controls.Add(this.shiftButton);
			this.Controls.Add(this.ctrlButton);
			this.Controls.Add(this.userHasBeenIdleForXSecondsLabel);
			this.Controls.Add(this.userHasBeenIdleForXSecondsTextBox);
			this.Controls.Add(this.whenUserIsIdleMoveMouseEveryXSecondsLabel);
			this.Controls.Add(this.whenUserIsIdleMoveMouseEveryXSecondsTextBox);
			this.Controls.Add(this.startStopMouseJigglerButton);
			this.FormBorderStyle = FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "mouseJigglerForm";
			this.Text = "Mouse Jiggler";
			this.FormClosing += new FormClosingEventHandler(this.Form_FormClosing);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		protected override void Dispose(bool d) {
			if (d && (components != null)) {
				components.Dispose();
			}

			base.Dispose(d);
		}

		private void MouseMovedTimer_Elapsed(object s, ElapsedEventArgs e) {
			if (currentMousePosition.X != Cursor.Position.X && currentMousePosition.Y != Cursor.Position.Y) {
				currentMousePosition = Cursor.Position;
				currentIdleTimeInMilliseconds = 0;
			} else {
				currentIdleTimeInMilliseconds += mouseMovedTimer.Interval;

				if (started) {
					if (currentIdleTimeInMilliseconds >= maxIdleTimeBeforeMouseJiggleInMilliseconds) {
						currentIdleTimeInMilliseconds = 0;
						Cursor.Position = new Point(random.Next(SystemInformation.VirtualScreen.Width), random.Next(SystemInformation.VirtualScreen.Height));
						SendKeys.Send("{F15}");
					}
				}
			}

			InvokeUserHasBeenIdleForXSecondsTextBoxText(Math.Floor(currentIdleTimeInMilliseconds / 1000).ToString());
		}

		private void StartStopMouseJigglerButton_Click(object s, EventArgs e) {
			if (!started) {
				if (int.TryParse(whenUserIsIdleMoveMouseEveryXSecondsTextBox.Text, out int interval) && interval > 0) {
					startStopMouseJigglerButton.Text = stopMouseJigglerText;
					currentIdleTimeInMilliseconds = 0;
					maxIdleTimeBeforeMouseJiggleInMilliseconds = interval * 1000;
					started = true;
				} else {
					MessageBox.Show("Only use whole numbers which are also greater than 0", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			} else {
				startStopMouseJigglerButton.Text = startMouseJigglerText;
				started = false;
			}
		}

		private void Form_FormClosing(object s, FormClosingEventArgs e) {
			UnregisterHotKey(this.Handle, WM_KEYPRESS_HOTKEY_CTRL_SHIFT_ALT_M_ID);
		}

		private void InvokeUserHasBeenIdleForXSecondsTextBoxText(string t) {
			if (userHasBeenIdleForXSecondsTextBox.InvokeRequired) {
				userHasBeenIdleForXSecondsTextBox.Invoke((MethodInvoker)(() => {
					InvokeUserHasBeenIdleForXSecondsTextBoxText(t);
				}));
			} else {
				userHasBeenIdleForXSecondsTextBox.Text = t;
			}
		}

		protected override void OnHandleCreated(EventArgs e) {
			IntPtr systemMenu = GetSystemMenu(this.Handle, false);

			AppendMenu(systemMenu, MF_SEPARATOR, 0, string.Empty);
			AppendMenu(systemMenu, MF_STRING, WM_SYSTEM_MENU_ABOUT_ID, "About...");

			base.OnHandleCreated(e);
		}

		protected override void WndProc(ref Message m) {
			if (m.Msg == WM_HOTKEY_MSG_ID && m.WParam.ToInt32() == WM_KEYPRESS_HOTKEY_CTRL_SHIFT_ALT_M_ID) {
				StartStopMouseJigglerButton_Click(null, null);
			}

			if ((m.Msg == WM_SYSCOMMAND) && m.WParam.ToInt32() == WM_SYSTEM_MENU_ABOUT_ID) {
				MessageBox.Show("Programmed by @MinusNolldag - https://github.com/minusnolldag", "About");
			}

			base.WndProc(ref m);
		}
	}
}