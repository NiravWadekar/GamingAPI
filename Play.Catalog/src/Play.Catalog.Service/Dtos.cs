using System;
using System.ComponentModel.DataAnnotations;

namespace src.Play.Catalog.Service.Dtos
{
    public record ItemDto(Guid id, string name, string description, decimal price, DateTimeOffset createdDate);

    public record CreatedItemDto([Required] string name, string description,[Range(0,1000)] decimal price);
    public record UpdatedItemDto([Required] string name, string description,[Range(0,1000)] decimal price);
}