using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using DataAccess.Repository;

namespace eStore.Controllers
{
    public class MembersController : Controller
    {
        private readonly IMemberRepository _memeberRepository;
        private readonly AssSalesContext _context = new AssSalesContext();
        private IMemberRepository @object;

        public MembersController(AssSalesContext context, IMemberRepository memberRepositoryy)
        {
            _memeberRepository = memberRepositoryy;

        }

        public MembersController(IMemberRepository @object)
        {
            this.@object = @object;
        }

        // GET: Members
        public async Task<IActionResult> Index()
        {
            IEnumerable<Member> members= _memeberRepository.GetMembers();
            return View(members); 

        }

        // GET: Members/Details/5
        public async Task<IActionResult> Details(string email)
        {
            if (email == null)
            {
                return NotFound();
            }

            var member = _memeberRepository.GetMember(email);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // GET: Members/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MemberId,Email,CompanyName,City,Country,Password")] Member member)
        {
            if (ModelState.IsValid)
            {
                _memeberRepository.Create(member);
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        // GET: Members/Edit/5
        public async Task<IActionResult> Edit(string email)
        {
            if (email == null)
            {
                return NotFound();
            }

            var member = _memeberRepository.GetMember(email);
            if (member == null)
            {
                return NotFound();
            }
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MemberId,Email,CompanyName,City,Country,Password")] Member member)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _memeberRepository.Update(member);
                }
                catch (DbUpdateConcurrencyException)
                {
                }
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        // GET: Members/Delete/5
        public async Task<IActionResult> Delete(string? email)
        {
            if (email == null)
            {
                return NotFound();
            }

            var member = _memeberRepository.GetMember(email);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string email)
        {
            var member = _memeberRepository.GetMember(email);
            if (member != null)
            {
                _memeberRepository.Delete(member.Email);
            }
            
            return RedirectToAction(nameof(Index));
        }
    }
}
