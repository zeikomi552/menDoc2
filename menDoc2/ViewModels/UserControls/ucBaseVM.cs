using MVVMCore.BaseClass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace menDoc2.ViewModels.UserControls
{
    public class ucBaseVM : ViewModelBase
    {
        /// <summary>
        /// ロガー
        /// </summary>
        protected static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()!.DeclaringType);


        public override void Init(object sender, EventArgs e)
        {
            
        }

        public override void Close(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
