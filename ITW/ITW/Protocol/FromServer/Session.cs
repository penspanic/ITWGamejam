using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITW.Protocol.FromServer
{
    public class Connect : Packet
    {
        enum ErrorResult
        {
            SUCCESS,
            FAIL,
        }
        public ErrorResult Result { get; set; }
}
