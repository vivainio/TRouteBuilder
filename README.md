# TRouteBuilder
Generate ASP.NET route links by Expressions

In ASP.net web api, creating links to api controllers is left as an "excercise for the reader".

Some existing approaches are:

- Use UrlHelper and create the url's with "stringly typed" approach. You avoid having assembly references to the api controllers you are
linking to, but since you are using strings your code links will be almost as brittle as typing out the url's directly.

- HyprLinkr. Works fine with convention based routes, but doesn't work with [RoutePrefix] + [Route] approach and doesn't allow having the full
{controller}/{action} based routes (so you are stuck with one http endpoint per controller). It's also very slow, so creating several routes
can dominate your api controller execution profile (we are talking hundreds of msec).

The approach implemented here is:

- You use [RoutePrefix] and [Route] to declare the routes for your api endpoints
- You use RouteMapper or one of the helpers to generate the url's by concatenating these, using linq expressions (like in HyprLinkr) to refer to these

You will be using RouteLinker like so:

```csharp
            var url1 = RouteBuilder.ResolveRoute<FakeController1>(c => c.GetStuff(""))
                .Fill("id", "1212");

            var url2 = RouteBuilder.ResolveRoute<FakeController2>(c => c.GetStuff());
            var fail = RouteBuilder.Fill("foo/{id}", "id", "world");

            var linker = new RelativeUrlBuilder("/myapproot");
            var rel = linker.To<FakeController3>(c => c.ManyVars("", "", 0)).Fill("a", "12", "b", "13", "c", "14");

```

## Installation

- Add TRouteLinker.cs to your project

## License

MIT
