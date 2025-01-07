using Library_Test_Task.DAL.Entities;
using System;
using System.Collections.Generic;

namespace Library_Test_Task
{
    public interface IBookService
    {
        void Create(Book model);
        Book? Get(Guid id);
        void Update(Book model);
        void Import(string filePath);
        void Export();
        IEnumerable<Book> Search(string subString);

    }
}
