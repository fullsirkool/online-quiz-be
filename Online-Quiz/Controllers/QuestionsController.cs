using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Quiz.Models;
using PagedList;

namespace Online_Quiz.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase {
        private readonly OnlineQuizContext _context;

        public QuestionsController(OnlineQuizContext context) {
            _context = context;
        }

        [HttpGet("GetToTalQuestion")]
        public async Task<ActionResult<int>> GetToTalQuestion() {
            int questionSize = await _context.Questions.CountAsync();
            return questionSize;
        }

        [Authorize(Roles = "1")]
        [HttpGet("GetPagingQuestion")]
        public IEnumerable<Question> GetPagingQuestion(int? pageIndex, int pageSize) {
            if (pageIndex == null) {
                pageIndex = 1;
            }

            int pageNumber = (pageIndex ?? 1);
            IEnumerable<Question> questions = _context.Questions.ToPagedList(pageNumber, pageSize);
            return questions;
        }

        [HttpGet("GetQuestionCodes")]
        public IQueryable<int> GetQuestionCodes(int numberOfQuestion) {
            IQueryable<int> questionCodes = _context.Questions.Select(x => x.QuestionId).OrderBy(s => Guid.NewGuid()).Take(numberOfQuestion);
            return questionCodes;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Question>> GetQuestion(int id) {
            var question = await _context.Questions.Select(q => new Question {
                QuestionId = q.QuestionId,
                Content = q.Content,
                UserId = q.UserId,
                CreateDate = q.CreateDate,
                Answers = _context.Answers.Where(a => a.QuestionId == q.QuestionId).ToList()
            }).Where(qt => qt.QuestionId == id).FirstOrDefaultAsync();

            if (question == null) {
                return NotFound();
            }

            return question;
        }

        [Authorize(Roles = "1")]
        [HttpPost]
        public async Task<ActionResult<Question>> PostQuestion(Question question) {
            _context.Questions.Add(question);
            foreach (Answer answer in question.Answers) {
                _context.Answers.Add(answer);
            }
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQuestion", new { id = question.QuestionId }, question);
        }

        [HttpPost("GetMarkForQuestion")]
        public double GetMarkForQuestion(List<Question> questions) {
            int userRightAnswer = 0;
            int totalRightAnswer = 0;

            foreach (Question question in questions) {
                int totalAnswer = _context.Answers.Where(a => a.QuestionId == question.QuestionId && a.IsTrue == true).Count();
                totalRightAnswer += totalAnswer;
                if (question.Answers.Count <= totalAnswer && question.Answers.Count > 0) {
                    foreach (Answer answer in question.Answers) {
                        int isTrueAnswer = _context.Answers.Where(a => a.QuestionId == question.QuestionId && a.AnswerId == answer.AnswerId && a.IsTrue == true).Count();
                        if (isTrueAnswer != 0) {
                            userRightAnswer += isTrueAnswer;
                        }
                    }
                }
            }

            return (double) userRightAnswer / totalRightAnswer;
        }

        [Authorize(Roles = "1")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestion(int id) {
            var question = await _context.Questions.FindAsync(id);
            if (question == null) {
                return NotFound();
            }
            List<Answer> answers = _context.Answers.Where(a => a.QuestionId == id).ToList();
            foreach(Answer a in answers) {
                _context.Remove(a);
            }
            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool QuestionExists(int id) {
            return _context.Questions.Any(e => e.QuestionId == id);
        }
    }
}
