using ReadingIsGood.Business.Base.Concrete;
using ReadingIsGood.Business.Services.Interface;
using ReadingIsGood.Core.DTOs;
using ReadingIsGood.Core.Entities;
using ReadingIsGood.Core.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingIsGood.Business.Services.Concrete
{
    public class BookService : BaseService<Book, BookDTO>, IBookService
    {
        public BookService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<BookDTO> Insert(BookRequest request)
        {
            var bookData = new Book()
            {
                Name = request.Name,
                Quantity = request.Quantity,
            };

            var book = await UnitOfWork.Repository<Book>().AddAsync(bookData);

            return Mapper.Map<BookDTO>(book);
        }
    }
}
