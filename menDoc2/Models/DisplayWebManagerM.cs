using Markdig;
using MVVMCore.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace menDoc2.Models
{
    /// <summary>
    /// WebView2で表示するためのHTMLファイル作成クラス
    /// </summary>
    public class DisplayWebManagerM
    {
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
        /// WebView2に表示するためのHTMLファイル出力先パスを作成する関数
        /// </summary>
        /// <param name="tmpfilename">ファイル名</param>
        /// <returns>ファイルパス</returns>
        public static string GetDisplayHtmlPath(string tmpfilename)
        {
            var fv = FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), fv.CompanyName!, fv.ProductName!, tmpfilename);
        }

        #region HTMLコード
        /// <summary>
        /// HTMLコード
        /// </summary>
        private string BaseHtmlCode { get; set; } = string.Empty;
        #endregion

        #region 実行ファイルのカレントディレクトリを返却する
        /// <summary>
        /// 実行ファイルのカレントディレクトリを返却する
        /// </summary>
        public static string ExeCurrentDir
        {
            get
            {
                Assembly myAssembly = Assembly.GetExecutingAssembly();
                string path = myAssembly.Location;
                DirectoryInfo di = new DirectoryInfo(path);
                // 親のディレクトリを取得する
                DirectoryInfo diParent = di.Parent!;
                return diParent.FullName;
            }
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
                this.BaseHtmlCode = html_sr.ReadToEnd();

                var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
                var html = Markdig.Markdown.ToHtml(markdown, pipeline);
                var html_txt = this.BaseHtmlCode.Replace("{menDoc:jsdir}", JsDirPath);
                return html_txt.Replace("{menDoc:htmlbody}", html);
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region HTMLファイルの保存処理
        /// <summary>
        /// HTMLファイルの保存処理
        /// </summary>
        /// <param name="markdown">マークダウン文字列</param>
        public void SaveHtml(string markdown, string filename)
        {
            try
            {
                var html = GetHtml(markdown);
                PathManager.CreateCurrentDirectory(filename);
                File.WriteAllText(GetDisplayHtmlPath(filename), html);
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}
