using Microsoft.EntityFrameworkCore;

namespace AuthModule
{
    public class AuthOptions<TUser>
    {

        public bool UseCookies { get; set; }
        public bool UseJWT { get; set; }

        public required DbContext DbContext { get; set; }
        public required Type UserType { get; set; }

        public void IncludeProperty<TUser>(Func<TUser, object> getProps)
        {
            typeof(TUser).GetProperties();
        }

    }
}
