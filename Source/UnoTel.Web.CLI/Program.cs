using Microsoft.Extensions.CommandLineUtils;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using UnoTel.Web.Cli.IoC;

namespace UnoTel.Web.Cli
{
    internal class Program
    {
        private static async Task<int> Main(string[] args)
        {
            Stopwatch sw = Stopwatch.StartNew();

            ExecutionService executionService = Bootstrapper.Container.GetInstance<ExecutionService>();

            var app = new CommandLineApplication();
            app.HelpOption("-? | -h | --help");

            app.Command("balance", config =>
            {
                config.Description = "check the current balance";
                config.HelpOption("-? | -h | --help");

                ConfigureLoginCommand(config, out CommandOption userName, out CommandOption password, out CommandOption subscriptionNumber);

                config.OnExecute(async () =>
                {
                    Console.WriteLine($"Checking balance for subscriptionNumber: {subscriptionNumber.Value()}");
                    decimal balance = await executionService.GetBalance(userName.Value(), password.Value(), Int32.Parse(subscriptionNumber.Value()));
                    Console.WriteLine($"Balance is: {balance}");
                    return 0;
                });
            });

            app.Command("sendsms", config =>
            {
                config.Description = "send a sms";
                config.HelpOption("-? | -h | --help");

                ConfigureLoginCommand(config, out CommandOption userName, out CommandOption password, out CommandOption subscriptionNumber);
                CommandOption recipientNumber = config.Option("-r | --recipientNumber", "the number of the recipient", CommandOptionType.SingleValue);
                CommandOption text = config.Option("-t | --text", "the text to send", CommandOptionType.SingleValue);

                config.OnExecute(async () =>
                {
                    Console.WriteLine($"Sending SMS to recipientNumber: {recipientNumber.Value()}");
                    await executionService.SendSMS(userName.Value(), password.Value(), Int32.Parse(subscriptionNumber.Value()), Int32.Parse(recipientNumber.Value()), text.Value());
                    return 0;
                });
            });

            int result = await Task.FromResult(app.Execute(args));

            Console.WriteLine($"Execution finished in {sw.ElapsedMilliseconds} ms");
            Console.ReadLine();

            Environment.Exit(result);
            return result;
        }

        private static void ConfigureLoginCommand(CommandLineApplication config, out CommandOption userName, out CommandOption password, out CommandOption subscriptionNumber)
        {
            userName = config.Option("-u | --userName", "the login user name", CommandOptionType.SingleValue);
            password = config.Option("-p | --password", "the login password", CommandOptionType.SingleValue);
            subscriptionNumber = config.Option("-s | --subscriptionNumber", "subscription number, if multiple subscriptions are available", CommandOptionType.SingleValue);
        }
    }
}
