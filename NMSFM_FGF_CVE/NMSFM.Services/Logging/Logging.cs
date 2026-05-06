using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMSFM.Services.Logging
{
    public class Logging : ILogging
    {
        private ILog webLogger;

        public ILog Log
        {
            get
            {
                if (webLogger == null)
                {
                    webLogger = LogManager.GetLogger("NMSFM");
                }
                return webLogger;
            }
        }
        
        public void Debug(String message)
        {
            Log.Debug(message);
        }

        public void Debug(String message, Exception ex)
        {
            Log.Debug(message, ex);
        }

        public void Info(String message)
        {
            Log.Info(message);
        }

        public void Info(String message, Exception ex)
        {
            Log.Info(message, ex);
        }

        public void Warn(String message)
        {
            Log.Warn(message);
        }

        public void Warn(String message, Exception ex)
        {
            Log.Warn(message, ex);
        }

        public void Error(String message)
        {
            Log.Error(message);
        }

        public void Error(String message, Exception ex)
        {
            Log.Error(message, ex);
        }

        public void Fatal(String message)
        {
            Log.Fatal(message);
        }

        public void Fatal(String message, Exception ex)
        {
            Log.Fatal(message, ex);
        }

    }
}
