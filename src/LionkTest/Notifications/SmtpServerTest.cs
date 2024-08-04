// Copyright © 2024 Lionk Project

using System.Buffers;
using SmtpServer;
using SmtpServer.Authentication;
using SmtpServer.ComponentModel;
using SmtpServer.Protocol;
using SmtpServer.Storage;

namespace LionkTest.Notifications;

/// <summary>
/// This class is used to start a SMTP server to test the email notification.
/// </summary>
public class SmtpServerTest
{
    /// <summary>
    /// This class is used to store the message received by the SMTP server.
    /// </summary>
    private class SampleMessageStore : MessageStore
    {
        public override Task<SmtpResponse> SaveAsync(ISessionContext context, IMessageTransaction transaction, ReadOnlySequence<byte> buffer, CancellationToken cancellationToken)
        {
            string message = System.Text.Encoding.UTF8.GetString(buffer.ToArray());
            Console.WriteLine($"Message reçu : {message}");

            // Traiter le message reçu ici
            return Task.FromResult(SmtpResponse.Ok);
        }
    }

    /// <summary>
    /// This class is used to authenticate the user.
    /// </summary>
    private class SampleUserAuthenticator : IUserAuthenticator
    {
        public Task<bool> AuthenticateAsync(ISessionContext context, string user, string password, CancellationToken cancellationToken)
        {
            // Authentifier l'utilisateur (exemple basique)
            if (user == "senderTest" && password == "passwordTest")
            {
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }
    }

    private static SmtpServer.SmtpServer? _server;
    private static CancellationTokenSource? _cts;

    /// <summary>
    /// This method is used to start the SMTP server.
    /// </summary>
    public static void Start()
    {
        ISmtpServerOptions options = new SmtpServerOptionsBuilder()
            .ServerName("localhost")
            .Port(2526)
            .Build();

        var serviceProvider = new ServiceProvider();
        serviceProvider.Add(new SampleUserAuthenticator());
        serviceProvider.Add(new SampleMessageStore());

        _server = new SmtpServer.SmtpServer(options, serviceProvider);
        _cts = new CancellationTokenSource();

        Console.WriteLine("Starting SMTP server...");
        Task<Task> serverTask = Task.Factory.StartNew(() => _server.StartAsync(_cts.Token), TaskCreationOptions.LongRunning);
        Console.WriteLine("SMTP server started.");
    }

    /// <summary>
    /// Method used to stop the SMTP server.
    /// </summary>
    public static void Stop()
    {
        Console.WriteLine("Stopping SMTP server");
        if (_cts is not null) _cts.Cancel();
        Console.WriteLine("SMTP server stopped.");
    }
}
