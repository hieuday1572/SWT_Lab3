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
    public class OrderDetailsController : Controller
    {
        private readonly AssSalesContext _context;
        private readonly IOrderDetailRepository _orderDetailRepository;
        public OrderDetailsController(AssSalesContext context)
        {
            _context = context;
            _orderDetailRepository = new OrderDetailRepository();
        }

        // GET: OrderDetails
        public async Task<IActionResult> Index()
        {
            var od = _orderDetailRepository.GetOrderDetails();
            return View(od);
        }

        // GET: OrderDetails/Details/5
        public async Task<IActionResult> Details(int id, int proId)
        {
            if (id == null || _context.OrderDetails == null)
            {
                return NotFound();
            }

            var orderDetail = _orderDetailRepository.GetOrderDetail(id,proId);
            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }

        // GET: OrderDetails/Create
        public IActionResult Create()
        {
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId");
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName");
            return View();
        }

        // POST: OrderDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,ProductId,UnitPrice,Quantity,Discount")] OrderDetail orderDetail)
        {
            var od = _orderDetailRepository.GetOrderDetails();
 foreach(var item in od)
            {
                if(orderDetail.OrderId == item.OrderId && orderDetail.ProductId == item.ProductId)
                {
                    return View(orderDetail);
                }
            }
            if (!ModelState.IsValid)
            {
                _orderDetailRepository.Create(orderDetail);
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", orderDetail.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName", orderDetail.ProductId);
            return View(orderDetail);

        }

        // GET: OrderDetails/Edit/5
        public async Task<IActionResult> Edit(int id, int proId)
        {
            if (id == null || _context.OrderDetails == null)
            {
                return NotFound();
            }

            var orderDetail = _orderDetailRepository.GetOrderDetail(id, proId);
            if (orderDetail == null)
            {
                return NotFound();
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", orderDetail.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName", orderDetail.ProductId);
            return View(orderDetail);
        }

        // POST: OrderDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,ProductId,UnitPrice,Quantity,Discount")] OrderDetail orderDetail)
        {
            if (id != orderDetail.OrderId)
            {
                return NotFound();
            }

                    _orderDetailRepository.Update(orderDetail);
                return RedirectToAction(nameof(Index));
            }
        

        // GET: OrderDetails/Delete/5
        public async Task<IActionResult> Delete(int id, int proId)
        {
            if (id == null || _context.OrderDetails == null)
            {
                return NotFound();
            }

            var orderDetail = _orderDetailRepository.GetOrderDetail(id, proId);
            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }

        // POST: OrderDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int proId)
        {
            if (_context.OrderDetails == null)
            {
                return Problem("Entity set 'AssSalesContext.OrderDetails'  is null.");
            }
            var orderDetail = _orderDetailRepository.GetOrderDetail(id, proId);
            if (orderDetail != null)
            {
                _orderDetailRepository.Delete(orderDetail.OrderId, orderDetail.ProductId);
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool OrderDetailExists(int id)
        {
          return (_context.OrderDetails?.Any(e => e.OrderId == id)).GetValueOrDefault();
        }
    }
}
