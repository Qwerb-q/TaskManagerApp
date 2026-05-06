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

        [TestMethod]
        public void Test_File_Content_Persistence_And_Restore()
        {
            var manager1 = new TaskManager();
            manager1.AddTask("Задача 1");
            manager1.ToggleTaskCompletion(0);
            manager1.AddTask("Задача 2");

            var manager2 = new TaskManager();

            Assert.AreEqual(2, manager2.Tasks.Count, "Количество задач не совпадает после перезагрузки");
            Assert.AreEqual("Задача 1", manager2.Tasks[0].Description);
            Assert.IsTrue(manager2.Tasks[0].IsCompleted, "Статус первой задачи должен быть True");
            Assert.AreEqual("Задача 2", manager2.Tasks[1].Description);
            Assert.IsFalse(manager2.Tasks[1].IsCompleted, "Статус второй задачи должен быть False");
        }

        [TestMethod]
        public void Test_File_Creation_On_First_Add()
        {
            if (File.Exists(TestFilePath)) File.Delete(TestFilePath);

            var manager = new TaskManager();
            Assert.IsFalse(File.Exists(TestFilePath), "Файл не должен существовать до добавления задач");

            manager.AddTask("Новая задача");
            Assert.IsTrue(File.Exists(TestFilePath), "Файл должен появиться после добавления задачи");
        }

        [TestMethod]
        public void Test_File_IsValidTextFile()
        {
            var manager = new TaskManager();
            manager.AddTask("Текстовая проверка");

            Assert.IsTrue(File.Exists(TestFilePath), "Файл tasks.txt не был создан.");

            string fileName = Path.GetFileName(TestFilePath);
            Assert.IsTrue(fileName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase),
                $"Файл должен иметь расширение .txt, но получено: {fileName}");

            try
            {
                string content = File.ReadAllText(TestFilePath);

                Assert.IsFalse(string.IsNullOrEmpty(content), "Файл пуст, хотя задача была добавлена.");

                Assert.IsTrue(content.Contains("False") || content.Contains("True"),
                    "Файл не содержит булевых значений статуса задачи.");
                Assert.IsTrue(content.Contains("Текстовая проверка"),
                    "Файл не содержит описания добавленной задачи.");
            }
            catch (Exception ex)
            {
                Assert.Fail($"Не удалось прочитать файл как текст. Возможно, файл поврежден или не является текстовым. Ошибка: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_File_Format_IsCorrect()
        {
            var manager = new TaskManager();
            manager.AddTask("Проверка формата");

            string fileContent = File.ReadAllText(TestFilePath);

            Assert.IsTrue(fileContent.Contains("\t"), "Файл не содержит символ табуляции (\\t)");

            bool startsWithFalse = fileContent.StartsWith("False\t");
            bool startsWithTrue = fileContent.StartsWith("True\t");

            Assert.IsTrue(startsWithFalse || startsWithTrue, $"Неправильный формат начала строки. Ожидается 'False\\t' или 'True\\t'. Реальное начало: '{fileContent.Substring(0, Math.Min(10, fileContent.Length))}'");
        }
    }
}