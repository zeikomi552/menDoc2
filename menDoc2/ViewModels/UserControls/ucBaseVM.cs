using Markdig;
using menDoc2.Common;
using menDoc2.Models;
using menDoc2.Models.Class;
using MVVMCore.BaseClass;
using MVVMCore.Common.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filename">ファイル名</param>
        public ucBaseVM(string filename)
        {
            this.OutFilename = filename;
        }
        #endregion

        /// <summary>
        /// ロガー
        /// </summary>
        protected static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()!.DeclaringType);

        /// <summary>
        /// HTMLファイルの出力先ファイル名
        /// </summary>
        public virtual string OutFilename { get; } = "sample.html";

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

        #region 初期化処理
        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void Init(object sender, EventArgs e)
        {
            
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

        #region HTMLファイルを所定の場所に保存する関数
        /// <summary>
        /// HTMLファイルを所定の場所に保存する関数
        /// </summary>
        public void SaveHtml()
        {
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

            // HTMLへの変換
            ConvetHtml(this.Markdown);
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
                var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
                this.Html = Markdig.Markdown.ToHtml(markdown, pipeline);
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
