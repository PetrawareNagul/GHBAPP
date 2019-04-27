using Silverlake.Service;
using Silverlake.Service.IService;
using Silverlake.Utility;
using Silverlake.Utility.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace Silverlake.Web.ServiceCalls
{
    public class LanguageManager
    {
        private static readonly Lazy<ILanguageDirService> lazyLanguageDirService = new Lazy<ILanguageDirService>(() => new LanguageDirService());

        public static ILanguageDirService ILanguageDirService { get { return lazyLanguageDirService.Value; } }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public static object GetLanguageDirByModule(int module)
        {
            StringBuilder filter = new StringBuilder();
            filter.Append(" " + (Converter.GetColumnNameByPropertyName<LanguageDir>(nameof(LanguageDir.Module))) + "='"+ module + "'");
            List<LanguageDir> languageDirectory = ILanguageDirService.GetDataByFilter(filter.ToString(), 0, 0, false);
            return languageDirectory;
        }

        public enum DiOTPModule
        {
            UID = 0,
            FUND = 1,
            TRANSACTION = 2,
            STATEMENT = 3
        }
    }
}