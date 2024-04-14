using Markdig;
using menDoc2.Common;
using menDoc2.Models;
using menDoc2.Models.Class;
using menDoc2.Views.UserControls;
using Microsoft.Web.WebView2.WinForms;
using Microsoft.Web.WebView2.Wpf;
using MVVMCore.BaseClass;
using MVVMCore.Common.Utilities;
using MVVMCore.Common.Wrapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebView2 = Microsoft.Web.WebView2.Wpf.WebView2;

namespace menDoc2.ViewModels.UserControls
{
    /// <summary>
    /// ファイル情報表示画面用ViewModel
    /// </summary>
    public class ucFileVM : ucBaseVM
    {
        #region HTMLファイルの出力先ファイル名
        /// <summary>
        /// HTMLファイルの出力先ファイル名
        /// </summary>
        protected override string OutFilename
        {
            get { return "filelist.html"; }
        }
        #endregion

        #region マークダウンの作成
        /// <summary>
        /// マークダウンの作成
        /// </summary>
        protected override string CreateMarkdown()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("## ファイル一覧");
                sb.AppendLine($"- 作成日:{DateTime.Now.ToString("yyyy/MM/dd")}");
                sb.AppendLine($"- 作成者:{Environment.UserName}");
                sb.AppendLine($"");


                sb.AppendLine("|ファイル名|作成日時|更新日時|サイズ(Byte)|");
                sb.AppendLine("|---|---|---|---|");
                foreach (var tmp in this.FileCollection.FileList.Items)
                {
                    //FileInfoを生成する
                    string filename = Path.GetFileName(tmp.FilePath.ToString());
                    var datetm = File.GetCreationTime(tmp.FilePath.ToString());
                    var renewdt = File.GetLastWriteTime(tmp.FilePath.ToString());

                    System.IO.FileInfo fi = new System.IO.FileInfo(tmp.FilePath.ToString());

                    sb.AppendLine($"|{filename}|{datetm.ToString("yyyy/MM/dd HH:mm:ss")}|{renewdt.ToString("yyyy/MM/dd HH:mm:ss")}|{fi.Length}|");
                }

                return sb.ToString();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                ShowMessage.ShowErrorOK(ex.Message, "Error");
                return string.Empty;
            }
        }
        #endregion
    }
}
