using System;
using System.Collections.Generic;
using Nancy;

namespace NinjaGold
{
    public class MainModule : NancyModule
    {
        public MainModule()
        {
            Get("/", _ =>
            {
                if(Session["gold"] == null)  Session["gold"] = 0;
                if(Session["action"] == null)  Session["action"] = new List<string>();
                List<string> ActList, Reverse = new List<string>();
                ActList = (List<string>)Session["action"];
                for(int i = ActList.Count-1; i >= 0; i--)
                    Reverse.Add(ActList[i]);
                ViewBag.Gold = (int)Session["gold"];
                return View["index", Reverse];
            });

            Get("/process/{place}", args =>
            {
                Random Rand = new Random();
                Dictionary<string, int> data = new Dictionary<string, int> {
                    ["farm"]   = Rand.Next(10, 21), 
                    ["cave"]   = Rand.Next(5, 11),
                    ["house"]  = Rand.Next(2, 6),
                    ["casino"] = Rand.Next(-50, 51)
                };
                int gold = data[args.place];
                string now = DateTime.Now.ToString("yyyy/MM/dd,  h:mm tt");
                string act;
                if (gold > 0)
                    act = $"<p id='green'>Earned {gold} golds from the {args.place}! ({now})</p>";
                else if (gold < 0)
                    act = $"<p id='red'>Entered a casino and lost {gold} golds... Ouch.. ({now})</p>";
                else 
                    act = $"<p>Entered a casino and got nothing... ({now})</p>";
                Session["gold"] = (int)Session["gold"] + gold;
                var action = new List<string>();
                action = (List<string>)Session["action"];
                action.Add(act);
                Session["action"] = action;
                return Response.AsRedirect("/");
            });

            Get("/reset", _ =>
            {
                Session.DeleteAll();
                return Response.AsRedirect("/");
            });
        }
    }
}