using System;
using System.Collections.Generic;

#nullable disable

namespace Online_Quiz.Models
{
    public partial class Question
    {
        public Question()
        {
            Answers = new HashSet<Answer>();
        }

        public int QuestionId { get; set; }
        public string Content { get; set; }
        public int? UserId { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }
    }
}
