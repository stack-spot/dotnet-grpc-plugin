{% if global_inputs.framework == 'net5.0' %}
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;{% endif %}
using {{project_name}}.{{project_type}};

{% if global_inputs.framework == 'net5.0' %}
namespace {{project_name}}.Application.SayHelloGrpc
{
{% else %}
namespace {{project_name}}.Application.SayHelloGrpc;{% endif %}

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
{% if global_inputs.framework == 'net5.0' %}
}{% endif %}