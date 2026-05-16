using Silva_Meminit;
using System.Media;
using static Silva_Meminit.Algos;
using static System.Console;
int Height = ReadIntegerBetweenInterval("Amount of rows of your Forest (max depending on Window Size) : ", 1, () => WindowHeight - 3);
int Width = ReadIntegerBetweenInterval("Amount of columns of your Forest (max depending on Window Size) : ", 1, () => WindowWidth - 1);
int amt = ReadInteger("Amount of starting Trees : ");
GameLoop gameLoop = new(Height, Width);
GameStats stats = new(gameLoop);
bool canPickFatherTree = true;
void WritePrompt()
{
    string reset = "\x1b[0m";
    WriteLine($@"Commands :
[f]     to view {ToAnsi(ConsoleColor.Green)}Forest{reset}
[h]     to view {ToAnsi(ConsoleColor.DarkGray)}Haunted Forest{reset}
[s]     to view {ToAnsi(ConsoleColor.Cyan)}Stats{reset}
[p]     to pick the {ToAnsi(ConsoleColor.DarkYellow)}Father Tree{reset}
[Enter] to evolve your {ToAnsi(ConsoleColor.Green)}Forest{reset}
[n]     to find the nearest object (Disabled in {ToAnsi(ConsoleColor.Cyan)}Stats{reset})
Type anything else to quit.");
}
void FindNearestObject(ViewMode current)
{
    if (current == ViewMode.GameStats) return;
    string objectName = current == ViewMode.Forest ? "Tree" : "Ghost Tree";
    int r = ReadInteger("Row index (or -1 to cancel) : ");
    if (r == -1) return;
    int c = ReadInteger("Column index (or -1 to cancel) : ");
    if (c == -1) return;
    (r, c) = current == ViewMode.Forest ? FindNearest(gameLoop.F.Trees, r, c, x => x is Tree)
                                                : FindNearest(gameLoop.HF.GhostTrees, r, c, x => x is GhostTree);
    WriteLine($"Nearest {objectName} at : ({r},{c}) ");
}
gameLoop.FillForest(amt);
stats.Update();
Clear();
gameLoop.F.Draw();
WritePrompt();
string? input = ReadLine();
ViewMode current = ViewMode.Forest;
ViewMode previous = ViewMode.Forest;
while ((input == "" || input == "p" || input == "f" || input == "h" || input == "s" || input == "n" || input == "lime")
    && !gameLoop.F.IsEmpty && !gameLoop.F.IsFull && !gameLoop.HF.IsFull)
{
    switch (input)
    {
        case "p":
            if (canPickFatherTree)
                canPickFatherTree = !gameLoop.F.PickFatherTree();
            Clear();
            break;
        case "f":
            current = ViewMode.Forest;
            break;
        case "h":
            current = ViewMode.HauntedForest;
            break;
        case "s":
            current = ViewMode.GameStats;
            break;
        case "":
            gameLoop.EvolveGame();
            stats.Update();
            break;
        case "n":
            if (current != ViewMode.GameStats)
            {
                FindNearestObject(current);
                Write("Press anything to exit command");
                ReadLine();
                Clear();
            }
            break;
        case "lime":
            PlaySoundEffect("lime.wav");
            Clear();
            break;
    }
    if (gameLoop.F.Family.Count == 0 && gameLoop.GhostTreeCount == 0 && stats.LongestLineage > 0)
    {
        Console.Clear();
        WriteLine("The Forest's memory fades away.");
        Thread.Sleep(3000);
        break;
    }
    if (current != previous)
    {
        Clear();
        previous = current;
    }
    if (current == ViewMode.Forest)
        gameLoop.F.Draw();
    else if (current == ViewMode.HauntedForest)
        gameLoop.HF.Draw();
    else
        stats.Display();
    WritePrompt();
    input = ReadLine();
}
Clear();
if (gameLoop.F.IsEmpty) WriteLine("Time has devoured the last Tree.");
else if (gameLoop.F.IsFull) WriteLine("Life fully blooms in the Forest.");
else if (gameLoop.HF.IsFull) WriteLine("The Forest fully remembers.");
Thread.Sleep(3000);