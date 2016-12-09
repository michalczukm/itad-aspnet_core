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

        [HttpGet, Route("alwaysbad")]
        public IActionResult AlwaysBadRequest() {
            return BadRequest(new {
                Message = "Because I'm bad, bad, bad.."
            });
        }
    }
}
