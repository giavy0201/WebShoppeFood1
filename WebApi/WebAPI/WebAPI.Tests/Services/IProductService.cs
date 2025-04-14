using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Tests.Models;

namespace WebAPI.Tests.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ContentProductDtos>> GetListContent();
        Task<ContentProductDtos> GetContentById(int id);
        Task<IEnumerable<ContentProductDtos>> GetListContentByCate(int categoryId);
        Task<CateDtos> GetCateById(int id);
        Task<IEnumerable<CateDtos>> GetListCategory();
        Task<ApiResponse<IEnumerable<ContentProductDtos>>> SearchProducts(string keyword, int? categoryId = null);
    }
} 