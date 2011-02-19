﻿namespace BizHawk.MultiClient
{    
    public class Config
    {
        public Config()
        {
            SMSController[0] = new SMSControllerTemplate(1);
            SMSController[1] = new SMSControllerTemplate(2);
            PCEController[0] = new PCEControllerTemplate(1);
            PCEController[1] = new PCEControllerTemplate(2);
            PCEController[2] = new PCEControllerTemplate(3);
            PCEController[3] = new PCEControllerTemplate(4);
            PCEController[4] = new PCEControllerTemplate(5);
        }

        // General Client Settings
        public int TargetZoomFactor = 2;
        public string LastRomPath = ".";
        public bool AutoLoadMostRecentRom = false;    //TODO: eventually make a class or struct for all the auto-loads, which will include recent roms, movies, etc, as well as autoloading any modeless dialog
        public RecentFiles RecentRoms = new RecentFiles(8);
        public bool PauseWhenMenuActivated = true;
        public bool SaveWindowPosition = true;
        public bool StartPaused = false;
        public int MainWndx = -1; //Negative numbers will be ignored
        public int MainWndy = -1;

        // Display options
        public bool DisplayFPS = false;
        public bool DisplayFrameCounter = false;
        public bool DisplayLagCounter = false;
        public bool DisplayInput = false;

        // Sound options
        public bool SoundEnabled = true;
        public bool MuteFrameAdvance = true;

        // RamWatch Settings
        public bool AutoLoadRamWatch = false;
        public RecentFiles RecentWatches = new RecentFiles(8);
        public int RamWatchWndx = -1;   //Negative numbers will be ignored even with save window position set
        public int RamWatchWndy = -1;
        public int RamWatchWidth = -1;  //Negative numbers will be ignored
        public int RamWatchHeight = -1; 

        // RamSearch Settings
        public bool AutoLoadRamSearch = false;
        public int RamSearchWndx = -1;   //Negative numbers will be ignored even with save window position set
        public int RamSearchWndy = -1;
        public int RamSearchWidth = -1;  //Negative numbers will be ignored
        public int RamSearchHeight = -1; 

        //Movie Settings
        public RecentFiles RecentMovies = new RecentFiles(8);

        // Client Hotkey Bindings
        //TODO: These should be allowed to be "", not every hotkey should have to be mapped somewhere
        public string HardResetBinding = "LeftShift+Tab"; //TODO: This needs to be Ctrl+R but how?
        public string FastForwardBinding = "J1 B6, Tab";
        public string RewindBinding = "J1 B5, LeftShift+R, RightShift+R";
        public string EmulatorPauseBinding = "Pause";
        public string FrameAdvanceBinding = "F";
        public string ScreenshotBinding = "F12";
        public string ToggleFullscreenBinding = "LeftAlt+Return, RightAlt+Return";
        public string QuickSave = "I";
        public string QuickLoad = "P";
        public string SelectSlot0 = "0";
        public string SelectSlot1 = "1";
        public string SelectSlot2 = "2";
        public string SelectSlot3 = "3";
        public string SelectSlot4 = "4";
        public string SelectSlot5 = "5";
        public string SelectSlot6 = "6";
        public string SelectSlot7 = "7";
        public string SelectSlot8 = "8";
        public string SelectSlot9 = "9";

        
        // SMS / GameGear Settings
        public bool SmsEnableFM = true;
        public bool SmsAllowOverlock = false;
        public bool SmsForceStereoSeparation = false;

        public string SmsReset = "Reset, Tab";
        public string SmsPause = "J1 B10, Space";
        public SMSControllerTemplate[] SMSController = new SMSControllerTemplate[2];

        // PCEngine Settings
        public PCEControllerTemplate[] PCEController = new PCEControllerTemplate[5];

        // Genesis Settings
        public string GenP1Up = "J1 Up, UpArrow";
        public string GenP1Down = "J1 Down, DownArrow";
        public string GenP1Left = "J1 Left, LeftArrow";
        public string GenP1Right = "J1 Right, RightArrow";
        public string GenP1A = "J1 B1, Z";
        public string GenP1B = "J1 B2, X";
        public string GenP1C = "J1 B9, C";
        public string GenP1Start = "J1 B10, Return"; 
    }
    public class SMSControllerTemplate
    {
        public string Up;
        public string Down;
        public string Left;
        public string Right;
        public string B1;
        public string B2;
        public SMSControllerTemplate(int i)
        {
            Up = string.Format("J{0} Up", i);
            Down = string.Format("J{0} Down", i);
            Left = string.Format("J{0} Left", i);
            Right = string.Format("J{0} Right", i);
            B1 = string.Format("J{0} B1", i);
            B2 = string.Format("J{0} B2", i);
        }
    }
    public class PCEControllerTemplate
    {
        public string Up;
        public string Down;
        public string Left;
        public string Right;
        public string I;
        public string II;
        public string Run;
        public string Select;
        public PCEControllerTemplate(int i)
        {
            Up = string.Format("J{0} Up", i);
            Down = string.Format("J{0} Down", i);
            Left = string.Format("J{0} Left", i);
            Right = string.Format("J{0} Right", i);
            I = string.Format("J{0} I", i);
            II = string.Format("J{0} II", i);
            Run = string.Format("J{0} Run", i);
            Select = string.Format("J{0} Select", i);
        }
    }

}