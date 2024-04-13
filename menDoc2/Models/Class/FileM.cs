using MVVMCore.BaseClass;
using MVVMCore.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace menDoc2.Models.Class
{
    public class FileM : ModelBase
    {
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
    }
}
