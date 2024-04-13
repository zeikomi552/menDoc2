using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using MVVMCore.BaseClass;
using MVVMCore.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;

namespace menDoc2.Models.Class
{
    public class FileM : ModelBase
    {
        public FileM()
        {
            this.JogaiFolder.Items.Add(@"\.git\");
            this.JogaiFolder.Items.Add(@"\.vs\");
            this.JogaiFolder.Items.Add(@"\.git\");
            this.JogaiFolder.Items.Add(@"\bin\");
            this.JogaiFolder.Items.Add(@"\obj\");
        }
        #region ファイル名
        /// <summary>
        /// ファイル名
        /// </summary>
        string _FileName = string.Empty;
        /// <summary>
        /// ファイル名
        /// </summary>
        public string FileName
        {
            get
            {
                return _FileName;
            }
            set
            {
                if (_FileName == null || !_FileName.Equals(value))
                {
                    _FileName = value;
                    NotifyPropertyChanged("FileName");
                }
            }
        }
        #endregion

        #region Class要素を保持するオブジェクト
        /// <summary>
        /// Class要素を保持するオブジェクト
        /// </summary>
        ModelList<ClassM> _ClassList = new ModelList<ClassM>();
        /// <summary>
        /// Class要素を保持するオブジェクト
        /// </summary>
        public ModelList<ClassM> ClassList
        {
            get
            {
                return _ClassList;
            }
            set
            {
                if (_ClassList == null || !_ClassList.Equals(value))
                {
                    _ClassList = value;
                    NotifyPropertyChanged("ClassList");
                }
            }
        }
        #endregion

        #region 除外フォルダリスト
        /// <summary>
        /// 除外フォルダリスト
        /// </summary>
        ModelList<string> _JogaiFolder = new ModelList<string>();
        /// <summary>
        /// 除外フォルダリスト
        /// </summary>
        public ModelList<string> JogaiFolder
        {
            get
            {
                return _JogaiFolder;
            }
            set
            {
                if (_JogaiFolder == null || !_JogaiFolder.Equals(value))
                {
                    _JogaiFolder = value;
                    NotifyPropertyChanged("JogaiFolder");
                }
            }
        }
        #endregion

        #region クラス図用マークダウンファイルの作成
        /// <summary>
        /// クラス図用マークダウンファイルの作成
        /// </summary>
        /// <returns>マークダウン文字列</returns>
        public static string CreateClassMarkdown(List<FileM> files)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("```mermaid");
            sb.AppendLine("classDiagram");

            Dictionary<string, List<string>> clsname_list = new Dictionary<string, List<string>>();
            foreach (var file in files)
            {
                foreach (var cls in file.ClassList.Items)
                {
                    sb.AppendLine("\tclass " + cls.Name + "{");

                    List<string> param_type_list = new List<string>();

                    foreach (var prop in cls.ParameterItems.Items)
                    {
                        string pt = string.Empty;
                        if (prop.Accessor == Common.Enums.AccessModifier.Public)
                        {
                            pt = "+";
                        }
                        else
                        {
                            pt = "-";
                        }
                        pt = pt + prop.TypeName + " " + prop.ValueName;
                        sb.AppendLine("\t\t" + pt);

                        param_type_list.Add(prop.TypeName);
                    }
                    // 冗長排除
                    param_type_list = param_type_list.Distinct().ToList();
                    // クラス名とプロパティの型一覧を保持
                    clsname_list.Add(cls.Name, param_type_list);

                    foreach (var method in cls.MethodItems.Items)
                    {
                        string mtd = string.Empty;
                        if (method.Accessor == Common.Enums.AccessModifier.Public)
                        {
                            mtd = "+";
                        }
                        else
                        {
                            mtd = "-";
                        }
                        mtd = mtd + method.ReturnValue + " " + method.MethodName + "()";
                        sb.AppendLine("\t\t" + mtd);
                    }



                    sb.AppendLine("\t" + "}");

                }

            }

            foreach (var dic in clsname_list)
            {
                foreach (var dic2 in clsname_list)
                {
                    if (dic.Key.Equals(dic2.Key))
                        continue;


                    foreach (var dic_vl in dic.Value)
                    {
                        if (dic_vl.Equals(dic2.Key) || dic_vl.Contains("<" + dic2.Key + ">"))
                        {
                            sb.AppendLine("\t" + dic.Key + " -- " + dic2.Key);
                        }
                    }
                }

            }
            sb.AppendLine("```");


            return sb.ToString();
        }
        #endregion


        #region 読込処理
        /// <summary>
        /// 読込処理
        /// </summary>
        public static List<ClassM>? LoadCS(string filename)
        {
            try
            {
                List<ClassM> classm = new List<ClassM>();
                var text = System.IO.File.ReadAllText(filename);
                SyntaxTree tree = CSharpSyntaxTree.ParseText(text);
                var root = tree.GetCompilationUnitRoot();
                var clss = root.DescendantNodes().OfType<ClassDeclarationSyntax>();
                var methods = root.DescendantNodes().OfType<MethodDeclarationSyntax>();

                foreach (var cls in clss)
                {
                    ClassM cls_tmp = new ClassM();
                    cls_tmp.Name = cls.Identifier.Text;
                    cls_tmp.CreateDate = DateTime.Now;
                    cls_tmp.CreateUser = Environment.UserName;
                    var trivia = cls.GetLeadingTrivia().ToString();
                    cls_tmp.Description = ExclusiveTextTrivia(trivia);

                    var properties = cls.DescendantNodes().OfType<PropertyDeclarationSyntax>();

                    cls_tmp.SynTaxTree = cls.SyntaxTree;

                    List<ClassParamM> cls_params = new List<ClassParamM>();
                    foreach (var field in properties)
                    {
                        ClassParamM param = new ClassParamM();
                        trivia = field.GetLeadingTrivia().ToString();
                        param.TypeName = field.Type.ToString();
                        param.ValueName = field.Identifier.Text;
                        param.Description = ExclusiveTextTrivia(trivia);
                        cls_params.Add(param);
                    }
                    cls_tmp.ParameterItems = new ModelList<ClassParamM>(cls_params);

                    List<ClassMethodM> cls_methods = new List<ClassMethodM>();
                    foreach (var method in methods)
                    {
                        ClassMethodM clsmethod = new ClassMethodM();
                        trivia = method.GetLeadingTrivia().ToString();
                        clsmethod.Description = ExclusiveTextTrivia(trivia);
                        clsmethod.MethodName = method.Identifier.Text;
                        clsmethod.ReturnValue = method.ReturnType.ToString();
                        clsmethod.Arguments = new ModelList<ClassParamM>();
                        var method_params = method.ParameterList.Parameters;

                        foreach (var method_param in method_params)
                        {
                            clsmethod.Arguments.Items.Add(
                                new ClassParamM()
                                {
                                    Description = ExclusiveTextTrivia(method_param.Identifier.LeadingTrivia.ToString()),
                                    TypeName = method_param.Type!.ToString()!,
                                    ValueName = method_param.Identifier.Value!.ToString()!,
                                }
                                );
                        }

                        cls_methods.Add(clsmethod);
                    }
                    cls_tmp.MethodItems = new ModelList<ClassMethodM>(cls_methods);

                    classm.Add(cls_tmp);
                }
                return classm;
            }
            catch
            {
                return null;
            }
        }
        #endregion

        private static string ExclusiveTextTrivia(string trivia)
        {
            // コメント抜き出し
            var match = Regex.Match(trivia, @"<summary>[\s\S]*?</summary>");
            // 不要な文字列削除
            return match.Value.Replace("<summary>", "").Replace("</summary>", "").Replace("\r\n", "").Replace("///", "").Replace(" ", "").Replace("\t", "");
        }

    }
}
