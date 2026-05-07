using System;
using System.Windows.Forms;
using System.Drawing;

public class TaskManagerForm : Form
{
    private TaskManager taskManager;
    private ListBox tasksListBox;
    private TextBox descriptionTextBox;
    private Button addTaskButton;
    private Button removeTaskButton;
    private Button toggleCompletionButton;

    public TaskManagerForm()
    {
        this.Text = "Управление задачами";
        this.Width = 400;
        this.Height = 350;
        this.BackColor = Color.PowderBlue;

        Label listLabel = new Label
        {
            Text = "Список задач:",
            Location = new Point(10, 10),
            AutoSize = true,
            Font = new Font(this.Font, FontStyle.Bold)
        };

        tasksListBox = new ListBox
        {
            Location = new Point(10, 30),
            Width = 200,
            Height = 250,
            BorderStyle = BorderStyle.Fixed3D
        };

        Label inputLabel = new Label
        {
            Text = "Новая задача:",
            Location = new Point(220, 10),
            AutoSize = true,
            Font = new Font(this.Font, FontStyle.Bold)
        };

        descriptionTextBox = new TextBox
        {
            Location = new Point(220, 30),
            Width = 150
        };

        addTaskButton = new Button
        {
            Location = new Point(220, 60),
            Text = "Добавить",
            Width = 70,
            Height = 35,
            BackColor = Color.Wheat
        };
        addTaskButton.Click += AddTaskButton_Click;

        removeTaskButton = new Button
        {
            Location = new Point(300, 60),
            Text = "Удалить",
            Width = 70,
            Height = 35,
            BackColor = Color.Wheat
        };
        removeTaskButton.Click += RemoveTaskButton_Click;

        toggleCompletionButton = new Button
        {
            Location = new Point(220, 105),
            Text = "Отметить",
            Width = 150,
            Height = 35,
            BackColor = Color.Wheat
        };
        toggleCompletionButton.Click += ToggleCompletionButton_Click;

        this.Controls.Add(listLabel);
        this.Controls.Add(inputLabel);
        this.Controls.Add(tasksListBox);
        this.Controls.Add(descriptionTextBox);
        this.Controls.Add(addTaskButton);
        this.Controls.Add(removeTaskButton);
        this.Controls.Add(toggleCompletionButton);

        taskManager = new TaskManager();
        UpdateTasksList();
    }

    private void UpdateTasksList()
    {
        tasksListBox.Items.Clear();
        foreach (var task in taskManager.Tasks)
        {
            tasksListBox.Items.Add($"{(task.IsCompleted ? "[X]" : "[ ]")} {task.Description}");
        }
    }

    private void AddTaskButton_Click(object sender, EventArgs e)
    {
        try
        {
            taskManager.AddTask(descriptionTextBox.Text);
            descriptionTextBox.Clear();
            UpdateTasksList();
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(this, ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void RemoveTaskButton_Click(object sender, EventArgs e)
    {
        if (tasksListBox.SelectedIndex == -1)
        {
            MessageBox.Show(this, "Выберите задачу для удаления!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        try
        {
            taskManager.RemoveTask(tasksListBox.SelectedIndex);
            UpdateTasksList();
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ToggleCompletionButton_Click(object sender, EventArgs e)
    {
        if (tasksListBox.SelectedIndex == -1)
        {
            MessageBox.Show(this, "Выберите задачу для изменения статуса!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        try
        {
            taskManager.ToggleTaskCompletion(tasksListBox.SelectedIndex);
            UpdateTasksList();
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}