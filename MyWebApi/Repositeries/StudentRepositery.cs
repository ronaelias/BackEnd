using MyWebApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace MyWebApi.Repositories
{
    public class StudentRepository
    {
        private static readonly List<Student> students = new List<Student>
        {
            new Student { Id = 1, Firstname = "Omar", Lastname = "Al-Mansoori", FatherName = "Ahmed", MotherName = "Fatima", Email = "omar.almansoori@gmail.com", Age = 21 },
            new Student { Id = 2, Firstname = "Layla", Lastname = "Hassan", FatherName = "George", MotherName = "Amina", Email = "layla.hassan@gmail.com", Age = 22 },
            new Student { Id = 3, Firstname = "Youssef", Lastname = "Ali", FatherName = "Nabil", MotherName = "Mona", Email = "youssef.ali@gmail.com", Age = 20 },
            new Student { Id = 4, Firstname = "Maya", Lastname = "Khalil", FatherName = "Rami", MotherName = "Salma", Email = "maya.khalil@gmail.com", Age = 23 },
            new Student { Id = 5, Firstname = "Ahmed", Lastname = "Fares", FatherName = "Jamal", MotherName = "Nadia", Email = "ahmed.fares@gmail.com", Age = 24 },
            new Student { Id = 6, Firstname = "Sara", Lastname = "Nasser", FatherName = "Michael", MotherName = "Hala", Email = "sara.nasser@gmail.com", Age = 22 },
            new Student { Id = 7, Firstname = "Ibrahim", Lastname = "Jaber", FatherName = "Peter", MotherName = "Rana", Email = "ibrahim.jaber@gmail.com", Age = 21 },
            new Student { Id = 8, Firstname = "Nadia", Lastname = "Younis", FatherName = "Khaled", MotherName = "Laila", Email = "nadia.younis@gmail.com", Age = 23 },
            new Student { Id = 9, Firstname = "Rami", Lastname = "Tariq", FatherName = "Samir", MotherName = "Mariam", Email = "rami.tariq@gmail.com", Age = 20 },
            new Student { Id = 10, Firstname = "Hana", Lastname = "Saeed", FatherName = "Omar", MotherName = "Aisha", Email = "hana.saeed@gmail.com", Age = 24 }
        };

        public IEnumerable<Student> GetAllStudents() => students;

        public Student GetStudentById(long id) => students.FirstOrDefault(s => s.Id == id);

        public bool DeleteStudentById(long id) {
            var student = GetStudentById(id);
            if (student != null) {
                students.Remove(student);
                return true; 
            }
            return false;
        }
    }
}
