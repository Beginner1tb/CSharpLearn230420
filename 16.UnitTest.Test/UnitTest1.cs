using System;
using Xunit;
using _15.UnitTest;
using FluentAssertions;

namespace _16.UnitTest.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Calc_Add_Test()
        {
            Calc calc = new Calc();
            int res = calc.Addnumber(2, 3);
            //Assert.Equal(5, res);
            res.Should().Be(5);
        }

        [Fact]
        public void StringOut_Test()
        {
            StrOut strOut = new StrOut();
            string outRes = strOut.SimpleStrOutput();
            Assert.Equal("1111", outRes);
        }
        [Fact]
        public void Add_Method_Test()
        {
            int num = Program.addTest1();
            Assert.Equal(3, num);
        }
        
        [Theory]
        [InlineData(1,2,3)]
        public void Add_Method_ReturnInt(int a,int b,int expected)
        {
            //Arrange
            var add = new Calc();
            //Act
            var res=add.Addnumber(a, b);
            //Assert
            res.Should().Be(expected);
        }
    }
}
