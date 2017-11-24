using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Http;

namespace TRouteBuilder
{
    // expands {foo}/{bar} strings to real values
    public class RouteFiller
    {
        public RouteFiller(string t)
        {
            this.Template = t;
        }
        public string Fill(params string[] values) =>
            RouteBuilder.Fill(Template, values);

        public string Value => Template;
        public readonly string Template;
    };

    // Use this in dependency injector or whatever to create urls starting with specific base path (e.g. httpcontext.VirtualApplicationRoot)
    public class RelativeUrlBuilder
    {
        private readonly string Root;

        public RelativeUrlBuilder(string root)
        {
            this.Root = root;
        }

        public RouteFiller To<Klass>(Expression<Action<Klass>> exp)
        {
            var url = RouteBuilder.ResolveRoute(exp).Value;
            return new RouteFiller(Root + "/" + url);
        }
    }

    // The class that does the actual work
    public static class RouteBuilder
    {
        public static RouteFiller ResolveRoute<Klass>(Expression<Action<Klass>> exp)
        {
            var mi = (exp.Body as MethodCallExpression)?.Method;
            var prefixAttrib = mi.DeclaringType.GetCustomAttribute<RoutePrefixAttribute>()?.Prefix;
            var routeAttrib = mi.GetCustomAttribute<RouteAttribute>()?.Template;
            var value = prefixAttrib == null ? routeAttrib : $"{prefixAttrib}/{routeAttrib}";
            return new RouteFiller(value);
        }

        public static string Fill(string template, params string[] values)
        {
            var result = template;
            if (values.Length % 2 != 0) throw new ArgumentException("Replacements must be array of key value pairs");
            for (var i = 0; i < values.Length; i += 2)
            {
                var pat = "{" + values[i] + "}";
                var repl = values[i + 1];
                var changed = result.Replace(pat, repl);
                if (changed == result)
                {
                    throw new ArgumentException($"RouteBuilder didn't find {pat} in '{repl}'");
                }
                result = changed;
            }

            return result;
        }
    }
}
