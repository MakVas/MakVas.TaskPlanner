using Moq;
using MakVas.TaskPlanner.Domain.Models;
using MakVas.TaskPlanner.Domain.Models.Enums;
using MakVas.TaskPlanner.DataAccess.Abstractions;

namespace MakVas.TaskPlanner.Domain.Logic.Tests
{
    public class SimpleTaskPlannerTests
    {
        [Fact]
        public void CreatePlan_ShouldSortTasksCorrectly()
        {
            // Arrange
            var mockRepository = new Mock<IWorkItemsRepository>();
            var workItems = new List<WorkItem>
            {
                new WorkItem
                {
                    Id = Guid.NewGuid(), Title = "Task 1", DueDate = DateTime.Now.AddDays(1), Priority = Priority.High,
                    IsCompleted = false
                },
                new WorkItem
                {
                    Id = Guid.NewGuid(), Title = "Task 2", DueDate = DateTime.Now.AddDays(2),
                    Priority = Priority.Medium, IsCompleted = false
                },
                new WorkItem
                {
                    Id = Guid.NewGuid(), Title = "Task 3", DueDate = DateTime.Now.AddDays(3), Priority = Priority.Low,
                    IsCompleted = false
                }
            };

            mockRepository.Setup(repo => repo.GetAll()).Returns(workItems.ToArray());

            var planner = new SimpleTaskPlanner(mockRepository.Object);

            // Act
            var plan = planner.CreatePlan();

            // Assert
            Assert.Equal("Task 1", plan[0].Title); // High priority and earliest due date
            Assert.Equal("Task 2", plan[1].Title); // Medium priority
            Assert.Equal("Task 3", plan[2].Title); // Low priority
        }

        [Fact]
        public void CreatePlan_ShouldIncludeAllRelevantTasks()
        {
            // Arrange
            var mockRepository = new Mock<IWorkItemsRepository>();
            var workItems = new List<WorkItem>
            {
                new WorkItem
                {
                    Id = Guid.NewGuid(), Title = "Task 1", DueDate = DateTime.Now.AddDays(1), Priority = Priority.High,
                    IsCompleted = false
                },
                new WorkItem
                {
                    Id = Guid.NewGuid(), Title = "Task 2", DueDate = DateTime.Now.AddDays(2),
                    Priority = Priority.Medium, IsCompleted = false
                },
                new WorkItem
                {
                    Id = Guid.NewGuid(), Title = "Task 3", DueDate = DateTime.Now.AddDays(3), Priority = Priority.Low,
                    IsCompleted = false
                }
            };

            mockRepository.Setup(repo => repo.GetAll()).Returns(workItems.ToArray());

            var planner = new SimpleTaskPlanner(mockRepository.Object);

            // Act
            var plan = planner.CreatePlan();

            // Assert
            Assert.Equal(3, plan.Length); // All 3 tasks should be included
        }

        [Fact]
        public void CreatePlan_ShouldNotIncludeIrrelevantTasks()
        {
            // Arrange
            var mockRepository = new Mock<IWorkItemsRepository>();
            var workItems = new List<WorkItem>
            {
                new WorkItem
                {
                    Id = Guid.NewGuid(), Title = "Task 1", DueDate = DateTime.Now.AddDays(1), Priority = Priority.High,
                    IsCompleted = false
                },
                new WorkItem
                {
                    Id = Guid.NewGuid(), Title = "Task 2", DueDate = DateTime.Now.AddDays(2),
                    Priority = Priority.Medium, IsCompleted = false
                },
                new WorkItem
                {
                    Id = Guid.NewGuid(), Title = "Task 3", DueDate = DateTime.Now.AddDays(3), Priority = Priority.Low,
                    IsCompleted = false
                },
                new WorkItem
                {
                    Id = Guid.NewGuid(), Title = "Task 4", DueDate = DateTime.Now.AddDays(4), Priority = Priority.High,
                    IsCompleted = true
                }
            };

            mockRepository.Setup(repo => repo.GetAll()).Returns(workItems.ToArray());

            var planner = new SimpleTaskPlanner(mockRepository.Object);

            // Act
            var plan = planner.CreatePlan();

            // Assert
            Assert.Equal(3, plan.Length); // Only 3 tasks should be included (excluding the completed one)
            Assert.DoesNotContain(plan, task => task.Title == "Task 4"); // Completed task should not be included
        }
    }
}