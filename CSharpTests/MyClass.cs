using System;
using SweIdNum.Core;
using SweIdNum;
namespace CSharpTests
{
    public class MyClass
    {
        public MyClass()
        {
            PersonalIdentityNumber x = PersonalIdentityNumbers.Parse("196408233234");
            string p = x.ToString("P");
            DateTime d =x.GetDate();
            string n = x.GetControlNumber();
            bool c = x.Control();
        }
    }
}

