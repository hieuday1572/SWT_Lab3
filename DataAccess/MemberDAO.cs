using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class MemberDAO
    {
        private static MemberDAO instance = null;
        private static readonly object instancelock = new object();
        public static MemberDAO Instance
        {
            get
            {
                lock (instancelock)
                {
                    if (instance == null)
                    {
                        instance = new MemberDAO();
                    }
                    return instance;
                }
            }
        }

        public IEnumerable<Member> GetMemberList()
        {
            var members = new List<Member>();
            try
            {
                using var context = new AssSalesContext();
                members = context.Members.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return members;
        }

        public Member GetMemberByemail(string email)
        {
            Member member = null;
            try
            {
                using var context = new AssSalesContext();
                member = context.Members.SingleOrDefault(c => c.Email.Equals(email.Trim()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return member;
        }

        public Member GetMemberByID(int id)
        {
            Member member = null;
            try
            {
                using var context = new AssSalesContext();
                member = context.Members.SingleOrDefault(c => c.MemberId==id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return member;
        }

        public void AddNew(Member member)
        {
            try
            {
                Member _member = GetMemberByID(member.MemberId);
                if (_member == null)
                {
                    using var context = new AssSalesContext();
                    context.Members.Add(member);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("The member is already exist");
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Update(Member member)
        {
            try
            {
                Member _member = GetMemberByID(member.MemberId);
                if (_member != null)
                {
                    using var context = new AssSalesContext();
                    context.Members.Update(member);
                    context.SaveChanges();
                }
                else { throw new Exception("The member does not already exist"); }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public void Remove(int ID)
        {
            try
            {
                Member member = GetMemberByID(ID);
                if (member != null)
                {
                    using var context = new AssSalesContext();
                    context.Members.Remove(member);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("The member does not already exist");
                }

            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        static void Main(string[] args)

        {
            var list = MemberDAO.instance.GetMemberList();
            foreach (Member member in list)
            {
                Console.WriteLine(member.CompanyName);
            }

        }
    }
}
