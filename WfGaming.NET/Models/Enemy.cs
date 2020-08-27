using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WfGaming.Models
{
    class Enemy
    {
        public uint id { get; set; }
        public uint team_id { get; set; }
        public uint ship_id { get; set; }
        public double health { get; set; }
        public double max_health { get; set; }
        public double yaw { get; set; }
        public double speed { get; set; }
        public bool is_visible { get; set; }
        public bool is_ship_visible { get; set; }
    }
}
