using MoviesMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoviesMVC.Controllers
{
    public class SiteController : Controller
    {
        // GET: Site
        MovieConext moviesContext = new MovieConext();
        public ActionResult Index()
        {
            return View(PopulateDate(""));
        }
        [HttpPost]
        [ValidateAntiForgeryTokenAttribute]
        public ActionResult Index(string sortValue)
        {
            sortValue = Request.Form["drpSort"];
            ViewBag.SortValue = sortValue;
            return View(PopulateDate(sortValue));
        }
        public List<Movie> PopulateDate(string sortValue)
        {
            int pageSize = 5;
            int pageId = Convert.ToInt32(Request.QueryString["pageId"]);
            List<Movie> movies = moviesContext.Movies.ToList();
            List<SelectListItem> drpList = new List<SelectListItem>();
            drpList.Add(new SelectListItem { Text = "By Title", Value = "Title" });
            drpList.Add(new SelectListItem { Text = "By Release Date", Value = "ReleaseDate" });
            drpList.Add(new SelectListItem { Text = "By Rating Ascending", Value = "Rating ASC" });
            drpList.Add(new SelectListItem { Text = "By Rating Descending", Value = "Rating DESC" });

            if (!string.IsNullOrEmpty(sortValue))
            {
                var value = drpList.Where(c => c.Value == ViewBag.SortValue).FirstOrDefault();
                value.Selected = true;
            }
            movies = FilterData(movies);
            ViewBag.SortValues = drpList;
            switch (sortValue)
            {
                case "Title":
                    movies = movies.OrderBy(x => x.Title).ToList();
                    break;
                case "ReleaseDate":
                    movies = movies.OrderBy(x => x.ReleaseDate).ToList();
                    break;
                case "Rating ASC":
                    movies = movies.OrderBy(x => x.Rating).ToList();
                    break;
                case "Rating DESC":
                    movies = movies.OrderByDescending(x => x.Rating).ToList();
                    break;
            }
            double pageCount = (double)movies.Count / (double)pageSize;
            ViewBag.PageCount = Math.Ceiling(pageCount);

            movies = movies.Skip((pageId - 1) * pageSize).Take(pageSize).ToList();
            return movies;
        }
        public List<Movie> FilterData(List<Movie> movies)
        {
            string keyword = Request.Form["txtKeyword"];
            int fromYear = 0;
            int.TryParse(Request.Form["drpFromYear"], out fromYear);
            int toYear = 0;
            int.TryParse(Request.Form["drpToYear"], out toYear);
            int ratingFrom = 0;
            int.TryParse(Request.Form["txtRatingFrom"], out ratingFrom);
            int ratingTo = 0;
            int.TryParse(Request.Form["txtRatingTo"], out ratingTo);
            if (!string.IsNullOrEmpty(keyword))
            {
                movies = movies.Where(x => x.Title.Contains(keyword) || x.Description.Contains(keyword)).ToList();
            }
            if (fromYear != 0)
            {
                movies = movies.Where(x => x.ReleaseDate.Year >= fromYear).ToList();
            }
            if (toYear != 0)
            {
                movies = movies.Where(x => x.ReleaseDate.Year <= toYear).ToList();
            }
            if (ratingFrom != 0)
            {
                movies = movies.Where(x => x.Rating >= ratingFrom).ToList();
            }
            if (ratingTo != 0)
            {
                movies = movies.Where(x => x.Rating <= ratingTo).ToList();
            }
            return movies;

        }
        public ActionResult Details(int id)
        {
            Movie movie = moviesContext.Movies.FirstOrDefault(x => x.Id == id);
            return View(movie);
        }
        public ActionResult Login()
        {
            return View();
        }

    }
}