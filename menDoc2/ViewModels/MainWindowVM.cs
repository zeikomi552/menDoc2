using menDoc2.Common;
using menDoc2.Models;
using menDoc2.Models.Class;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using MVVMCore.BaseClass;
using MVVMCore.Common.Utilities;
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
    public class MainWindowVM: WebViewPrevVM
    {
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

        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void Init(object sender, EventArgs e)
        {
            //base.Init(sender, e);
        }

        public override void Close(object sender, EventArgs e)
        {
            
        }

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

                            var lst = FileM.LoadCS(csfile);

                            if (lst == null)
                                continue;


                            foreach (var elem in lst)
                            {
                                file.ClassList.Items.Add(elem);
                            }
                            this.FileCollection.FileList.Items.Add(file);
                        }
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
