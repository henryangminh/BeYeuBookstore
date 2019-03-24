using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Utilities.DTOs
{
    public class IdAndName
    {
        public IdAndName()
        {
            Id = 0;
        }

        public IdAndName(int id,string name)
        {
            Id = id;
            Name = name;
        }

        public IdAndName(int id, string sId, string name) : this(id, name)
        {
            this.sId = sId;
        }

        public int Id { get; set; }
        public string sId { get; set; }
        public string Name { get; set; }
    }
    public class ListMenu
    {
        private static ListMenu instance;
        public static ListMenu Instance
        {
            get { if (instance == null) instance = new ListMenu(); return instance; }
            set { instance = value; }
        }
        public List<IdAndName> FindAll()
        {
            List<IdAndName> listData = new List<IdAndName>();
            listData.AddRange(new List<IdAndName>()
            {
                new IdAndName() { Id=1, Name="Bản vẽ kết cấu"},
                new IdAndName() { Id=1, Name="Bản vẽ kiến trúc"},
                 new IdAndName(){ Id=1, Name="Bản vẽ MEP (cơ điện)"},
                new IdAndName() { Id=1, Name="Bản vẽ nội thất"},
                 new IdAndName(){ Id=1, Name="Bản vẽ phối cảnh"},
                new IdAndName() { Id=1, Name="Speet vật liệu"},
                 new IdAndName(){ Id=1, Name="Giấy phép xây dựng"},
                new IdAndName() { Id=1, Name="Các quy định khi chào thầu"},
                 new IdAndName(){ Id=1, Name="Giấy khác (nén lại dạng file .rar)"}
            });
            return listData;
        }
    }
}
