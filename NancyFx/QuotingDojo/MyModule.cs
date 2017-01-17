using System.Text.RegularExpressions;
using Nancy;

namespace QuotingDojo
{
    public class MainModule : NancyModule
    {
        public MainModule()
        {
            Get("/", _ =>
            {
                ViewBag.errors = Session["errors"];
                return View["index"];
            });

            Post("/quotes", _ =>
            {
                Regex rgx = new Regex(@"^[a-zA-Z\s]+$");
                string quote = Request.Form.quote.ToString().Trim();
                string author = Request.Form.name.ToString().Trim();
                string errors = "";
                if(author == "") 
                    errors += "<p class='errors'>Please input your name!</p>";
                else if(!rgx.IsMatch(author))
                    errors += "<p class='errors'>Name should be letters only!</p>";
                else if(author.Length < 3)
                    errors += "<p class='errors'>Too short! Name should be at least 2 letters long!</p>";
                if(quote == "") 
                    errors += "<p class='errors'>Please input your quote!</p>";
                Session["errors"] = errors;

                if(errors.Length == 0)
                {
                    DbConnector.ExecuteQuery($"INSERT INTO qutoes (quote, author, likes, created_at, updated_at) VALUES ('{quote}', '{author}', 0, NOW(), NOW())");
                    return Response.AsRedirect("/quotes");
                } 
                else  return Response.AsRedirect("/");
            });

            Get("/quotes", _ =>
            {
                var quotes = DbConnector.ExecuteQuery("SELECT id, quote, author, likes, DATE_FORMAT(created_at, '%l:%i%p %M %e %Y') AS time FROM qutoes ORDER BY likes DESC");
                return View["quotes", quotes];
            });

            Get("/quotes/{id}", args =>
            {
                var quote = DbConnector.ExecuteQuery($"SELECT likes FROM qutoes WHERE id={args.id}");
                int likes = (int)quote[0]["likes"];
                likes += 1;
                DbConnector.ExecuteQuery($"UPDATE qutoes SET likes={likes} WHERE id={args.id}");
                return Response.AsRedirect("/quotes");
            });

            Get("/back", _ =>
            {
                Session["errors"] = "";
                return Response.AsRedirect("/");
            });
        }
    }
}