using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace VerilogParameterSeparator
{
    class VerilogParameterSeparator
    {
        public string srcWithoutParam;
        public Dictionary<string, string> paramOFInstances;

        private static string paramDefStartPattern = @"#\(";    //#(
        private static string paramNamePattern = @"\s*\.[\S^\(]+\(\s*[\S^\)]+\s*\)\s*";   //" .PARAM( DATA ) "
        private static string paramDefEndPattern = @"\)";   //)
        private static string paramDefPattern = paramDefStartPattern + @"(" + paramNamePattern + @"(," + paramNamePattern + @")*" + @")" + paramDefEndPattern; //.PARAM(DATA )(, .PARAM(DATA ))*
        private static string paramInstanceDefPattern = paramDefPattern + @"\s*(\S+)";  //#からinstance名まで含めた部分にマッチ
        private static string patternWithoutSpace = @"\s*([^\s\n]+)\s*";

        public VerilogParameterSeparator(string verilog_source)
        {
            paramOFInstances = new Dictionary<string, string>();
            extract(verilog_source);
        }

        private void extract(string verilog_source)
        {
            //parameter宣言文からパラメータを抽出する．書式はXilinx Vivadoが返すネットリストの書式に沿っている．
            srcWithoutParam = reduceString(verilog_source, paramDefPattern);
            MatchCollection mc = Regex.Matches(verilog_source, paramInstanceDefPattern);
            foreach (Match m in mc)
            {
                int loopCount = 0;
                string param = "";
                foreach (Group g in m.Groups)
                {
                    string str_without_space = Regex.Replace("" + g, patternWithoutSpace, "$1");
                    if (loopCount == 1)
                    {
                        param = str_without_space;
                    }
                    else if(loopCount == m.Groups.Count - 1)
                    {
                        string instance = str_without_space;
                        paramOFInstances.Add(instance, param);
                    }
                    loopCount++;
                }
            }
        }

        // patternで指定された正規表現パターンをsourceから一括削除してできる文字列を出力
        public static string reduceString(string source, string pattern)
        {
            return Regex.Replace(source, pattern, "");
        }

        public void outputParams()
        {
            foreach(var str in paramOFInstances)
            {
                Console.WriteLine(str.Key + " " + str.Value);
            }
        }
    }
}
