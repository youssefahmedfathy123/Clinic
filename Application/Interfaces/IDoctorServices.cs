using Application.Dtos;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IDoctorServices
    {
        // get doctor by name
        Task<List<DoctorDto>> GetDoctorByName(string input);


        // get doctor by category 
        Task<List<DoctorDto>> GetDoctorByCategory(Guid CategoryId);



    }
}


