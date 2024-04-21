using menDoc2.Common;
using menDoc2.Models.Class;
using menDoc2.Views.UserControls;
using MVVMCore.Common.Utilities;
using MVVMCore.Common.Wrapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace menDoc2.ViewModels.UserControls
{
    public class ucClassVM : ucBaseVM
    {
        #region HTMLファイルの出力先ファイル名
        /// <summary>
        /// HTMLファイルの出力先ファイル名
        /// </summary>
        protected override string OutFilename
        {
            get { return "classdetail.html"; }
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
                return this.FileCollection.CreateClassMarkdown();
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
