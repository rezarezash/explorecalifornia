using ExploreCalifornia.DAL;
using ExploreCalifornia.MiddleWares;
using ExploreCalifornia.Models;
using ExploreCalifornia.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Diagnostics;
using System.Linq;

namespace ExploreCalifornia.Controllers
{

    [Route("blog")]
    //[MiddlewareFilter(typeof(CompressionMiddlerWare))]
    public class BlogController : Controller
    {
        private readonly BlogDbContext _dbContext;
        private readonly IOptions<ConnectionStrings> _connectionStrings;

        public BlogController(BlogDbContext blogDataContext , IOptions<ConnectionStrings> connectionStrings)
        {
            _dbContext = blogDataContext;
            _connectionStrings = connectionStrings;
        }
        [Route("")]
        public IActionResult Index(int page = 0)
        {
            var pageSize = 2d;
            var totalPosts = _dbContext.Posts.Count();
            var totalPages = Math.Ceiling(totalPosts / pageSize);
            var previousePage = page - 1;
            var nextPage = page + 1;

            ViewBag.PrevioisePage = previousePage;
            ViewBag.HasPrevioisePage = previousePage>=0;

            ViewBag.NextPage = nextPage;
            ViewBag.HasNextPage = nextPage < totalPages;

            var posts = _dbContext.Posts
                .OrderByDescending(p=>p.PostedOn).Skip((int)pageSize * page)
                .Take((int)pageSize).Select(p => p.ToViewModel()).ToList();

            if (IsAjaxRequest())
            {
                return PartialView(posts);
            }

            return View(posts);
        }


        [Authorize]
        [HttpGet]
        [Route("create")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        [Route("{year:int}/{month:int}/{key}")]
        public IActionResult Post(int year, int month, int key)
        {
            var post = _dbContext.Posts.FirstOrDefault(x => x.Id == key);            
            return View(post.ToViewModel());
        }

        [Authorize]
        [HttpPost]
        [Route("create")]
        public IActionResult Create([FromForm] PostViewModel postData)
        {
            if (!ModelState.IsValid)
                return View(postData);

            try
            {                
                var post = postData.ToPostEntity();              
                _dbContext.Posts.Add(post);
                _dbContext.SaveChanges();

                return RedirectToAction("Post", new { year = post.PostedOn.Year, month = post.PostedOn.Month, key = post.Id });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

        private bool IsAjaxRequest()
        {
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
    }

    public static class PostExtensions
    {
        public static PostViewModel ToViewModel(this Post post)
        {
            return new PostViewModel
            {
                AuthorName = post.Author,
                Content = post.Body,
                Title = post.Title,
                Date = post.PostedOn,
                Id = post.Id
            };
        }

        public static Post ToPostEntity(this PostViewModel post)
        {
            var postDateParsed = DateTime.TryParse(post.Date.ToString(), out DateTime postDate);
            return new Post
            {
                Author = post.AuthorName ,
                Body = post.Content,
                Title = post.Title,
                PostedOn = postDateParsed ? postDate.Date : DateTime.UtcNow,
                Id = post.Id
            };
        }
    }
}
