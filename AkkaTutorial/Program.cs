using Akka.Actor;
using AkkaTutorial.Actors;

using var system = ActorSystem.Create("AkkaTutorial");

int target = 0;
int workers = 0;

var bTarget = false;
var bWorkers = false;

while (!bTarget)
{
    Console.WriteLine("To which number do you want to count: ");
    bTarget = int.TryParse(Console.ReadLine(), out target);
}

while (!bWorkers)
{
    Console.WriteLine("How many workers do you want to use?");
    bWorkers = int.TryParse(Console.ReadLine(), out workers);
}

var stateCounter = system.ActorOf(StateCounter.Props(target), "state-counter");
var output = system.ActorOf(CountOutput.Props(), "counter-output");
var workerPool = system.ActorOf(WorkerPool.Props(stateCounter, output, workers), "worker-pool");

workerPool.Tell("start");



Thread.Sleep(10000);