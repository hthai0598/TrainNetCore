using System;
using System.Reflection;

namespace CustomeAttribute
{
    class Program
    {
        static void Main(string[] args)
        {
            var props = typeof(ClientExcel).GetProperties();
            foreach (var item in props)
            {
                Console.WriteLine($"props name {item}");
                if (!Attribute.IsDefined(item, typeof(ExcelColumnAttribute)))
                {
                    Console.WriteLine(item.GetCustomAttribute<ExcelColumnAttribute>().Name);
                }
            }
        }
    }

    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
    public class ExcelColumnAttribute : Attribute
    {
        public ExcelColumnAttribute(string column)
        {
            this.column = column;
        }
        public string column { get; set; }
        public string Name { get; set; }
    }

    public class ClientExcel
    {
        [ExcelColumnAttribute("FullName")]
        public string ClientName { get; set; }
    }
}
