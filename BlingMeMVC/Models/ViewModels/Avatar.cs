namespace BlingMeMVC.Models.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using System.Drawing;
    using System.Web;

    using BlingMe.Domain.Entities;

    public class Avatar
    {
        [Required]
        public HttpPostedFileBase File { get; set; }

        [Required]
        public int ID { get; set; }

        public Image Image { get; set; }

        public virtual Bracelet Bracelet { get; set; }
    }
}