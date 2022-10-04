using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MyATM
{
    class MyATM
    {
        public static void Main(string[] args)
        {
            Queue transactionList = new Queue(); // Keeps track of the 5 most recent transactions
            ArrayList allTransactions = new ArrayList(); // Keeps track of ALL transactions
            List<User> userList = new List<User>(); // Keeps track of all Users ~ Acts as a psuedo database
            int failCount1 = 0; // Keeps track of failed login for card number attempts
            int failCount2 = 0; // Keeps track of failed login for account pin attempts

            // Insert some test users
            userList.Add(new User("4532772818527395", "John Doe", 1234, "651-123-4563", 500));
            userList.Add(new User("5128381368581872", "Jane Doe", 5678, "651-321-7894", 1000));

            // Create a user with multiple transactions
            User user1 = new User("4532772818527396", "James Doe", 1010, "651-789-7854", 25);
            for(int i = 0; i < 8; i++)
            {
                user1.updateTransactionList(new Transaction("Withdraw", i + 100, user1.getBalance()));
            }

            // Create a user with over 10 withdrawls
            User user2 = new User("4532772818527393", "Jamie Doe", 2222, "651-453-7854", 75.99);
            for (int i = 0; i < 15; i++)
            {
                user2.updateTransactionList(new Transaction("Withdraw", i + 100, user1.getBalance()));
            }

            // Add the users to the list
            userList.Add(user1);
            userList.Add(user2);



            // Begin the program
            Console.WriteLine("Welcome to Andrew's Transaction Machine (ATM)");
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("Please enter your card number: ");
            String cardNumber = "";
            User currentUser;

            while(true)
            {
                try
                {
                    cardNumber = Console.ReadLine();

                    // Check to see if user exists
                    currentUser = userList.FirstOrDefault(a => a.cardNum == cardNumber.Replace(" ", ""));
                    if(currentUser != null && failCount1 < 3) { break; }
                    else if(failCount1 == 2)
                    {
                        Console.WriteLine("Failed login too many times!");
                        System.Environment.Exit(1);
                    }
                    else 
                    { 
                        Console.WriteLine("Card not recognized. Please try again"); 
                        failCount1++; 
                    }
                }
                catch
                {
                    Console.WriteLine("Card not recognized. Please try again");
                }
            }

            Console.WriteLine("Please enter your pin: ");
            int userPin = 0;

            while (true)
            {
                try
                {
                    userPin = int.Parse(Console.ReadLine());

                    if (currentUser.getPin() == userPin) { break; }
                    else if (failCount2 == 2)
                    {
                        Console.WriteLine("Failed login too many times!");
                        System.Environment.Exit(1);
                    }
                    else 
                    { 
                        Console.WriteLine("Incorrect pin. Please try again");
                        failCount2++;
                    }
                }
                catch
                {
                    Console.WriteLine("Incorrect pin. Please try again");
                }
            }

            // User has been validated, display menu
            Console.WriteLine("Welcome " + currentUser.getName());
            int option = 0;
            do
            {
                printOptions();
                try
                {
                    option = int.Parse(Console.ReadLine());

                }
                catch (IOException e)
                {
                    Console.WriteLine(e);
                }

                // Menu functionality
                if (option == 1 && currentUser.getWithdrawCount() < 10) { withdraw(currentUser); }
                else if(option == 1 && currentUser.getWithdrawCount() >= 10) // User is only allowed to withdraw 10 times daily
                {
                    Console.WriteLine("You've reached your maximum amount of withdrawals");
                    Console.WriteLine("Please come back tomorrow and try again! Have a nice day!");
                }
                else if(option == 2) { deposit(currentUser);  }
                else if(option == 3) { currentBalance(currentUser);  }
                else if(option == 4) { viewTransactions(currentUser);  }
                else if(option == 5) { printAllTransactions(currentUser);  }
                else if(option == 6) { break; }
                else 
                { 
                    option = 0;
                    Console.WriteLine("Invalid Input, Please try again. Choose from 1-6");
                }

                // Checks to see if the day has changed to reset the daily withdraw count
                DateTime moment = DateTime.Now;
                if (currentUser.getWithdrawCount() >= 10 && currentUser.getLastTransaction().getDate().Date != moment.Date)
                {
                    Console.WriteLine("Daily Reset!");
                    currentUser.setWithdrawCount(0);
                }
                else
                {
                    Console.WriteLine("Current date: " + moment);
                    //Console.WriteLine("Date of Last Transaction: " + currentUser.getLastTransaction().getDate()); TESTING LINE TO CHECK FOR VALID TIME CHANGE
                }

            } while (option != 6);

            Console.WriteLine("Have a nice day!"); // End of main program
        }

        // Function to display the menu for the user
        // Automatically displays when valid credentials are entered
        public static void printOptions()
        {
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("1) Withdraw ");
            Console.WriteLine("2) Deposit ");
            Console.WriteLine("3) View Current Balance");
            Console.WriteLine("4) View Recent Transactions");
            Console.WriteLine("5) View All Transactions");
            Console.WriteLine("6) Exit ");
        }

        // Function to withdraw money from a user's account
        // Option 1
        public static void withdraw(User user)
        {
            Console.WriteLine("Enter the amount you'd like to withdraw: ");
            try
            {
                double withdraw = double.Parse(Console.ReadLine());
                // Maximum amount allowed per transaction is $1000
                if(withdraw > 1000)
                {
                    Console.WriteLine("Sorry! The maximum amount allowed to withdraw is $1000 per transaction");
                }
                else
                {
                    if (withdraw < user.getBalance())
                    {
                        double temp = user.getBalance() - withdraw;
                        user.setBalance(temp);
                        user.updateTransactionList(new Transaction("Withdraw", withdraw, user.getBalance()));
                        user.setWithdrawCount(user.getWithdrawCount() + 1);
                        Console.WriteLine("Success! Your new balance is: $" + user.getBalance());
                    }
                    else
                    {
                        Console.WriteLine("I'm sorry, you do not have enough funds in your account!");
                        Console.WriteLine("Please enter a smaller amount or deposit some funds!");
                    }
                }
            }
            catch(IOException e)
            {
                Console.WriteLine("I'm sorry, you do not have enough funds in your account!");
                Console.WriteLine("Please enter a smaller amount or deposit some funds!");
            }
        }
        
        // Function to deposit money into a user's account
        // Option 2
        public static void deposit(User user)
        {
            Console.WriteLine("Enter the amount you'd like to deposit: ");
            try
            {
                double deposit = double.Parse(Console.ReadLine());
                double temp = user.getBalance() + deposit;
                user.setBalance(temp);
                user.updateTransactionList(new Transaction("Deposit", deposit, user.getBalance()));
                Console.WriteLine("Success! Your new balance is: $" + user.getBalance());
            }
            catch (IOException e)
            {
                Console.WriteLine(e);
            }
        }
        // Function to print the current balance of the user
        // Option 3
        public static void currentBalance(User user)
        {
            Console.WriteLine("Your current balance is: $" + user.getBalance());
        }

        // Function to print the 5 latest transactions
        // Option 4
        public static void viewTransactions(User user)
        {
            foreach (Transaction t in user.getTransactionList())
            {
                Console.WriteLine(t);
            }
        }

        // Function to print all transactions
        // Option 5
        public static void printAllTransactions(User user)
        {
            foreach(Transaction t in user.getAllTransactions())
            {
                Console.WriteLine(t);
            }
        }
    }
}
