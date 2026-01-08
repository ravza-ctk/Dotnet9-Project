using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApplication1.Core.DTOs;
using WebApplication1.Service.Services; // Ensure Ref to Service namespace

namespace WebApplication1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PurchasesController : ControllerBase
{
    private readonly IPurchaseService _service;

    public PurchasesController(IPurchaseService service)
    {
        _service = service;
    }

    [HttpGet]
    [Authorize] // Any logged in user
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (!result.Success) return NotFound(result);
        return Ok(result);
    }

    [HttpPost]
    [Authorize] //kullancilar authorize ile adminsiz sipariş yapabilr
    public async Task<IActionResult> Create(CreatePurchaseDto dto)
    {
        var result = await _service.CreatePurchaseAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result);
    }
}
