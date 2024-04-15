using MVVMCore.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace menDoc2.ViewModels.UserControls
{
    public class ucClassDiagramVM : ucClassVM
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
