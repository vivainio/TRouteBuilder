using TRouteBuilder;
using System.Web.Http;


// just some demo stuff
namespace Test.TRouteBuilder
{

    [RoutePrefix("hello")]
    class FakeController1 : ApiController
    {
        [Route("world/{id}")]
        public void GetStuff(string id)
        {

        }
    }

    class FakeController2 : ApiController
    {
        [Route("hello/world")]
        public void GetStuff()
        {

        }
    }

    class FakeController3 : ApiController
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
            var url1 = RouteBuilder.ResolveRoute<FakeController1>(c => c.GetStuff("foo"))
                .Fill("id", "1212");

            var url2 = RouteBuilder.ResolveRoute<FakeController2>(c => c.GetStuff());
            var fail = RouteBuilder.Fill("foo/{id}", "id", "world");

            var linker = new RelativeUrlBuilder("/myapproot");
            var rel = linker.To<FakeController3>(c => c.ManyVars("", "", 0)).Fill("a", "12", "b", "13", "c", "14");
        }
    }
}
