using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium_Interface
{
    public class CodeGenerator
    {
        public static string generate(string filePath,string[] codeLines)
        {
            string code = File.ReadAllText(filePath);
            for (int i = 1; i < codeLines.Length; i++)
            {
                code += "                case " + i.ToString() + ":\r\n" +
                        "                    " + codeLines[i] + "\r\n" +
                        "                    break;\r\n";
            }
            code += "                default: \r\n" + "                    break;\r\n}\r\n}" +
                "                catch (Exception ex)\r\n{\r\n" +
                "                return ex.Message;\r\n}\r\n" +
                "                return \"true\";\r\n" +
                "                }\r\n                }\r\n                }\r\n";
            return code;
        }
    }
}