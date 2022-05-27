{% if global_inputs.framework == 'net5.0' %}
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
{% endif %}using Grpc.Core;
using {{project_name}}.Application.SayHelloGrpc;

{% if global_inputs.framework == 'net5.0' %}
namespace {{project_name}}.{{project_type}}.Services
{
{% else %}
namespace {{project_name}}.{{project_type}}.Services;{% endif %}

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
{% if global_inputs.framework == 'net5.0' %}
}
{% endif %}