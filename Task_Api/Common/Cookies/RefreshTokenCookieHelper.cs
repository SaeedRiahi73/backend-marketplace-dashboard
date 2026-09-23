using Microsoft.AspNetCore.Http;
using Task_Application.Dtos.RefreshToken;

namespace Task_Api.Common.Cookies;

public static class RefreshTokenCookieHelper
{
    public const string CookieName = "refresh_token";

    public static void Set(HttpResponse response, RefreshTokenCookieDto refreshTokenCookie)
    {
        CookieOptions cookieOptions = CreateOptions();

        if (refreshTokenCookie.IsPersistent)
            cookieOptions.Expires = new DateTimeOffset(refreshTokenCookie.ExpiresAt);

        response.Cookies.Append(CookieName, refreshTokenCookie.Token, cookieOptions);
    }

    public static void Delete(HttpResponse response)
    {
        response.Cookies.Delete(CookieName, CreateOptions());
    }

    private static CookieOptions CreateOptions()
    {
        return new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            IsEssential = true,
            Path = "/api/Auth"
        };
    }
}
