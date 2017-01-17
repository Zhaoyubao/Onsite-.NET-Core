using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Notes.Models;
using Notes.Factory;

namespace Notes.Controllers
{
    public class HomeController : Controller
    {
        private readonly NoteFactory noteFactory;
        public HomeController(NoteFactory note)
        {
            noteFactory = note;
        }

// ================================================================================
        [HttpGet("")]
        public IActionResult Index()
        {
             ViewBag.Notes = noteFactory.FindAll();
             return View();
        }

        [HttpGet("main")]
         public IActionResult Main()
        {
             ViewBag.Notes = noteFactory.FindAll();
             return View();
        }
// Return partial html ===========================================================

        [HttpGet("notes")]
        public IActionResult Notes()
        {
            ViewBag.Notes = noteFactory.FindAll();
            return View();
        }
// Return Json data ==============================================================

        [HttpGet("show")]
        public IEnumerable<Note> Show() => noteFactory.FindAll();

        [HttpGet("show/{id}")]
        public IActionResult ShowNote(int id)
        {
            var note = noteFactory.FindById(id);
            return new ObjectResult(note);
        }
// ================================================================================

        [HttpPost("notes")]
        public IActionResult Create(Note note)
        {
            noteFactory.Add(note);
            return RedirectToAction("Notes");
            // return RedirectToAction("Show");
        }
// ================================================================================

        [HttpPost("notes/{id}")]
        public IActionResult Update(int id, Note note)
        {
            noteFactory.UpdateByID(id, note);
            return RedirectToAction("Notes");
            // return RedirectToAction("Show");
        }
// ================================================================================

        [HttpGet("notes/{id}")]
        public IActionResult Delete(int id)
        {
            noteFactory.DelByID(id);
            return RedirectToAction("Notes");
            // return RedirectToAction("Show");
        }

    }
}
