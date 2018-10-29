using Blog1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;
using Microsoft.AspNet.Identity;
using System.Net;
using System.Data.Entity;
using System.IO;
using System.Web.Routing;

namespace Blog1.Controllers
{
    [Authorize]
    public class BlogController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();
        private ApplicationDbContext _db1 = new ApplicationDbContext();
        // GET: Blog
        public static int tepmBlogId;

        [HttpGet]
        public ActionResult Manage()
        {
            if (User.Identity.GetUserName() != "ar@id.com")
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                return RedirectToAction("List", "Blog");
            }                        
        }
        
        [HttpGet]
        public ActionResult List()
        {
                return View(_db.Blogs.ToList());
        }
        public void categorySort()
        {
            List<Blog> blogMovies = (from s in _db.Blogs
                                     where s.Category == category.Movies && s.Status == true
                                     orderby s.BlogId descending
                                     select s).ToList();
            int countblogMovies = blogMovies.Count();
            ViewBag.countblogMovies = countblogMovies;


            List<Blog> blogTechnologies = (from s in _db.Blogs
                                           where s.Category == category.Technologies && s.Status == true
                                           orderby s.BlogId descending
                                           select s).ToList();
            int countblogTechnologies = blogTechnologies.Count();
            ViewBag.countblogTechnologies = countblogTechnologies;
            List<Blog> blogGames = (from s in _db.Blogs
                                    where s.Category == category.Games && s.Status == true
                                    orderby s.BlogId descending
                                    select s).ToList();
            int countblogGames = blogGames.Count();
            ViewBag.countblogGames = countblogGames;
            List<Blog> blogAnimals = (from s in _db.Blogs
                                      where s.Category == category.Animals && s.Status == true
                                      orderby s.BlogId descending
                                      select s).ToList();
            int countblogAnimals = blogAnimals.Count();
            ViewBag.countblogAnimals = countblogAnimals;
            List<Blog> blogArt = (from s in _db.Blogs
                                  where s.Category == category.Art && s.Status == true
                                  orderby s.BlogId descending
                                  select s).ToList();
            int countblogArt = blogArt.Count();
            ViewBag.countblogArt = countblogArt;

            List<Blog> blogSupers = (from s in _db.Blogs
                                     where s.Category == category.Supers && s.Status == true
                                     orderby s.BlogId descending
                                     select s).ToList();
            int countblogSupers = blogSupers.Count();
            ViewBag.countblogSupers = countblogSupers;
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Browse()
        {
            List<Blog> blogss = (from s in _db.Blogs
                                 where s.Status==true 
                                 orderby s.BlogId descending 
                                 select s).ToList();

            List<Blog> blogz = (from s in _db.Blogs
                                 where s.Status == false
                                 orderby s.BlogId descending
                                 select s).ToList();

            int countApprovals = blogz.Count();
            ViewBag.countApprovals = countApprovals;

            int countBlogs = blogss.Count();
            ViewBag.countBlogs = countBlogs;

            //Categories
            categorySort();
            var blgs = _db.Blogs.OrderByDescending(o => o.BlogComments.Count()).Take(10).ToList();
            ViewBag.blgs = blgs;

            return View(blogss);
        }


        
        [HttpGet]
        public ActionResult BrowseApprove()
        {
            if (User.Identity.GetUserName() != "ar@id.com")
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                List<Blog> blogss = (from s in _db.Blogs
                                     where s.Status == false
                                     orderby s.BlogId descending
                                     select s).ToList();


                return View(blogss);
            }
        }

        [HttpGet]
        public ActionResult TrendingPost()
        {
            List<Blog> blogss = (from s in _db.Blogs
                                 orderby s.BlogId ascending
                                 select s).ToList();

            var trendingPosts = _db.BlogComments.GroupBy(g => g.BlogId).Select(s=>new {
                BlogId=s.Key,
                Count=s.Count()
            }).OrderByDescending(araf=>araf.Count).ToList();
            var blogs = trendingPosts.Select(s => new
            {
                blog= _db.Blogs.Where(w=>w.BlogId==s.BlogId).FirstOrDefault()
            }).ToList();

            var blgs = _db.Blogs.OrderByDescending(o => o.BlogComments.Count()).Take(10).ToList();
            return View(blgs);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Blog blog , HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (User.Identity.GetUserName() == "")
                {
                    return RedirectToAction("Login", "Account");
                }
                else { 
                System.IO.Directory.CreateDirectory(Server.MapPath("~/Images/Blog"));
                string path = "";
                if (file != null)
                {
                    string pic = System.IO.Path.GetFileName(file.FileName);
                    string physicalPath =
                        System.IO.Path.Combine(Server.MapPath("~/Images/Blog"), pic);
                    path = "/Images/Blog/" + pic;
                    // file is uploaded
                    file.SaveAs(physicalPath);
                    blog.ThumbnailImageUrl = path;

                }
                blog.CreationDate = DateTime.Now;
                blog.Author = User.Identity.GetUserName();
                    if (User.Identity.GetUserName() == "ar@id.com")
                    {
                        blog.Status = true;
                    }
                    else
                    {
                        blog.Status = false;
                    }
                _db.Blogs.Add(blog);
                _db.SaveChanges();
                return RedirectToAction("Browse", "Blog");
                }
            }

            return View();
            
        }

        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = _db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }

            tepmBlogId = blog.BlogId;
            categorySort();
            //ViewBag.TempBlogId = blog.BlogId;

            List<BlogComment> blogComments = (from s in _db.BlogComments
                                              where s.BlogId == id
                                              select s).ToList();
            ViewBag.blogComments = blogComments;
            int countBlogComments = blogComments.Count();
            ViewBag.countBlogComments = countBlogComments;

            List<BlogLike> blogLikes = (from s in _db.BlogLikes
                                              where s.BlogId == id && s.Like == true
                                              select s).ToList();
            ViewBag.blogLikes = blogLikes;
            int countBlogLikes = blogLikes.Count();
            ViewBag.countBlogLikes = countBlogLikes;

            List<BlogSection> blogSectioss = (from s in _db.BlogSections
                                              where s.BlogId == id
                                              select s).ToList();
            ViewBag.blogSectionss = blogSectioss;

            var blogz1 = (from t1 in _db.Blogs
                                join t2 in _db.BlogComments on
                                t1.BlogId equals t2.BlogId
                                let count = t2.CommentId
                                orderby t2.CommentId descending
                                group new { t1.BlogId,t1.Title,t1.ThumbnailImageUrl,t1.Author } by t1.BlogId into newgroup
                                
                                select newgroup).ToList();

            var blogz = (from s in _db.Blogs
                         orderby s.BlogId ascending
                         select s).ToList();

            string user = User.Identity.GetUserName();
            var likeCheck = (from s in _db.BlogLikes
                             where s.BlogId == id && s.LikeAuthor == user  && s.Like == true
                             select s).ToList();
            int countLikeCheck = likeCheck.Count();
            ViewBag.countLikeCheck = countLikeCheck;

            ViewBag.blogz = blogz;
            ViewBag.TempBlogId = blog.BlogId;
            ViewBag.Author = blog.Author;
            var blgs = _db.Blogs.OrderByDescending(o => o.BlogComments.Count()).Take(5).ToList();
            ViewBag.blgs = blgs;
            return View(blog);
        }

        

        [HttpGet]
        public ActionResult CreateBlogSection(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else { 
                
            Blog blog = _db.Blogs.Find(id);
                if(blog.Author == User.Identity.GetUserName())
                {
                    tepmBlogId = blog.BlogId;
                    ViewBag.TempBlogId = blog.BlogId;
                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }

            }
        }

        [HttpPost]
        public ActionResult CreateBlogSection(BlogSection blogSection,HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (User.Identity.GetUserName() == "")
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath("~/Images/Blog/BlogSection"));
                    string path = "";
                    if (file != null)
                    {
                        string pic = System.IO.Path.GetFileName(file.FileName);
                        string physicalPath =
                            System.IO.Path.Combine(Server.MapPath("~/Images/Blog/BlogSection"), pic);
                        path = "/Images/Blog/BlogSection/" + pic;
                        // file is uploaded
                        file.SaveAs(physicalPath);
                        blogSection.ImageUrl = path;

                    }
                    blogSection.BlogId = tepmBlogId;


                    _db1.BlogSections.Add(blogSection);
                    _db1.SaveChanges();
                    return RedirectToAction("Details", new RouteValueDictionary(new { controller = "Blog", action = "Details", Id = blogSection.BlogId }));
                }
            }

            return View();


        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = _db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        // POST: demo1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BlogId,Title,Content,ThumbnailImageUrl,Author,Status,CreationDate,LastUpdateDate")] Blog blog)
        {
            blog.LastUpdateDate = DateTime.Now;
            blog.Author = User.Identity.GetUserName();

            _db.Entry(blog).State = EntityState.Modified;

                _db.SaveChanges();
                return RedirectToAction("List");

        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = _db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        // POST: demo1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Blog blog = _db.Blogs.Find(id);
            _db.Blogs.Remove(blog);
            _db.SaveChanges();
            return RedirectToAction("Browse");
        }

        [AllowAnonymous]
        public ActionResult CategorizedBrowse(category id )
        {
            categorySort();
            ViewBag.Movies = category.Movies;
            ViewBag.Technologies = category.Technologies;
            ViewBag.Games = category.Games;
            ViewBag.Animals = category.Animals;
            ViewBag.Art = category.Art;
            ViewBag.Supers = category.Supers;
            List<Blog> blogss = (from s in _db.Blogs
                               where s.Category == id && s.Status==true
                                 orderby s.BlogId descending
                                 select s).ToList();
            ViewBag.category = id;
            //Blog blog = _db.Blogs.Find(categories);
            if (blogss == null)
            {
                return HttpNotFound();
            }
            var blogz = (from s in _db.Blogs
                         orderby s.BlogId descending
                         select s).ToList();
            var blgs = _db.Blogs.OrderByDescending(o => o.BlogComments.Count()).Take(7).ToList();
            ViewBag.blgs = blgs;

            ViewBag.blogz = blogz;
            return View(blogss);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult BrowseBlogSection(int id)
        {
            List<BlogSection> blogSectioss = (from s in _db.BlogSections
                                        where s.BlogId == id
                                        select s).ToList();
            //Blog blog = _db.Blogs.Find(categories);
            if (blogSectioss == null)
            {
                return HttpNotFound();
            }
            var blogz = (from s in _db.Blogs
                         orderby s.BlogId ascending
                         select s).ToList();

            ViewBag.blogz = blogz;
            return View(blogSectioss);
        }


        

        [HttpGet]
        public ActionResult CreateComment()
        {
            BlogComment blogComment = new BlogComment();
            return RedirectToAction("Details", new RouteValueDictionary(new { controller = "Blog", action = "Details", Id = tepmBlogId }));
        }

        [HttpPost]
        public ActionResult CreateComment(BlogComment blogComment)
        {
            if (ModelState.IsValid)
            {
                if (User.Identity.GetUserName() == "")
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    blogComment.BlogId = tepmBlogId;
                    blogComment.CommentAuthor = User.Identity.GetUserName();
                    blogComment.DateCreated = DateTime.Now;
                   
                    _db1.BlogComments.Add(blogComment);
                    _db1.SaveChanges();
                    return RedirectToAction("Details", new RouteValueDictionary( new { controller = "Blog", action = "Details", Id = blogComment.BlogId }));
                    //return RedirectToAction("Details", "Blog");
                }
            }
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult ShowComment(int id)
        {
            List<BlogComment> blogComments = (from s in _db.BlogComments
                                              where s.BlogId == id
                                              select s).ToList();
            //Blog blog = _db.Blogs.Find(categories);
            if (blogComments == null)
            {
                return HttpNotFound();
            }
            return View(blogComments);
        }

        [AllowAnonymous]
        public ActionResult BrowseAuthor(string id1)
        {
            categorySort();
            List<Blog> blogs = (from s in _db.Blogs
                                where s.Author == id1
                                select s).ToList();
            ViewBag.author = id1;
            //Blog blog = _db.Blogs.Find(categories);
            if (blogs == null)
            {
                return HttpNotFound();
            }
            var blogz = (from s in _db.Blogs
                         orderby s.BlogId ascending
                         select s).ToList();
            var blgs = _db.Blogs.OrderByDescending(o => o.BlogComments.Count()).Take(7).ToList();
            ViewBag.blgs = blgs;
            ViewBag.blogz = blogz;
            return View(blogs);
        }
        public ActionResult Approve(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = _db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            blog.LastUpdateDate = DateTime.Now;
            blog.Status = true;
            _db.SaveChanges();
            return RedirectToAction("BrowseApprove");
        }

        [HttpGet]
        public ActionResult CreateLike()
        {
            
            BlogLike blogLike = new BlogLike();
            return RedirectToAction("Details", new RouteValueDictionary(new { controller = "Blog", action = "Details", Id = tepmBlogId }));
        }

        [HttpPost]
        public ActionResult CreateLike(BlogLike blogLike)
        {
            if (ModelState.IsValid)
            {
                if (User.Identity.GetUserName() == "")
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    blogLike.BlogId = tepmBlogId;
                    blogLike.LikeAuthor = User.Identity.GetUserName();
                    blogLike.DateCreated = DateTime.Now;
                    blogLike.Like = true;

                    _db1.BlogLikes.Add(blogLike);
                    _db1.SaveChanges();
                    return RedirectToAction("Details", new RouteValueDictionary(new { controller = "Blog", action = "Details", Id = blogLike.BlogId }));
                    //return RedirectToAction("Details", "Blog");
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult CreateDisLike()
        {

            BlogLike blogLike = new BlogLike();
            return RedirectToAction("Details", new RouteValueDictionary(new { controller = "Blog", action = "Details", Id = tepmBlogId }));
        }

        [HttpPost]
        public ActionResult CreateDisLike(BlogLike blogLike)
        {
            if (ModelState.IsValid)
            {
                if (User.Identity.GetUserName() == "")
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    string user = User.Identity.GetUserName();
                    var likeCheck = (from s in _db.BlogLikes
                                     where s.BlogId == tepmBlogId && s.LikeAuthor == user && s.Like == true
                                     select s).FirstOrDefault();
                    //likeCheck.BlogId = tepmBlogId;
                    //likeCheck.LikeAuthor = User.Identity.GetUserName();
                    //likeCheck.DateCreated = DateTime.Now;
                    //likeCheck.Like = false;
                    _db.BlogLikes.Remove(likeCheck);

                    //_db1.BlogLikes.Add(likeCheck);
                    _db.SaveChanges();
                    return RedirectToAction("Details", new RouteValueDictionary(new { controller = "Blog", action = "Details", Id = likeCheck.BlogId }));
                    //return RedirectToAction("Details", "Blog");
                }
            }
            return View();
        }


    }
}
