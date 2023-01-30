using System.ComponentModel.DataAnnotations;

namespace ApiVote.UserInfo
{
    public class Answers
    {
        [Key]
        public int IDAnswer { get; set; }
        public int IDQuestion { get; set; }
        public string Answer { get; set; }
        public int CounterAnswer { get; set; }
    }
}
