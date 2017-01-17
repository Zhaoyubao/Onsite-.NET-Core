using System;
using System.Collections.Generic;
using Nancy;
using ApiCaller;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PokeInfo
{
    public class MainModule : NancyModule
    {
        public MainModule()
        {
            Get("/", _ => View["index"]);

            Get("/pokemon/{id}", async param =>
            {
                await WebRequest.SendRequest( $"http://pokeapi.co/api/v2/pokemon/{param.id}", new Action<Dictionary<string, object>> ( JsonResponse => 
                    {
                        JArray types = (JArray)JsonConvert.DeserializeObject(JsonResponse["types"].ToString()); 
                        JArray stats = (JArray)JsonConvert.DeserializeObject(JsonResponse["stats"].ToString());

                        ViewBag.Id = param.id;
                        ViewBag.Name = JsonResponse["name"];
                        ViewBag.Weight = JsonResponse["weight"];
                        ViewBag.Height = JsonResponse["height"];
                        ViewBag.PriType = types[0]["type"]["name"];
                        ViewBag.SecType = types.Count == 1 ? "" : types[1]["type"]["name"];
                        ViewBag.Hp = stats[5]["base_stat"];
                        ViewBag.Attack = stats[4]["base_stat"];
                        ViewBag.Defense = stats[3]["base_stat"];
                        ViewBag.SpAtk = stats[2]["base_stat"];
                        ViewBag.SpDef = stats[1]["base_stat"];
                        ViewBag.Speed = stats[0]["base_stat"];
                    }
                ));
                ViewBag.imageUrl = $"<img src='https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/{param.id}.png'>";
                return View["partial"];
            });
        }
    }
}