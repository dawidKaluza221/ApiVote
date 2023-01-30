using System.ComponentModel.DataAnnotations;

namespace ApiVote.UserInfo
{
    public class OwnerPoll
    {
        [Key]
        public int ID_Poll { get; set; }
        public int ID_User { get; set; }
        public string PollName { get; set; }
        public int ProtectionAction { get; set; }
        public int ProtectionView { get; set; }

    }
}
