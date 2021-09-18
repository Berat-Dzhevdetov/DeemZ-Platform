namespace DeemZ.Web.Infrastructure
{
    using System.Security.Claims;

    using static DeemZ.Global.WebConstants.Constant;

    public static class ClaimsPrincipalExtensions
    {
        public static string GetId(this ClaimsPrincipal principal)
            => principal.FindFirst(ClaimTypes.NameIdentifier).Value;

        public static bool IsAdmin(this ClaimsPrincipal principal)
            => principal.IsInRole(Role.AdminRoleName);

        public static bool IsLecture(this ClaimsPrincipal principal)
            => principal.IsInRole(Role.LecturerRoleName);
    }
}