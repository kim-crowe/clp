using System;

namespace CogsLite.NpgSql.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
    }    
}