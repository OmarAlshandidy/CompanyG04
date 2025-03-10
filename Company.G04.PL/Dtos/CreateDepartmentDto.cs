using System.ComponentModel.DataAnnotations;

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

    }
}
