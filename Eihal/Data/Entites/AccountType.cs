﻿using NuGet.Protocol.Core.Types;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eihal.Data.Entites
{
    public class AccountType
    {
        public int Id { get; set; }
        public string? TitleAr { get; set; }
        public string? TitleEn { get; set; }
        public List<ApplicationUser> ApplicationUsers { get; set; }
    }
}
