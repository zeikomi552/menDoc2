using menDoc2.Models.Class;
using MVVMCore.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace menDoc2.ViewModels.UserControls
{
    public class ucParameterVM : ucBaseVM
    {
        #region パラメーターリスト
        /// <summary>
        /// パラメーターリスト
        /// </summary>
        ModelList<ClassParam4CodeCreatorM> _Parameters = new ModelList<ClassParam4CodeCreatorM>();
        /// <summary>
        /// パラメーターリスト
        /// </summary>
        public ModelList<ClassParam4CodeCreatorM> Parameters
        {
            get
            {
                return _Parameters;
            }
            set
            {
                if (_Parameters == null || !_Parameters.Equals(value))
                {
                    _Parameters = value;
                    NotifyPropertyChanged("Parameters");
                }
            }
        }
        #endregion

        #region クラスの表示/非表示
        /// <summary>
        /// クラスの表示/非表示
        /// </summary>
        bool _IsClassVisible = false;
        /// <summary>
        /// クラスの表示/非表示
        /// </summary>
        public bool IsClassVisible
        {
            get
            {
                return _IsClassVisible;
            }
            set
            {
                if (!_IsClassVisible.Equals(value))
                {
                    _IsClassVisible = value;
                    NotifyPropertyChanged("IsClassVisible");
                }
            }
        }
        #endregion

        #region ソースコード
        /// <summary>
        /// ソースコード
        /// </summary>
        public string SourceCode
        {
            get
            {
                return RefreshCode();
            }
        }
        #endregion

        #region ソースコードの更新イベント通知処理
        /// <summary>
        /// ソースコードの更新イベント通知処理
        /// </summary>
        public void ExecRefreshCode()
        {
            RefreshCode();
            NotifyPropertyChanged("SourceCode");
        }
        #endregion

        #region コードの更新
        /// <summary>
        /// コードの更新
        /// </summary>
        public string RefreshCode()
        {
            StringBuilder code = new StringBuilder();
            string clsname = string.Empty;

            if (this.FileCollection.FileList.SelectedItem == null
                || this.FileCollection.FileList.SelectedItem.ClassList.SelectedItem == null)
            {
                ;
            }
            else
            {
                clsname = this.FileCollection.FileList.SelectedItem.ClassList.SelectedItem.Name;
            }

            if (this.IsClassVisible)
            {
                code.AppendLine($"public class {clsname} : INotifyPropertyChanged");
                code.AppendLine($"{{");
            }

            foreach (var tmp in this.Parameters)
            {
                if (tmp.IsVisible)
                {
                    code.AppendLine(tmp.PropertyCode);
                }
            }

            if (this.IsClassVisible)
            {
                code.AppendLine("	#region INotifyPropertyChanged");
                code.AppendLine("	public event PropertyChangedEventHandler? PropertyChanged;");
                code.AppendLine("");
                code.AppendLine("	private void NotifyPropertyChanged(String info)");
                code.AppendLine("	{");
                code.AppendLine("		if (PropertyChanged != null)");
                code.AppendLine("		{");
                code.AppendLine("			PropertyChanged(this, new PropertyChangedEventArgs(info));");
                code.AppendLine("		}");
                code.AppendLine("	}");
                code.AppendLine("	#endregion");
                code.AppendLine($"}}");
            }
            return code.ToString();
        }
        #endregion

        #region マークダウンの作成処理
        /// <summary>
        /// マークダウンの作成処理
        /// </summary>
        /// <returns></returns>
        protected override string CreateMarkdown()
        {
            return string.Empty;
        }
        #endregion

        #region クラスの切替処理
        /// <summary>
        /// クラスの切替処理
        /// </summary>
        public void ClassChanged()
        {
            try
            {
                this.Parameters.Items.Clear();

                if (FileCollection.FileList == null || FileCollection.FileList.SelectedItem == null
                    || FileCollection.FileList.SelectedItem.ClassList == null || FileCollection.FileList.SelectedItem.ClassList.SelectedItem == null
                    || FileCollection.FileList.SelectedItem.ClassList.SelectedItem.ParameterItems == null
                    )
                {
                    return;
                }


                foreach (var tmp in FileCollection.FileList.SelectedItem.ClassList.SelectedItem.ParameterItems.Items)
                {
                    ClassParam4CodeCreatorM cls = new ClassParam4CodeCreatorM(tmp);
                    this.Parameters.Items.Add(cls);
                }
            }
            catch
            {

            }
        }
        #endregion

        #region ファイル名の切替処理
        /// <summary>
        /// ファイル名の切替処理
        /// </summary>
        public void FileChanged()
        {
            try
            {
                this.Parameters.Items.Clear();

                if (FileCollection.FileList == null || FileCollection.FileList.SelectedItem == null)
                {
                    return;
                }

                FileCollection.FileList.SelectedItem.ClassList.SelectedFirst();
            }
            catch
            {

            }
        }
        #endregion

        public void AllCheck()
        {
            try
            {
                foreach (var prop in Parameters.Items)
                {
                    prop.IsVisible = true;
                }
            }
            catch
            {

            }
        }

        public void AllUncheck()
        {
            foreach (var prop in Parameters.Items)
            {
                prop.IsVisible = false;
            }
        }
    }
}
