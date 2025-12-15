using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ServerSide.Listeners
{
    public static class CookieManager
    {
        static public void SetHttpOnlyCookie(HttpListenerResponse response, string cookieName, string cookieValue, int expireDays = 7)
        {
            Cookie cookie = new Cookie(cookieName, cookieValue)
            {
                HttpOnly = true,
                Secure = false,
                Path = "/",
                Expires = DateTime.Now.AddDays(expireDays)
            };

            response.AppendCookie(cookie);
        }
    }
}
