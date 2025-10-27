namespace Application.Dtos
{
    public class DoctorDto
    {
        public string Name { get; set; } = "";
        public string About { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public string CategoryName { get; private set; } = "";

    }
}


