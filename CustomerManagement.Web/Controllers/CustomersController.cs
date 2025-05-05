using AutoMapper;
using CustomerManagement.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using CustomerManagement.Application.Interfaces;
using CustomerManagement.Common.DTOs;

namespace CustomerManagement.Web.Controllers;

public class CustomersController : BaseController
{
    private readonly ICustomerService _customerService;
    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }
    #region SPA
    public async Task<IActionResult> SPA()
    {
        var customerDtos = await _customerService.GetAllCustomersAsync();
        return View(customerDtos);
    }
    
    [HttpPost]
    public async Task<JsonResult> CreateCustomerJ([FromBody] CustomerDto objCustomer)
    {
        try
        {
            await _customerService.AddCustomerAsync(objCustomer);
            return Json(new { success = true, data = objCustomer });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<JsonResult> GetCustomerByIdJ(int id)
    {
        try
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer != null)
            {
                return Json(new { success = true, data = customer });
            }
            return Json(new { success = false, message = "Customer not found" });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }
    
    [HttpPost]
    public async Task<JsonResult> UpdateCustomerJ([FromBody] CustomerDto objCustomer)
    {
        try
        {
            await _customerService.UpdateCustomerAsync(objCustomer);
            return Json(new { success = true, data = objCustomer });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<JsonResult> DeleteCustomerJ(int id)
    {
        try
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer != null)
            {
                await _customerService.DeleteCustomerAsync(id);
                return Json(new { success = true, message = "Customer deleted" });
            }
            return Json(new { success = false, message = "Customer not found" });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }
    #endregion SPA
    #region GRUD

    public async Task<IActionResult> Index()
    {
        var customerDtos = await _customerService.GetAllCustomersAsync();
        return View(customerDtos);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CustomerDto customer)
    {
        if (ModelState.IsValid)
        {
            await _customerService.AddCustomerAsync(customer);
            return RedirectToAction(nameof(Index));
        }
        return View(customer);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var customer = await _customerService.GetCustomerByIdAsync(id);
        if (customer == null) return NotFound();
        return View(customer);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(CustomerDto customer)
    {
        if (ModelState.IsValid)
        {
            await _customerService.UpdateCustomerAsync(customer);
            return RedirectToAction(nameof(Index));
        }
        return View(customer);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var customer = await _customerService.GetCustomerByIdAsync(id);
        if (customer == null) return NotFound();
        return View(customer);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _customerService.DeleteCustomerAsync(id);
        return RedirectToAction(nameof(Index));
    }
    #endregion GRUD
}
