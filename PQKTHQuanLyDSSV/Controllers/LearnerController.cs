using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PQKTHQuanLyDSSV.Data;
using PQKTHQuanLyDSSV.Models;

namespace PQKTHQuanLyDSSV.Controllers
{
    public class LearnerController : Controller
    {
        private readonly SchoolContext db;
        private const int pageSize = 3;

        public LearnerController(SchoolContext context)
        {
            db = context;
        }

        public IActionResult Index(int? mid)
        {
            IQueryable<Learner> learners = db.Learners.Include(l => l.Major);

            if (mid != null)
                learners = learners.Where(l => l.MajorID == mid);

            int pageNum = (int)Math.Ceiling(learners.Count() / (float)pageSize);
            ViewBag.pageNum = pageNum;
            ViewBag.mid = mid;

            return View(learners.Take(pageSize).ToList());
        }

        public IActionResult LearnerByMajorID(int mid)
        {
            var learners = db.Learners
                .Where(l => l.MajorID == mid)
                .Include(l => l.Major)
                .ToList();

            return PartialView("LearnerTable", learners);
        }

        public IActionResult LearnerFilter(int? mid, string? keyword, int? pageIndex)
        {
            IQueryable<Learner> learners = db.Learners;
            int page = pageIndex == null || pageIndex <= 0 ? 1 : pageIndex.Value;

            if (mid != null)
            {
                learners = learners.Where(l => l.MajorID == mid);
                ViewBag.mid = mid;
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string search = keyword.Trim().ToLower();
                learners = learners.Where(l =>
                    l.FirstMidName.ToLower().Contains(search) ||
                    l.LastName.ToLower().Contains(search));
                ViewBag.keyword = keyword;
            }

            int pageNum = (int)Math.Ceiling(learners.Count() / (float)pageSize);
            ViewBag.pageNum = pageNum;
            ViewBag.mid = mid;
            ViewBag.keyword = keyword;
            ViewBag.currentPage = page;

            var result = learners
                .OrderBy(l => l.LearnerID)
                .Skip(pageSize * (page - 1))
                .Take(pageSize)
                .Include(l => l.Major)
                .ToList();

            return PartialView("LearnerTable", result);
        }

        public IActionResult Create()
        {
            LoadMajors();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("FirstMidName,LastName,MajorID,EnrollmentDate")] Learner learner)
        {
            if (ModelState.IsValid)
            {
                db.Learners.Add(learner);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            LoadMajors(learner.MajorID);
            return View(learner);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var learner = db.Learners.Find(id);
            if (learner == null)
                return NotFound();

            LoadMajors(learner.MajorID);
            return View(learner);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("LearnerID,FirstMidName,LastName,MajorID,EnrollmentDate")] Learner learner)
        {
            if (id != learner.LearnerID)
                return NotFound();

            if (ModelState.IsValid)
            {
                db.Update(learner);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            LoadMajors(learner.MajorID);
            return View(learner);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var learner = db.Learners
                .Include(l => l.Major)
                .FirstOrDefault(l => l.LearnerID == id);

            if (learner == null)
                return NotFound();

            return View(learner);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var learner = db.Learners.Find(id);
            if (learner != null)
            {
                db.Learners.Remove(learner);
                db.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        private void LoadMajors(int? selectedMajor = null)
        {
            ViewBag.MajorID = new SelectList(db.Majors, "MajorID", "MajorName", selectedMajor);
        }
    }
}
