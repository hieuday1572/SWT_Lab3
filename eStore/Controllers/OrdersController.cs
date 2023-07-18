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
    public class OrdersController : Controller
    {
        private readonly AssSalesContext _context;
        private readonly IOrderRepository _orderRepository;
        public OrdersController(AssSalesContext context)
        {
            _context = context;
            _orderRepository = new OrderRepository();
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var orders = _orderRepository.GetOrders();
            return View(orders);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = _orderRepository.GetOrder(id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["MemberId"] = new SelectList(_context.Members, "MemberId", "City");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,MemberId,OrderDate,RequiredDate,ShippedDate,Freight")] Order order)
        {
            if (order.MemberId == null)
            {
                return View(order);
            }
                _orderRepository.Create(order);
                return RedirectToAction(nameof(Index));
            
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = _orderRepository.GetOrder(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["MemberId"] = _context.Members.ToList();
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,MemberId,OrderDate,RequiredDate,ShippedDate,Freight")] Order order)
        {
            try
            {
                _orderRepository.Update(order);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(order.OrderId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
                return RedirectToAction(nameof(Index));

            }
        

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = _orderRepository.GetOrder(id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'AssSalesContext.Orders'  is null.");
            }
            var order = _orderRepository.GetOrder(id);
            if (order != null)
            {
                _orderRepository.Delete(order.OrderId);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> UserOrder(string username)
        {
            Member user = _context.Members.FirstOrDefault(p=>p.Email.Equals(username));
            var order = _context.Orders.Include(p => p.Member).Where(p => p.MemberId == user.MemberId).ToList();
            return View(order);
        }

        private bool OrderExists(int id)
        {
          return (_context.Orders?.Any(e => e.OrderId == id)).GetValueOrDefault();
        }
    }
}
