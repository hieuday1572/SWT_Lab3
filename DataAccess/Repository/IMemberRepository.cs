using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IMemberRepository
    {
        IEnumerable<Member> GetMembers();

        void Update(Member member);

        void Create(Member member);

        void Delete(string email);

        Member GetMember(string email);
        void GetMember(int memberId);
    }
}
