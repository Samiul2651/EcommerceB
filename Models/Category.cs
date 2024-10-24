﻿using EcommerceWebApi.Interfaces;
using MongoDB.Bson;

namespace EcommerceWebApi.Models
{
    public class Category : IModel
    {
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public string name { get; set; }
        public string ParentCategoryId { get; set; }
    }
}
