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
            System.Console.WriteLine("LinqFileSystemProvider");
            if (0 < args.Count())
            {
                var query = from element in new FileSystemContext(args[0])
                            where element.ElementType == ElementType.File && element.Path.EndsWith(".zip")
                            orderby element.Path ascending
                            select element;

                int i = 0;

                foreach (var result in query)
                {
                    StringBuilder s = new StringBuilder();
                    s.AppendFormat("Result {0} '{1}'", ++i, result.ToString());
                    System.Console.WriteLine(s.ToString());
                }
                //  The following code works and serves to confirm that 
                //  FileSystemContext.GetEnumerator() does what you expect.
                //TraceFileSystem(args[0]);
            }
            else 
            {
                System.Console.WriteLine("Usage: LinqFileSystemProvider <root folder name>");
            }
            Console.ReadKey();
         }

        static void TraceFileSystem(string root)
        {
            var query = FileSystemQueryContext.GetAllFilesAndFolders(root).AsEnumerable<FileSystemElement>();
            int i = 0;
            int n;
            for (n = 0; n != query.Count(); ++n)
            {
                var result = query.ElementAt(n);
                StringBuilder s = new StringBuilder();
                s.AppendFormat("Result {0,3} '{1}'", ++i, result.ToString());
                System.Console.WriteLine(s.ToString());
            }
        }
    }
}
