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
    }
}
