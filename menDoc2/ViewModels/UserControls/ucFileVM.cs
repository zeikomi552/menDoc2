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
        #region ファイルのリスト
        /// <summary>
        /// ファイルのリスト
        /// </summary>
        public ModelList<FileM> FileList
        {
            get
            {
                return GblValues.Instance.FileList;
            }
            set
            {
                if (GblValues.Instance.FileList == null || !GblValues.Instance.FileList.Equals(value))
                {
                    GblValues.Instance.FileList = value;
                    NotifyPropertyChanged("FileList");
                }
            }
        }
        #endregion

        #region マークダウン文字列
        /// <summary>
        /// マークダウン文字列
        /// </summary>
        string _Markdown = string.Empty;
        /// <summary>
        /// マークダウン文字列
        /// </summary>
        public string Markdown
        {
            get
            {
                return _Markdown;
            }
            set
            {
                if (_Markdown == null || !_Markdown.Equals(value))
                {
                    _Markdown = value;
                    NotifyPropertyChanged("Markdown");
                }
            }
        }
        #endregion

        #region HTML文字列
        /// <summary>
        /// HTML文字列
        /// </summary>
        string _Html = string.Empty;
        /// <summary>
        /// HTML文字列
        /// </summary>
        public string Html
        {
            get
            {
                return _Html;
            }
            set
            {
                if (_Html == null || !_Html.Equals(value))
                {
                    _Html = value;
                    NotifyPropertyChanged("Html");
                }
            }
        }
        #endregion

        #region 一時ファイル名
        /// <summary>
        /// 一時ファイル名
        /// </summary>
        public const string TmploraryFileName = "FileInformationHtml.html";
        #endregion

        #region 一時ファイルのパス
        /// <summary>
        /// 一時ファイルのパス
        /// </summary>
        public static string TmploraryFilePath
        {
            get
            {
                var fv = FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
                var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), fv.CompanyName!, fv.ProductName!, @"Temporary");
                return Path.Combine(dir, TmploraryFileName);
            }
        }
        #endregion

        #region 一時ファイルのURI
        /// <summary>
        /// 一時ファイルのURI
        /// </summary>
        public Uri TmpURI
        {
            get
            {
                return new Uri(DisplayWebManagerM.DisplayHtmlPath);
            }
        }
        #endregion

        #region WebView2用オブジェクト[WebviewObject]プロパティ
        /// <summary>
        /// WebView2用オブジェクト[WebviewObject]プロパティ用変数
        /// </summary>
        WebView2? _WebviewObject = null;
        /// <summary>
        /// WebView2用オブジェクト[WebviewObject]プロパティ
        /// </summary>
        public WebView2? WebviewObject
        {
            get
            {
                return _WebviewObject;
            }
            set
            {
                if (_WebviewObject == null || !_WebviewObject.Equals(value))
                {
                    _WebviewObject = value;
                }
            }
        }
        #endregion

        #region 画面初期化処理
        /// <summary>
        /// 画面初期化処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void Init(object sender, EventArgs e)
        {
            try
            {
                var tmp = VisualTreeHelperWrapper.GetWindow<ucFileV>(sender) as ucFileV;

                if (tmp != null)
                {
                    SetWebviewObject(tmp.wv2);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        #region WebViewObjectのセット
        /// <summary>
        /// WebViewObjectのセット
        /// </summary>
        /// <param name="webview"></param>
        async public void SetWebviewObject(WebView2 webview)
        {
            try
            {
                if (this.WebviewObject == null)
                {
                    this.WebviewObject = webview;
                    await webview.EnsureCoreWebView2Async(null);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }

        }
        #endregion

        #region リロード処理
        /// <summary>
        /// リロード処理
        /// </summary>
        public void WebViewReload()
        {
            if (this.WebviewObject != null)
            {
                this.WebviewObject.Reload();
                NotifyPropertyChanged("TmpURI");
            }
        }
        #endregion

        #region マークダウンの作成
        /// <summary>
        /// マークダウンの作成
        /// </summary>
        public void CreateMarkdown()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("## ファイル一覧");

                foreach (var tmp in this.FileList.Items)
                {
                    sb.AppendLine(Path.GetFileName(tmp.FileName.ToString()));
                }
                this.Markdown = sb.ToString();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                ShowMessage.ShowErrorOK(ex.Message, "Error");
                this.Markdown = string.Empty;
            }
        }
        #endregion

        #region HTML文の作成
        /// <summary>
        /// HTML文の作成
        /// </summary>
        public void ConvetHtml()
        {
            try
            {
                var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
                this.Html = Markdig.Markdown.ToHtml(this.Markdown, pipeline);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                ShowMessage.ShowErrorOK(ex.Message, "Error");
                this.Html = string.Empty;
            }

        }
        #endregion



        //#region テンポラリデータの保存処理
        ///// <summary>
        ///// テンポラリデータの保存処理
        ///// </summary>
        ///// <returns>保存処理</returns>
        //public string SaveTemporary()
        //{
        //    try
        //    {
        //        // UTF-8
        //        StreamReader html_sr = new StreamReader(ClassDiagramPath.OutputHtmlTmpletePath, Encoding.UTF8);
        //        string tmp = ClassDiagramPath.OutputHtmlTmpletePath;
        //        // テンプレートファイル読み出し
        //        string html_txt = html_sr.ReadToEnd();

        //        html_txt = html_txt.Replace("{menDoc:jsdir}", Utilities.JSDir);
        //        html_txt = html_txt.Replace("{menDoc:htmlbody}", this.Html);
        //        File.WriteAllText(ClassDiagramPath.TmploraryFilePath, html_txt);

        //        // 一時フォルダの作成
        //        Utilities.CreateTemporaryDir();

        //        return ClassDiagramPath.TmploraryFilePath;
        //    }
        //    catch (Exception e)
        //    {
        //        ShowMessage.ShowErrorOK(e.Message, "Error");
        //        return string.Empty;
        //    }
        //}
        //#endregion
    }
}
