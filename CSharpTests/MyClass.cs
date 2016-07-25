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
            PersonalIdentityNumber x = PersonalIdentityNumbers.Parse("196408233234");
            string p = x.ToString("P");
            DateTime d =x.GetDate();
            string n = x.GetControlNumber();
            bool c = x.Control();
            Console.WriteLine("{0} {1} {2} {3}", p, d, n, c);
            if (PersonalIdentityNumbers.TryParse("196408233234", out x)) 
            {
            }
            OrganizationalIdentityNumber o = OrganizationalIdentityNumbers.Parse("556000-4615");
            p = o.ToString();
            if (OrganizationalIdentityNumbers.TryParse("556000-4615", out o))
            {
            }
        }
    }
}

