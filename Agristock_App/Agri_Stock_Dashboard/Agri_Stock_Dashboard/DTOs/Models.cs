using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.Json.Serialization;

namespace Agri_Stock_Dashboard.DTOs
{
    public class Machinery
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Status { get; set; } = null!;
        [JsonPropertyName("statusDescription")]       //luam direct din backend status corect din JSON
        public string? StatusDescription { get; set; } //descriere optionala
        public Warehouse Warehouse { get; set; } = null!;
    }

   
    public class Warehouse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public string Location { get; set; } = null!;
        [JsonPropertyName("capacity_kg")]          //luam din backend prin JSON
        public double? CapacityKg { get; set; } 

        public override string ToString() => Name; //pentru a afisa doar numele in dropdown
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;
    }
    public class InventoryItem
    {
        public int Id { get; set; }
        public Warehouse Warehouse { get; set; } = null!;
        public Product Product { get; set; } = null!;
        [JsonPropertyName("current_quantity_kg")]  //luam din backend prin JSON
        public double CurrentQuantityKg { get; set; }
    }


    public class ChatbotRequest
    {
        public string Prompt { get; set; } = null!;
    }
}