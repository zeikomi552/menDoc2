using menDoc2.Common;
using menDoc2.Models;
using menDoc2.Models.Class;
using menDoc2.ViewModels.UserControls;
using menDoc2.Views.UserControls;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using MVVMCore.BaseClass;
using MVVMCore.Common.Utilities;
using MVVMCore.Common.Wrapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace menDoc2.ViewModels
{
    /// <summary>
    /// メインウィンドウ用ViewModel
    /// </summary>
    public class MainWindowVM: ViewModelBase
    {
        #region ロガー
        /// <summary>
        /// ロガー
        /// </summary>
        protected static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()!.DeclaringType);
        #endregion

        #region 選択しているタブ[SelectedTab]プロパティ
        /// <summary>
        /// 選択しているタブ[SelectedTab]プロパティ用変数
        /// </summary>
        int _SelectedTab = 0;
        /// <summary>
        /// 選択しているタブ[SelectedTab]プロパティ
        /// </summary>
        public int SelectedTab
        {
            get
            {
                return _SelectedTab;
            }
            set
            {
                if (!_SelectedTab.Equals(value))
                {
                    _SelectedTab = value;
                    NotifyPropertyChanged("SelectedTab");
                }
            }
        }
        #endregion
        #region ファイル情報一式
        /// <summary>
        /// ファイル情報一式
        /// </summary>
        public FileCollectionM FileCollection
        {
            get
            {
                return GblValues.Instance.FileCollection;
            }
            set
            {
                if (GblValues.Instance.FileCollection == null || !GblValues.Instance.FileCollection.Equals(value))
                {
                    GblValues.Instance.FileCollection = value;
                    NotifyPropertyChanged("FileCollection");
                }
            }
        }
        #endregion

        #region クラス図マークダウン
        /// <summary>
        /// クラス図マークダウン
        /// </summary>
        string _ClassDialgram = string.Empty;
        /// <summary>
        /// クラス図マークダウン
        /// </summary>
        public string ClassDialgram
        {
            get
            {
                return _ClassDialgram;
            }
            set
            {
                if (_ClassDialgram == null || !_ClassDialgram.Equals(value))
                {
                    _ClassDialgram = value;
                    NotifyPropertyChanged("ClassDialgram");
                }
            }
        }
        #endregion

        MainWindow? _Wnd = null;

        #region 初期化処理
        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void Init(object sender, EventArgs e)
        {
            try
            {
                _Wnd = VisualTreeHelperWrapper.GetWindow<MainWindow>(sender) as MainWindow;

            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        #region 画面を閉じる処理
        /// <summary>
        /// 画面を閉じる処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void Close(object sender, EventArgs e)
        {
            
        }
        #endregion

        #region フォルダの選択処理
        /// <summary>
        /// フォルダの選択処理
        /// </summary>
        public void SelectDirectory()
        {
            try
            {
                using (var cofd = new CommonOpenFileDialog()
                {
                    Title = "フォルダを選択してください",
                    InitialDirectory = @"",
                    // フォルダ選択モードにする
                    IsFolderPicker = true,
                })
                {
                    if (cofd.ShowDialog() != CommonFileDialogResult.Ok)
                    {
                        return;
                    }


                    var list = GetFileList(cofd.FileName, "*.*");

                    this.FileCollection.FileList.Items.Clear();
                    foreach (var csfile in list)
                    {
                        FileM file = new FileM();
                        
                        bool jogai_f = false;
                        foreach (var jogai in this.FileCollection.JogaiFolder.Items)
                        {
                            if (csfile.Contains(jogai))
                            {
                                jogai_f = true; 
                                break;
                            }
                        }

                        if (!jogai_f)
                        {
                            file.FilePath = csfile;
                            this.FileCollection.FileList.Items.Add(file);

                            // .csファイルのみを確保する変数にセット
                            if (Path.GetExtension(csfile).ToLower().Equals(".cs"))
                            {
                                GblValues.Instance.FileCollectionCSOnly.FileList.Items.Add(file);
                            }


                            var lst = FileM.LoadCS(csfile);

                            if (lst == null || lst.Count <= 0)
                                continue;

                            foreach (var elem in lst)
                            {
                                if(string.IsNullOrEmpty(elem.Name))
                                    continue;

                                file.ClassList.Items.Add(elem);
                            }


                        }
                    }

                    // 各画面のリフレッシュ
                    Refresh();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        #region 選択行が変化した場合の処理
        /// <summary>
        /// 選択行が変化した場合の処理
        /// </summary>
        public void SelectionChanged()
        {
            try
            {
                var filelist = GblValues.Instance.FileCollection.FileList;
                filelist.SelectedItem.ClassList.SelectedFirst();

                if (filelist.SelectedItem != null 
                    && filelist.SelectedItem.ClassList != null
                    && filelist.SelectedItem.ClassList.SelectedItem != null 
                    && filelist.SelectedItem.ClassList.SelectedItem.ParameterItems != null)
                {
                    if (filelist.SelectedItem.ClassList.SelectedItem.ParameterItems != null)
                    {
                        filelist.SelectedItem.ClassList.SelectedItem.ParameterItems!.SelectedFirst();
                    }
                    if (filelist.SelectedItem.ClassList.SelectedItem.MethodItems != null)
                    {
                        filelist.SelectedItem.ClassList.SelectedItem.MethodItems!.SelectedFirst();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        public void Refresh()
        {
            try
            {

                for (int i = 0; i < 3; i++)
                {
                    this.SelectedTab = i;
                }
                this.SelectedTab = 0;

                var vm = this._Wnd!.filev.DataContext as ucFileVM;
                //vm.WebviewObject = this._Wnd!.filev.wv2;
                vm!.Reload();

                var vm2 = this._Wnd!.classdiagramv.DataContext as ucClassDiagramVM;
                //vm2.WebviewObject = this._Wnd!.classdiagramv.wv2;
                vm2!.Reload();

                var vm3 = this._Wnd!.classdetailv.DataContext as ucClassVM;
                //vm3.WebviewObject = this._Wnd!.classdetailv.wv2;
                vm3!.Reload();

                var vm4 = this._Wnd!.parameterv.DataContext as ucParameterVM;
                //vm4.WebviewObject = this._Wnd!.parameterv.wv2;
                vm4!.Reload();

            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }

        #region ファイルリスト一覧取得
        /// <summary>
        /// ファイルリスト一覧取得
        /// </summary>
        /// <param name="dir">ディレクトリ</param>
        /// <param name="pattern">ファイルパターン</param>
        /// <returns>ファイルリスト一覧</returns>
        List<string> GetFileList(string dir, string pattern)
        {
            // フォルダ内のファイル一覧を取得
            var fileArray = Directory.GetFiles(dir, pattern, SearchOption.AllDirectories);
            return fileArray.ToList();
        }
        #endregion
    }
}
