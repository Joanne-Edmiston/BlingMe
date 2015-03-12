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

            byte[] imageByteData = bracelet.Avatar;
            string imageBase64Data = Convert.ToBase64String(imageByteData);
            return string.Format("data:image/png;base64,{0}", imageBase64Data);
        }
    }
}