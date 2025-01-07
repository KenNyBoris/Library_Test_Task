using Library_Test_Task.DAL.Entities;
using Library_Test_Task.DAL.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Library_Test_Task.Tests
{
    [TestClass]
    public class BookServiceTests
    {
        private BookService _sut;
        private Mock<IBookRepository> _bookRepositoryMock;
        [TestInitialize]
        public void Initialize()
        {
            _bookRepositoryMock = new Mock<IBookRepository>();
            _sut = new BookService(_bookRepositoryMock.Object);
        }

        [TestMethod]
        public void Create_BookAddedToListSuccesfully()
        {
            // Arrange
            var testId = Guid.NewGuid();
            var item = new Book
            {
                Author = "John Doe",
                Id = testId,
                PagesCount = 33,
                Title = "test"
            };

            // Act
            _sut.Create(item);

            // Assert
            _bookRepositoryMock.Verify(s => s.Create(It.Is<Book>(x => x == item)), Times.Once);
        }

        [TestMethod]
        public void Get_BookReturnedSuccesfully()
        {
            // Arrange
            var testId = Guid.NewGuid();
            _bookRepositoryMock.Setup(s => s.Get(It.IsAny<Guid>())).Returns(new Book());

            // Act
            _sut.Get(testId);

            // Assert
            _bookRepositoryMock.Verify(s => s.Get(It.Is<Guid>(x => x == testId)), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Get_BookReturnsException()
        {
            // Arrange
            var testId = Guid.NewGuid();
            _bookRepositoryMock.Setup(s => s.Get(It.IsAny<Guid>())).Returns(default(Book));

            // Act
            _sut.Get(testId);
        }

        [TestMethod]
        public void Update_BookUpdateedSuccesfully()
        {
            // Arrange
            var testId = Guid.NewGuid();
            var item = new Book
            {
                Author = "John Doe",
                Id = testId,
                PagesCount = 33,
                Title = "test"
            };

            // Act
            _sut.Update(item);

            // Assert
            _bookRepositoryMock.Verify(s => s.Update(It.Is<Book>(x => x == item)), Times.Once);

        }

        [TestMethod]
        public void Search_BooksFilteredAndSorted_Succesfully()
        {
            // Arrange
            var testId = Guid.NewGuid();
            _bookRepositoryMock.Setup(s => s.GetAll()).Returns(new List<Book>
            {
                new Book
                {
                    Author = "a",
                    Id = Guid.NewGuid(),
                    PagesCount = 4,
                    Title = "btest"
                },
                new Book
                {
                    Author = "c",
                    Id = Guid.NewGuid(),
                    PagesCount = 4,
                    Title = "ztest"
                },
                new Book
                {
                    Author = "o",
                    Id = Guid.NewGuid(),
                    PagesCount = 4,
                    Title = "ntest"
                },
                new Book
                {
                    Author = "w",
                    Id = Guid.NewGuid(),
                    PagesCount = 4,
                    Title = "a"
                },
                new Book
                {
                    Author = "e",
                    Id = Guid.NewGuid(),
                    PagesCount = 4,
                    Title = "htest"
                },
            });

            // Act
            var result = _sut.Search("test").ToList();

            // Assert
            Assert.IsTrue(result.All(s => s.Title.Contains("test")));
            Assert.IsTrue(Enumerable.SequenceEqual(result, result.OrderBy(s => s.Author).ThenBy(s => s.Title)));
            _bookRepositoryMock.Verify(s => s.GetAll(), Times.Once);
        }

        [TestMethod]
        public void Import_BooksImported()
        {
            // Arrange
            _sut.Import("test.xml");

            // Assert
            _bookRepositoryMock.Verify(s => s.AddMany(It.Is<List<Book>>(s => s.Count == 5000)), Times.Once);
        }

        [TestMethod]
        public void Export_BooksExported()
        {
            // Arrange
            var list = new List<Book>();
            for (int i = 0; i < 3; i++)
            {
                list.Add(new Book
                {
                    Author = "test",
                    Id = Guid.NewGuid(),
                    PagesCount = i,
                    Title = $"test item {i}"
                });
            }
            _bookRepositoryMock.Setup(s => s.GetAll()).Returns(list);

            // Act
            _sut.Export();

            // Assert
            _bookRepositoryMock.Verify(s => s.GetAll(), Times.Once);
        }
    }
}
