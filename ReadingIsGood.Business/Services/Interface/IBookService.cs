using ReadingIsGood.Core.DTOs;
using ReadingIsGood.Core.Models;
using ReadingIsGood.Core.Models.Requests;

namespace ReadingIsGood.Business.Services.Interface
{
    public interface IBookService
    {
        public Task<Response<BookDTO>> Insert(BookRequest request);
        public Task<Response<BookDTO>> UpdateStock(BookStockUpdateRequest request);
    }
}
