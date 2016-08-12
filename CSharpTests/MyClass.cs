using System;
using SweIdNum.Core;
using SweIdNum;
namespace CSharpTests
{
    public class MyClass
    {
        public MyClass()
        {
            // just to test that it compiles:
            var x = PersonalIdentityNumbers.Parse("196408233234");
            string p = x.ToString("P");
            DateTime d =x.GetDate();
            Console.WriteLine("{0} {1}", p, d);
            if (PersonalIdentityNumbers.TryParse("196408233234", out x)) 
            {
            }
            var o = OrganizationalIdentityNumbers.Parse("556000-4615");
            p = o.ToString();
            if (OrganizationalIdentityNumbers.TryParse("556000-4615", out o))
            {
            }
        }
    }
}

