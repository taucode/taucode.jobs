﻿using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TauCode.Jobs.Tests.Jobs
{
    [TestFixture]
    public partial class JobTests
    {
        [Test]
        public void IsDisposed_JobIsNotDisposed_ReturnsFalse()
        {
            // Arrange
            using IJobManager jobManager = TestHelper.CreateJobManager(true);

            var start = "2000-01-01Z".ToUtcDayOffset();
            var timeMachine = ShiftedTimeProvider.CreateTimeMachine(start);
            TimeProvider.Override(timeMachine);

            var job = jobManager.Create("my-job");

            job.Routine = async (parameter, tracker, output, token) =>
            {
                await output.WriteAsync("Hello!");
                await Task.Delay(TimeSpan.FromHours(1), token);
            };

            job.IsEnabled = true;

            job.ForceStart();

            // Act
            var isDisposed = job.IsDisposed;


            // Assert
            Assert.That(isDisposed, Is.False);
        }

        [Test]
        public void IsDisposed_JobIsDisposed_ReturnsTrue()
        {
            // Arrange
            using IJobManager jobManager = TestHelper.CreateJobManager(true);

            var start = "2000-01-01Z".ToUtcDayOffset();
            var timeMachine = ShiftedTimeProvider.CreateTimeMachine(start);
            TimeProvider.Override(timeMachine);

            var job = jobManager.Create("my-job");

            job.Routine = async (parameter, tracker, output, token) =>
            {
                await output.WriteAsync("Hello!");
                await Task.Delay(TimeSpan.FromHours(1), token);
            };

            job.IsEnabled = true;

            job.ForceStart();
            job.Dispose();

            // Act
            var isDisposed = job.IsDisposed;


            // Assert
            Assert.That(isDisposed, Is.True);
        }
    }
}
