using DocsVision.Platform.ObjectManager;
using DocsVision.Platform.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDocsvision55
{
    public class SessionCurrent
    {
        protected UserSession _userSession;

        /// <summary>
        /// рабочий сервер
        /// </summary>
        /// <returns></returns>
        protected ObjectContext GetContextActual()
        {
            SessionManager oSessionManager;
            oSessionManager = SessionManager.CreateInstance();
            oSessionManager.Connect("http://dc-edo55/docsvision/StorageServer/StorageServerService.asmx", "DVbase55");

            _userSession = oSessionManager.CreateSession();
            ObjectContextHelper contextHelper = new ObjectContextHelper(_userSession);
            return contextHelper.LocalContext;
        }



        public ObjectContext Get()
        {

            return GetContextActual();
        }

        public UserSession GetSession()
        {
            return _userSession;
        }
    }
}
