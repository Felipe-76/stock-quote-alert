# Stock Quote Alert

Real-time stock price monitoring with email alerts when your predefined **buy** or **sell** targets are crossed.

---

## Table of contents

- [Overview](#overview)
- [Features](#features)
- [Quick Start](#quick-start)
  - [Prerequisites](#prerequisites)
  - [Clone & Build](#clone--build)
  - [Configuration](#configuration)
    - [`smtpsettings.json`](#smtpsettingsjson)
    - [`email_recipients.csv`](#email_recipientscsv)
- [Usage](#usage)
- [Project Layout](#project-layout)
- [Running the Tests](#running-the-tests)
- [Extending the Project](#extending-the-project)
- [License](#license)

---

## Overview

**Stock Quote Alert** is a small console application written in **C# / .NET 9** that continuously polls Yahoo Finance for the latest quotation of a single ticker and sends an e-mail notification to a list of recipients when either of the following conditions becomes true:

1. The last traded price **exceeds** your _sell_ target → *time to cash-out*  
2. The last traded price **drops below** your _buy_ target → *time to buy more*

## Features

- ✅ Uses the [YahooFinanceApi](https://github.com/lppkarl/YahooFinanceApi) NuGet package for market data.
- ✅ Simple **SMTP** integration.
- ✅ Recipients managed via a plain **CSV** file – easy to update without recompiling.
- ✅ Clean architecture with **dependency injection ready** services and interfaces.
- ✅ Fully unit-tested core business logic.

## Quick Start

### Prerequisites

| Requirement | Version |
|-------------|---------|
| [.NET SDK](https://dotnet.microsoft.com/en-us/download) | 9.0 preview (or newer) |
| Internet access | Needed to reach Yahoo Finance & SMTP server |

> 💡 You can check your SDK version with:
>
> ```bash
> dotnet --version
> ```

### Clone & Build

```bash
# clone the repository
git clone https://github.com/your-org/stock-quote-alert.git
cd stock-quote-alert

# restore dependencies & build
 dotnet build
```

### Configuration

Two small files in the repository root drive all runtime configuration.

#### `smtpsettings.json`

```json
{
  "Smtp": {
    "Host": "smtp.your-provider.com",
    "Port": 587,
    "EnableSsl": true,
    "Username": "your-smtp-user",
    "Password": "your-smtp-password",
    "From": "alerts@your-domain.com"
  }
}
```

#### `email_recipients.csv`

A minimal, header-based CSV with **name, e-mail** columns. Only the second column is currently used, so feel free to keep the first for notes.

```csv
Name,Email
Alice,alice@example.com
Bob,bob@example.com
```

Feel free to add as many recipients as you need – one per line.

## Usage

The application is executed from the command line via `dotnet run` (or the produced DLL/EXE).  The syntax is:

```bash
dotnet run -- <TICKER> <SELL_PRICE> <BUY_PRICE>
```

Examples:

```bash
# Alert me when Petrobras (São Paulo) is above 22.67 or below 22.59

dotnet run -- PETR4.SA 22.67 22.59

# US ticker – Microsoft. Sell above 360, buy below 300

dotnet run -- MSFT 360 300
```

Once started, the program:

1. Reads SMTP & recipients configuration.
2. Polls Yahoo Finance every **10 minutes** (see `AlertIntervalMins` in `Program.cs`).
3. Sends an e-mail when either target is hit.
4. Sleeps and repeats indefinitely.

Stop the application with **Ctrl-C**.

## Project Layout

```
├── ApiClients              # Connectivity to external quote providers
│   ├── IStockQuoteClient.cs
│   └── YahooStockQuoteClient.cs
├── DataModels              # Plain-old C# objects used throughout
├── Services                # Core business logic & abstractions
│   ├── ArgsParser.cs
│   ├── CsvRecipientsService.cs
│   ├── SmtpEmailService.cs
│   └── StockMonitorService.cs
├── Tests                   # xUnit test project
└── Program.cs              # Console entry point
```

Key abstractions:

- `IStockQuoteClient` → **YahooStockQuoteClient** implementation (easy to swap for another data feed).
- `IEmailService`     → **SmtpEmailService** implementation.
- `IRecipientsService`→ **CsvRecipientsService** implementation.
- `IStockMonitorService` → **StockMonitorService** orchestrates the decision.

## Running the Tests

```bash
dotnet test
```

Core behaviour (buy/sell alert detection) is covered with xUnit + Moq.

## Extending the Project

- **Different market data provider** – implement `IStockQuoteClient` and register your class.
- **Alternative notification channel** – implement `IEmailService` (e.g. push notifications, Slack webhook, SMS…).
- **Multiple tickers** – loop over a list of tickers in `Program.cs` or refactor the monitoring service.

Contributions & pull requests are welcome!

## License

This project is licensed under the terms of the **MIT** license – see the [LICENSE](LICENSE) file for details.
