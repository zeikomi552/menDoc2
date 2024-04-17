using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using MVVMCore.BaseClass;
using MVVMCore.Common.Utilities;
using System.Text.RegularExpressions;
using System.Security.Cryptography.Xml;
using Microsoft.CodeAnalysis;
using menDoc2.Common.Enums;
using System.Collections.ObjectModel;

namespace menDoc2.Models.Class
{
    public class ClassM : ModelBase
    {
        #region 親クラス
        /// <summary>
        /// 親クラス
        /// </summary>
        ObservableCollection<string> _BaseClass = new ObservableCollection<string>();
        /// <summary>
        /// 親クラス
        /// </summary>
        public ObservableCollection<string> BaseClass
        {
            get
            {
                return _BaseClass;
            }
            set
            {
                if (_BaseClass == null || !_BaseClass.Equals(value))
                {
                    _BaseClass = value;
                    NotifyPropertyChanged("BaseClass");
                }
            }
        }
        #endregion



        #region アクセス修飾子[Accessor]プロパティ
        /// <summary>
        /// アクセス修飾子[Accessor]プロパティ
        /// </summary>
        AccessModifier _Accessor = AccessModifier.Public;
        /// <summary>
        /// アクセス修飾子[Accessor]プロパティ
        /// </summary>
        public AccessModifier Accessor
        {
            get
            {
                return _Accessor;
            }
            set
            {
                if (!_Accessor.Equals(value))
                {
                    _Accessor = value;
                    NotifyPropertyChanged("Accessor");
                }
            }
        }
        #endregion

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
        ModelList<ClassParamM> _ParameterItems = new ModelList<ClassParamM>();
        /// <summary>
        /// 変数リスト[ParameterItems]プロパティ
        /// </summary>
        public ModelList<ClassParamM> ParameterItems
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
        ModelList<ClassMethodM> _MethodItems = new ModelList<ClassMethodM>();
        /// <summary>
        /// 関数リスト[MethodItems]プロパティ
        /// </summary>
        public ModelList<ClassMethodM> MethodItems
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

        #region 文法ツリー
        /// <summary>
        /// 文法ツリー
        /// </summary>
        SyntaxTree? _SynTaxTree;
        /// <summary>
        /// 文法ツリー
        /// </summary>
        public SyntaxTree? SynTaxTree
        {
            get
            {
                return _SynTaxTree;
            }
            set
            {
                if (_SynTaxTree == null || !_SynTaxTree.Equals(value))
                {
                    _SynTaxTree = value;
                    NotifyPropertyChanged("SynTaxTree");
                }
            }
        }
        #endregion



    }
}
