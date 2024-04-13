using menDoc2.Common.Enums;
using MVVMCore.BaseClass;
using MVVMCore.Common.Utilities;

namespace menDoc2.Models.Class
{
    public class ClassMethodM : ModelBase
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

        #region 戻り値[ReturnValue]プロパティ
        /// <summary>
        /// 戻り値[ReturnValue]プロパティ
        /// </summary>
        string _ReturnValue = string.Empty;
        /// <summary>
        /// 戻り値[ReturnValue]プロパティ
        /// </summary>
        public string ReturnValue
        {
            get
            {
                return _ReturnValue;
            }
            set
            {
                if (_ReturnValue == null || !_ReturnValue.Equals(value))
                {
                    _ReturnValue = value;
                    NotifyPropertyChanged("ReturnValue");
                }
            }
        }
        #endregion

        #region 関数名[MethodName]プロパティ
        /// <summary>
        /// 関数名[MethodName]プロパティ
        /// </summary>
        string _MethodName = string.Empty;
        /// <summary>
        /// 関数名[MethodName]プロパティ
        /// </summary>
        public string MethodName
        {
            get
            {
                return _MethodName;
            }
            set
            {
                if (_MethodName == null || !_MethodName.Equals(value))
                {
                    _MethodName = value;
                    NotifyPropertyChanged("MethodName");
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

        #region 引数
        /// <summary>
        /// 引数
        /// </summary>
        ModelList<ClassParamM>? _Arguments = null;
        /// <summary>
        /// 引数
        /// </summary>
        public ModelList<ClassParamM>? Arguments
        {
            get
            {
                return _Arguments;
            }
            set
            {
                if (_Arguments == null || !_Arguments.Equals(value))
                {
                    _Arguments = value;
                    NotifyPropertyChanged("Arguments");
                }
            }
        }
        #endregion


    }
}
