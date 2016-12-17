using Microsoft.AspNetCore.Mvc;

namespace Goyello.ITADApp.Controllers
{
    [Route("api/[controller]")]
    public class ItemsController : Controller
    {
        private readonly IItemsService _itemsService;
        public ItemsController(IItemsService itemsService)
        {
            _itemsService = itemsService; 
        }

        [HttpGet]
        public IActionResult Get()
        {
            var items = _itemsService.Get();
            return Ok(items);
        }

        [HttpGet, Route("{id:int}")]
        public IActionResult Get(int id)
        {
            var item = _itemsService.GetById(id);
            return Ok(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Item item)
        {
            var createdItem = _itemsService.Create(item);
            return CreatedAtAction("Get", new { id = item.Id }, createdItem);
        }

        [HttpPut, Route("{id:int}")]
        public IActionResult Update([FromBody] Item item, int id)
        {
            var result = _itemsService.UpdateFor(id, item);
            if (result) 
            {
                return Ok();
            } 
            else 
            {
                return BadRequest("Cannot remove object");
            }
        }

        [HttpDelete, Route("{id:int}")]
        public IActionResult Delete(int id) 
        {
            var result = _itemsService.DeleteById(id);
            if (result) 
            {
                return Ok();
            } 
            else 
            {
                return BadRequest("Cannot remove object");
            }
        }

        [HttpGet, Route("alwaysbad")]
        public IActionResult AlwaysBadRequest() {
            return BadRequest(new {
                Message = "Because I'm bad, bad, bad.."
            });
        }
    }
}
