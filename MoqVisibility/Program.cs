using System;
using Moq;
using MyLibrary;
using System.Runtime.CompilerServices;

//[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace MoqVisibility
{
  class Program
  {
    static void Main(string[] args)
    {
      var mock = new Mock<MyTestClass>();
      mock.Object.PublicMethod(); //does not show output from ProtectedInternalVirtualMethod -> virtual methods are overriden with default implementation (=empty method)
      Console.WriteLine();

      var mock2 = new Mock<MyTestClass>() { CallBase = true };
      mock2.Object.PublicMethod(); //shows output from ProtectedInternalVirtualMethod
      Console.WriteLine();

      var mock3 = new Mock<MyTestClass>();
      mock3.Setup(m => m.ProtectedInternalVirtualMethod()).Callback(() => { Console.WriteLine($"{nameof(mock3.Object.ProtectedInternalVirtualMethod)} has been mocked"); });
      mock3.Object.PublicMethod();
      Console.WriteLine();

      //var mock4 = new Mock<MyTestSubClass>();
      //mock4.Object.CallProtectedMethod();
      //Console.WriteLine();

      //var mock5 = new Mock<MyTestSubClass>();
      //mock5.Object.PublicMethod();
      //mock5.Object.CallProtectedVirtualMethod();
      //Console.WriteLine();

      //var mock6 = new Mock<MyTestClass>();
      //mock6.Setup(m => m.ProtectedInternalMethod()).Callback(() => { Console.WriteLine($"{nameof(mock6.Object.ProtectedInternalMethod)} has been mocked"); });
      //Does not work because the method is not virtual -> runtime error
      //mock6.Object.PublicMethod();
      //Console.ReadLine();

      var mock7 = new Mock<MyTestClass>();
      mock7.Setup(m => m.InternalVirtualMethod()).Callback(() => { Console.WriteLine($"{nameof(mock7.Object.InternalVirtualMethod)} has been mocked"); });
      //When InternalsVisibleTo("DynamicProxyGenAssembly2") is omitted on MyTestClass, this shows the output for InternalVirtualMethod from MyTestClass 
      //(i.e. no mocking happened (as set up here) and also no default implementation was called (because of the fact that the mock class cannot see InternalVirtualMethod)
      mock7.Object.PublicMethod();
       Console.ReadLine();
    }
  }

  class MyTestSubClass : MyTestClass
  {
    public void CallProtectedMethod()
    {
      ProtectedMethod();
    }

    protected override void ProtectedVirtualMethod()
    {
      CallProtectedVirtualMethod();
      Console.WriteLine($"Called {nameof(CallProtectedVirtualMethod)}");
    }

    public void CallProtectedVirtualMethod()
    {
      base.ProtectedVirtualMethod();
    }
  }
}
