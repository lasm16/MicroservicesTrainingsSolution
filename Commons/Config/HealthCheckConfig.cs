using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons.Config
{
    public class HealthCheckConfig
    {
        public TimeSpan DegradedThreshold { get; set; } = TimeSpan.FromMilliseconds(500);
        public TimeSpan UnhealthyThreshold { get; set; } = TimeSpan.FromMilliseconds(2000);
        public TimeSpan ConnectionTimeout { get; set; } = TimeSpan.FromMilliseconds(3000);
    }
}
