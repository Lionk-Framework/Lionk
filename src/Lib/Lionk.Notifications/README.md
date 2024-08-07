# Notification Library

This library allows sending notifications through various channels, such as email. It also supports the addition of custom channels and recipients.

## Table of Contents

- [Installation](#installation)
- [Usage](#usage)
  - [Creating a Notifyer](#creating-a-notifyer)
  - [Creating a Notification](#creating-a-notification)
  - [Using EmailChannel and EmailRecipients](#using-emailchannel-and-emailrecipients)
  - [NotificationService](#notificationservice)
  - [Implementing New Channels and Recipients](#implementing-new-channels-and-recipients)

## Installation

1. Clone the repository.
2. Ensure you have the necessary dependencies
  - Newtonsoft.Json
3. Add the library files to your project.

## Usage

### Creating a Notifyer

A `Notifyer` is an entity that sends notifications. You can create a notifyer by implementing the `INotifyer` interface.

```csharp
public class MyNotifyer : INotifyer
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }

    public MyNotifyer(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }

    public bool Equals(INotifyer? other)
    {
        if (other == null) return false;
        return Id == other.Id;
    }
}
```

### Creating a Notification

A notification consists of content (`Content`) and a notifyer (`INotifyer`).

```csharp
Content content = new Content(Severity.Information, "Notification Title", "Notification Message");
INotifyer notifyer = new MyNotifyer("MyNotifyer");

Notification notification = new Notification(content, notifyer);
NotificationService.Send(notification);
```

### Using EmailChannel and EmailRecipients

To send notifications via email, use `EmailChannel` and `EmailRecipients`.

1. **Create an email recipient:**

```csharp
EmailRecipients recipient = new EmailRecipients("Recipient Name", "email@example.com");
```

2. **Create and initialize an email channel:**

```csharp
EmailChannel emailChannel = new EmailChannel("MyEmailChannel");
emailChannel.AddRecipients(recipient);
emailChannel.Initialize();
```

3. **Map the notifyer to the channel and send the notification:**

```csharp
NotificationService.MapNotifyerToChannel(notifyer, emailChannel);
NotificationService.Send(notification);
```

### NotificationService

The `NotificationService` class manages notifyers and channels, sends notifications, and maintains notification history.
Mapping notifyers to channels is necessary to send notifications. When `NotificationService.MapNotifyerToChannel` is called, `AddChannels` and `AddNotifyers` are called automatically.

#### Adding a Channel

To add a channel to the notification service:

```csharp
EmailChannel emailChannel = new EmailChannel("MyEmailChannel");
NotificationService.AddChannels(emailChannel);
```

#### Adding a Notifyer

To add a notifyer to the notification service:

```csharp
INotifyer notifyer = new MyNotifyer("MyNotifyer");
NotificationService.AddNotifyers(notifyer);
```

#### Mapping Notifyers to Channels

To map a notifyer to one or more channels:

```csharp
NotificationService.MapNotifyerToChannel(notifyer, emailChannel);
```

### Implementing New Channels and Recipients

#### Implementing a New Channel

To create a new type of channel, implement the `IChannel` interface.

```csharp
public class SmsChannel : IChannel
{
    public Guid Guid { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; }
    public List<IRecipient> Recipients { get; private set; } = new List<IRecipient>();
    public bool IsInitialized { get; private set; } = false;

    public SmsChannel(string name)
    {
        Name = name;
    }

    public void AddRecipients(params IRecipient[] recipients)
    {
        Recipients.AddRange(recipients);
    }

    public void Initialize()
    {
        // Initialize necessary resources
        IsInitialized = true;
    }

    public void Send(INotifyer notifyer, Content content)
    {
        if (!IsInitialized) throw new InvalidOperationException("Channel not initialized.");
        // Send the notification via SMS
    }

    public bool Equals(object? obj)
    {
        return obj is SmsChannel channel && Guid == channel.Guid;
    }
}
```

#### Implementing a New Recipient

To create a new type of recipient, implement the `IRecipient` interface.

```csharp
public class SmsRecipient : IRecipient
{
    public Guid Guid { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; }
    public string PhoneNumber { get; private set; }

    public SmsRecipient(string name, string phoneNumber)
    {
        Name = name;
        PhoneNumber = phoneNumber;
    }
}
```

---

This concludes the documentation for the notification library. For any questions or issues, please open an issue on the repository.