using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ArtConspiracy.Models;
using System.Dynamic;

namespace ArtConspiracy.Controllers
{
    public class ShowMembersController : Controller
    {
        // GET: ShowMembers

        private Models.ArtConspiracyEntities db = new Models.ArtConspiracyEntities();
        public ActionResult Index()
        {
            return View(db.AspNetUsers);
        }

        public ActionResult ViewMember(string id)
        {
            var piecesProc = db.GetMemberArt(id).ToList();
            ViewBag.MemberArt = piecesProc;
            if (piecesProc.Count() == 0)
            {
                TempData["emptyMessage"] = "No artwork uploaded yet.";
            }
            ViewData["MemberInfo"] = db.AspNetUsers.Find(id);
            

            return View();
        }

    }
}