using System.Collections;

public class User
{
    public String cardNum;
    public String name;
    public int pin;
    public String phoneNumber;
    public double balance;
    public Queue transactionList = new Queue(); // Keeps track of the 5 most recent transactions
    public ArrayList allTransactions = new ArrayList(); // Keeps track of ALL transactions
    private int withdrawCount = 0; // User is allowed to withdraw up to 10 times a day
    public Transaction lastTransaction; // Keeps track of the last transaction

    // Constructor
    public User(string cardNum, string name, int pin, string phoneNumber, double balance)
    {
        this.cardNum = cardNum;
        this.pin = pin;
        this.name = name;
        this.phoneNumber = phoneNumber;
        this.balance = balance;
    }

    public User()
    {
        this.cardNum = "void";
        this.pin = 0000;
        this.name = "John Doe";
        this.phoneNumber = "DNE";
        this.balance = 0;
    }
    // Getters and Setters
    public string getNum()
    {
        return cardNum;
    }

    public void setNum(String num)
    {
        cardNum = num;
    }

    public string getName()
    {
        return name;
    }

    public void setName(String newName)
    {
        name = newName;
    }
    public int getPin()
    {
        return pin;
    }

    public void setPin(int newPin)
    {
        pin = newPin;
    }

    public string getPhoneNumber()
    {
        return phoneNumber;
    }

    public void setPhoneNumber(String newPhoneNumber)
    {
        phoneNumber = newPhoneNumber;   
    }
    public double getBalance()
    {
        return balance;
    }

    public void setBalance(double newBalance)
    {
        balance = newBalance;
    }

    public Queue getTransactionList()
    {
        return transactionList;
    }

    public ArrayList getAllTransactions()
    {
        return allTransactions;
    }

    public int getWithdrawCount()
    {
        return withdrawCount;
    }

    public void setWithdrawCount(int newCount)
    {
        withdrawCount = newCount; 
    }
    public Transaction getLastTransaction()
    {
        return lastTransaction;
    }

    public void updateTransactionList(Transaction t)
    {
        transactionList.Enqueue(t);
        allTransactions.Add(t);
        lastTransaction = t; 

        if(t.getType() == "Withdraw")
        {
            withdrawCount++;
        }

        if (transactionList.Count > 5)
        {
            transactionList.Dequeue();
        }
    }
}
