using Markdig;
using menDoc2.Common;
using menDoc2.Models;
using menDoc2.Models.Class;
using menDoc2.Views.UserControls;
using Microsoft.Web.WebView2.Wpf;
using Microsoft.Win32;
using MVVMCore.BaseClass;
using MVVMCore.Common.Utilities;
using MVVMCore.Common.Wrapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace menDoc2.ViewModels.UserControls
{
    public abstract class ucBaseVM : ViewModelBase
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ucBaseVM()
        {

        }
        #endregion

        #region ベースとなるHTMLコードの保存場所
        /// <summary>
        /// ベースとなるHTMLコードの保存場所
        /// </summary>
        public static string BaseHtml { get; set; } = @".\Common\Templete\HtmlCode\BaseHtml.mdtmpl";
        #endregion

        #region mermaid用JavaScriptのフォルダパス
        /// <summary>
        /// mermaid用JavaScriptのフォルダパス
        /// </summary>
        private static string JsDirPath
        {
            get
            {
                return "https://cdnjs.cloudflare.com/ajax/libs/mermaid/10.9.0/mermaid.min.js";
            }
        }
        #endregion
        /// <summary>
        /// ロガー
        /// </summary>
        protected static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()!.DeclaringType);

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

        #region リロード処理
        /// <summary>
        /// リロード処理
        /// </summary>
        private void WebViewReload()
        {
            if (this.WebviewObject != null)
            {
                SetMarkdown();
                this.WebviewObject.Reload();
                NotifyPropertyChanged("HtmlPath");
            }
        }
        #endregion

        #region HTMLファイルの出力先ファイル名
        /// <summary>
        /// HTMLファイルの出力先ファイル名
        /// </summary>
        protected virtual string OutFilename
        {
            get
            {
                return "sample.html";
            }
        }
        #endregion

        #region HTMLパス
        /// <summary>
        /// HTMLパス
        /// </summary>
        public string HtmlPath
        {
            get
            {
                return GetHtmlPath();
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

        #region ファイル情報一式
        /// <summary>
        /// ファイル情報一式
        /// </summary>
        public FileCollectionM FileCollectionCSOnly
        {
            get
            {
                return GblValues.Instance.FileCollectionCSOnly;
            }
            set
            {
                if (GblValues.Instance.FileCollectionCSOnly == null || !GblValues.Instance.FileCollectionCSOnly.Equals(value))
                {
                    GblValues.Instance.FileCollectionCSOnly = value;
                    NotifyPropertyChanged("FileCollectionCSOnly");
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
                SaveHtml();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        #region クローズ処理
        /// <summary>
        /// クローズ処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        public override void Close(object sender, EventArgs e)
        {
        }
        #endregion

        #region HTMLファイルパスを取得する
        /// <summary>
        /// HTMLファイルパスを取得する
        /// </summary>
        /// <returns></returns>
        public string GetHtmlPath()
        {
            return DisplayWebManagerM.GetDisplayHtmlPath(this.OutFilename);
        }
        #endregion

        #region HTMLコードの取得処理
        /// <summary>
        /// HTMLコードの取得処理
        /// </summary>
        /// <param name="markdown">マークダウン文字列</param>
        /// <returns>HTMLコード</returns>
        public string GetHtml(string markdown)
        {
            try
            {
                // テンプレートファイルの読み込み
                StreamReader html_sr = new StreamReader(BaseHtml, Encoding.UTF8);
                this.Html = html_sr.ReadToEnd();

                var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
                var html = Markdig.Markdown.ToHtml(markdown, pipeline);
                var html_txt = this.Html.Replace("{menDoc:jsdir}", JsDirPath);
                return html_txt.Replace("{menDoc:htmlbody}", html);
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region HTMLファイルを所定の場所に保存する関数
        /// <summary>
        /// HTMLファイルを所定の場所に保存する関数
        /// </summary>
        public void SaveHtml()
        {
            SetMarkdown();
            DisplayWebManagerM disp = new DisplayWebManagerM();
            disp.SaveHtml(this.Markdown, DisplayWebManagerM.GetDisplayHtmlPath(this.OutFilename));
        }
        #endregion

        /// <summary>
        /// マークダウンの設定処理
        /// </summary>
        /// <param name="markdown"></param>
        public void SetMarkdown()
        {
            // マークダウンの作成処理
            var markdown = CreateMarkdown();

            // マークダウンのセット
            this.Markdown = markdown;
        }

        /// <summary>
        /// リロード処理
        /// </summary>
        public void Reload()
        {
            // マークダウンのセット処理
            SetMarkdown();


            // HTMLへの変換
            ConvetHtml(this.Markdown);

            // HTMLファイルの保存
            SaveHtml();

            // WebView2 Reload
            WebViewReload();
        }

        #region HTML文の作成
        /// <summary>
        /// HTML文の作成処理
        /// </summary>
        /// <param name="markdown">マークダウンコード</param>
        private void ConvetHtml(string markdown)
        {
            try
            {
                this.Html = GetHtml(markdown);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                ShowMessage.ShowErrorOK(ex.Message, "Error");
                this.Html = string.Empty;
            }

        }
        #endregion

        #region マークダウン出力
        /// <summary>
        /// マークダウン出力
        /// </summary>
        public void OutputMarkdown()
        {
            try
            {
                // ダイアログのインスタンスを生成
                var dialog = new SaveFileDialog();

                // ファイルの種類を設定
                dialog.Filter = "マークダウンファイル (*.md)|*.md";

                // ダイアログを表示する
                if (dialog.ShowDialog() == true)
                {
                    this.Markdown = CreateMarkdown();
                    File.WriteAllText(dialog.FileName, this.Markdown);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                ShowMessage.ShowErrorOK(ex.Message, "Error");
                this.Html = string.Empty;
            }
        }
        #endregion

        #region マークダウン作成処理
        /// <summary>
        /// マークダウン作成処理
        /// </summary>
        protected abstract string CreateMarkdown();
        #endregion
    }
}
