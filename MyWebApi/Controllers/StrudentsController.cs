// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Hosting;
// using Microsoft.AspNetCore.Http;
// using MyWebApi.Repositories;
// using MyWebApi.Models;
// using System;
// using System.IO;
// using System.Threading.Tasks;
// using System.Collections.Generic;
// using System.Globalization;
// using System.Linq;

// namespace MyWebApi.Controllers {

//     [ApiController]
//     [Route("api/[controller]")]
//     public class StudentsController : ControllerBase {
//         private readonly StudentRepository _studentRepository;
//         private readonly IWebHostEnvironment _environment;

//         // Constructor with both StudentRepository and IWebHostEnvironment injected
//         public StudentsController(StudentRepository studentRepository, IWebHostEnvironment environment) {
//             _studentRepository = studentRepository;
//             _environment = environment;
//         }

//         [HttpGet]
//         public ActionResult<IEnumerable<Student>> GetAllStudents() {
//             var students = _studentRepository.GetAllStudents();
//             return Ok(students);
//         }

//         [HttpGet("{id:long}")]
//         public ActionResult<object> GetStudentById(long id) {
//             var student = _studentRepository.GetStudentById(id);
//             if (student == null) {
//                 return NotFound(new { Error = $"No student has an Id {id}" });
//             }
//             string message = $"{student.Firstname} {student.Lastname} has an id = {student.Id}";
//             return Ok(new {
//                 Message = message,
//                 Student = student
//             });
//         }

//         [HttpGet("search")]
//         public ActionResult<IEnumerable<Student>> SearchStudentsByName([FromQuery] string name) {
//             if (string.IsNullOrEmpty(name)) {
//                 return BadRequest(new { Message = "This field is required." });
//             }
            
//             var matchedStudents = _studentRepository.GetAllStudents()
//                 .Where(s => s.Firstname.Contains(name, StringComparison.OrdinalIgnoreCase) 
//                 || s.Lastname.Contains(name, StringComparison.OrdinalIgnoreCase))
//                 .ToList();
                
//                 if (!matchedStudents.Any()) {
//                     return NotFound(new { Message = $"No students name contains '{name}'." });
//                 }
                
//                 return Ok(matchedStudents);
//         }
        
//         [HttpGet("current-date")]
//         public ActionResult<string> GetCurrentDate([FromQuery] string lang = "en") {
//             var cultureInfo = new CultureInfo("en-US");
//             var format = "D";

//             switch (lang.ToLower()) {
//                 case "fr":
//                     cultureInfo = new CultureInfo("fr-FR");
//                     format = "dddd d MMMM yyyy";
//                     break;
//                 case "es":
//                     cultureInfo = new CultureInfo("es-ES");
//                     format = "dddd, d MMMM yyyy";
//                     break;
//                 case "en":
//                 default:
//                     cultureInfo = new CultureInfo("en-US");
//                     format = "D";
//                     break;
//             }

//             var formattedDate = DateTime.Now.ToString(format, cultureInfo);

//             return Ok(new { FormattedDate = formattedDate });
//         }

//         [HttpPut("{id}")]
//         public ActionResult UpdateStudent(long id, [FromBody] UpdateStudentRequest request) {
//             var student = _studentRepository.GetStudentById(id);

//             if (student == null) {
//                 return NotFound();
//             }

//             if (!string.IsNullOrEmpty(request.Firstname)) 
//                 student.Firstname = request.Firstname;
        
//             if (!string.IsNullOrEmpty(request.Lastname)) 
//                 student.Lastname = request.Lastname;
        
//             if (!string.IsNullOrEmpty(request.FatherName)) 
//                 student.FatherName = request.FatherName;

//             if (!string.IsNullOrEmpty(request.MotherName)) 
//                 student.MotherName = request.MotherName;
        
//             if (!string.IsNullOrEmpty(request.Email)) 
//                 student.Email = request.Email;
        
//             if (request.Age.HasValue)
//                 student.Age = request.Age.Value;

//             var updatedStudent = new {
//                 Id = student.Id,
//                 Firstname = student.Firstname,
//                 Lastname = student.Lastname,
//                 Email = student.Email
//             };

//             return Ok(updatedStudent);
//         }

//         [HttpPost("upload-image")]
//         public async Task<ActionResult> UploadImage([FromForm] IFormFile image)
//         {
//             bool Results = false;
            
//             try {
//                 var _uplpadedfiles = Request.Form.Files;

//                 foreach(IformFile source in _uplpadedfiles){
//                     string Filename = source.FileName;
//                     string Filepath =GetFilePath(Filename);

//                 }
                

//                 if(!System.IO.Directory.Exists(Filepath)) {
//                     System.IO.Directory.CreateDirectory(Filepath);
//                 }

//                 string imagepath = Filepath +"\\image.png";

//                 if(System.IO.File.Exists(imagepath)) {
//                     System.IO.File.Delete(imagepath);
//                 }

//                 using(FileStream stream = System.IO.File.Create(imagepath)) {
//                     await source.CopyToAsync(stream);
//                     Results = true;
//                 }
//             }
            
//             catch (Exception ex)
//             {
//                 return StatusCode(500, $"Internal server error: {ex.Message}");
//             }

//             return Ok(Results);
//         }

       

//         [HttpDelete("{id}")]
//         public ActionResult DeleteStudent(long id) {
//             var success = _studentRepository.DeleteStudentById(id);
//             if (!success) {
//                 return NotFound(new { Message = $"Student with Id {id} not found." });
//             }
//             return Ok(new { Message = $"Student with Id {id} has been deleted successfully." });
//         }  

//         [NonAction]
//         private string GetFilePath(string studentCode)
//         {
//             return Path.Combine(this._environment.WebRootPath, "Uploads", "Student", studentCode);
//         }
//     }

//     public class UpdateStudentRequest {
//         public string Firstname { get; set; }
//         public string Lastname { get; set; }
//         public string FatherName { get; set; }
//         public string MotherName { get; set; }
//         public string Email { get; set; }
//         public int? Age { get; set; }
//     }


// }
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MyWebApi.Repositories;
using MyWebApi.Models;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;


namespace MyWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly StudentRepository _studentRepository;
        private readonly IWebHostEnvironment _environment;

        public StudentsController(StudentRepository studentRepository, IWebHostEnvironment environment)
        {
            _studentRepository = studentRepository;
           // _environment = environment;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Student>> GetAllStudents()
        {
            var students = _studentRepository.GetAllStudents();
            return Ok(students);
        }

        [HttpGet("{id:long}")]
        public ActionResult<object> GetStudentById(long id)
        {
            var student = _studentRepository.GetStudentById(id);
            if (student == null)
            {
                return NotFound(new { Error = $"No student has an Id {id}" });
            }
            string message = $"{student.Firstname} {student.Lastname} has an id = {student.Id}";
            return Ok(new
            {
                Message = message,
                Student = student
            });
        }

        [HttpGet("search")]
        public ActionResult<IEnumerable<Student>> SearchStudentsByName([FromQuery] string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest(new { Message = "This field is required." });
            }

            var matchedStudents = _studentRepository.GetAllStudents()
                .Where(s => s.Firstname.Contains(name, StringComparison.OrdinalIgnoreCase)
                || s.Lastname.Contains(name, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (!matchedStudents.Any())
            {
                return NotFound(new { Message = $"No students name contains '{name}'." });
            }

            return Ok(matchedStudents);
        }

        [HttpGet("current-date")]
        public ActionResult<string> GetCurrentDate([FromQuery] string lang = "en")
        {
            var cultureInfo = new CultureInfo("en-US");
            var format = "D";

            switch (lang.ToLower())
            {
                case "fr":
                    cultureInfo = new CultureInfo("fr-FR");
                    format = "dddd d MMMM yyyy";
                    break;
                case "es":
                    cultureInfo = new CultureInfo("es-ES");
                    format = "dddd, d MMMM yyyy";
                    break;
                case "en":
                default:
                    cultureInfo = new CultureInfo("en-US");
                    format = "D";
                    break;
            }

            var formattedDate = DateTime.Now.ToString(format, cultureInfo);

            return Ok(new { FormattedDate = formattedDate });
        }

        [HttpPut("{id}")]
        public ActionResult UpdateStudent(long id, [FromBody] UpdateStudentRequest request)
        {
            var student = _studentRepository.GetStudentById(id);

            if (student == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(request.Firstname))
                student.Firstname = request.Firstname;

            if (!string.IsNullOrEmpty(request.Lastname))
                student.Lastname = request.Lastname;

            if (!string.IsNullOrEmpty(request.FatherName))
                student.FatherName = request.FatherName;

            if (!string.IsNullOrEmpty(request.MotherName))
                student.MotherName = request.MotherName;

            if (!string.IsNullOrEmpty(request.Email))
                student.Email = request.Email;

            if (request.Age.HasValue)
                student.Age = request.Age.Value;

            var updatedStudent = new
            {
                Id = student.Id,
                Firstname = student.Firstname,
                Lastname = student.Lastname,
                Email = student.Email
            };

            return Ok(updatedStudent);
        }

        [HttpPost("upload-image")]
        public async Task<ActionResult> UploadImage(IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                return BadRequest("No image uploaded.");
            }

            var uploadsDirectory = Path.Combine("wwwroot", "Uploads");
            if (!Directory.Exists(uploadsDirectory))
            {
                Directory.CreateDirectory(uploadsDirectory);
            }

            var imageName = Path.GetFileName(image.FileName);
            var imagePath = Path.Combine(uploadsDirectory, imageName);

            try
            {
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

            var relativePath = $"/Uploads/{imageName}";

            return Ok(new { imagePath = relativePath });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error uploading file");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteStudent(long id)
        {
            var success = _studentRepository.DeleteStudentById(id);
            if (!success)
            {
                return NotFound(new { Message = $"Student with Id {id} not found." });
            }
            return Ok(new { Message = $"Student with Id {id} has been deleted successfully." });
        }

        [NonAction]
        private string GetFilePath(string studentCode)
        {
            return Path.Combine(_environment.WebRootPath, "Uploads", "Student", studentCode);
        }
    }

    public class UpdateStudentRequest
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string Email { get; set; }
        public int? Age { get; set; }
    }
}
