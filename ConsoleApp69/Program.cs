using System;

interface ICurrencyConverter
{
    double ConvertToDollars(double amount);
    double ConvertToEuros(double amount);
}

abstract class Account : ICurrencyConverter
{
    protected string ownerLastName;
    protected string accountNumber;
    protected double interestRate;
    protected double balance;

    public Account(string ownerLastName, string accountNumber, double interestRate, double balance)
    {
        this.ownerLastName = ownerLastName;
        this.accountNumber = accountNumber;
        this.interestRate = interestRate;
        this.balance = balance;
    }
    ~Account()
    {
        Console.WriteLine("Объект счета уничтожен.");
    }

    public void ChangeOwner(string newOwnerLastName)
    {
        ownerLastName = newOwnerLastName;
        Console.WriteLine($"Владелец изменен на: {ownerLastName}");
    }



    public void Withdraw(double amount)
    {
        if (amount <= balance)
        {
            balance -= amount;
            Console.WriteLine($"Снято {amount} RUB. Новый баланс: {balance} RUB");
        }
        else
        {
            Console.WriteLine("Недостаточно средств.");
        }
    }

    public void Deposit(double amount)
    {
        balance += amount;
        Console.WriteLine($"Внесено {amount} RUB. Новый баланс: {balance} RUB");
    }

    public void AccrueInterest()
    {
        double interest = balance * interestRate / 100;
        balance += interest;
        Console.WriteLine($"Начислены проценты: {interest} RUB. Новый баланс: {balance} RUB");
    }

    public double ConvertToDollars(double amount)
    {
        return amount / 70;
    }

    public double ConvertToEuros(double amount)
    {
        return amount / 80;
    }

    public abstract string ConvertToWords(double amount);
}


class SavingsAccount : Account
{
    public SavingsAccount(string ownerLastName, string accountNumber, double interestRate, double balance)
        : base(ownerLastName, accountNumber, interestRate, balance)
    {
    }


    public override string ConvertToWords(double amount)
    {
        return ConvertToWordsInternal((long)amount) + " RUB";
    }

    private string ConvertToWordsInternal(long amount)
    {
        if (amount == 0)
            return "ноль";

        if (amount < 0)
            return "минус " + ConvertToWordsInternal(-amount);

        string words = "";

        if ((amount / 1000000) > 0)
        {
            words += ConvertToWordsInternal(amount / 1000000) + " миллион ";
            amount %= 1000000;
        }

        if ((amount / 1000) > 0)
        {
            words += ConvertToWordsInternal(amount / 1000) + " тысяча ";
            amount %= 1000;
        }

        if ((amount / 100) > 0)
        {
            words += ConvertToWordsInternal(amount / 100) + " сто ";
            amount %= 100;
        }

        if (amount > 0)
        {
            if (words != "")
                words += "и ";

            string[] unitsMap = new string[] { "ноль", "один", "два", "три", "четыре", "пять", "шесть", "семь", "восемь", "девять", "десять", "одиннадцать", "двенадцать", "тринадцать", "четырнадцать", "пятнадцать", "шестнадцать", "семнадцать", "восемнадцать", "девятнадцать" };
            string[] tensMap = new string[] { "ноль", "десять", "двадцать", "тридцать", "сорок", "пятьдесят", "шестьдесят", "семьдесят", "восемьдесят", "девяносто" };

            if (amount < 20)
                words += unitsMap[amount];
            else
            {
                words += tensMap[amount / 10];
                if ((amount % 10) > 0)
                    words += " " + unitsMap[amount % 10];
            }
        }

        return words;
    }
}

class Program
{
    static void Main(string[] args)
  
     {
        Console.WriteLine("Введите фамилию владельца счета:");
        string ownerLastName = Console.ReadLine();
    Console.WriteLine("Введите начальный баланс счета:");
        double initialBalance;
        if (!double.TryParse(Console.ReadLine(), out initialBalance))
        {
            Console.WriteLine("Некорректный формат начального баланса.");
            return;
        }

        SavingsAccount account = new SavingsAccount(ownerLastName, "1234567890", 5, initialBalance);

        Console.WriteLine("Введите сумму для снятия:");
        double withdrawAmount;
        if (!double.TryParse(Console.ReadLine(), out withdrawAmount))
        {
            Console.WriteLine("Некорректный формат суммы для снятия.");
            return;
        }
        account.Withdraw(withdrawAmount);

        Console.WriteLine("Введите сумму для внесения:");
        double depositAmount;
        if (!double.TryParse(Console.ReadLine(), out depositAmount))
        {
            Console.WriteLine("Некорректный формат суммы для внесения.");
            return;
        }
        account.Deposit(depositAmount);

        account.AccrueInterest();

        Console.WriteLine("Введите фамилию нового владельца:");
        string newOwnerLastName = Console.ReadLine();
        account.ChangeOwner(newOwnerLastName);

        Console.WriteLine("Введите сумму для конвертации в доллары:");
        double amountToConvert;
        if (!double.TryParse(Console.ReadLine(), out amountToConvert))
        {
            Console.WriteLine("Некорректный формат суммы для конвертации в доллары.");
            return;
        }
        Console.WriteLine($"Сумма в долларах: {account.ConvertToDollars(amountToConvert)}");

        Console.WriteLine("Введите сумму для конвертации в евро:");
        if (!double.TryParse(Console.ReadLine(), out amountToConvert))
        {
            Console.WriteLine("Некорректный формат суммы для конвертации в евро.");
            return;
        }
        Console.WriteLine($"Сумма в евро: {account.ConvertToEuros(amountToConvert)}");

        Console.WriteLine("Введите сумму для преобразования в пропись:");
        if (!double.TryParse(Console.ReadLine(), out amountToConvert))
        {
            Console.WriteLine("Некорректный формат суммы для преобразования в пропись.");
            return;
        }
        Console.WriteLine($"Сумма прописью: {account.ConvertToWords(amountToConvert)}");

        Console.ReadLine();
        }
}
