using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using MVVMCore.BaseClass;
using MVVMCore.Common.Utilities;
using System.Text.RegularExpressions;
using System.Security.Cryptography.Xml;

namespace menDoc2.Models.Class
{
    public class ClassM : ModelBase
    {
        #region クラス名[Name]プロパティ
        /// <summary>
        /// クラス名[Name]プロパティ
        /// </summary>
        string _Name = string.Empty;
        /// <summary>
        /// クラス名[Name]プロパティ
        /// </summary>
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (_Name == null || !_Name.Equals(value))
                {
                    _Name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }
        #endregion

        #region クラスの説明[Description]プロパティ
        /// <summary>
        /// クラスの説明[Description]プロパティ
        /// </summary>
        string _Description = string.Empty;
        /// <summary>
        /// クラスの説明[Description]プロパティ
        /// </summary>
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                if (_Description == null || !_Description.Equals(value))
                {
                    _Description = value;
                    NotifyPropertyChanged("Description");
                }
            }
        }
        #endregion

        #region 作成日時[CreateDate]プロパティ
        /// <summary>
        /// 作成日時[CreateDate]プロパティ
        /// </summary>
        DateTime _CreateDate = DateTime.MinValue;
        /// <summary>
        /// 作成日時[CreateDate]プロパティ
        /// </summary>
        public DateTime CreateDate
        {
            get
            {
                return _CreateDate;
            }
            set
            {
                if (!_CreateDate.Equals(value))
                {
                    _CreateDate = value;
                    NotifyPropertyChanged("CreateDate");
                }
            }
        }
        #endregion

        #region 作成者[CreateUser]プロパティ
        /// <summary>
        /// 作成者[CreateUser]プロパティ
        /// </summary>
        string _CreateUser = string.Empty;
        /// <summary>
        /// 作成者[CreateUser]プロパティ
        /// </summary>
        public string CreateUser
        {
            get
            {
                return _CreateUser;
            }
            set
            {
                if (_CreateUser == null || !_CreateUser.Equals(value))
                {
                    _CreateUser = value;
                    NotifyPropertyChanged("CreateUser");
                }
            }
        }
        #endregion

        #region 変数リスト[ParameterItems]プロパティ
        /// <summary>
        /// 変数リスト[ParameterItems]プロパティ
        /// </summary>
        ModelList<ClassParamM>? _ParameterItems = null;
        /// <summary>
        /// 変数リスト[ParameterItems]プロパティ
        /// </summary>
        public ModelList<ClassParamM>? ParameterItems
        {
            get
            {
                return _ParameterItems;
            }
            set
            {
                if (_ParameterItems == null || !_ParameterItems.Equals(value))
                {
                    _ParameterItems = value;
                    NotifyPropertyChanged("ParameterItems");
                }
            }
        }
        #endregion

        #region 関数リスト[MethodItems]プロパティ
        /// <summary>
        /// 関数リスト[MethodItems]プロパティ
        /// </summary>
        ModelList<ClassMethodM>? _MethodItems = null;
        /// <summary>
        /// 関数リスト[MethodItems]プロパティ
        /// </summary>
        public ModelList<ClassMethodM>? MethodItems
        {
            get
            {
                return _MethodItems;
            }
            set
            {
                if (_MethodItems == null || !_MethodItems.Equals(value))
                {
                    _MethodItems = value;
                    NotifyPropertyChanged("MethodItems");
                }
            }
        }
        #endregion

        #region 読込処理
        /// <summary>
        /// 読込処理
        /// </summary>
        public List<ClassM>? LoadCS(string filename)
        {
            try
            {
                List<ClassM> classm = new List<ClassM>();
                var text = System.IO.File.ReadAllText(filename);
                var tree = CSharpSyntaxTree.ParseText(text);
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
                    // コメント抜き出し
                    var match = Regex.Match(trivia, @"<summary>[\s\S]*?</summary>");
                    // 不要な文字列削除
                    var comment = match.Value.Replace("<summary>", "").Replace("</summary>", "").Replace("\r\n", "").Replace("///", "").Replace(" ", "").Replace("\t", "");
                    cls_tmp.Description = comment;

                    var properties = cls.DescendantNodes().OfType<PropertyDeclarationSyntax>();

                    List<ClassParamM> cls_params = new List<ClassParamM>();
                    foreach (var field in properties)
                    {
                        ClassParamM param = new ClassParamM();
                        trivia = field.GetLeadingTrivia().ToString();

                        // コメント抜き出し
                        match = Regex.Match(trivia, @"<summary>[\s\S]*?</summary>");
                        // 不要な文字列削除
                        comment = match.Value.Replace("<summary>", "").Replace("</summary>", "").Replace("\r\n", "").Replace("///", "").Replace(" ", "").Replace("\t", "");
                        var type = field.Type.ToString();
                        var value = field.Identifier.Text;


                        param.TypeName = type;
                        param.ValueName = value;
                        param.Description = comment;
                        cls_params.Add(param);
                    }
                    cls_tmp.ParameterItems = new ModelList<ClassParamM>(cls_params);

                    List<ClassMethodM> cls_methods = new List<ClassMethodM>();
                    foreach (var method in methods)
                    {
                        ClassMethodM clsmethod = new ClassMethodM();
                        trivia = method.GetLeadingTrivia().ToString();
                        // コメント抜き出し
                        match = Regex.Match(trivia, @"<summary>[\s\S]*?</summary>");
                        // 不要な文字列削除
                        comment = match.Value.Replace("<summary>", "").Replace("</summary>", "").Replace("\r\n", "").Replace("///", "").Replace(" ", "").Replace("\t", "");
                        clsmethod.Description = comment;
                        clsmethod.MethodName = method.Identifier.Text;
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
    }
}
