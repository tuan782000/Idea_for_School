using IDS_School.Data;
using IDS_School.Models;
using IDS_School.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web.Mvc;
using JsonResult = Microsoft.AspNetCore.Mvc.JsonResult;

namespace IDS_School.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class ChartsController : Microsoft.AspNetCore.Mvc.Controller
    {


        private readonly ApplicationDbContext _context;

        public ChartsController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: ChartController
        public async Task<IActionResult> Index()
        {
            return View(await _context.Submissions.ToListAsync());
        }

        // GET: ChartController/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var getIdea = _context.Ideas.Include(i => i.Submission).Where(s => s.SubmissionId == id);
            int countIdea = getIdea.Count();
            ViewData["countIdea"] = countIdea;

            ViewData["SubmissionId"] = id;
            return View(getIdea);
        }
        //public IActionResult IdeaData(int? id)
        //{
        //    //GetChartData(id);
        //    var data = _context.Ideas.Where(i => i.Id == id).Select(c => new
        //    {
        //        like = c.Reactions.Select(l => l.reaction == reaction.Like).Count(),
        //        dislike = c.Reactions.Select(d => d.reaction == reaction.Dislike).Count(),
        //        comment = c.Comments.Count,
        //        replies = c.Comments.Select(r => new
        //        {
        //            reply = r.Replies.Where(t => t.Comment.IdeaId == id).Select(r => r.Comment.Replies).Count()
        //        }),
        //        view = c.Views.Count
        //    }).ToList();
        //    var jsonList = JsonSerializer.Serialize(data);
        //    return View(jsonList);
        //}

        public JsonResult IdeaData(int? id)
        {
            var data = _context.Ideas.Where(i => i.Id == id).Select(c => new
            {
                like = c.Reactions.Where(i => i.reaction == reaction.Like).Select(l => l.reaction == reaction.Like).Count(),
                dislike = c.Reactions.Where(i => i.reaction == reaction.Dislike).Select(d => d.reaction == reaction.Dislike).Count(),
                comment = c.Comments.Count,
                replies = c.Comments.Select(r => r.Replies.Where(w => w.CommentId==id)).Count(), 
                view = c.Views.Count
            }).ToList();

            //new
            //{
            //    reply = r.Replies.Where(t => t.Comment.IdeaId == id).Select(r => r.Comment.Replies).Count()
            //}),
            var result = data.Select(s => s.comment == 0 && s.dislike == 0 && s.like == 0 && s.replies == 0 && s.view == 0).FirstOrDefault() == true ? null : data;
            var jsonList = JsonSerializer.Serialize(result);
            return Json(new {JSONList = jsonList });
        }
        //public object[] GetChartData(int? id)
        //{
        //    try 
        //    {
        //        //string[] GetChartData = new string[4];

        //        //SqlConnection con = new SqlConnection(ConnectionString);
        //        //con.Open();
        //        //SqlCommand cmd = new SqlCommand("select count() ")
        //        List<Idea> data = new List<Idea>();
        //        data = (List<Idea>)_context.Ideas.Where(i => i.Id == id).Select(c => new
        //        {
        //            like = c.Reactions.Select(l => l.reaction == reaction.Like).Count(),
        //            dislike = c.Reactions.Select(d => d.reaction == reaction.Dislike).Count(),
        //            comment = c.Comments.Sum(c => c.Idea.Comments.Count()),
        //            replies = c.Comments.Select(r => new
        //            {
        //                reply = r.Replies.Sum(reply => reply.Comment.Replies.Count())
        //            })
        //        }) ;
        //        var chartData = new object[4];
        //        //DataTable dt = new DataTable();
        //        chartData[0] = new object[]
        //        {
        //            "Like",
        //            "Dislike",
        //            "Comments",
        //            "Replies"
        //        };
        //        int j = 0;
        //        //chartData[0].Columns.Add("Like");
        //        //chartData[1].Columns.Add("Dislike");
        //        //chartData.Columns.Add("Comments");
        //        //chartData.Columns.Add("Replies");
        //        foreach (var item in data)
        //        {
        //            j++;                    
        //            chartData[j] = new object[] { item.Reactions, item.Comments };
        //            //dt.Rows.Add(item.like, item.dislike, item.comment, item.replies);
        //        }
        //        return chartData;
        //        //if (dt.Rows.Count == 0)
        //        //{
        //        //    GetChartData[0] = "0";
        //        //    GetChartData[1] = "0";
        //        //    GetChartData[2] = "0";
        //        //    GetChartData[3] = "0";
        //        //}
        //        //else
        //        //{
        //        //    GetChartData[0] = dt.Rows[0]["Like"].ToString();
        //        //    GetChartData[1] = dt.Rows[0]["Dislike"].ToString();
        //        //    GetChartData[2] = dt.Rows[0]["Comments"].ToString();
        //        //    GetChartData[3] = dt.Rows[0]["Replies"].ToString();
        //        //}
        //        //return Json(new { chartData }, JsonRequestBehavior.AllowGet);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //if (id == null)
        //{
        //    return NotFound();
        //}

        //var idea = await _context.Ideas
        //    .Include(r => r.Reactions)
        //    .Include(c => c.Comments)
        //    .FirstOrDefaultAsync(m => m.Id == id);

        //var getReact = await _context.Reactions
        //   .Where(t => t.IdeaId == id && t.reaction == reaction.Like)
        //   .Select(t => t.reaction == reaction.Like)
        //   .ToListAsync();

        //var getReact2 = await _context.Reactions
        //  .Where(t => t.IdeaId == id && t.reaction == reaction.Dislike)
        //  .Select(t => t.reaction == reaction.Dislike)
        //  .ToListAsync();

        //var getComment = await _context.Comments
        //  .Where(t => t.IdeaId == id)
        //  .Select(c => c.Id)
        //  .ToListAsync();

        //var getReply = await _context.Replies
        //  .Where(t => t.Comment.IdeaId == id)
        //  .ToListAsync();

        //int countLike = getReact.Count;
        //int countDislike = getReact2.Count;
        //int countComment = getComment.Count;
        //int countReply = getReply.Count;

        //int totalComments = countComment + countReply;

        //ViewData["countLike"] = countLike;
        //ViewData["countDislike"] = countDislike;
        //ViewData["totalComments"] = totalComments;

        //return View(idea);


    }
}

        
