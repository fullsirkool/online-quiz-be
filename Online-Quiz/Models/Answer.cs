using System;
using System.Collections.Generic;

#nullable disable

namespace Online_Quiz.Models
{
    public partial class Answer
    {
        public int QuestionId { get; set; }
        public int AnswerId { get; set; }
        public string Content { get; set; }
        public bool IsTrue { get; set; }

        public virtual Question Question { get; set; }
    }
}
