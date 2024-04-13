using menDoc2.Common.Enums;
using MVVMCore.BaseClass;

namespace menDoc2.Models.Class
{
    /// <summary>
    /// クラス用パラメーター
    /// </summary>
    public class ClassParamM : ModelBase
    {
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

        #region 型名[TypeName]プロパティ
        /// <summary>
        /// 型名[TypeName]プロパティ
        /// </summary>
        string _TypeName = string.Empty;
        /// <summary>
        /// 型名[TypeName]プロパティ
        /// </summary>
        public string TypeName
        {
            get
            {
                return _TypeName;
            }
            set
            {
                if (_TypeName == null || !_TypeName.Equals(value))
                {
                    _TypeName = value;
                    NotifyPropertyChanged("TypeName");
                }
            }
        }
        #endregion

        #region 変数名[ValueName]プロパティ
        /// <summary>
        /// 変数名[ValueName]プロパティ
        /// </summary>
        string _ValueName = string.Empty;
        /// <summary>
        /// 変数名[ValueName]プロパティ
        /// </summary>
        public string ValueName
        {
            get
            {
                return _ValueName;
            }
            set
            {
                if (_ValueName == null || !_ValueName.Equals(value))
                {
                    _ValueName = value;
                    NotifyPropertyChanged("ValueName");
                }
            }
        }
        #endregion

        #region 説明[Description]プロパティ
        /// <summary>
        /// 説明[Description]プロパティ
        /// </summary>
        string _Description = string.Empty;
        /// <summary>
        /// 説明[Description]プロパティ
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
    }
}
