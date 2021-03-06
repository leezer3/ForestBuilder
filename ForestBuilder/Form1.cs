﻿//Copyright: ManagedCode
//2014. 10.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace ForestBuilder
{
    public partial class Form1 : Form
    {
        LanguageManager languageManager;
        public Form1()
        {
            InitializeComponent();
            dataGridView1.Rows.Clear();
            languageManager = new LanguageManager();
            ApplyLanguage();
        }
        private void ApplyLanguage()
        {
	        try
	        {
		        labelVersion.Text = "ForestBuilder: v" + Assembly.GetEntryAssembly().GetName().Version;
		        this.Text = languageManager.CurrentLanguage.GetTranslation("AppName");
		        label1.Text = languageManager.CurrentLanguage.GetTranslation("STARTING_POINT");
		        label2.Text = languageManager.CurrentLanguage.GetTranslation("END_POINT");
		        label3.Text = languageManager.CurrentLanguage.GetTranslation("FREQUENCY");
		        dataGridView1.Columns["areaName"].HeaderText = languageManager.CurrentLanguage.GetTranslation("AREA_NAME");
		        dataGridView1.Columns["Distance"].HeaderText = languageManager.CurrentLanguage.GetTranslation("DISTANCE");
		        dataGridView1.Columns["XOffset"].HeaderText = languageManager.CurrentLanguage.GetTranslation("X_OFFSET");
		        dataGridView1.Columns["ZOffset"].HeaderText = languageManager.CurrentLanguage.GetTranslation("Z_OFFSET");
		        dataGridView1.Columns["Indices"].HeaderText = languageManager.CurrentLanguage.GetTranslation("INDICES");
		        dataGridView1.Columns["Height"].HeaderText = languageManager.CurrentLanguage.GetTranslation("HEIGHT");
		        dataGridView1.Columns["YRotationMin"].HeaderText = languageManager.CurrentLanguage.GetTranslation("Y_ROTATION_MIN");
		        dataGridView1.Columns["YRotationMax"].HeaderText = languageManager.CurrentLanguage.GetTranslation("Y_ROTATION_MAX");
		        dataGridView1.Columns["Frequency"].HeaderText = languageManager.CurrentLanguage.GetTranslation("FREQUENCY");
		        button1.Text = languageManager.CurrentLanguage.GetTranslation("START");
		        button2.Text = languageManager.CurrentLanguage.GetTranslation("ADD_LAYER");
		        button3.Text = languageManager.CurrentLanguage.GetTranslation("DELETE_LAYER");
		        button4.Text = languageManager.CurrentLanguage.GetTranslation("SAVE_PROJECT_AS_A_FILE");
		        button5.Text = languageManager.CurrentLanguage.GetTranslation("OPEN_PROJECT");
	        }
	        catch
	        {
		        MessageBox.Show("Error applying languange. \n Please check that language.txt is present and un-altered.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
	        }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Add();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in this.dataGridView1.SelectedRows)
            {
                dataGridView1.Rows.RemoveAt(item.Index);
            }
        }
        /// <summary>
        /// Opens a save file dialog and saves the include file with the currently set options to the dialog's specified path.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            Generator generator;
            if (!GetGenerator(out generator))
                return;
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Include files|*.include|All files|*.*";
            dialog.DefaultExt = "include";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                generator.Generate(dialog.FileName);
            }
        }
        private bool GetGenerator(out Generator generator)
        {
            generator = new Generator();
            System.Globalization.NumberStyles floatStyle = System.Globalization.NumberStyles.Float;
            System.Globalization.CultureInfo invariantCulture = System.Globalization.CultureInfo.InvariantCulture;
            try
            {
                generator.StartingPoint = float.Parse(textBox1.Text, floatStyle, invariantCulture);
                generator.EndPoint = float.Parse(textBox2.Text, floatStyle, invariantCulture);
	            float f;
	            generator.Frequency = float.TryParse(textBox3.Text, floatStyle, invariantCulture, out f) ? f : 0;
                generator.Areas = new List<Generator.Area>();
                foreach (DataGridViewRow c in dataGridView1.Rows)
                {
					Generator.Area area = new Generator.Area();
					//Temp variable
					float pF2;
					if (float.TryParse((string)c.Cells["Frequency"].Value, floatStyle, invariantCulture, out pF2))
					{
						//If our column contains a float, use it
						//Otherwise use the original number
						if (pF2 != 0)
						{
							area.Frequency = pF2;
						}
						else
						{
							//A frequency of 0 will result in an infinite loop.....
							if (generator.Frequency == 0)
							{
								throw new InvalidDataException("One of the main frequency and the area frequency must not be zero.");
							}
							area.Frequency = generator.Frequency;
						}
					}
					else
					{
						area.Frequency = generator.Frequency;
					}
	                area.Name = (string)c.Cells["areaName"].Value;
					area.XOffset = float.Parse((string)c.Cells["XOffset"].Value, floatStyle, invariantCulture);
                    area.ZOffset = float.Parse((string)c.Cells["ZOffset"].Value, floatStyle, invariantCulture);
                    area.Distance = float.Parse((string)c.Cells["Distance"].Value, floatStyle, invariantCulture);
                    area.Height = float.Parse((string)c.Cells["Height"].Value, floatStyle, invariantCulture);
                    area.Indices = FromString((string)c.Cells["Indices"].Value);
                    area.YRotationMin = float.Parse((string)c.Cells["YRotationMin"].Value, floatStyle, invariantCulture);
                    area.YRotationMax = float.Parse((string)c.Cells["YRotationMax"].Value, floatStyle, invariantCulture);
                    generator.Areas.Add(area);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(languageManager.CurrentLanguage.GetTranslation("MALFORMED_VALUE_MSG"), languageManager.CurrentLanguage.GetTranslation("MALFORMED_VALUE"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        private void SetGenerator(Generator generator)
        {
            System.Globalization.CultureInfo invariantCulture = System.Globalization.CultureInfo.InvariantCulture;
            textBox1.Text = generator.StartingPoint.ToString(invariantCulture);
            textBox2.Text = generator.EndPoint.ToString(invariantCulture);
            textBox3.Text = generator.Frequency.ToString(invariantCulture);
            dataGridView1.Rows.Clear();
            foreach(Generator.Area a in generator.Areas)
            {
	            string n = String.Empty;
	            if (a.Name != null)
	            {
		            n = a.Name;
	            }
                dataGridView1.Rows.Add(n, a.Distance.ToString(invariantCulture), a.XOffset.ToString(invariantCulture),
                    a.ZOffset.ToString(invariantCulture), string.Join("; ", a.Indices), a.Height.ToString(invariantCulture),
                    a.YRotationMin.ToString(invariantCulture), a.YRotationMax.ToString(invariantCulture), a.Frequency.ToString(invariantCulture));
            }
        }
        private int[] FromString(string s)
        {
            List<int> result = new List<int>();
            string[] arguments = s.Split(';');
            foreach (string str in arguments)
            {
                int i;
                if (int.TryParse(str, out i))
                    result.Add(i);
                else
                {
                    int i1 = int.Parse(str.Substring(0, str.IndexOf("..")));
                    int i2 = int.Parse(str.Substring(str.IndexOf("..") + 2, str.Length - str.IndexOf("..") - 2));
                    for (int k = i1; k <= i2; k++)
                        result.Add(k);
                }
            }
            return result.ToArray();
        }
        /// <summary>
        /// Saves the currently set options as a file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            Generator generator;
            if (!GetGenerator(out generator))
                return;
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.FileName = "default";
            dialog.Filter = "INI file (*.ini)|*.ini|All files|*.*";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                IniWriter.Write(dialog.FileName, generator);
            }
        }
        /// <summary>
        /// Opens an ini file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            Generator generator = new Generator();
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "INI file (*.ini)|*.ini|All files|*.*";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                IniReader.Read(dialog.FileName, out generator);
                SetGenerator(generator);
            }
        }
    }
}
