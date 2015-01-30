using System;
using System.Diagnostics;
using System.Linq;
using System.IO;

using Atmosphere.SexyLib.Exceptions;
using Atmosphere.Extensions;

namespace Atmosphere.SexyLib
{
    public static partial class Primitives
    {                
        public static Atom RETURN_CODE_0 = Atom.CreateLong(0);
        public static Atom RETURN_CODE_1 = Atom.CreateLong(1);

        public static ISExp Pwd(string name, params ISExp[] parameters)
        {
            CheckArity(name, 0, parameters);
            
            return Atom.CreateString(Directory.GetCurrentDirectory());
        }

        public static ISExp Cd(string name, params ISExp[] parameters)
        {
            CheckArity(name, 0, 2, parameters);

            Atom retCode = RETURN_CODE_0;

            string targetDir = null;

            if (parameters.Length == 0)
            {
                targetDir = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            }
            else
            {
                CheckType(IsText, name, 1, "text", parameters[1]);

                targetDir = ((Atom)parameters[1]).ToDisplay();

                if (Path.GetInvalidPathChars().Any(x => targetDir.Contains(x)))
                {
                    throw new BadArgumentException("valid path string", targetDir);
                }
            }

            if (parameters.Length == 2)
            {
                // follow symbolic links?
            }

            try
            {
                Directory.SetCurrentDirectory(targetDir);
            }
            catch (Exception e)
            {
                retCode = RETURN_CODE_1;
            }

            return retCode;
        }

        public static ISExp Exec(string name, params ISExp[] parameters)
        {
            CheckEnoughArguements(name, 1, parameters);
            
            for (int i = 0; i < parameters.Length; i++)
            {
                CheckType(IsText, name, i, "text", parameters[i]);
            }
            
            string[] arguments = new string[parameters.Length - 1];
            
            for (int i = 1; i < parameters.Length; i++)
            {
                arguments[i - 1] = ((Atom)parameters[i]).ToDisplay();
            }
            
            string cmd = ((Atom)parameters[0]).ToDisplay();
            
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

