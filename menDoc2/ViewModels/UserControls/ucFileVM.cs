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
        #region 一時ファイルのURI
        /// <summary>
        /// 一時ファイルのURI
        /// </summary>
        public Uri TmpURI
        {
            get
            {
                return new Uri(DisplayWebManagerM.GetDisplayHtmlPath("filelist.html"));
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
        protected override string CreateMarkdown()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("## ファイル一覧");

                foreach (var tmp in this.FileCollection.FileList.Items)
                {
                    sb.AppendLine(Path.GetFileName(tmp.FilePath.ToString()));
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
