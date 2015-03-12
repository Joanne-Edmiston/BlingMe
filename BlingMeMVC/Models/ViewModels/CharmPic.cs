namespace BlingMeMVC.Models.ViewModels
{
    using BlingMe.Domain.Entities;

    public class CharmPic
    {
        public enum CharmLocation
        {
            Child,
            Parent,
            StrayChild,
            StrayParent
        }

        public CharmPic(Bracelet bracelet, CharmLocation location)
        {
            Location = location;
            Bracelet = bracelet;

            if (Bracelet.Avatar == null)
            {
                ImageUrl = "../Content/Images/Charm_"
                           + bracelet.Type.ToString().ToLower() + ".png";
            }
            else
            {
                ImageUrl = Utilities.GetImageUrlString(bracelet.ID);
            }

            OffBracelet = false;
        }

        public bool OffBracelet { get; set; }

        public CharmLocation Location { get; set; }

        public int Degrees { get; set; }

        public string ImageUrl { get; set; }

        public virtual Bracelet Bracelet { get; set; }
    }
}
