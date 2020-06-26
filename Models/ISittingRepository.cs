using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationSystem.Models
{
    public interface ISittingRepository
    {
        Sitting GetSitting(int sittingId);
        List<Sitting> GetAllSittings();
        void AddSitting(Sitting sitting);

        Sitting Update(Sitting sitting);
        Sitting Delete(Sitting sitting);
        List<Sitting> GetSittingWithType();
        void DeleteSittingItem(int sittingTypeId);
        bool ValidateDate(Sitting a);
    }
}
