# Jobsity Live Chat
This is a real-time chat using the following technologies

## Technologies
* ASP.NET MVC Core
* Swagger
* Bootstrap
* JQuery
* font-awesome
* RabbitMq
* SignalR Core
* EntityFramework.Core

### Requirements
* [.Net Core](https://dotnet.microsoft.com/download/visual-studio-sdks).
* [RabbitMq Server](https://www.rabbitmq.com/download.html) running on default ports.

## Setup
`git clone https://github.com/jrom20/LiveChatWebApp`

### Configurations
* Configure connections string as IdentityConnectionString and DataConnectionString that are located into Web assembly in order to seed the database.
* Configure the **Web** assembly as Default Start Up.
* Run as new instance the executable program which is in **BotStock.StandAlone** assembly to set the listener of Rabbit Queues and consume each message.

### Run chat website
You'll only need register/login to access to the default chat room, its run over Google Chhrome, this approach only shows the use of Entity Framework Core and MVC Core technologies.

## Chat Usage
### Stock bot
The Bot Stock is full working it supports any kind of inputs. By default it will shows the last 50 messages that were saved.

Display quotes by submitting the following command:
**/stock={stock_code}** e.g: `/stock=aapl.us`

those will be extracted and parsed from [stooq.com](https://stooq.com/)


