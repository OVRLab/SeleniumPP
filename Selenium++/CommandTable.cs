using MetroFramework.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Selenium_Recorder
{
    public abstract class Command
    {
        public string id;
        protected string _TextCommand;
        public string Value,Result;
    }
    public class Pattern
    {
        const string patterns_path = "C:\\patterns\\";
        public Bitmap img;
        public string path;
        public Pattern(Bitmap _img)
        {
            img = _img;
            int random = new Random().Next(9999);
            while(File.Exists(patterns_path+random+".png"))
            {
                random = new Random().Next(9999);
            }
            path = patterns_path + random + ".png";
            img.Save(path);
        }
    }
    public struct target_command_pair
    {
        public string Target;
        public string CSharpCommand;
    }
    public class Command_WD : Command
    {
        public string baseURL, lastURL;
        public int selectedTargetIndex;
        public List<target_command_pair> target_command_list = new List<target_command_pair>();
        public string TextCommand 
        { 
            get {return _TextCommand;}
            set {_TextCommand = value;}
        }
    }

    public class Command_SK : Command
    {
        public static string[] sk_type = new string[18]
        { "IP.Find", "IP.Exists", "IP.Wait", "IP.WaitVanish",
            "IP.Click", "IP.DoubleClick", "IP.RightClick", "IP.Type", "IP.ScrollDown",
        "LCDC.Find", "LCDC.Wait", "LCDC.WaitVanish",
            "LCDC.Click", "LCDC.DoubleClick", "LCDC.RightClick", "LCDC.Type","LCDC.Var","LCDC.ValidateLayout"};
        public Pattern pattern;

        public Command_SK(string _TextCommand,Pattern _p)
        {
            TextCommand = _TextCommand;
            pattern = _p;
        }
        public string TextCommand 
        {
            get {return _TextCommand;}
            set {_TextCommand = value;}
        }
    }
    public class CommandTable
    {
        private bool CodeCompiled = false;
        Selenium_Interface.CodeCompiler cc;
        Selenium_Interface.IDesktop iDesktop;
        int timeout = 15;
        public CommandTable()
        {
            iDesktop = new Selenium_Interface.IDesktop();
        }
        public List<Command> commands = new List<Command>();
        public int Count()
        {
            return commands.Count;
        }
        public void Clear()
        {
            CodeCompiled = false;
            commands.Clear();
        }

        public void Delete(int index)
        {
            CodeCompiled = false;
            commands.RemoveAt(index);
        }

        public Command GetCommandbyIndex(int index)
        {
            if(index < commands.Count)
                return commands[index];
            return null;
        }

        public void SetCommand(Command c,int index = -1)
        {
            CodeCompiled = false;
            if (index != -1 && index < commands.Count)
                commands[index] = c;
            else
            {
                c.id = commands.Count.ToString();
                commands.Add(c);
            }
        }

        public void UpdateLastRow2Grid(MetroGrid grid,int index=-1)
        {
            if(index == -1)
                index = grid.Rows.Add();
            if(commands[commands.Count - 1].GetType() == typeof(Command_WD))
            {
                Command_WD c = (Command_WD)commands[commands.Count - 1];
                grid.Rows[index].Cells[0].ToolTipText = c.id.ToString();
                grid.Rows[index].Cells[1].Value = c.TextCommand;
                grid.Rows[index].Cells[2].Value = c.target_command_list[c.selectedTargetIndex].Target;
                grid.Rows[index].Cells[3].Value = c.Value;
            }
            else if (commands[commands.Count - 1].GetType() == typeof(Command_SK))
            {
                Command_SK c = (Command_SK)commands[commands.Count - 1];
                grid.Rows[index].Cells[0].ToolTipText = c.id;
                grid.Rows[index].Cells[1].Value = c.TextCommand;
                grid.Rows[index].Cells[4].Value = c.pattern.img;
            }
        }

        private bool compile()
        {
            List<string> switchCode = new List<string>();
            for (int i = 0; i < commands.Count; i++)
            {
                if(commands[i].GetType() == typeof(Command_WD))
                {
                    Command_WD c = (Command_WD)commands[i];
                    string csharp = c.target_command_list[c.selectedTargetIndex].CSharpCommand;
                    if (csharp.StartsWith("ERROR:"))
                        csharp = "//Do Nothing";
                    switchCode.Add(csharp);
                }
                else if (commands[i].GetType() == typeof(Command_SK))
                {
                    switchCode.Add("//Do Nothing");
                }
            }
            string code = Selenium_Interface.CodeGenerator.generate("SeleniumWebDriver.cs", switchCode.ToArray());
            cc = new Selenium_Interface.CodeCompiler();
            string[] references = new string[5] { "System.dll", "System.Windows.Forms.dll", "System.Drawing.dll", "WebDriver.dll", "WebDriver.Support.dll" };
            if (cc.CompileCode(references, code, "SeleniumGeneratedCode", "SeleniumWebDriver"))
                CodeCompiled = true;
            else
            {
                CodeCompiled = false;
                code = "";
                cc = null;
                MessageBox.Show("Compile Error");
            }
            return CodeCompiled;
        }
        public bool ExecuteCommand(int index)
        {
            if (!CodeCompiled)
                compile();
            if (CodeCompiled)
            {
                object res = "";
                if (commands[index].GetType() == typeof(Command_SK))
                {
                    Command_SK c = (Command_SK)commands[index];
                    string command = c.TextCommand;
                    res = false;
                    switch (command)
                    {
                        case "IP.Click":
                            if (iDesktop.Click(c.pattern.path, new Point(10, 10), 0.5, true))
                                res = true;
                            break;
                        case "IP.Wait":
                            if (iDesktop.Wait(c.pattern.path, new Point(10, 10), 0.5, timeout))
                                res = true;
                            break;
                        case "IP.Find":
                            if (iDesktop.Find(c.pattern.path, new Point(10, 10), 0.5, true))
                                res = true;
                            break;
                        case "IP.Exists":
                            if (iDesktop.Exists(c.pattern.path, new Point(10, 10), 0.5, timeout))
                                res = true;
                            break;
                        case "IP.WaitVanish":
                            if (iDesktop.WaitVanish(c.pattern.path, new Point(10, 10), 0.5, 10))
                                res = true;
                            break;
                        case "IP.DoubleClick":
                            if (iDesktop.DoubleClick(c.pattern.path, new Point(10, 10), 0.5, true))
                                res = true;
                            break;
                        case "IP.RightClick":
                            if (iDesktop.RightClick(c.pattern.path, new Point(10, 10), 0.5, true))
                                res = true;
                            break;
                        case "IP.Type":
                            if (iDesktop.Type(c.pattern.path, new Point(10, 10),c.Value, 0.5))
                                res = true;
                            break;
                        //case "IP.DragDrop":
                        //    if (iDesktop.DragDrop(c.pattern.path, new Point(10, 10), 0.5, 10))
                        //        res = true;
                        //    break;
                        default:
                            break;
                    }
                }
                else if (commands[index].GetType() == typeof(Command_WD))
                {
                    Command_WD c = (Command_WD)commands[index];
                    string baseURL = c.baseURL;
                    res = cc.ExecuteCode("ExecuteCommand", new string[2] { baseURL, index.ToString() });
                }
                commands[index].Result = res.ToString();
                if (res.ToString() == "true" || res.ToString() == (true).ToString())
                    return true;
                else
                    return false;
            }
            return false;
        }
    }
}