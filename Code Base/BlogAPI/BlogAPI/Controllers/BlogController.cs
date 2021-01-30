using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataAccess;

namespace BlogAPI.Controllers
{
    [RoutePrefix("api/Blog")]
    public class BlogController : ApiController
    {
        [HttpPost]
        [BasicAuthentication]
        [Route("AddBlog")]
        public HttpResponseMessage AddBlog([FromBody] Blog blog)
        {
            try
            {
                if (blog == null || string.IsNullOrEmpty(blog.Title) || string.IsNullOrEmpty(blog.BlogContent))
                {
                    var message = Request.CreateResponse(HttpStatusCode.BadRequest, "Empty input data");

                    return message;
                }
                using (BlogEntities entities = new BlogEntities())
                {
                    entities.Blogs.Add(blog);
                    entities.SaveChanges();
                    var message = Request.CreateResponse(HttpStatusCode.Created, "New blog added successfully");

                    return message;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }


        [BasicAuthentication]
        [HttpGet]
        [Route("GetAllBlog")]
        public IEnumerable<Blog> GetAllBlog()
        {
            using (BlogEntities entities = new BlogEntities())
            {
                return entities.Blogs.ToList();
            }
        }

        [HttpPost]
        [BasicAuthentication]
        [Route("EditBlog")]
        public HttpResponseMessage EditBlog([FromBody] Blog blog)
        {
            try
            {
                HttpResponseMessage message = new HttpResponseMessage();
                if (blog == null)
                {
                     message = Request.CreateResponse(HttpStatusCode.BadRequest, "Empty input data");

                    return message;
                }
                if (blog.BlogId == 0)
                {
                    message = Request.CreateResponse(HttpStatusCode.BadRequest, "Blog id does not exist");

                    return message;
                }
                if (string.IsNullOrEmpty(blog.Title))
                {
                    message = Request.CreateResponse(HttpStatusCode.BadRequest, "Title does not exist");

                    return message;
                }
                if (string.IsNullOrEmpty(blog.BlogContent))
                {
                    message = Request.CreateResponse(HttpStatusCode.BadRequest, "Blog content does not exist");

                    return message;
                }

                using (var entities = new BlogEntities())
                {
                    var existingBlog = entities.Blogs.Where(s => s.BlogId == blog.BlogId).FirstOrDefault<Blog>();

                    if (existingBlog != null)
                    {
                        existingBlog.Title = blog.Title;
                        existingBlog.BlogContent = blog.BlogContent;

                        entities.SaveChanges();
                         message = Request.CreateResponse(HttpStatusCode.Created, "New blog id " + blog.BlogId + " updated successfully");
                    }
                    else
                    {
                         message = Request.CreateResponse(HttpStatusCode.BadRequest, "Blog id " + blog.BlogId + " does not exist");
                    }
                    
                    return message;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpGet]
        [BasicAuthentication]
        [Route("DeleteBlog/{id?}")]
        public HttpResponseMessage DeleteBlog(int id)
        {
            try
            {
                using (var entities = new BlogEntities())
                {
                    var blog = entities.Blogs
                        .Where(s => s.BlogId == id)
                        .FirstOrDefault();

                    entities.Entry(blog).State = System.Data.Entity.EntityState.Deleted;
                    entities.SaveChanges();
                    var message = Request.CreateResponse(HttpStatusCode.Created, "New blog id " + blog.BlogId + " deleted successfully");

                    return message;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Blog id does not exist");
            }
        }
    }
}
