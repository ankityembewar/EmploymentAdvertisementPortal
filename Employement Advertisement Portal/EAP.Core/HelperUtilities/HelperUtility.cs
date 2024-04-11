using Microsoft.AspNetCore.Http;
using System.Linq;

namespace EAP.Core.HelperUtilities
{
    public class HelperUtility
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HelperUtility(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetLoginUserId()
        {
            var session = _httpContextAccessor?.HttpContext?.Session;
            var userIdKey = "UserId";

            if (session != null && session.TryGetValue(userIdKey, out byte[] bytes))
            {
                return System.Text.Encoding.UTF8.GetString(bytes);
            }

            return null;
        }
    }
}
