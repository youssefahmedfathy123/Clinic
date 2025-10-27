using Domain.Premitives;

namespace Domain.Entities
{
    public class Category : Audited
    {
        private Category()
        {
            
        }

        public string Name { get;private set; }

        public Category(string name)
        {
            Name = name;
        }
        // Navigation properties 

        public List<Doctor> Doctors { get; set; } 


    }
}


