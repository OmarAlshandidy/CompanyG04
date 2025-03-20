using System.ComponentModel.DataAnnotations;
using Company.G04.DAL.Moudel;

namespace Company.G04.PL.Dtos
{
    public class CreateDepartmentDto
    {
        [Required(ErrorMessage ="Code Is Required")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Name Is Required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Create At Is Required")]
        public DateTime CreateAt { get; set; }
        public List<Employee>? Employees { get; set; }

    }
}
