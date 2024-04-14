using menDoc2.Models.Class;
using MVVMCore.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace menDoc2.Common
{
    public sealed class GblValues
    {
        private static GblValues _Instance = new GblValues();

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        private GblValues()
        {

        }
        #endregion

        #region インスタンス
        /// <summary>
        /// インスタンス
        /// </summary>
        public static GblValues Instance
        {
            get
            {
                return _Instance;
            }
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


    }
}
