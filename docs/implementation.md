### **Inputs configurados automaticamente**  

O input abaixo é usado para configurar o plugin:  

| **Campo** | **Valor** | **Descrição** |
| :--- | :--- | :--- |
| gRPC Port | ex.: 50051 |  Porta em que será exposta a comunicação gRPC. |

- A configuração abaixo será feita no **`IServiceCollection`**, através do `services.AddGrpcServer();`, no arquivo `Startup` ou `Program`.      

```csharp
services.AddGrpcServer();
```

- A configuração abaixo será feita no **`IApplicationBuilder,`**, através do `app.UseGrpc`, no `Startup` ou `Program`:  

```csharp
app.UseGrpc(environment, Assembly.GetEntryAssembly(), Path.Combine(Directory.GetParent(environment.ContentRootPath).FullName, "Protos"));
```

- As configurações de portas do `WebHost` serão adicionadas, através do `builder.WebHost.ConfigureKestrel`, no arquivo `Program`. Confira o exemplo abaixo:   

```csharp
builder.WebHost.ConfigureKestrel((context, options) =>
{
    options.Listen(IPAddress.Any, 80, listenOptions =>
   {
       listenOptions.Protocols = HttpProtocols.Http1;
   });
    options.Listen(IPAddress.Any, 50051, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
    });
    options.ListenLocalhost(5005, o => o.Protocols = HttpProtocols.Http2); 
});
```

### **Outros exemplos de configurações automáticas**

Confira abaixo alguns exemplos de configurações feitas automaticamente pelo plugin quando ele é aplicado:  

#### ***Arquivo Proto***

O arquivo **`.proto`** tem as definições dos serviços e mensagens que serão utilizadas na comunicação.

Com as configurações de **gRPC** ativadas no seu projeto, ao compilar a sua aplicação, alguns arquivos serão criados no projeto `MyApp.Application\Base`, com as classes geradas pelo plugin de **gRPC**. 

No exemplo abaixo os arquivos criados são: **`Greet.cs`** e **`GreetGrpc.cs`**:  

```proto
syntax = "proto3";

option csharp_namespace = "MyApp.Api";

package greet;

service Greeter {
  rpc SayHello (HelloRequest) returns (HelloReply);
}

message HelloRequest {
  string name = 1;
}

message HelloReply {
  string message = 1;
}
```

#### ***Service***

No projeto `Api` o arquivo `SayHelloService`foi adicionado. Ele quem define o endpoint que será utilizado para receber as requisições.

> Esta classe é herdada da classe `Greeter.GreeterBase`, que foi gerada automaticamente a partir do arquivo `.proto` e compilado da solução.

Confira o exemplo dela abaixo:  

```csharp
public class SayHelloService : Greeter.GreeterBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SayHelloService> _logger;

        public SayHelloService(IMediator mediator, ILogger<SayHelloService> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public override async Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Start Process", request);

            return await _mediator.Send(new SayHelloCommand(request));
        }
    }
```

Além disso, no projeto `Application` foram criadas as classes **`Command`** e `Handler`**, seguindo o padrão inicial do template com [**Clean Architeture**](https://www.zup.com.br/blog/clean-architecture-arquitetura-limpa). 

Confira os exemplos abaixo:  

```csharp
    public class SayHelloCommand : IRequest<HelloReply>
    {
        public SayHelloCommand(HelloRequest helloRequest)
        {
            HelloRequest = helloRequest;
        }

        public HelloRequest HelloRequest { get; set; }
    }
```

```csharp
    public class SayHelloHandler : IRequestHandler<SayHelloCommand, HelloReply>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SayHelloHandler> _logger;

        public SayHelloHandler(IMediator mediator, ILogger<SayHelloHandler> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public Task<HelloReply> Handle(SayHelloCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Process Handler", request);

            return Task.FromResult(new HelloReply {Message = $"Hello {request.HelloRequest.Name }!"});

        }
    }
```

#### ***Client***

Um projeto `*.GrpcClient.csproj` é adicionado à sua solução e ao compilar o seu projeto, ele criará as classes **`Greet.cs`** e **`GreetGrpc.cs`** com as implementações para aplicações clientes. 

Estas classes vão lançar requisições para o seu endpoint **gRPC**. O projeto já vem pré-configurado para que você possa, por exemplo, empacotar com o [**Nuget**](https://www.nuget.org/packages/StackSpot.Grpc/) e distribuir para quem quiser utilizá-lo. 

#### **Execução em Ambiente local**  

Como configuração adicional, a porta para o serviço **gRPC** informada nos `Inputs` foi exposta nos arquivos `Dockerfile`e `docker-compose.yml`.
