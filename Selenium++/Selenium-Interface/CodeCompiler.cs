using System;
using System.Text;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;

namespace Selenium_Interface
{
    /// <summary>
    /// This class can be used to execute dynamic uncompiled code at runtime
    /// This class is not exception safe, all function calls should be exception handled.
    /// </summary>
    public class CodeCompiler
    {
        object instance = null;
        Type type = null;
        /// <summary>
        /// Default Constructor.
        /// It is necessary to have an instance of the class so that the reflection
        /// can get the namespace of this class dynamically.
        /// </summary>
        /// <remarks>
        /// This class could be static, but I wanted to make it easy for developers
        /// to use this class by allowing them to change the namespace without
        /// harming the code.
        /// </remarks>
        public CodeCompiler()
        {
        }
        /// <summary>
        /// This is the main code execution function.
        /// It allows for any compilable c# code to be executed.
        /// </summary>
        /// <param name="code">the code to be compiled then executed</param>
        /// <param name="namespacename">the name of the namespace to be executed</param>
        /// <param name="classname">the name of the class of the function in the code that you will execute</param>
        /// <param name="functionname">the name of the function that you will execute</param>
        /// <param name="isstatic">True if the function you will execute is static, otherwise false</param>
        /// <param name="args">any parameters that must be passed to the function</param>
        /// <returns>what ever the executed function returns, in object form</returns>
        public bool CompileCode(string[] references, string code, string namespacename, string classname)
        {
            try
            {
                Assembly asm = BuildAssembly(references, code);
                //if (isstatic)
                //    type = asm.GetType(namespacename + "." + classname);
                //else
                //{
                instance = asm.CreateInstance(namespacename + "." + classname,false);
                type = instance.GetType();
                //}
                return true;
            }
            catch
            {
                return false;
            }
        }
        public object ExecuteCode(string functionname, params object[] args)
        {
            MethodInfo method = type.GetMethod(functionname);
            object returnval = method.Invoke(instance, args);
            return returnval;
        }
        /// <summary>
        /// This private function builds the assembly file into memory after compiling the code
        /// </summary>
        /// <param name="code">C# code to be compiled</param>
        /// <returns>the compiled assembly object</returns>
        private Assembly BuildAssembly(string[] references, string code)
        {
            Microsoft.CSharp.CSharpCodeProvider provider = new CSharpCodeProvider();
            ICodeCompiler compiler = provider.CreateCompiler();
            CompilerParameters compilerparams = new CompilerParameters(references);
            compilerparams.GenerateExecutable = false;
            compilerparams.GenerateInMemory = true;
            CompilerResults results = compiler.CompileAssemblyFromSource(compilerparams, code);
            if (results.Errors.HasErrors)
            {
                StringBuilder errors = new StringBuilder("Compiler Errors :\r\n");
                foreach (CompilerError error in results.Errors)
                {
                    errors.AppendFormat("Line {0},{1}\t: {2}\n", error.Line, error.Column, error.ErrorText);
                }
                throw new Exception(errors.ToString());
            }
            else
            {
                return results.CompiledAssembly;
            }
        }
    }
}
