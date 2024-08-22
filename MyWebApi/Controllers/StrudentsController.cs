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

        public StudentsController(StudentRepository studentRepository)
        {
            _studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
        }

        [HttpGet]
        public ActionResult<IEnumerable<Student>> GetAllStudents()
        {
            try
            {
                var students = _studentRepository.GetAllStudents();

                if (students == null || !students.Any())
                {
                    return NotFound("No students registered.");
                }

                return Ok(students);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data.");
            }
        }


        [HttpGet("{id:long}")]
        public ActionResult<object> GetStudentById(long id)
        {
            try
            {
                var student = _studentRepository.GetStudentById(id);
                if (student == null)
                {
                    return NotFound($"No student has an Id {id}");
                }

                string message = $"{student.Firstname} {student.Lastname} has an id = {student.Id}";
                return Ok(new
                {
                    Message = message,
                    Student = student
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving student data.");
            }
        }

        [HttpGet("search")]
        public ActionResult<IEnumerable<Student>> SearchStudentsByName([FromQuery] string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    return BadRequest("This field is required.");
                }

                var matchedStudents = _studentRepository.GetAllStudents()
                    .Where(s => s.Firstname.Contains(name, StringComparison.OrdinalIgnoreCase)
                    || s.Lastname.Contains(name, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                if (!matchedStudents.Any())
                {
                    return NotFound($"No students name contains '{name}'.");
                }

                return Ok(matchedStudents);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error searching for students.");
            }
        }

        [HttpGet("current-date")]
        public ActionResult<string> GetCurrentDate([FromQuery] string lang = "en")
        {
            try
            {
                var supportedLanguages = new List<string> { "en", "fr", "es" };
                if (!supportedLanguages.Contains(lang.ToLower()))
                {
                    return BadRequest(new { Message = $"Date invalid in this language or language '{lang}' does not exist." });
                }

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
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error processing the request.");
            }
        }




        [HttpPut("{id}")]
        public ActionResult UpdateStudent(long id, [FromBody] UpdateStudentRequest request)
        {
            try
            {
                var student = _studentRepository.GetStudentById(id);

                if (student == null)
                {
                    return NotFound($"No student has an Id {id}");
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
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating student data.");
            }
        }

        [HttpPost("upload-image")]
        public async Task<ActionResult> UploadImage(IFormFile image)
        {
            try
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

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                var relativePath = $"/Uploads/{imageName}";

                return Ok(new { imagePath = relativePath });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error uploading file.");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteStudent(long id)
        {
            try
            {
                var success = _studentRepository.DeleteStudentById(id);
                if (!success)
                {
                    return NotFound($"Student with Id {id} not found.");
                }
                return Ok($"Student with Id {id} has been deleted successfully.");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting student.");
            }
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
