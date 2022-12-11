using Shared;

DataContext d;
while (true)
{
    SystemWrite("Connection String:\n");

    Console.ForegroundColor = Console.BackgroundColor = ConsoleColor.Black;
    var server = Console.ReadLine();

    Console.ResetColor();

    d = new DataContext(server);


    SystemWrite("Connecting..... please wait!");
    if (!d.OpenConnection())
    {
        SystemWrite("DB is not available!\n\n");
    }
    else
    {
        SystemWrite("Connected!");
        Console.ResetColor();
        Thread.Sleep(2000);
        Console.Clear();
        break;
    }

}
while (true)
{

    SystemWrite("\nWrite Query:");

    var sql = Console.ReadLine();

    if (sql.Equals("tables"))
    {
        d.GetTables().ToList().ForEach(q =>
        {
            Console.Write(q + '\t');
        });
        continue;
    }
    else if (sql.StartsWith("table ") && sql.Split(' ').Count() == 2)
    {
        d.GetTableSchema(sql.Split(' ')[1]).ToList().ForEach(q =>
        {
            Console.Write(q+'\t');
        });
        continue;
    }

    var result = d.Run(sql);

    result.ForEach(q => Console.WriteLine(q));

}



void SystemWrite(string msg)
{
    Console.BackgroundColor = ConsoleColor.Blue;
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine(msg);
    Console.ResetColor();
}