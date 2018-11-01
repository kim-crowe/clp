using System.Collections.Generic;

namespace CogsLite.NpgSql.Entities
{
    public class UserEntity : BaseEntity
    {
        public string DisplayName { get; set; }

        public List<GameEntity> Games { get; set; }
    }
}