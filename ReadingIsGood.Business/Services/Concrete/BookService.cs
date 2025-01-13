using Azure.Core;
using ReadingIsGood.Business.Base.Concrete;
using ReadingIsGood.Business.Services.Interface;
using ReadingIsGood.Core.DTOs;
using ReadingIsGood.Core.Entities;
using ReadingIsGood.Core.Models;
using ReadingIsGood.Core.Models.Requests;

namespace ReadingIsGood.Business.Services.Concrete
{
    public class BookService : BaseService<Book, BookDTO>, IBookService
    {
        public BookService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<Response<BookDTO>> Insert(BookRequest request)
        {
            var bookData = new Book()
            {
                Name = request.Name,
                Quantity = request.Quantity,
                Price = request.Price
            };

            var book = await UnitOfWork.Repository<Book>().AddAsync(bookData);

            return new Response<BookDTO>(Mapper.Map<BookDTO>(book));
        }

        public async Task<Response<BookDTO>> UpdateStock(BookStockUpdateRequest request)
        {
            var book = await UnitOfWork.Repository<Book>().GetByIdAsync(request.BookId);

            if (book == null)
            {
                return new Response<BookDTO>(null, false)
                {
                    Errors = new List<Error>() { new Error() { ErrorMessage = "Record Not Found." } }
                };
            }

            book.Quantity = request.Quantity;

            _ = await UnitOfWork.Repository<Book>().UpdateAsync(book);

            return new Response<BookDTO>(Mapper.Map<BookDTO>(book));
        }
    }
}
