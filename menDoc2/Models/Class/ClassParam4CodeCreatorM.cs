using menDoc2.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace menDoc2.Models.Class
{
    public class ClassParam4CodeCreatorM : ClassParamM
    {
        public ClassParam4CodeCreatorM()
        {

        }

        public ClassParam4CodeCreatorM(ClassParamM cls)
        {
            this.ValueName = cls.ValueName;
            this.TypeName = cls.TypeName;
            this.Accessor = cls.Accessor;
            this.Description = cls.Description;
        }

        #region アクセス修飾子[Accessor]プロパティ
        /// <summary>
        /// アクセス修飾子[Accessor]プロパティ
        /// </summary>
        public override AccessModifier Accessor
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
                    NotifyPropertyChanged("PropertyCode");
                }
            }
        }
        #endregion

        #region 型名[TypeName]プロパティ
        /// <summary>
        /// 型名[TypeName]プロパティ
        /// </summary>
        public override string TypeName
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
                    NotifyPropertyChanged("PropertyCode");
                    InitialValueAutoSet();
                }
            }
        }
        #endregion

        #region 変数名[ValueName]プロパティ
        /// <summary>
        /// 変数名[ValueName]プロパティ
        /// </summary>
        public override string ValueName
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
                    NotifyPropertyChanged("PropertyCode");
                }
            }
        }
        #endregion

        #region 説明[Description]プロパティ
        /// <summary>
        /// 説明[Description]プロパティ
        /// </summary>
        public override string Description
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
                    NotifyPropertyChanged("PropertyCode");
                }
            }
        }
        #endregion

        #region 表示 or 非表示
        /// <summary>
        /// 表示 or 非表示
        /// </summary>
        bool _IsVisible = false;
        /// <summary>
        /// 表示 or 非表示
        /// </summary>
        public bool IsVisible
        {
            get
            {
                return _IsVisible;
            }
            set
            {
                if (!_IsVisible.Equals(value))
                {
                    _IsVisible = value;
                    NotifyPropertyChanged("IsVisible");
                    NotifyPropertyChanged("PropertyCode");
                }
            }
        }
        #endregion

        #region リスト or 非リスト
        /// <summary>
        /// リスト or 非リスト
        /// </summary>
        bool _IsList = false;
        /// <summary>
        /// リスト or 非リスト
        /// </summary>
        public bool IsList
        {
            get
            {
                return _IsList;
            }
            set
            {
                if (!_IsList.Equals(value))
                {
                    _IsList = value;
                    NotifyPropertyChanged("IsList");
                    NotifyPropertyChanged("PropertyCode");
                }
            }
        }
        #endregion

        #region Nullチェック
        /// <summary>
        /// Nullチェック
        /// </summary>
        bool _IsNullChk = false;
        /// <summary>
        /// Nullチェック
        /// </summary>
        public bool IsNullChk
        {
            get
            {
                return _IsNullChk;
            }
            set
            {
                if (!_IsNullChk.Equals(value))
                {
                    _IsNullChk = value;
                    NotifyPropertyChanged("IsNullChk");
                    NotifyPropertyChanged("PropertyCode");
                }
            }
        }
        #endregion

        #region 初期値[InitializeValue]プロパティ
        /// <summary>
        /// 初期値[InitializeValue]プロパティ用変数
        /// </summary>
        string _InitializeValue = string.Empty;
        /// <summary>
        /// 初期値[InitializeValue]プロパティ
        /// </summary>
        public string InitializeValue
        {
            get
            {
                return _InitializeValue;
            }
            set
            {
                if (_InitializeValue == null || !_InitializeValue.Equals(value))
                {
                    _InitializeValue = value;
                    NotifyPropertyChanged("InitializeValue");
                    NotifyPropertyChanged("PropertyCode");
                }
            }
        }
        #endregion

        #region プロパティのソースコード
        /// <summary>
        /// プロパティのソースコード
        /// </summary>
        public string PropertyCode
        {
            get
            {
                return CreateProperty();
            }
        }
        #endregion

        #region プロパティ作成関数
        /// <summary>
        /// プロパティ作成関数
        /// </summary>
        /// <returns>作成したプロパティ</returns>
        private string CreateProperty()
        {
            StringBuilder cmd = new StringBuilder();
            cmd.AppendLine($"	#region {Description}");
            cmd.AppendLine("	/// <summary>");
            cmd.AppendLine($"	/// {Description}");
            cmd.AppendLine("	/// </summary>");

            // リスト表示
            if (this.IsList)
            {
                cmd.AppendLine($"	ObservableCollection<{TypeName}> _{ValueName} = new ObservableCollection<{TypeName}>();");
            }
            else
            {
                cmd.AppendLine($"	{TypeName} _{ValueName} = {InitializeValue};");
            }

            cmd.AppendLine("	/// <summary>");
            cmd.AppendLine($"	/// {Description}");
            cmd.AppendLine("	/// </summary>");

            // リスト表示
            if (this.IsList)
            {
                cmd.AppendLine($"	public ObservableCollection<{TypeName}> {ValueName}");
            }
            else
            {
                cmd.AppendLine($"	public {TypeName} {ValueName}");
            }

            cmd.AppendLine("	{");
            cmd.AppendLine("		get");
            cmd.AppendLine("		{");
            cmd.AppendLine($"			return _{ValueName};");
            cmd.AppendLine("		}");
            cmd.AppendLine("		set");
            cmd.AppendLine("		{");

            // nullチェック
            if (this.IsNullChk)
            {
                cmd.AppendLine($"			if (_{ValueName} == null || !_{ValueName}.Equals(value))");
            }
            else
            {
                cmd.AppendLine($"			if (!_{ValueName}.Equals(value))");
            }

            cmd.AppendLine("			{");
            cmd.AppendLine($"				_{ValueName} = value;");
            cmd.AppendLine($"				NotifyPropertyChanged(\"{ValueName}\");");
            cmd.AppendLine("			}");
            cmd.AppendLine("		}");
            cmd.AppendLine("	}");
            cmd.AppendLine("	#endregion");
            return cmd.ToString();
        }
        #endregion

        #region 初期値の設定
        /// <summary>
        /// 初期値の設定
        /// </summary>
        public void InitialValueAutoSet()
        {
            string typename = this.TypeName.Trim();
            bool null_check = false;
            this.InitializeValue = GetInitialValue(this.TypeName, out null_check);
            this.IsNullChk = null_check;
        }
        #endregion


        #region 初期値の設定
        /// <summary>
        /// 初期値の設定
        /// </summary>
        /// <param name="type_name">型名</param>
        /// <param name="null_check">nullチェックが必要かどうか</param>
        /// <returns>初期化の際のコード</returns>
        public static string GetInitialValue(string type_name, out bool null_check)
        {
            string kata = type_name.Trim().ToLower();
            null_check = false;

            if (kata.Length > 0)
            {
                if (kata.Substring(kata.Length - 1).Equals("?"))
                {
                    null_check = true;
                    return "null";
                }
                else if (kata.Equals("int"))
                {
                    return "0";
                }
                else if (kata.Equals("double") || kata.Equals("float"))
                {
                    return "0.0";
                }
                else if (kata.Equals("bool"))
                {
                    return "false";
                }
                else if (kata.Equals("string"))
                {
                    null_check = true;
                    return "string.Empty";
                }
                else if (kata.Equals("datetime"))
                {
                    return "DateTime.MinValue";
                }
                else
                {
                    null_check = true;
                    return string.Format("new {0}()", type_name.Trim());
                }
            }
            else
            {
                return string.Empty;
            }

        }
        #endregion
    }
}
