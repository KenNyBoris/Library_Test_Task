using Library_Test_Task.DAL.Entities;
using Library_Test_Task.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Library_Test_Task
{
    public class BookService : IBookService
    {
        private IBookRepository _bookRepository;
        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }
        public void Create(Book model)
        {
            _bookRepository.Create(model);
        }

        public void Export()
        {
            var serializer = new XmlSerializer(typeof(List<Book>));

            using (StreamWriter writer = new StreamWriter("output.xml"))
            {
                var books = _bookRepository.GetAll();
                serializer.Serialize(writer, books);
            }
        }

        public Book? Get(Guid id)
        {
            var item = _bookRepository.Get(id);
            if (item == null)
            {
                throw new Exception($"Book with id - {id} not found");
            }

            return item;
        }

        public void Import(string filePath)
        {
            var serializer = new XmlSerializer(typeof(List<Book>));

            using (Stream reader = new FileStream(filePath, FileMode.Open))
            {
                var books = (List<Book>)serializer.Deserialize(reader);
                _bookRepository.AddMany(books);
            }
        }

        public IEnumerable<Book> Search(string subString)
        {
            return _bookRepository.GetAll()
                    .Where(s => s.Title.ToLower()
                    .Contains(subString.ToLower()))
                    .OrderBy(s => s.Author)
                    .ThenBy(s => s.Title);
        }

        public void Update(Book model)
        {
            _bookRepository.Update(model);
        }
    }
}
