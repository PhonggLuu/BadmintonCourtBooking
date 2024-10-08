﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace BY.Data.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public string Address { get; set; }

    public string Phone { get; set; }

    public int? NumberSlot { get; set; }

    public bool Gender { get; set; }

    public DateOnly? YearOfBirth { get; set; }

    public DateTime? RegisterDate { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}