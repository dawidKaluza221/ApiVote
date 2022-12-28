﻿using Microsoft.EntityFrameworkCore;
using ApiVote.UserInfo;

namespace ApiVote.Data
{
    public class ApiContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public ApiContext(DbContextOptions<ApiContext> options) : base(options) 
        {
        
        }

    }
}
