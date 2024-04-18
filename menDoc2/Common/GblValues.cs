﻿using menDoc2.Models.Class;
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

        #region ファイル情報一式
        /// <summary>
        /// ファイル情報一式
        /// </summary>
        FileCollectionM _FileCollection = new FileCollectionM();
        /// <summary>
        /// ファイル情報一式
        /// </summary>
        public FileCollectionM FileCollection
        {
            get
            {
                return _FileCollection;
            }
            set
            {
                if (_FileCollection == null || !_FileCollection.Equals(value))
                {
                    _FileCollection = value;
                }
            }
        }
        #endregion
    }
}
