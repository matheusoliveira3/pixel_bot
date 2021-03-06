﻿using MiniBot.Core;
using MiniBot.Domain;
using MiniBot.Infra.CrossCutting;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using Util;
using t = System.Timers;

namespace MiniBot
{
    public partial class FormPrincipal : Form
    {
        private t.Timer timerMana;
        private t.Timer timerHealth;
        private t.Timer timerAntiLogout;

        private Thread threadFindBars;
        private ConfigurationModel configurationModel;

        private IMana Mana { get; set; }

        public FormPrincipal(IMana mana)
        {
            Mana = mana;

            InitializeComponent();

            TibiaClient.FindTibiaClientProcess();

            InitializeTimers();

            StartTimers();

            LoadConfiguration();
        }

        private void TimerMana_Elapsed(object sender, t.ElapsedEventArgs e)
        {
            try
            {
                Invoke(new Action(() => { Mana.UsePotionOrWait(); }));
            }
            catch
            {
            }
            
        }      
        
        private void TimerHealth_Elapsed(object sender, t.ElapsedEventArgs e)
        {
            try
            {
                Invoke(new Action(() => { Health.UsePotionOrWait(); }));
            }
            catch
            {
            }   
        }

        private void TimerAntiLogout_Elapsed(object sender, t.ElapsedEventArgs e)
        {
            try
            {
                Invoke(new Action(() => { new KeyBoardSimulator().MoveCharWithControlArrow(); }));
            }
            catch
            {
            }
        }

        private void InitializeTimers()
        {
            timerMana = new t.Timer();
            timerMana.Interval = 500;
            timerMana.Elapsed += TimerMana_Elapsed;

            timerHealth = new t.Timer();
            timerHealth.Interval = 333;
            timerHealth.Elapsed += TimerHealth_Elapsed;

            timerAntiLogout = new t.Timer();
            timerAntiLogout.Interval = TimeSpan.FromMinutes(10).TotalMilliseconds;
            timerAntiLogout.Elapsed += TimerAntiLogout_Elapsed;
        }

        private void StartTimers()
        {
            timerMana.Start();
            timerHealth.Start();
        }

        private void LoadConfiguration()
        {
            configurationModel = Configuration.Load(this);

            if (!string.IsNullOrWhiteSpace(configurationModel.Health.HotKey))
                cbLifeHotkey.SelectedItem = configurationModel.Health.HotKey;

            if (!string.IsNullOrWhiteSpace(configurationModel.Mana.HotKey))
                cbManaHotKey.SelectedItem = configurationModel.Mana.HotKey;

            nupLifePercent.Value = configurationModel.Health.UseAtPercent;
            nupManaPercent.Value = configurationModel.Mana.UseAtPercent;

            cbLifeActive.Checked = configurationModel.Health.Active;
            cbManaActive.Checked = configurationModel.Mana.Active;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnSaveConfig_Click(object sender, EventArgs e)
        {
            configurationModel.Health.HotKey = cbLifeHotkey.SelectedItem.ToString();
            configurationModel.Health.UseAtPercent = (short)nupLifePercent.Value;
            configurationModel.Health.Active = cbLifeActive.Checked;

            configurationModel.Mana.HotKey = cbManaHotKey.SelectedItem.ToString();
            configurationModel.Mana.UseAtPercent = (short)nupManaPercent.Value;
            configurationModel.Mana.Active = cbManaActive.Checked;

            Configuration.Save();
        }

        private void btnFindBars_Click(object sender, EventArgs e)
        {
            threadFindBars = new Thread(new ThreadStart(FindHealthAndManaBar));
            threadFindBars.Start();
        }

        private void FindHealthAndManaBar()
        {
            Configuration.FindHealthAndManaBar();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            

            timerAntiLogout.Start();
        }

        private void cbAntiLogout_CheckedChanged(object sender, EventArgs e)
        {
            timerAntiLogout.Stop();

            if (cbAntiLogout.Checked)
                timerAntiLogout.Start();
        }
    }
}
