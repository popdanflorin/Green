namespace Green.Models
{
    public class UserRestaurant
    {
        public string id { get; set; }

        public string Name { get; set; }
        public string Address { get; set; }
        public Entities.Enums.RestaurantType Type { get; set; }
        public string TypeDisplay
        {
            get
            {
                return Type.ToString();
            }
        }
        public string ImageName { get; set; }
        public int Rating { get; set; }
    }
}