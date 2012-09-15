﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroLog.Targets;
using Windows.Storage;
using Xunit;

namespace MetroLog.NetCore.Tests
{
    
    public class FileSnapshotTests
    {
        [Fact]
        public async Task TestFileSnapshot()
        {
            var target = new FileSnapshotTarget();

            // send through a log entry...
            var op = await target.WriteAsync(new LogWriteContext(),
                new LogEventInfo(LogLevel.Fatal, "TestLogger", "Testing file write...", new InvalidOperationException("An exception message...")));

            // TODO: This should be Faked! We shouldn't be writing to the disk

            // load the file...
            var folder = await FileSnapshotTarget.EnsureInitializedAsync();
            var files = await folder.GetFilesAsync();
            var file = files.First(v => v.Name.Contains(op.GetEntries().First().SequenceID.ToString()));
            string contents = await FileIO.ReadTextAsync(file);

            // check...
            Assert.True(contents.Contains("Testing file write..."));
            Assert.True(contents.Contains("An exception message..."));
        }
    }
}
