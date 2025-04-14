using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using WebAPI.Tests.Models;
using WebAPI.Tests.Services;

namespace WebAPI.Tests.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductService productService, ILogger<ProductsController> logger = null)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> ListContent()
        {
            try
            {
                var contents = await _productService.GetListContent();
                return Ok(new ApiResponse<IEnumerable<ContentProductDtos>>(true, "Truy Xuất Thành Công", contents));
            }
            catch (System.Exception ex)
            {
                _logger?.LogError(ex, "Lỗi khi lấy danh sách sản phẩm");
                return BadRequest(new ApiResponse<IEnumerable<ContentProductDtos>>(false, "Có lỗi xảy ra khi lấy danh sách sản phẩm", null));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContentById(int id)
        {
            try
            {
                var content = await _productService.GetContentById(id);
                if (content == null)
                {
                    return NotFound(new ApiResponse<string>(false, "Không Tìm Thấy Nội Dung", string.Empty));
                }
                return Ok(new ApiResponse<ContentProductDtos>(true, "Truy Xuất Thành Công", content));
            }
            catch (System.Exception ex)
            {
                _logger?.LogError(ex, "Lỗi khi lấy thông tin sản phẩm");
                return BadRequest(new ApiResponse<string>(false, "Có lỗi xảy ra khi lấy thông tin sản phẩm", string.Empty));
            }
        }

        [HttpGet("category/{id}")]
        public async Task<IActionResult> ListContentByCate(int id)
        {
            try
            {
                var contents = await _productService.GetListContentByCate(id);
                if (!contents.Any())
                {
                    return NotFound(new ApiResponse<string>(false, "Không Tìm Thấy Sản Phẩm Trong Danh Mục Này", string.Empty));
                }
                return Ok(new ApiResponse<IEnumerable<ContentProductDtos>>(true, "Truy Xuất Thành Công", contents));
            }
            catch (System.Exception ex)
            {
                _logger?.LogError(ex, "Lỗi khi lấy danh sách sản phẩm theo danh mục");
                return BadRequest(new ApiResponse<string>(false, "Có lỗi xảy ra khi lấy danh sách sản phẩm theo danh mục", string.Empty));
            }
        }

        [HttpGet("categories")]
        public async Task<IActionResult> ListCategory()
        {
            try
            {
                var categories = await _productService.GetListCategory();
                return Ok(new ApiResponse<IEnumerable<CateDtos>>(true, "Truy Xuất Thành Công", categories));
            }
            catch (System.Exception ex)
            {
                _logger?.LogError(ex, "Lỗi khi lấy danh sách danh mục");
                return BadRequest(new ApiResponse<string>(false, "Có lỗi xảy ra khi lấy danh sách danh mục", string.Empty));
            }
        }

        [HttpGet("categories/{id}")]
        public async Task<IActionResult> GetCateById(int id)
        {
            try
            {
                var category = await _productService.GetCateById(id);
                if (category == null)
                {
                    return NotFound(new ApiResponse<string>(false, "Không Tìm Thấy Danh Mục", string.Empty));
                }
                return Ok(new ApiResponse<CateDtos>(true, "Truy Xuất Thành Công", category));
            }
            catch (System.Exception ex)
            {
                _logger?.LogError(ex, "Lỗi khi lấy thông tin danh mục");
                return BadRequest(new ApiResponse<string>(false, "Có lỗi xảy ra khi lấy thông tin danh mục", string.Empty));
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchProducts([FromQuery] string keyword, [FromQuery] int? categoryId = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                {
                    return BadRequest(new ApiResponse<string>(false, "Từ khóa tìm kiếm không được để trống", string.Empty));
                }

                var result = await _productService.SearchProducts(keyword, categoryId);
                if (!result.IsSuccess)
                {
                    return NotFound(new ApiResponse<string>(false, "Không tìm thấy sản phẩm phù hợp", string.Empty));
                }

                return Ok(result);
            }
            catch (System.Exception ex)
            {
                _logger?.LogError(ex, "Lỗi khi tìm kiếm sản phẩm");
                return BadRequest(new ApiResponse<string>(false, "Có lỗi xảy ra khi tìm kiếm sản phẩm", string.Empty));
            }
        }
    }
} 