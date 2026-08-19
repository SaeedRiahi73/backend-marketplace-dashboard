using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task_Application.Common.Responses;
using Task_Application.Dtos.Product;
using Task_Application.Features.Products.Requests.Commands;
using Task_Application.Features.Products.Requests.Queries;
using Task_Application.Features.Users.Requests.Commands;
using Task_Domain.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Task_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Demo")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/<ProductsController>
        [HttpGet("GetAllProduct")]
        public async Task<IActionResult> GetAllProduct()
        {
            GetAllProductsQueryRequest Request = new GetAllProductsQueryRequest();
            ResultInfo<IEnumerable<ProductDto>> response = await _mediator.Send(Request);

            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }

        // GET api/<ProductsController>/5
        [HttpGet("GetProduct/{productId}")]
        public async Task<IActionResult> GetProduct(Guid productId)
        {
            GetProductQueryRequest Request = new GetProductQueryRequest { productId = productId };
            ResultInfo<ProductDto> response = await _mediator.Send(Request);

            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }

        // POST api/<ProductsController>
        [Authorize(Roles = "Admin")]
        [HttpPost("CreateProduct")]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductDto createProductDto)
        {

            CreateProductCommandRequest Request = new CreateProductCommandRequest { CreateProduct = createProductDto };
            ResultInfo<Guid> response = await _mediator.Send(Request);

            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);

        }

        // PUT api/<ProductsController>/5
        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> Put([FromForm] UpdateProductDto updateProductDto)
        {
            UpdateProductCommandRequest Request = new UpdateProductCommandRequest { updateProductDto = updateProductDto };
            ResultInfo<Unit> response = await _mediator.Send(Request);

            if (!response.IsSuccess)
                return BadRequest(response.Errors);

            return Ok(response);
        }

        // DELETE api/<ProductsController>/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteProduct/{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            DeleteProductCommandRequest Request = new DeleteProductCommandRequest { productId = id };
            ResultInfo<Unit> response = await _mediator.Send(Request);

            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
