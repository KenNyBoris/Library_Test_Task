using Library_Test_Task.DAL.Entities;
using System;
using System.Collections.Generic;

namespace Library_Test_Task.DAL.Repositories
{
    public interface IBookRepository
    {
        void Create(Book model);
        void AddMany(IEnumerable<Book> models);
        Book? Get(Guid id);
        void Update(Book model);
        List<Book> GetAll();
    }
}
