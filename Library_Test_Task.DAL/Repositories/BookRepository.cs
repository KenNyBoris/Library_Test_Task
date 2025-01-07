using Library_Test_Task.DAL.Entities;
using Library_Test_Task.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Library_Test_Task.DAL
{
    public class BookRepository : IBookRepository
    {
        private static List<Book> _books;

        static BookRepository()
        {
            _books = new List<Book>();
        }

        public void AddMany(IEnumerable<Book> models)
        {
            _books.AddRange(models);
        }

        public void Create(Book model)
        {
            _books.Add(model);
        }

        public Book? Get(Guid id)
        {
            return _books.FirstOrDefault(s => s.Id == id);
        }

        public List<Book> GetAll()
        {
            return _books.ToList();
        }

        public void Update(Book model)
        {
            var item = _books.FirstOrDefault(s => s.Id == model.Id);
            if (item == null)
            {
                throw new Exception($"Book {model.Title} was not found.");
            }

            item = model;
        }
    }
}
