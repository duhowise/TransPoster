﻿namespace TransPoster.Data.Models;

public class Supplier
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string SupplierNo { get; set; }
    public string Location { get; set; }
    public DateTime CreatedAt { get; set; }
}