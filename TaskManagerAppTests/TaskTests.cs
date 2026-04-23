using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TaskManagerApp.Tests
{
    [TestClass]
    public class TaskTests
    {
        [TestMethod]
        public void Test_Constructor_SetsDescription()
        {
            var task = new Task("Тестовая задача");

            Assert.AreEqual("Тестовая задача", task.Description);
            Assert.IsFalse(task.IsCompleted);
        }

        [TestMethod]
        public void Test_Constructor_DefaultNotCompleted()
        {
            var task = new Task("Любая задача");
            Assert.IsFalse(task.IsCompleted);
        }

        [TestMethod]
        public void Test_Description_ChangeValue()
        {
            var task = new Task("Старое");
            task.Description = "Новое";

            Assert.AreEqual("Новое", task.Description);
        }

        [TestMethod]
        public void Test_Description_SetNull()
        {
            var task = new Task("Было");
            task.Description = null;

            Assert.IsNull(task.Description);
        }

        [TestMethod]
        public void Test_Description_SetEmpty()
        {
            var task = new Task("Было");
            task.Description = "";

            Assert.AreEqual("", task.Description);
        }

        [TestMethod]
        public void Test_Description_VeryLong()
        {
            var task = new Task("Коротко");
            string longText = new string('X', 10000);

            task.Description = longText;

            Assert.AreEqual(10000, task.Description.Length);
        }

        [TestMethod]
        public void Test_IsCompleted_SetTrue()
        {
            var task = new Task("Задача");
            task.IsCompleted = true;

            Assert.IsTrue(task.IsCompleted);
        }

        [TestMethod]
        public void Test_IsCompleted_SetFalse()
        {
            var task = new Task("Задача");
            task.IsCompleted = true;
            task.IsCompleted = false;

            Assert.IsFalse(task.IsCompleted);
        }

        [TestMethod]
        public void Test_IsCompleted_ToggleMultiple()
        {
            var task = new Task("Задача");

            task.IsCompleted = true;
            Assert.IsTrue(task.IsCompleted);

            task.IsCompleted = false;
            Assert.IsFalse(task.IsCompleted);

            task.IsCompleted = true;
            Assert.IsTrue(task.IsCompleted);
        }

        [TestMethod]
        public void Test_Constructor_NullDescription()
        {
            var task = new Task(null);
            Assert.IsNull(task.Description);
        }

        [TestMethod]
        public void Test_Constructor_EmptyDescription()
        {
            var task = new Task("");
            Assert.AreEqual("", task.Description);
        }
    }
}