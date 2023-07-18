using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class MemberRepository : IMemberRepository
    {
        public void Create(Member member)
        {
            MemberDAO.Instance.AddNew(member);
        }

        public void Delete(string email)
        {
            Member member = GetMember(email);
            MemberDAO.Instance.Remove(member.MemberId);
        }

        public Member GetMember(string email)
        {
            return MemberDAO.Instance.GetMemberByemail(email);
        }

        public IEnumerable<Member> GetMembers() => MemberDAO.Instance.GetMemberList();

        public void Update(Member member)
        {
            MemberDAO.Instance.Update(member);
        }
    }
}
