using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Utilities.DTOs
{
   public class General3col
    {
        public General3col()
        {
        }

        public General3col(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public General3col(int id, string name, string des) : this(id, name)
        {
            Des = des;
        }

        public General3col(int id, string name, string des, int soStatusKf) : this(id, name, des)
        {
            SoStatusKf = soStatusKf;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Des { get; set; }
        public int SoStatusKf { get; set; }
    }
}
