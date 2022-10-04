using System;

public class Transaction
{
    string type;
    double amount;
    double balance;
    DateTime dateTime = DateTime.Now;

    // Constructor
    public Transaction(string type, double amount, double balance)
    {
        this.type = type;
        this.amount = amount;
        this.balance = balance;
    }

    // Getters and Setters

    public string getType()
    {
        return type; 
    }

    public void setType(String newType)
    {
        this.type = newType;
    }

    public double getAmount()
    {
        return amount;
    }

    public void setAmount(double newAmount)
    {
        this.amount = newAmount; 
    }

    public double getBalance()
    {
        return balance; 
    }

    public void setBalance(double newBalance)
    {
        this.balance = newBalance;
    }
    public DateTime getDate()
    {
        return dateTime;
    }

    // Overrides the ToString method to display the way we want it to
    public override string ToString()
    {
        Console.WriteLine("------------------------------------------");
        return "Transaction Date: "  + dateTime + " \nTransaction Type: " + type + "\nTransaction Amount: " + amount + "\nBalance: " + balance;
    }
}
