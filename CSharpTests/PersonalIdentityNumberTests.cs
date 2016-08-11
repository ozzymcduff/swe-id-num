using System;
using Xunit;
using SweIdNum.Core;
using SweIdNum;
using System.Collections.Generic;
using System.Linq;

namespace CSharpTests
{
    public class PersonalIdentityNumberTests
    {
        [Fact]
        public void FiltersAndRemovesPlusAndMinusCharactersCorrectly()
        {
            Assert.Equal("201212121212", PersonalIdentityNumbers.Parse("121212-1212").ToString());
            Assert.Equal("191212121212", PersonalIdentityNumbers.Parse("121212+1212").ToString());

            Assert.Equal("191212121212", PersonalIdentityNumbers.Parse("19121212-1212").ToString());
        }

        [Fact]
        public void PlusAndMinusCharactersOnlyAllowedInCertainPositions()
        {
            PersonalIdentityNumber v;
            Assert.True(PersonalIdentityNumbers.TryParse("121212-1212", out v));
            Assert.True(PersonalIdentityNumbers.TryParse("121212+1212", out v));
            Assert.False(PersonalIdentityNumbers.TryParse("12121212-12", out v));
            Assert.False(PersonalIdentityNumbers.TryParse("12121212+12", out v));
            Assert.False(PersonalIdentityNumbers.TryParse("121-212-1212", out v));
            Assert.False(PersonalIdentityNumbers.TryParse("121212+12+12", out v));
        }
        public static IEnumerable<object[]> MalePersonalNumbers()
        {
            return new[] {
                "121212-1212","19121212-1212"}.Select(p => new object[] { p });
        }
        public static IEnumerable<object[]> FemalePersonalNumbers()
        {
            return new[] {
                "121212-1220","19121212-1220"}.Select(p => new object[] { p });
        }
        [Theory,
            MemberData("MalePersonalNumbers")]
        public void Male_personal_numbers(string personalNumber)
        {
            Assert.True(PersonalIdentityNumbers.Parse(personalNumber).IsMale());
            Assert.False(PersonalIdentityNumbers.Parse(personalNumber).IsFemale());
        }
        [Theory,
            MemberData("FemalePersonalNumbers")]
        public void Female_personal_numbers(string personalNumber)
        {
            Assert.False(PersonalIdentityNumbers.Parse(personalNumber).IsMale());
            Assert.True(PersonalIdentityNumbers.Parse(personalNumber).IsFemale());
        }
        public static IEnumerable<object[]> ValidPersonalNumbers()
        {
            return new[] {
                "1212121212", "191212121212", "19121212-1212", "121212-1212", "121212-1212"}.Select(p => new object[] { p });
        }

        [Theory,
            MemberData("ValidPersonalNumbers")]
        public void ValidPersonalNumbers_ParsedAsValid(string personalNumber)
        {
            PersonalIdentityNumber pn;

            Assert.True(PersonalIdentityNumbers.TryParse(personalNumber, out pn));
        }

        public static IEnumerable<object[]> InvalidPersonalNumbers() 
        {
            return new[] { "127102240475", "19710XY40475", "19710224=0475", "1971", "14532436-45", "556194-7986", "262000-0113", "460531-12" }.Select(p => new object[]{p});
        }

        [Theory,
            MemberData("InvalidPersonalNumbers")]
        public void InvalidPersonalNumbers_ParsedAsNotValid(string personalNumber)
        {
            PersonalIdentityNumber pn;

            Assert.False(PersonalIdentityNumbers.TryParse(personalNumber, out pn));
        }

        [Fact]
        public void TwoNumbers_ConsideredEqual_IfNormalizedNumbersAreEqual()
        {
            Assert.True(PersonalIdentityNumbers.Parse("1212121212").Equals(PersonalIdentityNumbers.Parse("201212121212")), "A");
            Assert.True(PersonalIdentityNumbers.Parse("1212121212").Equals(PersonalIdentityNumbers.Parse("1212121212")), "B");
            Assert.True(PersonalIdentityNumbers.Parse("1212121212").Equals(PersonalIdentityNumbers.Parse("20121212-1212")), "C");
        }
    }
}

