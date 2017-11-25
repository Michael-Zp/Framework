﻿using System.Windows.Forms;
using System;
using Zenseless.ShaderDebugging;

namespace ShaderForm.Camera
{
	public partial class FormCamera : Form
	{
		public FormCamera()
		{
			InitializeComponent();
			this.LoadLayout();
			checkBoxOnTop.Checked = TopMost;
		}

		public void Set(AdapterCamera cam)
		{
			propertyGrid1.SelectedObject = cam;
		}

		public void SaveData()
		{
			this.SaveLayout();
		}

		private void BtnReset_Click(object sender, EventArgs e)
		{
			var cam = propertyGrid1.SelectedObject as AdapterCamera;
			if (ReferenceEquals(null,  cam)) return;
			cam.Reset();
			propertyGrid1.SelectedObject = cam;
		}

		private void FormCamera_FormClosing(object sender, FormClosingEventArgs e)
		{
			e.Cancel = true;
			Visible = false;
		}

		private void CheckBoxOnTop_CheckedChanged(object sender, EventArgs e)
		{
			TopMost = checkBoxOnTop.Checked;
		}
	}
}
