﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace BizHawk.MultiClient
{
    public partial class PathConfig : Form
    {
        //TODO:
        //Rom loading should check for always use Recent for ROMs
        //  Make a path manager function to handle this
        //Make all base path text boxes not allow  %recent%
        //All path text boxes should do some kind of error checking
        //TODO config path under base, config will default to %exe%
        //Think of other modifiers (perhaps all environment paths?)
        //If enough modifiers, path boxes can do a pull down of suggestions when user types %

        //******************
        //Modifiers
        //%exe% - path of EXE
        //%recent% - most recent directory (windows environment path)
        //******************

        //******************
        //Relative path logic
        // . will always be relative to to a platform base
        //    unless it is a tools path or a platform base in which case it is relative to base
        //    base is always relative to exe
        //******************

        public PathConfig()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void PathConfig_Load(object sender, EventArgs e)
        {
            BasePathBox.Text = Global.Config.BasePath;
            
            NESBaseBox.Text = Global.Config.BaseNES;
            NESROMsBox.Text = Global.Config.PathNESROMs;
            NESSavestatesBox.Text = Global.Config.PathNESSavestates;
            NESSaveRAMBox.Text = Global.Config.PathNESSaveRAM;
            NESScreenshotsBox.Text = Global.Config.PathNESScreenshots;
            NESCheatsBox.Text = Global.Config.PathNESCheats;
            
            Sega8BaseBox.Text = Global.Config.BaseSMS;
            Sega8ROMsBox.Text = Global.Config.PathSMSROMs;
            Sega8SavestatesBox.Text = Global.Config.PathSMSSavestates;
            Sega8SaveRAMDescription.Text = Global.Config.PathSMSSaveRAM;
            Sega8ScreenshotsBox.Text = Global.Config.PathSMSScreenshots;
            Sega8CheatsBox.Text = Global.Config.PathSMSCheats;

            PCEBaseBox.Text = Global.Config.BasePCE;
            PCEROMsBox.Text = Global.Config.PathPCEROMs;
            PCESavestatesBox.Text = Global.Config.PathPCESavestates;
            PCESaveRAMBox.Text = Global.Config.PathPCESaveRAM;
            PCEScreenshotsBox.Text = Global.Config.PathPCEScreenshots;
            PCECheatsBox.Text = Global.Config.PathPCECheats;

            GenesisBaseBox.Text = Global.Config.BaseGenesis;
            GenesisROMsBox.Text = Global.Config.PathGenesisROMs;
            GenesisSavestatesBox.Text = Global.Config.PathGenesisScreenshots;
            GenesisSaveRAMBox.Text = Global.Config.PathGenesisSaveRAM;
            GenesisScreenshotsBox.Text = Global.Config.PathGenesisScreenshots;
            GenesisCheatsBox.Text = Global.Config.PathGenesisCheats;

            GBBaseBox.Text = Global.Config.BaseGameboy;
            GBROMsBox.Text = Global.Config.PathGBROMs;
            GBSavestatesBox.Text = Global.Config.PathGBSavestates;
            GBSaveRAMBox.Text = Global.Config.PathGBSaveRAM;
            GBScreenshotsBox.Text = Global.Config.PathGBScreenshots;
            GBCheatsBox.Text = Global.Config.PathGBCheats;

            TI83BaseBox.Text = Global.Config.BaseTI83;
            TI83ROMsBox.Text = Global.Config.PathTI83ROMs;
            TI83SavestatesBox.Text = Global.Config.PathTI83Savestates;
            TI83SaveRAMBox.Text = Global.Config.PathTI83SaveRAM;
            TI83ScreenshotsBox.Text = Global.Config.PathTI83Screenshots;
            TI83CheatsBox.Text = Global.Config.PathTI83Cheats;

            MoviesBox.Text = Global.Config.MoviesPath;
            LuaBox.Text = Global.Config.LuaPath;
            WatchBox.Text = Global.Config.WatchPath;
            AVIBox.Text = Global.Config.AVIPath;
        }

        private void SaveSettings()
        {
            Global.Config.BasePath = BasePathBox.Text;
            Global.Config.WatchPath = WatchBox.Text;
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OK_Click(object sender, EventArgs e)
        {
            SaveSettings();
            this.Close();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //TODO: make base text box Controls[0] so this will focus on it
            //tabControl1.TabPages[tabControl1.SelectedIndex].Controls[0].Focus(); 
        }

        private void RecentForROMs_CheckedChanged(object sender, EventArgs e)
        {
            if (RecentForROMs.Checked)
            {
                NESROMsBox.Enabled = false;
                BrowseNESROMs.Enabled = false;
                Sega8ROMsBox.Enabled = false;
                Sega8BrowseROMs.Enabled = false;
                GenesisROMsBox.Enabled = false;
                GenesisBrowseROMs.Enabled = false;
                PCEROMsBox.Enabled = false;
                PCEBrowseROMs.Enabled = false;
                GBROMsBox.Enabled = false;
                GBBrowseROMs.Enabled = false;
                TI83ROMsBox.Enabled = false;
                TI83BrowseROMs.Enabled = false;     
            }
            else
            {
                NESROMsBox.Enabled = true;
                BrowseNESROMs.Enabled = true;
                Sega8ROMsBox.Enabled = true;
                Sega8BrowseROMs.Enabled = true;
                GenesisROMsBox.Enabled = true;
                GenesisBrowseROMs.Enabled = true;
                PCEROMsBox.Enabled = true;
                PCEBrowseROMs.Enabled = true;
                GBROMsBox.Enabled = true;
                GBBrowseROMs.Enabled = true;
                TI83ROMsBox.Enabled = true;
                TI83BrowseROMs.Enabled = true;
            }
        }

        private void BrowseFolder(TextBox box, string Name)
        {
            FolderBrowserDialog f = new FolderBrowserDialog();
            f.Description = "Set the directory for " + Name;
            f.SelectedPath = PathManager.MakeAbsolutePath(box.Text, "");
            DialogResult result = f.ShowDialog();
            if (result == DialogResult.OK)
                box.Text = f.SelectedPath;
        }

        private void BrowseFolder(TextBox box, string Name, string System)
        {
            FolderBrowserDialog f = new FolderBrowserDialog();
            f.Description = "Set the directory for " + Name;
            f.SelectedPath = PathManager.MakeAbsolutePath(box.Text, System);
            DialogResult result = f.ShowDialog();
            if (result == DialogResult.OK)
                box.Text = f.SelectedPath;
        }

        private void BrowseWatch_Click(object sender, EventArgs e)
        {
            BrowseFolder(WatchBox, WatchDescription.Text);
        }

        private void BrowseBase_Click(object sender, EventArgs e)
        {
            BrowseFolder(BasePathBox, BaseDescription.Text);
        }
        
        private void BrowseAVI_Click(object sender, EventArgs e)
        {
            BrowseFolder(AVIBox, AVIDescription.Text);
        }

        private void BrowseLua_Click(object sender, EventArgs e)
        {
            BrowseFolder(LuaBox, LuaDescription.Text);
        }

        private void BrowseMovies_Click(object sender, EventArgs e)
        {
            BrowseFolder(MoviesBox, MoviesDescription.Text);
        }

        private void BrowseNESBase_Click(object sender, EventArgs e)
        {
            BrowseFolder(NESBaseBox, NESBaseDescription.Text);
        }

        private void BrowseNESROMs_Click(object sender, EventArgs e)
        {
            BrowseFolder(NESROMsBox, NESROMsDescription.Text, "NES");
        }

        private void BrowseNESSavestates_Click(object sender, EventArgs e)
        {
            BrowseFolder(NESSavestatesBox, NESSavestatesDescription.Text, "NES");
        }

        private void BrowseNESSaveRAM_Click(object sender, EventArgs e)
        {
            BrowseFolder(NESSaveRAMBox, NESSaveRAMDescription.Text, "NES");
        }

        private void BrowseNESScreenshots_Click(object sender, EventArgs e)
        {
            BrowseFolder(NESScreenshotsBox, NESScreenshotsDescription.Text, "NES");
        }

        private void NESBrowseCheats_Click(object sender, EventArgs e)
        {
            BrowseFolder(NESCheatsBox, NESCheatsDescription.Text, "NES");
        }

        private void Sega8BrowseBase_Click(object sender, EventArgs e)
        {
            BrowseFolder(Sega8BaseBox, Sega8BaseDescription.Text, "SMS");
        }

        private void Sega8BrowseROMs_Click(object sender, EventArgs e)
        {
            BrowseFolder(Sega8ROMsBox, Sega8ROMsDescription.Text, "SMS");
        }

        private void Sega8BrowseSavestates_Click(object sender, EventArgs e)
        {
            BrowseFolder(Sega8SavestatesBox, Sega8SavestatesDescription.Text, "SMS");
        }

        private void Sega8BrowseSaveRAM_Click(object sender, EventArgs e)
        {
            BrowseFolder(Sega8SaveRAMBox, Sega8SaveRAMDescription.Text, "SMS");
        }

        private void Sega8BrowseScreenshots_Click(object sender, EventArgs e)
        {
            BrowseFolder(Sega8ScreenshotsBox, Sega8ScreenshotsDescription.Text, "SMS");
        }

        private void Sega8BrowseCheats_Click(object sender, EventArgs e)
        {
            BrowseFolder(Sega8CheatsBox, Sega8CheatsDescription.Text, "SMS");
        }

        private void GenesisBrowseBase_Click(object sender, EventArgs e)
        {
            BrowseFolder(GenesisBaseBox, GenesisBaseDescription.Text);
        }

        private void GenesisBrowseROMs_Click(object sender, EventArgs e)
        {
            BrowseFolder(GenesisROMsBox, GenesisROMsDescription.Text, "GEN");
        }

        private void GenesisBrowseSavestates_Click(object sender, EventArgs e)
        {
            BrowseFolder(GenesisSavestatesBox, GenesisSavestatesDescription.Text, "GEN");
        }

        private void GenesisBrowseSaveRAM_Click(object sender, EventArgs e)
        {
            BrowseFolder(GenesisSaveRAMBox, GenesisSaveRAMDescription.Text, "GEN");
        }

        private void GenesisBrowseScreenshots_Click(object sender, EventArgs e)
        {
            BrowseFolder(GenesisScreenshotsBox, GenesisScreenshotsDescription.Text, "GEN");
        }

        private void GenesisBrowseCheats_Click(object sender, EventArgs e)
        {
            BrowseFolder(GenesisCheatsBox, GenesisCheatsDescription.Text, "GEN");
        }

        private void PCEBrowseBase_Click(object sender, EventArgs e)
        {
            BrowseFolder(PCEBaseBox, PCEBaseDescription.Text);
        }

        private void PCEBrowseROMs_Click(object sender, EventArgs e)
        {
            BrowseFolder(PCEROMsBox, PCEROMsDescription.Text, "PCE");
        }

        private void PCEBrowseSavestates_Click(object sender, EventArgs e)
        {
            BrowseFolder(PCESavestatesBox, PCESavestatesDescription.Text, "PCE");
        }

        private void PCEBrowseSaveRAM_Click(object sender, EventArgs e)
        {
            BrowseFolder(PCESaveRAMBox, PCESaveRAMDescription.Text, "PCE");
        }

        private void PCEBrowseScreenshots_Click(object sender, EventArgs e)
        {
            BrowseFolder(PCEScreenshotsBox, PCEScreenshotsDescription.Text, "PCE");
        }

        private void PCEBrowseCheats_Click(object sender, EventArgs e)
        {
            BrowseFolder(PCECheatsBox, PCECheatsDescription.Text, "PCE");
        }

        private void GBBrowseBase_Click(object sender, EventArgs e)
        {
            BrowseFolder(GBBaseBox, GBBaseDescription.Text);
        }

        private void GBBrowseROMs_Click(object sender, EventArgs e)
        {
            BrowseFolder(GBROMsBox, GBROMsDescription.Text, "GB");
        }

        private void GBBrowseSavestates_Click(object sender, EventArgs e)
        {
            BrowseFolder(GBSavestatesBox, GBSavestatesDescription.Text, "GB");
        }

        private void GBBrowseSaveRAM_Click(object sender, EventArgs e)
        {
            BrowseFolder(GBSaveRAMBox, GBSaveRAMDescription.Text, "GB");
        }

        private void GBBrowseScreenshots_Click(object sender, EventArgs e)
        {
            BrowseFolder(GBScreenshotsBox, GBScreenshotsDescription.Text, "GB");
        }

        private void GBBrowseCheats_Click(object sender, EventArgs e)
        {
            BrowseFolder(GBCheatsBox, GBCheatsDescription.Text, "GB");
        }

        private void TI83BrowseBase_Click(object sender, EventArgs e)
        {
            BrowseFolder(TI83BaseBox, TI83BaseDescription.Text);
        }

        private void TI83BrowseROMs_Click(object sender, EventArgs e)
        {
            BrowseFolder(TI83ROMsBox, TI83ROMsDescription.Text, "TI83");
        }

        private void TI83BrowseSavestates_Click(object sender, EventArgs e)
        {
            BrowseFolder(TI83SavestatesBox, TI83SavestatesDescription.Text, "TI83");
        }

        private void TI83BrowseSaveRAM_Click(object sender, EventArgs e)
        {
            BrowseFolder(TI83SaveRAMBox, TI83SaveRAMDescription.Text, "TI83");
        }

        private void TI83BrowseScreenshots_Click(object sender, EventArgs e)
        {
            BrowseFolder(TI83ScreenshotsBox, TI83ScreenshotsDescription.Text, "TI83");
        }

        private void TI83BrowseBox_Click(object sender, EventArgs e)
        {
            BrowseFolder(TI83CheatsBox, TI83CheatsDescription.Text, "TI83");
        }
    }
}
