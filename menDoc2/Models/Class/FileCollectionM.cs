using Microsoft.CodeAnalysis.CSharp.Syntax;
using MVVMCore.BaseClass;
using MVVMCore.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace menDoc2.Models.Class
{
    public class FileCollectionM : ModelBase
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FileCollectionM()
        {
            this.JogaiFolder.Items.Add(@"\.git\");
            this.JogaiFolder.Items.Add(@"\.vs\");
            this.JogaiFolder.Items.Add(@"\.git\");
            this.JogaiFolder.Items.Add(@"\bin\");
            this.JogaiFolder.Items.Add(@"\obj\");
        }
        #endregion

        #region ファイルのリスト
        /// <summary>
        /// ファイルのリスト
        /// </summary>
        ModelList<FileM> _FileList = new ModelList<FileM>();
        /// <summary>
        /// ファイルのリスト
        /// </summary>
        public ModelList<FileM> FileList
        {
            get
            {
                return _FileList;
            }
            set
            {
                if (_FileList == null || !_FileList.Equals(value))
                {
                    _FileList = value;
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
        public string CreateClassMarkdown()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("```mermaid");
            sb.AppendLine("classDiagram");
            sb.AppendLine("direction LR");

            Dictionary<string, List<string>> clsname_list = new Dictionary<string, List<string>>();
            foreach (var file in this.FileList.Items)
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

                    if (clsname_list.ContainsKey(cls.Name))
                        continue;

                    // クラス名とプロパティの型一覧を保持
                    clsname_list.Add(cls.Name, param_type_list);

                    foreach (var method in cls.MethodItems.Items)
                    {
                        string mtd = string.Empty;
                        if (method.Accessor == Common.Enums.AccessModifier.Public)
                        {
                            mtd = "+";
                        }
                        else if (method.Accessor == Common.Enums.AccessModifier.Protected)
                        {
                            mtd = "#";
                        }
                        else if (method.Accessor == Common.Enums.AccessModifier.Package)
                        {
                            mtd = "~";
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

    }
}
