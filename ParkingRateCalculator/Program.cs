using ParkingRateCalculator.Model;

class Program
{
    static void Main(string[] args)
    {
        DateTime entryTime, exitTime;
        Console.WriteLine("CarPark Rate calculator, press Control + c to exit");

        while (true)
        {
            Console.WriteLine("Enter Entry Date and Time (yyyy-MM-dd HH:mm): ");
            if (DateTime.TryParse(Console.ReadLine(), out entryTime))
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter the date and time in the correct format (yyyy-MM-dd HH:mm).");
            }
        }

        while (true)
        {
            Console.WriteLine("Enter Exit Date and Time (yyyy-MM-dd HH:mm): ");
            if (DateTime.TryParse(Console.ReadLine(), out exitTime))
            {
                if (exitTime >= entryTime)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Exit time must be after entry time. Please try again.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter the date and time in the correct format (yyyy-MM-dd HH:mm).");
            }
        }

        var patron = new Patron
        {
            EntryTime = entryTime,
            ExitTime = exitTime
        };

        var (rateType, price) = RateCalculator.CalculateRate(patron);

        Console.WriteLine($"Rate Type: {rateType}");
        Console.WriteLine($"Total Price: ${price:F2}");
    }
}