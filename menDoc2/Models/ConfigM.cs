using MVVMCore.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace menDoc2.Models
{
    public class ConfigM : ModelBase
    {
        #region テンプレートディレクトリパス
        /// <summary>
        /// テンプレートディレクトリパス
        /// </summary>
        string _TempleteDir = string.Empty;
        /// <summary>
        /// テンプレートディレクトリパス
        /// </summary>
        public string TempleteDir
        {
            get
            {
                return _TempleteDir;
            }
            set
            {
                if (_TempleteDir == null || !_TempleteDir.Equals(value))
                {
                    _TempleteDir = value;
                    NotifyPropertyChanged("TempleteDir");
                }
            }
        }
        #endregion


    }
}
