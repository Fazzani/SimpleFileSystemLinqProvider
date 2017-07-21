using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LinqFileSystemProvider
{
    /// <summary>
    /// See 'Implementing a custom LINQ provider'<para>
    /// http://jacopretorius.net/2010/01/implementing-a-custom-linq-provider.html </para>
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("LinqFileSystemProvider");
            if (0 < args.Count())
            {
                var query = from element in new FileSystemContext(args[0])
                            where element.ElementType == ElementType.File && element.Path.EndsWith(".m3u")
                            orderby element.Path ascending
                            select new { element.Path };

                var query1 = from element in new FileSystemContext(args[0])
                             where element.ElementType == ElementType.File && element.Path.EndsWith(".zip")
                             orderby element.Path ascending
                             select element.Path;

                var query2 = from element in new FileSystemContext(args[0])
                             where element.ElementType == ElementType.File && element.Path.EndsWith(".zip")
                             orderby element.Path ascending
                             select element;

                foreach (var result in query1)
                    Console.WriteLine($"Result '{result.ToString()}'");
            }
            else
            {
                Console.WriteLine("Usage: LinqFileSystemProvider <root folder name>");
            }
            Console.ReadKey();
        }
    }
}
