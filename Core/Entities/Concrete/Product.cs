using Core.Entities.Abstract;
using Core.Entities.Abstract.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.Concrete
{
    public class Product:BaseEntity,IEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string ImageUrl { get; set; }

        // Foreign Key
        public int CategoryId { get; set; }

        // Navigation Property (Ürünün bağlı olduğu kategori)
        public Category Category { get; set; }
    }
}
