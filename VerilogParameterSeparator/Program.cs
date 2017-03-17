using System;
using System.IO;

namespace VerilogParameterSeparator
{
    class Program
    {
        static void Main(string[] args)
        {
            string src_file = args[0];
            var paramOfInstances = new VerilogParameterSeparator(File.ReadAllText(src_file));
            Console.Write(paramOfInstances.srcWithoutParam);
            Console.Write("\n\n");
            paramOfInstances.outputParams();
        }
    }
}
