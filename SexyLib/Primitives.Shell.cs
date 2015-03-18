using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;

using Atmosphere.SexyLib.Exceptions;
using Atmosphere.Extensions;

namespace Atmosphere.SexyLib
{
    public static partial class Primitives
    {   
        #region Directory helpers

        private static Stack<string> DirectoryStack = new Stack<string>();

        private static Pair GetDirectoriesInStack()
        {
            Pair stack = Pair.List(DirectoryStack.Select(x => Atom.CreateString(x)).ToArray());
            
            stack = Pair.Cons(Atom.CreateString(Directory.GetCurrentDirectory()), stack);
            
            return stack;
        }
        
        private static void ValidateDirectory(string dir)
        {
            if (!Directory.Exists(dir))
            {
                throw new BadArgumentException("existing directory", dir);
            }
        }

        private static void SetCurrentDirectory(string dir)
        {
            try
            {
                Directory.SetCurrentDirectory(dir);
            }
            catch (Exception e)
            {
                throw new DirectoryException("Couldn't set current directory: {0}", e.Message);
            }
        }

        #endregion Directory helpers
        






        
        [@PrimitiveMethod("list-files")]
        public static ISExp ListFiles(string name, params ISExp[] parameters)
        {
            CheckArity(name, 0, 1, parameters);

            string dir = Directory.GetCurrentDirectory();

            if (parameters.Length > 0)
            {
                ISExp arg = parameters[0];
                
                if (IsString(arg) || IsSymbol(arg))
                {
                    dir = (String)((Atom)arg).Value;
                }
            }

            ISExp fileList = Pair.Empty;

            foreach (string file in Directory.GetFiles(dir))
            {
                fileList = Pair.Cons(Atom.CreateString(file), fileList);
            }
            
            return fileList;
        }
        
        [@PrimitiveMethod("pwd")]
        public static ISExp Pwd(string name, params ISExp[] parameters)
        {
            CheckArity(name, 0, parameters);
            
            return Atom.CreateString(Directory.GetCurrentDirectory());
        }
        
        [@PrimitiveMethod("cd")]
        public static ISExp Cd(string name, params ISExp[] parameters)
        {
            CheckArity(name, 0, 2, parameters);

            string targetDir = null;

            if (parameters.Length == 0)
            {
                targetDir = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            }
            else
            {
                CheckType(IsText, name, 0, "text", parameters);

                targetDir = ((Atom)parameters[0]).ToDisplay();
            }

            if (parameters.Length == 2)
            {
                // follow symbolic links?
            }

            ValidateDirectory(targetDir);

            SetCurrentDirectory(targetDir);
                        
            return Atom.CreateString(Directory.GetCurrentDirectory());
        }

        
        [@PrimitiveMethod("pushd")]
        public static ISExp Pushd(string name, params ISExp[] parameters)
        {
            CheckArity(name, 0, 1, parameters);


            if (parameters.Length == 0)
            {
                if (DirectoryStack.Count < 1)
                {
                    throw new UndefinedOperationException("Cannot rotate top of directory stack; no other directories", DirectoryStack.Count);
                }

                string dir0 = Directory.GetCurrentDirectory();
                string dir1 = DirectoryStack.Pop();

                DirectoryStack.Push(dir0);

                Directory.SetCurrentDirectory(dir1);
            }
            else
            {
                ISExp arg = parameters[0];

                if (IsString(arg) || IsSymbol(arg))
                {
                    string dir = (String)((Atom)arg).Value;

                    ValidateDirectory(dir);

                    DirectoryStack.Push(Directory.GetCurrentDirectory());
                    
                    SetCurrentDirectory(dir);
                }
                else if (IsLong(parameters[0]))
                {
                    throw new UnsupportedOperationException("Rotating the directory stack by N is not yet supported.");
                }
                else
                {
                    throw new UnexpectedTypeException(name, 0, "string, symbol, or integer", arg);
                }
            }
            
            return GetDirectoriesInStack();
        }
        
        [@PrimitiveMethod("popd")]
        public static ISExp Popd(string name, params ISExp[] parameters)
        {
            CheckArity(name, 0, 1, parameters);

            if (parameters.Length == 0)
            {
                if (DirectoryStack.Count < 1)
                {
                    throw new UndefinedOperationException("Cannot pop; stack is empty");
                }

                string dir = DirectoryStack.Pop();
                
                SetCurrentDirectory(dir);
            }
            else
            {
                ISExp arg = parameters[0];

                if (IsLong(parameters[0]))
                {
                    throw new UnsupportedOperationException("Popping a specific directory off of the stack is not yet supported.");
                }
                else
                {
                    throw new UnexpectedTypeException(name, 0, "integer", arg);
                }
            }
            
            return GetDirectoriesInStack();
        }
        
        [@PrimitiveMethod("dirs")]
        public static ISExp Dirs(string name, params ISExp[] parameters)
        {
            CheckArity(name, 0, parameters);

            return GetDirectoriesInStack();
        }







        
        [@PrimitiveMethod("exec")]
        public static ISExp Exec(string name, params ISExp[] parameters)
        {
            CheckEnoughArguements(name, 1, parameters);

            string cmd;
            string[] arguments;

            if (parameters.Length == 1 && IsPair(parameters[0]))
            {
                parameters = ((Pair)parameters[0]).ToArray();
            }


            for (int i = 0; i < parameters.Length; i++)
            {
                CheckType(IsText, name, i, "text", parameters);
            }
            
            arguments = new string[parameters.Length - 1];
            
            for (int i = 1; i < parameters.Length; i++)
            {
                arguments[i - 1] = ((Atom)parameters[i]).ToDisplay();
            }
            
            cmd = ((Atom)parameters[0]).ToDisplay();
           

            if (!File.Exists(Path.Combine(Directory.GetCurrentDirectory(), cmd)))
            {
                bool success = false;
                
                string paths = System.Environment.GetEnvironmentVariable("PATH");
                
                foreach (string path in paths.Split(Path.PathSeparator))
                {
                    string tmp = Path.Combine(path, cmd);
                    
                    if (File.Exists(tmp))
                    {
                        success = true;
                        cmd = tmp;
                        break;
                    }
                }
                
                if (!success)
                {
                    throw new Exceptions.FileNotFoundException("Could not exec {0}; not found in current directory nor PATH", cmd);
                }
            }

            Process process = new Process{
                StartInfo = new ProcessStartInfo
                {
                    FileName = cmd,
                    Arguments = String.Join(" ", arguments),
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true
                }
            };
            
            process.Start();
            
            String output = process.StandardOutput.ReadToEnd();
            
            process.WaitForExit();
            
            return Atom.CreateString(output);
        }
        
        
    }
}

