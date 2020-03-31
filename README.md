# Jobsity Live Chat
This is a real-time chat using the following technologies

## Technologies
* ASP.NET MVC Core
* Swagger
* Bootstrap
* JQuery
* RabbitMq
* SignalR Core
* EntityFramework.Core


### Requirements
* .net core 3.0
* [RabbitMq Server](https://www.rabbitmq.com/download.html) running on default ports.

## Setup
`git clone https://github.com/jrom20/jrom20.git`

### Configurations
* Configure connections string as IdentityConnectionString and DataConnectionString that are located into Web assembly in order to seed the database.

### Run chat website
You'll only need register/login to access to the default chat room, it run over Coogle Chhrome, this approach only shows the use of entity framework and mvc technologies.

### Stock bot
It is not 100% implemented, but it still collect data from the following URL https://stooq.com/q/l/?s=aapl.us&f=sd2t2ohlcv&h&e=csv 


