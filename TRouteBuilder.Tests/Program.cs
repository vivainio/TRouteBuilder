using Microsoft.AspNetCore.Mvc;
using TRouteBuilder;


// just some demo stuff
namespace Test.TRouteBuilder
{

    class FakeController1 : ControllerBase
    {
        [Route("world/{id}")]
        public void GetStuff(string id)
        {

        }
    }

    class FakeController2 : ControllerBase
    {
        [Route("hello/world")]
        public void GetStuff()
        {

        }
    }

    class FakeController3 : ControllerBase
    {
        [Route("{a}/{b}/{c}")]
        public string ManyVars(string a, string b, int c)
        {
            return a + b + c.ToString();
        }
    }

    static class Program
    {
        static void Main(string[] args)
        {
            var url2 = RouteBuilder.ResolveRoute<FakeController2>(c => c.GetStuff());
            var fail = RouteBuilder.Fill("foo/{id}", "id", "world");

            var url1 = RouteBuilder.ResolveRoute<FakeController1>(c => c.GetStuff("foo"))
                .Fill("id", "1212");


            var linker = new RelativeUrlBuilder("/myapproot");
            var rel = linker.To<FakeController3>(c => c.ManyVars("", "", 0)).Fill("a", "12", "b", "13", "c", "14");
        }
    }
}
