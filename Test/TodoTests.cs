using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NovaFori.Models;
using NovaFori.Controllers;
using NUnit.Framework;

namespace Test {
    [TestFixture]
    public class TodoTests {

	    private List<TodoItem> TodoList { get; set; }

        private TodoContext DbContext { get; set; }

        [OneTimeSetUp]
        public void OneTimeSetup() {
	        // Build DbContextOptions
            var dbContextOptions = new DbContextOptionsBuilder<TodoContext>()
	            .UseInMemoryDatabase("TodoList")
	            .Options;

            DbContext = new TodoContext(dbContextOptions);

            TodoList = new List<TodoItem> {
	            new TodoItem { Id = 1, Description = "test1", IsComplete = false},
	            new TodoItem { Id = 2, Description = "test2", IsComplete = false},
	            new TodoItem { Id = 3, Description = "test3", IsComplete = false},
	            new TodoItem { Id = 4, Description = "test4", IsComplete = false},
	            new TodoItem { Id = 5, Description = "test5", IsComplete = false}
            };

            DbContext.TodoItems.AddRange(TodoList);
            DbContext.SaveChanges();

        }

        [Test]
        public void GetTodoItems_Test() {
            //arrange
            var controller = new TodoItemsController(DbContext);

            //act
            var items = controller.GetTodoItems();

            //assert
            Assert.NotNull(items.Result.Value);
            Assert.AreEqual(5, items.Result.Value.Count());
            Assert.IsInstanceOf<IEnumerable<TodoItemDTO>>(items.Result.Value);
        }

        [Test]
        public void GetTodoItem_Test_Correct() {
            //arrange
            var controller = new TodoItemsController(DbContext);

            //act
            var item = controller.GetTodoItem(1);

	        //assert
	        Assert.NotNull(item.Result.Value);
	        Assert.AreEqual("test1", item.Result.Value.Description);
	        Assert.IsInstanceOf<TodoItemDTO>(item.Result.Value);

        }

        [Test]
        public void GetTodoItem_Test_NotFound() {
            //arrange
            var controller = new TodoItemsController(DbContext);

            //act
            var item = controller.GetTodoItem(0);

	        //assert
	        Assert.IsNull(item.Result.Value);
	        Assert.IsInstanceOf<NotFoundResult>(item.Result.Result);
        }

        [Test]
        public void PutTodoItem_Test_Correct() {
	        //arrange
	        var controller = new TodoItemsController(DbContext);
	        long id = 5;
	        var newDescription = "new description";
	        var newIsComplete = true;


	        var dto = new TodoItemDTO()
	        {
		        Id = id,
		        Description = newDescription,
		        IsComplete = newIsComplete
	        };

	        //act
	        var result = controller.PutTodoItem(id, dto);

            //assert
            var updatedItem = DbContext.TodoItems.Find(id);
            Assert.NotNull(updatedItem);
            Assert.AreEqual(newDescription, updatedItem.Description);
            Assert.AreEqual(newIsComplete, updatedItem.IsComplete);
            Assert.IsInstanceOf<NoContentResult>(result.Result);
        }

        [Test]
        public void PutTodoItem_Test_BadRequest() {
	        //arrange
	        var controller = new TodoItemsController(DbContext);
	        var id = 5;
	        var newDescription = "new description";
	        var newIsComplete = true;


	        var dto = new TodoItemDTO() {
		        Id = 6,
		        Description = newDescription,
		        IsComplete = newIsComplete
	        };

	        //act
	        var result = controller.PutTodoItem(id, dto);

	        //assert
	        Assert.IsInstanceOf<BadRequestResult>(result.Result);
        }

        [Test]
        public void PutTodoItem_Test_NotFound() {
	        //arrange
	        var controller = new TodoItemsController(DbContext);
	        var id = 100;
	        var newDescription = "new description";
	        var newIsComplete = true;


	        var dto = new TodoItemDTO() {
		        Id = id,
		        Description = newDescription,
		        IsComplete = newIsComplete
	        };

	        //act
	        var result = controller.PutTodoItem(id, dto);

	        //assert
	        Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        [Test]
        public void PostTodoItem_Test_Correct() {
	        //arrange
	        var controller = new TodoItemsController(DbContext);
	        long id = 6;
	        var newDescription = "test6";
	        var newIsComplete = false;


	        var dto = new TodoItemDTO() {
		        Id = id,
		        Description = newDescription,
		        IsComplete = newIsComplete
	        };

	        //act
	        var result = controller.PostTodoItem(dto);

	        //assert
	        var newItem = DbContext.TodoItems.Find(id);
            Assert.NotNull(newItem);
	        Assert.AreEqual(newDescription, newItem.Description);
	        Assert.AreEqual(newIsComplete, newItem.IsComplete);
	        Assert.AreEqual(6, DbContext.TodoItems.Count());
            Assert.IsInstanceOf<CreatedAtActionResult>(result.Result.Result);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown() {
	        DbContext.Dispose();
        }
    }
}