﻿using System;
using System.Windows.Forms;

namespace HarmonyAccessViolationPoC
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Button1_Click");
        }
    }
}
