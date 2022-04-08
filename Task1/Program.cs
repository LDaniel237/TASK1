using System.Diagnostics;
using System.Timers;

Process proc = new Process();
StreamWriter SW = new StreamWriter(path: @"D:\C#\VEEAM_TASK\Task1\Task1\Data.txt"); // used to write the data in the txt file
System.Timers.Timer timer = new System.Timers.Timer(); // declaring a timer so we can work with the time interval

Console.WriteLine("Input the path of the process you want to execute: ");
proc.StartInfo.FileName = Console.ReadLine(); // setting the path of the executable given by the user
if(proc.StartInfo.FileName == null) {
    Console.WriteLine("You haven't given an input!");
}

/* READING THE TIME INTERVAL */
Console.WriteLine("\nNow input the time interval between data collection iterations (in seconds): ");
int interval;
bool succ = int.TryParse(Console.ReadLine(), out interval);
if(succ) {
    timer.Interval = interval * 1000;
}
else {
    Console.WriteLine("Failed Conversion - You haven't given a proper number!");
}

bool v = proc.Start();
timer.Elapsed += timer_Elapsed;
if (timer.Interval != 0) {
    timer.Enabled = true;
}
else {
    timer.Enabled = false;
}

timer.AutoReset = true;
timer.Start();
Console.WriteLine("Press any key to stop ...");
Console.ReadKey();
proc.Kill();

void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e) {
    proc.Refresh();
    /* HERE I TRIED TO GET THE CPU USAGE, FAILED UNFORTUNATELY :(
    if(lastTime == null || lastTime == new DateTime()) {
        lastTime = DateTime.Now;
        lastProcTime = proc.TotalProcessorTime;
    }
    else {
        currTime = DateTime.Now;
        currProcTime = proc.TotalProcessorTime;

        double usage = (currProcTime.TotalMilliseconds - lastProcTime.TotalMilliseconds) / 
            currTime.Subtract(lastTime).TotalMilliseconds / Convert.ToDouble(Environment.ProcessorCount);
        Console.WriteLine("{0} CPU: {1:0.0}%", proc.ProcessName, usage * 100);

        lastTime = currTime;
        lastProcTime = currProcTime;
    }*/
    SW.WriteLine(Convert.ToDecimal(proc.WorkingSet64) / 1024 / 1024 + " MegaBytes Working Set - " +
        + Convert.ToDecimal(proc.PrivateMemorySize64) / 1024 / 1024 + " MegaBytes Private Memory - " +
        + proc.HandleCount + " Open handles");
}
SW.Close(); // Closing the StreamWriter

// C:\Program Files\Google\Chrome\Application\chrome.exe
// C:\Program Files\Codeblocks\codeblocks.exe
