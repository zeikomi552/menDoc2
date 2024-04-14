using menDoc2.Common;
using menDoc2.Models.Class;
using MVVMCore.Common.Utilities;
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
