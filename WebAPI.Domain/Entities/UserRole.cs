using Microsoft.AspNetCore.Identity;
using WebAPI.Domain.Common;

namespace WebAPI.Domain.Entities
{
    public class UserRole : IdentityRole<int>
    {
        public override int Id { get; set; }
        public override string Name { get; set; }
        public List<User> User { get; set; }
    }
}
