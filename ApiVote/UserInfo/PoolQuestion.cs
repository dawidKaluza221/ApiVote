using System.ComponentModel.DataAnnotations;

namespace ApiVote.UserInfo
{
    public class PoolQuestion
    {
        [Key]
        public int ID_Question { get; set; }
        public int ID_Poll { get; set; }
        public string Question { get; set; }
    }
}
