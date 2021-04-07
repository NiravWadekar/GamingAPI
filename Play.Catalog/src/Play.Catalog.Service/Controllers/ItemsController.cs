using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Play.Common;
using src.Play.Catalog.Service.Dtos;
using src.Play.Catalog.Service.Entities;

namespace src.Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly IRepository<Item> _itemsRepository;
        public ItemsController(IRepository<Item> itemsRepository)
        {
            _itemsRepository = itemsRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetAsync()
        {
            var items = (await _itemsRepository.GetAllAsync())
                        .Select(item => item.AsDto());
            return items;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid id)
        {
            var item = (await _itemsRepository.GetAsync(id));

            if (item == null) return NotFound();

            return item.AsDto();
        }

        [HttpPost]
        public async Task<ActionResult<ItemDto>> PostAsync(CreatedItemDto createdItemDto)
        {
            var item = new Item
            {
                Name = createdItemDto.name,
                Description = createdItemDto.description,
                Price = createdItemDto.price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            await _itemsRepository.CreateAsync(item);

            return CreatedAtAction(nameof(GetByIdAsync), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(Guid id, UpdatedItemDto updatedItemDto)
        {
            var existingItem = await _itemsRepository.GetAsync(id);

            if (existingItem == null) return NotFound();

            existingItem.Name = updatedItemDto.name;
            existingItem.Description = updatedItemDto.description;
            existingItem.Price = updatedItemDto.price;

            await _itemsRepository.UpdateAsync(existingItem);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var item = await _itemsRepository.GetAsync(id);

            if (item == null) return NotFound();

            await _itemsRepository.RemoveAsync(item.Id);

            return NoContent();
        }
    }
}