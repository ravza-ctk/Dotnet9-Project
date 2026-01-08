using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;
using WebApplication1.Core.Entities;
using WebApplication1.Core.Interfaces;
using WebApplication1.Core.DTOs;
using WebApplication1.Core.Models;
using AutoMapper;

namespace WebApplication1.Endpoints;

public static class CollectionEndpoints
{
    //endpoint listim
    public static void MapCollectionEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/collections")
            .WithTags("Collections")
            .WithOpenApi();

        group.MapGet("/", async (IGenericService<Collection, CollectionDto> service) =>
        {
            return Results.Ok(await service.GetAllAsync());
        });

        group.MapGet("/{id}", async (int id, IGenericService<Collection, CollectionDto> service) =>
        {
             var result = await service.GetByIdAsync(id);
             return result.Success ? Results.Ok(result) : Results.NotFound(result);
        });

        group.MapPost("/", async (CollectionDto dto, IGenericService<Collection, CollectionDto> service) =>
        {
             var result = await service.AddAsync(dto);
             return Results.Created($"/api/collections/{result.Data.Id}", result);
        });

        group.MapPut("/{id}", async (int id, CollectionDto dto, IGenericService<Collection, CollectionDto> service) =>
        {
             var result = await service.UpdateAsync(id, dto);
             return result.Success ? Results.Ok(result) : Results.NotFound(result);
        });

        group.MapDelete("/{id}", async (int id, IGenericService<Collection, CollectionDto> service) =>
        {
             var result = await service.DeleteAsync(id);
             return result.Success ? Results.Ok(result) : Results.NotFound(result);
        });
    }
}
