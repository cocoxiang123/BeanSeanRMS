using System.Collections.Generic;

namespace ReservationSystem.Models
{
    public class SittingType
    {
        public SittingType()
        {
            Sittings = new List<Sitting>();
        }
        public int Id { get; set; }

        public string Description { get; set; }

        public List<Sitting> Sittings { get; set; }


    }
}