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
using System.IO;
using menDoc2.Common;

namespace menDoc2.Models.Class
{
    public class FileM : ModelBase
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FileM()
        {
        }
        #endregion

        #region ファイル名
        /// <summary>
        /// ファイル名
        /// </summary>
        string _FilePath = string.Empty;
        /// <summary>
        /// ファイル名
        /// </summary>
        public string FilePath
        {
            get
            {
                return _FilePath;
            }
            set
            {
                if (_FilePath == null || !_FilePath.Equals(value))
                {
                    _FilePath = value;
                    NotifyPropertyChanged("FilePath");
                    NotifyPropertyChanged("FileName");
                }
            }
        }
        #endregion

        public string FileName
        {
            get
            {
                return Path.GetFileName(this.FilePath);
            }
        }

        public string FilePathShort
        {
            get
            {
                var dirs = (from x in GblValues.Instance.FileCollection.FileList.Items
                            where x.FilePath.Contains(".csproj")
                            orderby x.FilePath
                            select PathManager.GetCurrentDirectory(PathManager.GetCurrentDirectory(x.FilePath)) + @"\").Distinct().ToList();

                string filepath = this.FilePath;
                foreach (var dir in dirs)
                {
                    filepath = filepath.Replace(dir,"");
                }
                return filepath;
            }
        }

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

                    if (cls.BaseList != null)
                    {
                        foreach (var tmp in cls.BaseList.Types)
                        {
                            cls_tmp.BaseClass.Add(tmp.ToString());
                        }
                    }

                    cls_tmp.Accessor = Common.Enums.AccessModifier.Private;
                    foreach (var modi in cls.Modifiers)
                    {
                        switch (modi.ToString())
                        {
                            case "public":
                                {
                                    cls_tmp.Accessor = Common.Enums.AccessModifier.Public;
                                    break;
                                }
                            case "private":
                                {
                                    cls_tmp.Accessor = Common.Enums.AccessModifier.Private;
                                    break;
                                }
                            case "protected":
                                {
                                    cls_tmp.Accessor = Common.Enums.AccessModifier.Protected;
                                    break;
                                }
                            case "package":
                                {
                                    cls_tmp.Accessor = Common.Enums.AccessModifier.Package;
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                    }


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

                        param.Accessor = Common.Enums.AccessModifier.Private;
                        foreach (var modi in field.Modifiers)
                        {
                            switch (modi.ToString())
                            {
                                case "public":
                                    {
                                        param.Accessor = Common.Enums.AccessModifier.Public;
                                        break;
                                    }
                                case "private":
                                    {
                                        param.Accessor = Common.Enums.AccessModifier.Private;
                                        break;
                                    }
                                case "protected":
                                    {
                                        param.Accessor = Common.Enums.AccessModifier.Protected;
                                        break;
                                    }
                                case "package":
                                    {
                                        param.Accessor = Common.Enums.AccessModifier.Package;
                                        break;
                                    }
                                default:
                                    {
                                        break;
                                    }
                            }
                        }

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

                        clsmethod.Accessor = Common.Enums.AccessModifier.Private;
                        foreach (var modi in method.Modifiers)
                        {
                            switch (modi.ToString())
                            {
                                case "public":
                                    {
                                        clsmethod.Accessor = Common.Enums.AccessModifier.Public;
                                        break;
                                    }
                                case "private":
                                    {
                                        clsmethod.Accessor = Common.Enums.AccessModifier.Private;
                                        break;
                                    }
                                case "protected":
                                    {
                                        clsmethod.Accessor = Common.Enums.AccessModifier.Protected;
                                        break;
                                    }
                                case "package":
                                    {
                                        clsmethod.Accessor = Common.Enums.AccessModifier.Package;
                                        break;
                                    }
                                default:
                                    {
                                        break;
                                    }
                            }
                        }

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
