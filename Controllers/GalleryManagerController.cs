using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ArtConspiracy.Models;
using System.Data.Entity;
using System.IO;

namespace ArtConspiracy.Controllers
{
    public class GalleryManagerController : Controller
    {

        private Models.ArtConspiracyEntities db = new Models.ArtConspiracyEntities();

        // GET: GalleryManager
        [Authorize]
        public ActionResult Index()
        {
            var id = User.Identity.GetUserId();
            var piecesProc = db.GetMemberArt(id).ToList();
            ViewBag.GalleryItems = piecesProc;
            return View();
        }

        [Authorize]
        public ActionResult CreateGalleryItem()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult CreateGalleryItem(ArtPiece artpiece)
        {
            if (ModelState.IsValid)
            {

                ArtPiece newArtPiece = new ArtPiece();

                newArtPiece.MID = User.Identity.GetUserId();
                newArtPiece.PieceTitle = artpiece.PieceTitle;
                newArtPiece.PieceDesc = artpiece.PieceDesc;

                db.ArtPieces.Add(newArtPiece);

                db.SaveChanges();

                int pid = newArtPiece.PID;

                HttpPostedFileBase file = Request.Files["galleryImage"];
                if (file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var extension = Path.GetExtension(fileName);

                    string path = @"C:\Users\John\Documents\Visual Studio 2015\Projects\ArtConspiracy\ArtConspiracy\ArtImages\" + User.Identity.GetUserId().ToString() + @"\" + pid.ToString() + extension;                 
                    string dbpath = "/ArtImages/" + User.Identity.GetUserId().ToString() + "/" + pid.ToString() + extension;
                    string directory = @"C:\Users\John\Documents\Visual Studio 2015\Projects\ArtConspiracy\ArtConspiracy\ArtImages\" + User.Identity.GetUserId().ToString() + @"\";

                    Directory.CreateDirectory(directory);

                    file.SaveAs(path);

                    artpiece = db.ArtPieces.FirstOrDefault(p => p.PID.Equals(pid));

                    artpiece.imgURL = dbpath;

                    db.Entry(artpiece).State = EntityState.Modified;

                    db.SaveChanges();

                }



                return RedirectToAction("Index", "GalleryManager");

            }

            return View(artpiece);
        }

        [Authorize]
        public ActionResult EditGalleryItem(int id)
        {
            var pieceData = db.ArtPieces.Find(id);
            return View(pieceData);
        }

        [Authorize]
        [HttpPost]
        public ActionResult EditGalleryItem(ArtPiece newartpiece)
        {
            if (ModelState.IsValid)
            {

                ArtPiece newArtPiece = db.ArtPieces.FirstOrDefault(p => p.PID.Equals(newartpiece.PID));

                newArtPiece.MID = User.Identity.GetUserId();
                newArtPiece.PieceTitle = newartpiece.PieceTitle;
                newArtPiece.PieceDesc = newartpiece.PieceDesc;

                db.Entry(newArtPiece).State = EntityState.Modified;

                db.SaveChanges();

                HttpPostedFileBase file = Request.Files["galleryImage"];
                if (file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var extension = Path.GetExtension(fileName);

                    string path = @"C:\Users\John\Documents\Visual Studio 2015\Projects\ArtConspiracy\ArtConspiracy\ArtImages\" + User.Identity.GetUserId().ToString() + @"\" + newartpiece.PID.ToString() + extension;
                    string dbpath = "/ArtImages/" + User.Identity.GetUserId().ToString() + "/" + newartpiece.PID.ToString() + extension;
                    string directory = @"C:\Users\John\Documents\Visual Studio 2015\Projects\ArtConspiracy\ArtConspiracy\ArtImages\" + User.Identity.GetUserId().ToString() + @"\";

                    Directory.CreateDirectory(directory);

                    file.SaveAs(path);

                    

                    newArtPiece.imgURL = dbpath + "?ver=" + DateTime.Now.ToString("yyyyMMddHHmmss");

                    db.Entry(newArtPiece).State = EntityState.Modified;

                    db.SaveChanges();

                }



                return RedirectToAction("Index", "GalleryManager");

            }

            return View(newartpiece);
        }

        [Authorize]
        public ActionResult DeleteGalleryItem(int? id)
        {
            var pieceData = db.ArtPieces.Find(id);
            return View(pieceData);
        }

        [Authorize]
        [HttpPost]
        public ActionResult DeleteGalleryItem(int id)
        {
            ArtPiece artpiece = db.ArtPieces.Find(id);
            db.ArtPieces.Remove(artpiece);
            db.SaveChanges();
            return RedirectToAction("Index", "GalleryManager");
        }

    }
}