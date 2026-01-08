using AutoMapper;
using System.Threading.Tasks;
using System.Collections.Generic;
using WebApplication1.Core.DTOs;
using WebApplication1.Core.Entities;
using WebApplication1.Core.Interfaces;
using WebApplication1.Core.Models;

namespace WebApplication1.Service.Services;

public interface IPurchaseService : IGenericService<Purchase, PurchaseDto>
{
    //siparis olustrma icin metod
    Task<ServiceResponse<PurchaseDto>> CreatePurchaseAsync(CreatePurchaseDto dto);
}

public class PurchaseService : GenericService<Purchase, PurchaseDto>, IPurchaseService
{
    private readonly IGenericRepository<Merchandise> _merchandiseRepo;

    public PurchaseService(IGenericRepository<Purchase> repository, IMapper mapper, IGenericRepository<Merchandise> merchandiseRepo) : base(repository, mapper)
    {
        _merchandiseRepo = merchandiseRepo;
    }

    public override async Task<ServiceResponse<IEnumerable<PurchaseDto>>> GetAllAsync()
    {
        // Include properties
        var entities = await _repository.GetAllAsync("User", "PurchaseItems.Merchandise");
        var dtos = _mapper.Map<IEnumerable<PurchaseDto>>(entities);
        return new ServiceResponse<IEnumerable<PurchaseDto>> { Data = dtos };
    }

    public override async Task<ServiceResponse<PurchaseDto>> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id, "User", "PurchaseItems.Merchandise");
        if (entity == null)
            return new ServiceResponse<PurchaseDto> { Success = false, Message = "Not Found" };
        var dto = _mapper.Map<PurchaseDto>(entity);
        return new ServiceResponse<PurchaseDto> { Data = dto };
    }

    public async Task<ServiceResponse<PurchaseDto>> CreatePurchaseAsync(CreatePurchaseDto dto)
    {
        var purchase = _mapper.Map<Purchase>(dto);
        purchase.OrderDate = System.DateTime.UtcNow;
        
        decimal totalAmount = 0;

        var debugInfo = new System.Text.StringBuilder();
        debugInfo.AppendLine("Debug Log:");

        foreach (var item in purchase.PurchaseItems)
        {
            var merchandise = await _merchandiseRepo.GetByIdAsync(item.MerchandiseId);
            if (merchandise != null)
            {
                debugInfo.AppendLine($"Found Merchandise: {merchandise.Name} (ID: {merchandise.Id}), Price from DB: {merchandise.Price}");
                
                item.UnitPrice = merchandise.Price; // Snapshot price
                totalAmount += item.Quantity * item.UnitPrice;
                
                // Optional: Decrease stock
                merchandise.Stock -= item.Quantity;
                await _merchandiseRepo.UpdateAsync(merchandise);
            }
            else
            {
                 debugInfo.AppendLine($"Merchandise ID {item.MerchandiseId} NOT FOUND!");
            }
        }
        
        purchase.TotalAmount = totalAmount;
        debugInfo.AppendLine($"Calculated Total: {totalAmount}");
        
        await _repository.AddAsync(purchase);
        var purchaseDto = _mapper.Map<PurchaseDto>(purchase);
        
        return new ServiceResponse<PurchaseDto> { Data = purchaseDto, Message = debugInfo.ToString() };
    }
}
