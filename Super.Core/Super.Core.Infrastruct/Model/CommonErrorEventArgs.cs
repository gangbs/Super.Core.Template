using System;
using System.Collections.Generic;
using System.Text;

namespace Super.Core.Infrastruct.Model
{
    public class CommonErrorEventArgs : EventArgs
    {
        readonly Exception _error;
        public CommonErrorEventArgs(Exception error)
        {
            this._error = error;
        }

        public Exception ErrorInfo
        {
            get
            {
                return this._error;
            }
        }

    }
}
