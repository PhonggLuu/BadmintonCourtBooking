﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace BY.Data.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public string UserName { get; set; }

    public string Password { get; set; }

    public int CustomerId { get; set; }

    public virtual Customer Customer { get; set; }
}