namespace BlingMeMVC
{
    using System;

    using BlingMe.Domain.EF;
    using BlingMe.Domain.Entities;

    public static class Utilities
    {
        public static string GetImageUrlString(int id)
        {
            EFDbContext db = new EFDbContext();
            Bracelet bracelet = db.Bracelets.Find(id);
            if (bracelet == null || bracelet.Avatar == null)
            {
                return string.Empty;
            }

            return string.Format("data:image/png;base64,{0}", Convert.ToBase64String(bracelet.Avatar));
        }
    }
}