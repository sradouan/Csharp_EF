
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Security.AccessControl;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace Queries
{
    class Program
    {
        static void Main(string[] args)
        {


                UseExplicitloding(); //load related
                //UseSqlProfiler();
                //UseAggregationFunctions
                //UseGroubByExtension();
                //UseGroupJoin();
                //UsejoinBetweenTables();
                //UseGroubBy();
                //UseLinq();
                // UseExtensionMethod();
                Console.ReadKey();

        }

        static void UseLinq()
        {
            var context = new PlutoContext();
            var query =
                from c in context.Courses
                where c.Name.Contains("C#")
                orderby c.Name
                select c;
            foreach (var course in query)
            {
                Console.WriteLine("Data is {0},{1}",course.Id,course.Name);

            }
        }

        static void UseExtensionMethod()
        {
            var context = new PlutoContext();

            var courses = context.Courses
                .Where(c => c.Name.Contains("C#"))
                .OrderBy(c => c.Name)
                .OrderByDescending(c => c.Level)
                .Select(c => new {CourseName = c.Name, AuthorName = c.Author.Name}); //we can getAuthorName from the table authors  using navigation instead of join

             //var courses = context.Courses.Where(c => c.Name.Contains("C#")).OrderBy(c => c.Name);

            foreach (var course in courses)
            {
                Console.WriteLine(course.CourseName);

            }

        }

        static void UseGroubBy()
        {
            var context = new PlutoContext();
            var query =
                from c in context.Courses
                 group c by c.Level into g
                 select g;
               
            foreach (var group in query)
            {
                Console.WriteLine("{0}/{1}",group.Key,group.Count());

                foreach (var course  in group)
                {
                    Console.WriteLine("\t{0}",course.Name);
                }

            }

            //  var groups = context.Courses.GroupBy(c => c.Level);//important line
        }

        static void UseAnonymObject()
        {
            var context = new PlutoContext();
            var query =
                from c in context.Courses
                where c.Name.Contains("C#")
                orderby c.Name
                select new {Name = c.Name, Id = c.Id};

        }

        static void UsejoinBetweenTables()
        {
            var context = new PlutoContext();
            var query =
                from c in context.Courses
                join a in context.Authors on c.AuthorId equals a.Id
                select new { CourseName = c.Name, AuthorName = a.Name }; //we have  just foreinKey
                                                                         //select new {CourseName = c.Name, AuthorName = c.Author.Name}; //we have object author
                 foreach (var x in query)
                 {
                     Console.WriteLine("Course/Author: {0},{1}", x.CourseName, x.AuthorName);

                 }

                 //ExtensionMethod
                 var courses = context.Courses.Join(context.Authors, c => c.AuthorId, a => a.Id,
                     (course, author) => new {CourseName = course.Name, AuthorName = author.Name}); //new anonymous objec
                //EndExtenstionMethod for join between tables

        }

        static void UseGroupJoin()
        {
            var context = new PlutoContext();
            var query =
                from a in context.Authors
                join c in context.Courses on a.Id equals c.AuthorId into g
                select new { AuthorName = a.Name, CoursesNumber = g.Count() };

            foreach (var  author in query)
            {
                Console.WriteLine("Author/NbrOfCourses: {0},{1}", author.AuthorName, author.CoursesNumber);

            }


            //ExtensionMethod
               context.Authors.GroupJoin(context.Courses,  a => a.Id, c => c.AuthorId,
                (author,courses) => new{
                   
                    AuthorName = author.Name,
                    CourseCount =courses.Count()
                }); //new anonymous objec
            //EndExtenstionMethod for join between tables

        }
        static void UseGroubByExtension()
        {
            var context = new PlutoContext();
            var groups = context.Courses.GroupBy(c => c.Level);//important line

            foreach (var group in groups)
            {
                Console.WriteLine("{0}/{1}", group.Key, group.Count());

                foreach (var course in group)
                {
                    Console.WriteLine("\t{0}", course.Name);
                }

            }
        }

        static void UseAggregationFunction()
        {
            var context = new PlutoContext();
            context.Courses.OrderBy(c => c.Level).FirstOrDefault(c => c.FullPrice > 100);
            context.Courses.OrderBy(c => c.Level).LastOrDefault(c => c.FullPrice > 100); //not used with Sql order by and get first
            context.Courses.OrderBy(c => c.Level).SingleOrDefault(c => c.Id==1);
            var allabove10Dollars = context.Courses.All(c => c.FullPrice > 10);
            var isFindOne = context.Courses.Any(c => c.Level==1);

            var Course1 = context.Courses.Find(1);//PrimaryKey-==1

            var count = context.Courses.Count();
            var count2 =context.Courses.Where(c => c.Level ==1).Count();
            var max = context.Courses.Max(c=>c.FullPrice);
            var min = context.Courses.Min(c => c.FullPrice);
            var avg = context.Courses.Average(c => c.FullPrice);


        }

        static void UseSqlProfiler()
        {
            var context = new PlutoContext();

            var courses = context.Courses;
            var filtred = courses.Where(c => c.Level == 1);
            var sorted = courses.OrderBy(c => c.Name);

            foreach (var course in filtred)
            {
                Console.WriteLine(course.Name);

            }

        }

        //Load related Objects
        static void UseExplicitloding()
        { 
            var context = new PlutoContext();

            var author = context.Authors.Single(a=>a.Id==2);
             context.Courses.Where(c => c.AuthorId == author.Id && c.FullPrice==0).Load(); //add filter for more more efficiently


            foreach (var course in author.Courses)
            {
                Console.WriteLine("{0},{1}",course.Author.Name,course.Name);

            }

        }

    }
}
