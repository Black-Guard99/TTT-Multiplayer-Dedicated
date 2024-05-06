using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTT.Server.Games;

namespace TTT.Server.Matchmaking
{
    public class MMRequest {
        public ServerConnection connections { get; set; }
        public DateTime serachStartDT { get; set; }
        public bool matchFound { get; set; }
    }
}
