using IDS_School.Data;
using IDS_School.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReturnTrue.AspNetCore.Identity.Anonymous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IDS_School.Controllers
{
    public class IdeasController : Controller
    {
        private readonly ApplicationDbContext _context;
        // private readonly UserManager<CUser> _userManager;

        public IdeasController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: ContributionsController
        public async Task<IActionResult> Index()
        {
            return View(await _context.Ideas.ToListAsync());
        }

        // GET: IdeasController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
           
            var idea = await _context.Ideas
                .Include(r => r.Reactions)
                .Include(c => c.Comments)
                .FirstOrDefaultAsync(m => m.Id == id);

            var getReact = await _context.Reactions
               .Where(t => t.IdeaId == id && t.reaction == reaction.Like)
               .Select(t => t.reaction == reaction.Like)
               .ToListAsync();

            var getReact2 = await _context.Reactions
              .Where(t => t.IdeaId == id && t.reaction == reaction.Dislike)
              .Select(t => t.reaction == reaction.Dislike)
              .ToListAsync();
            int countLike = getReact.Count;
            int countDislike = getReact2.Count;

            List<Comment> comments = null;
            comments = await _context.Comments
                .Include(u => u.User)
                .Where(i => i.IdeaId == id)
                .OrderBy(c => c.CreatedDate)
                .ToListAsync();

            List<Reply> replies = null;
            replies = await _context.Replies
                .Include(u => u.User)
                .Include(u => u.Comment)
                .OrderBy(c => c.CreatedDate)
                .ToListAsync();

            ViewData["countLike"] = countLike;
            ViewData["countDislike"] = countDislike;
            ViewData["comments"] = comments;
            ViewData["replies"] = replies;
           


            if (idea == null)
            {
                return NotFound();
            }

            return View(idea);
        }

        // GET: IdeasController/Create
        public async Task<IActionResult> Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var submission = await _context.Submissions.FindAsync(id);
            if (submission == null || userId == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: IdeasController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, Idea idea, Boolean isAnoymous, bool isAcceptTerms = false)
        {
            if (isAcceptTerms)
            {
                var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
                idea.UserId = user;
                var submission = await _context.Submissions.FindAsync(id);
                int submissionId = submission.Id;

                Idea newIdea = new Idea
                {
                    UserId = user,
                    SubmissionId = submissionId,
                    CreatedDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now,
                    Title = idea.Title,
                    Description = idea.Description,
                    Content = idea.Content,
                    CategoryId = idea.CategoryId,
                    isAnoymous = isAnoymous,
                };

                ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", idea.CategoryId);

                _context.Add(newIdea);
                await _context.SaveChangesAsync();
            }
            else
            {
                return RedirectToAction(nameof(Details), new { id, error = "You must accept Terms." });
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        // GET: ContributionsController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var idea = await _context.Ideas.FindAsync(id);
            if (idea == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View(idea);
        }

        // POST: IdeasController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Idea idea)
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            idea.UserId = user;
            var createdated = idea.CreatedDate;
            var categoryid = idea.CategoryId;
            var sumissionid = idea.SubmissionId;
            if (id != idea.Id)
            {
                return NotFound();
            }
            try
            {
                Idea newIdea = new()
                {
                    UserId = user,
                    CreatedDate = createdated,
                    LastModifiedDate = DateTime.Now,
                    Title = idea.Title,
                    Description = idea.Description,
                    Content = idea.Content,
                    SubmissionId = sumissionid,
                    CategoryId = categoryid
                };
                _context.Update(newIdea);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IdeaExists(idea.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        // GET: ContributionsController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var @idea1 = await _context.Ideas
                .Include(u => u.User)
                .Include(c => c.Category)
                .Include(s => s.Submission)
                .FirstOrDefaultAsync(m => m.Id == id);

            var getCateName = _context.Categories.FirstOrDefaultAsync(c => c.Id == @idea1.CategoryId).Result;
            @idea1.Category.Name = getCateName.Name;

            var getSubName = _context.Submissions.FirstOrDefaultAsync(s => s.Id == @idea1.SubmissionId).Result;
            @idea1.Submission.Name = getSubName.Name;
            // var getUserName = _context.Users.FirstOrDefaultAsync(u => u.Id == @idea1.UserId).Result;
            // @idea1.UserId = getUserName.Email;
            if (@idea1 == null)
            {
                return NotFound();
            }

            return View(@idea1);
        }

        // POST: IdeasController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var idea = await _context.Ideas.FirstOrDefaultAsync(m => m.Id == id);
                _context.Ideas.Remove(idea);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            catch
            {
                return View();
            }
        }

        // POST: IdeasController/Like/10
        [HttpPost, ActionName("Like")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Like(int ideaId, reaction getReaction)
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var idea = await _context.Ideas.FindAsync(ideaId);
            var oldArticle = (from x in _context.Reactions
                              where x.UserId == user
                              select x).FirstOrDefault();
            if (oldArticle == null && getReaction == reaction.Like)
            {
                Reaction newReaction = new Reaction
                {
                    UserId = user,
                    IdeaId = ideaId,
                    CreatedDate = DateTime.Now,
                    reaction = Models.reaction.Like
                };

                _context.Add(newReaction);
                await _context.SaveChangesAsync();

            }
            else if (oldArticle == null  && getReaction == reaction.Dislike)
            {
                Reaction newReaction = new Reaction
                {
                    UserId = user,
                    IdeaId = ideaId,
                    CreatedDate = DateTime.Now,
                    reaction = Models.reaction.Dislike
                };
                _context.Add(newReaction);
                await _context.SaveChangesAsync();
            }
            else if (oldArticle != null && oldArticle.reaction != getReaction)
            {
                oldArticle.reaction = getReaction;
                _context.Update(oldArticle);
                await _context.SaveChangesAsync();
            }
            else if (oldArticle != null && oldArticle.reaction == getReaction)
            {
                _context.Remove(oldArticle);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Details), new { id = ideaId });
        }

        // POST: IdeasController/AddComment/12
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Comment(string message, int id, Boolean isAnoymous)
        {

            var idea = await _context.Ideas.FindAsync(id);
            int ideaid = idea.Id;
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!String.IsNullOrEmpty(message))
            {
                var newComment = new Comment();
                newComment.Content = message;
                newComment.IdeaId = ideaid;
                newComment.UserId = user;
                newComment.CreatedDate = DateTime.Now;
                newComment.LastModifiedDate = DateTime.Now;
                newComment.IsAnoymousComment = isAnoymous;

                _context.Add(newComment);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Details), new { id });
        }
        // POST: IdeasController/Reply/13
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reply(string replyMessage, int ideaId, Boolean isAnoymous, int CommentId)
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!String.IsNullOrEmpty(replyMessage))
            {
                var newReplyComment = new Reply();
                newReplyComment.Content = replyMessage;
                newReplyComment.CommentId = CommentId;
                newReplyComment.UserId = user;
                newReplyComment.CreatedDate = DateTime.Now;
                newReplyComment.LastModifiedDate = DateTime.Now;
                newReplyComment.IsAnoymousReply = isAnoymous;

                _context.Add(newReplyComment);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Details), new { id = ideaId });
        }
        private bool IdeaExists(int id)
        {
            return _context.Ideas.Any(e => e.Id == id);
        }
    }
}
