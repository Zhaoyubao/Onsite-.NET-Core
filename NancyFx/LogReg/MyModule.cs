using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Nancy;
using CryptoHelper;

namespace LogReg
{
    public class MainModule : NancyModule
    {
        public MainModule()
        {
            Get("/", _ =>
            {
                if(Session["UserId"] != null) 
                    return Response.AsRedirect("/main");
                ViewBag.errors = Session["errors"];
                return View["index"];
            });

            Post("/register", _ =>
            {
                string fname = Request.Form.fname;
                string lname = Request.Form.lname;
                string email = Request.Form.email;
                string pw = Request.Form.pw;
                Session["errors"] = ValidateReg(fname, lname, email, pw, Request.Form.pw_confirm);
                if(Session["errors"] == "")
                {
                    string pw_hash = Crypto.HashPassword(pw);
                    DbConnector.ExecuteQuery($"INSERT INTO users (first_name, last_name, email, pw_hash, created_at, updated_at) VALUES ('{fname}', '{lname}', '{email}', '{pw_hash}', NOW(), NOW())");
                    var users = GetUser(email);
                    Session["UserId"] = users[0]["id"];
                    return Response.AsRedirect("/main");
                } 
                else  return Response.AsRedirect("/");
            });

            Post("/login", _ => 
            {
                Session["errors"] = ValidateLog(Request.Form.email, Request.Form.pw);
                if(Session["errors"] == "")
                    return Response.AsRedirect("/main");
                return Response.AsRedirect("/");
            });

            Get("/main", _ =>
            {
                if(Session["UserId"] == null) 
                    return Response.AsRedirect("/");
                int id = (int)Session["UserId"];
                var users = DbConnector.ExecuteQuery($"SELECT * FROM users WHERE id={id}");
                return View["main", users];
            });

            Get("/logout", _ =>
            {
                Session.DeleteAll();
                return Response.AsRedirect("/");
            });
        }

        public string ValidateReg(string fname, string lname, string email, string pw, string pw_confirm)
        {
            Regex NameRegex = new Regex(@"^[a-zA-Z]+$");
            Regex EmailRegex = new Regex(@"^[a-zA-Z0-9\.\+_-]+@[a-zA-Z0-9\._-]+\.[a-zA-Z]+$");
            Regex PwRegex = new Regex(@"^[A-Za-z\d@$!%*?&]*$");
            string errors = "";

            if(fname == "") 
                errors += "<p class='errors fname'>Please enter your first name!</p>";
            else if(!NameRegex.IsMatch(fname))
                errors += "<p class='errors fname'>Name should be letters only!</p>";
            else if(fname.Length < 3)
                errors += "<p class='errors fname'>Too short!Name should be at least 2 letters!</p>";
            
            if(lname == "") 
                errors += "<p class='errors lname'>Please enter your last name!</p>";
            else if(!NameRegex.IsMatch(lname))
                errors += "<p class='errors lname'>Name should be letters only!</p>";
            else if(lname.Length < 2)
                errors += "<p class='errors lname'>Too short!Name should be at least 2 letters!</p>";
            
            if(email == "")
                errors += "<p class='errors email'>Please enter your email!</p>";
            else if(!EmailRegex.IsMatch(email))
                errors += "<p class='errors email'>Email is invalid!</p>";
            else if(GetUser(email).Count == 1)
                errors += "<p class='errors email'>Email already exists!</p>";

            if(pw == "") 
                errors += "<p class='errors pw'>Please create a new password!</p>";
            else if(!PwRegex.IsMatch(pw) || pw.Length < 8)
                errors += "<p class='errors pw'>Please create a valid password as per the criteria!</p>";

            if(pw != pw_confirm)
                errors += "<p class='errors confirm'>The passwords entered don't match!</p>";
            return errors;
        }

        public string ValidateLog(string email, string pw)
        {
            string errors = "";
            var users = GetUser(email);
            if(users.Count == 1)
            {
                string pw_hash = users[0]["pw_hash"].ToString();
                bool valid = Crypto.VerifyHashedPassword(pw_hash, pw);
                if(valid)
                    Session["UserId"] = users[0]["id"];
                else
                    errors += "<p class='errors login_pw'>Incorrect password!</p>";
            }    
            else
                errors += "<p class='errors login_email'>Email address does not exist!</p>";
            return errors;
        } 

        public List<Dictionary<string, object>> GetUser(string email)
        {
            var users = DbConnector.ExecuteQuery($"SELECT * FROM users WHERE email='{email}'");
            return users;
        }
    }
}