using menDoc2.Views.UserControls;
using MVVMCore.Common.Utilities;
using MVVMCore.Common.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace menDoc2.ViewModels.UserControls
{
    public class ucClassDiagramVM : ucBaseVM
    {
        #region HTMLファイルの出力先ファイル名
        /// <summary>
        /// HTMLファイルの出力先ファイル名
        /// </summary>
        protected override string OutFilename
        {
            get { return "classdiagram.html"; }
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
                var tmp = VisualTreeHelperWrapper.GetWindow<ucClassDiagramV>(sender) as ucClassDiagramV;

                //if (tmp != null)
                //{
                //    SetWebviewObject(tmp.wv2);
                //}
                base.Init(sender, e);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                ShowMessage.ShowErrorOK(ex.Message, "Error");
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
                return this.FileCollection.CreateClassMarkdown2();
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
