using System;

namespace CogsLite.Core
{
    public class Member
    {
        public Guid Id { get; set; }
        public string Username { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Member member)
                return member.Id == Id;

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
