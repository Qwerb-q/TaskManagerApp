using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace TaskManagerApp.Tests
{
    [TestClass]
    public class TaskManagerTests
    {
        private const string TestFilePath = "tasks.txt";

        [TestInitialize]
        public void TestInitialize()
        {
            if (File.Exists(TestFilePath))
                File.Delete(TestFilePath);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (File.Exists(TestFilePath))
                File.Delete(TestFilePath);
        }


        [TestMethod]
        public void Test_AddTask_Single()
        {
            var manager = new TaskManager();
            manager.AddTask("Новая задача");

            Assert.AreEqual(1, manager.Tasks.Count);
            Assert.AreEqual("Новая задача", manager.Tasks[0].Description);
            Assert.IsFalse(manager.Tasks[0].IsCompleted);
        }

        [TestMethod]
        public void Test_AddTask_Multiple()
        {
            var manager = new TaskManager();
            manager.AddTask("Задача 1");
            manager.AddTask("Задача 2");
            manager.AddTask("Задача 3");

            Assert.AreEqual(3, manager.Tasks.Count);
        }

        [TestMethod]
        public void Test_AddTask_SavesToFile()
        {
            var manager = new TaskManager();
            manager.AddTask("Проверка сохранения");

            Assert.IsTrue(File.Exists(TestFilePath));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_AddTask_Empty_ThrowsError()
        {
            var manager = new TaskManager();
            manager.AddTask("");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_AddTask_Null_ThrowsError()
        {
            var manager = new TaskManager();
            manager.AddTask(null);
        }


        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void Test_RemoveTask_BadIndex_Negative()
        {
            var manager = new TaskManager();
            manager.AddTask("Задача");
            manager.RemoveTask(-1);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void Test_RemoveTask_BadIndex_TooBig()
        {
            var manager = new TaskManager();
            manager.AddTask("Задача");
            manager.RemoveTask(10);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void Test_RemoveTask_FromEmptyList()
        {
            var manager = new TaskManager();
            manager.RemoveTask(0);
        }

        [TestMethod]
        public void Test_Toggle_CompleteToIncomplete()
        {
            var manager = new TaskManager();
            manager.AddTask("Задача");
            manager.Tasks[0].IsCompleted = true;

            manager.ToggleTaskCompletion(0);

            Assert.IsFalse(manager.Tasks[0].IsCompleted);
        }

        [TestMethod]
        public void Test_Toggle_IncompleteToComplete()
        {
            var manager = new TaskManager();
            manager.AddTask("Задача");

            manager.ToggleTaskCompletion(0);

            Assert.IsTrue(manager.Tasks[0].IsCompleted);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void Test_Toggle_BadIndex()
        {
            var manager = new TaskManager();
            manager.AddTask("Задача");
            manager.ToggleTaskCompletion(99);
        }

        [TestMethod]
        public void Test_Constructor_EmptyList()
        {
            var manager = new TaskManager();
            Assert.IsNotNull(manager.Tasks);
            Assert.AreEqual(0, manager.Tasks.Count);
        }

        [TestMethod]
        public void Test_Tasks_Property()
        {
            var manager = new TaskManager();
            manager.AddTask("Проверка");
            var tasks = manager.Tasks;

            Assert.IsNotNull(tasks);
            Assert.AreEqual(1, tasks.Count);
        }
    }
}