using Microsoft.Web.WebView2.Wpf;
using MVVMCore.BaseClass;
using MVVMCore.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace menDoc2.ViewModels
{
    public class WebViewPrevVM : ViewModelBase
    {
        /// <summary>
        /// ロガー
        /// </summary>
        protected static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()!.DeclaringType);

        #region WebView2用オブジェクト[WebviewObject]プロパティ
        /// <summary>
        /// WebView2用オブジェクト[WebviewObject]プロパティ用変数
        /// </summary>
        WebView2 _WebviewObject = new WebView2();
        /// <summary>
        /// WebView2用オブジェクト[WebviewObject]プロパティ
        /// </summary>
        public WebView2 WebviewObject
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

        #region 一時Htmlファイルパス[TempHtmlPath]プロパティ
        /// <summary>
        /// 一時Htmlファイルパス[TempHtmlPath]プロパティ
        /// </summary>
        public virtual string TempHtmlPath
        {
            get
            {
                return string.Empty;
            }
        }
        #endregion

        #region ブラウザのパス[DefaultBrowzerPath]プロパティ
        /// <summary>
        /// ブラウザのパス[DefaultBrowzerPath]プロパティ用変数
        /// </summary>
        string _DefaultBrowzerPath = @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe";
        /// <summary>
        /// ブラウザのパス[DefaultBrowzerPath]プロパティ
        /// </summary>
        public string DefaultBrowzerPath
        {
            get
            {
                return _DefaultBrowzerPath;
            }
            set
            {
                if (!_DefaultBrowzerPath.Equals(value))
                {
                    _DefaultBrowzerPath = value;
                    NotifyPropertyChanged("DefaultBrowzerPath");
                }
            }
        }
        #endregion

        #region プレビュー処理
        /// <summary>
        /// プレビュー処理
        /// </summary>
        public virtual void Preview()
        {
            try
            {
                //// プレビューの表示処理（ブラウザを使用する）
                //System.Diagnostics.Process.Start(this.DefaultBrowzerPath, this.TempHtmlPath);
            }
            catch (Exception e)
            {
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region アプリケーションフォルダの取得
        /// <summary>
        /// アプリケーションフォルダの取得
        /// </summary>
        /// <returns>アプリケーションフォルダパス</returns>
        public static string GetApplicationFolder()
        {
            var fv = FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), fv.CompanyName!, fv.ProductName!);
        }
        #endregion

        #region テンポラリフォルダの作成
        /// <summary>
        /// テンポラリフォルダの作成
        /// </summary>
        public static string TempDir
        {
            get
            {
                string path = GetApplicationFolder() + @"\Temporary";
                return path;
            }
        }
        #endregion


        #region 初期化処理
        /// <summary>
        /// 初期化処理
        /// </summary>
        public override void Init(object sender, EventArgs e)
        {
            try
            {

                PathManager.CreateDirectory(GetApplicationFolder());

                //var conf = ConfigManager.LoadConf();

                this.DefaultBrowzerPath = this.DefaultBrowzerPath;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                ShowMessage.ShowErrorOK(ex.Message + "ClassDoc_SetRelationVM", "Error");
            }
        }
        #endregion

        #region WebView用のオブジェクトのセット処理
        /// <summary>
        /// WebView用のオブジェクトのセット処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SetWebViewObject(object sender, EventArgs e)
        {
            try
            {
                var wv = sender as WebView2;

                // nullチェック
                if (wv != null)
                {
                    // オブジェクトの保持
                    SetWebviewObject(wv);
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
                //string folder = Path.Combine(Utilities.GetApplicationFolder(), "Temporary");

                //ShowMessage.ShowErrorOK(folder, "aasa");

                //var webView2Environment = await Microsoft.Web.WebView2.Core.CoreWebView2Environment.CreateAsync(null, folder);

                await webview.EnsureCoreWebView2Async(null);

                this.WebviewObject = webview;
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
        public virtual void WebViewReload()
        {
            if (this.WebviewObject != null)
            {
                this.WebviewObject.Reload();
            }
        }
        #endregion

        #region 画面を閉じる処理
        /// <summary>
        /// 画面を閉じる処理
        /// </summary>
        public override void Close(object sender, EventArgs e)
        {
        }
        #endregion


    }

}
