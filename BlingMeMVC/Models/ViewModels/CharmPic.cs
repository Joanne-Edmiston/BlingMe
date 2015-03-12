namespace BlingMeMVC.Models.ViewModels
{
    using BlingMe.Domain.Entities;

    public class CharmPic
    {
        public CharmPic(Bracelet bracelet)
        {
            Name = bracelet.Name;
            ID = bracelet.ID;
            Email = bracelet.Email;

            if (bracelet.Avatar == null)
            {
                ImageUrl = "../Content/Images/Charm_"
                           + bracelet.Type.ToString().ToLower() + ".png";
            }
            else
            {
                ImageUrl = Utilities.GetImageUrlString(ID);
            }

            OffBracelet = false;
        }

        public bool OffBracelet { get; set; }

        public int Degrees { get; set; }

        public string ImageUrl { get; set; }

        public string Name { get; set; }

        public int ID { get; set; }


        public string Email { get; set; }
    }
}
