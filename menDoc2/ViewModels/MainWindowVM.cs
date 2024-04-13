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
    public class MainWindowVM: ViewModelBase
    {
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
                    NotifyPropertyChanged("FileList");
                }
            }
        }
        #endregion


        public override void Init(object sender, EventArgs e)
        {
            
        }

        public override void Close(object sender, EventArgs e)
        {
            
        }


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


                    var list = GetFileList(cofd.FileName, "*.cs");

                    foreach (var csfile in list)
                    {
                        FileM file = new FileM();
                        file.FileName = csfile;

                        ClassM cls = new ClassM();
                        var lst = cls.LoadCS(csfile);

                        if (lst == null)
                            continue;


                        foreach (var elem in lst)
                        {
                            file.ClassList.Items.Add(elem);
                        }

                        this.FileList.Items.Add(file);
                    }
                }
            }
            catch (Exception e)
            {

            }
        }

        public void SelectionChanged()
        {
            try
            {
                this.FileList.SelectedItem.ClassList.SelectedFirst();

                if (this.FileList.SelectedItem != null 
                    && this.FileList.SelectedItem.ClassList != null
                    && this.FileList.SelectedItem.ClassList.SelectedItem != null 
                    && this.FileList.SelectedItem.ClassList.SelectedItem.ParameterItems != null)
                {
                    if (this.FileList.SelectedItem.ClassList.SelectedItem.ParameterItems != null)
                    {
                        this.FileList.SelectedItem.ClassList.SelectedItem.ParameterItems!.SelectedFirst();
                    }
                    if (this.FileList.SelectedItem.ClassList.SelectedItem.MethodItems != null)
                    {
                        this.FileList.SelectedItem.ClassList.SelectedItem.MethodItems!.SelectedFirst();
                    }
                }
            }
            catch (Exception e)
            {

            }
        }


        List<string> GetFileList(string dir, string pattern)
        {
            // フォルダ内のファイル一覧を取得
            var fileArray = Directory.GetFiles(dir, pattern, SearchOption.AllDirectories);
            return fileArray.ToList();
        }
    }
}
