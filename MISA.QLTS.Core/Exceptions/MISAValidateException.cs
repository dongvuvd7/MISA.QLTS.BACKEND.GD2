using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Core.Exceptions
{
    public class MISAValidateException : Exception
    {
        IDictionary MISAData = new Dictionary<string, object>();
        public MISAValidateException(object data)
        {
            MISAData.Add("exception", data);
        }
        public override string Message
        {
            get
            {
                return Properties.Resources.ValidateErrorMsg;
            }
        }
        public override IDictionary Data
        {
            get
            {
                return MISAData;
            }
        }
    }
}