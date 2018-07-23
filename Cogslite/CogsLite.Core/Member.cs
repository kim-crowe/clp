using System;

namespace CogsLite.Core
{
    public class Member : BaseObject
    {
        public string EmailAddress { get; set; }
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
