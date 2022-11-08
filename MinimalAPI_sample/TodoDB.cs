﻿using Microsoft.EntityFrameworkCore;

namespace MinimalAPI_sample
{
    public class TodoDB: DbContext
    {
        public TodoDB(DbContextOptions<TodoDB> options) : base(options) { }
        public DbSet<Todo> Todos => Set<Todo>();
    }
}
