{% if global_inputs.framework == 'net5.0' %}
using MediatR;{% endif %}
using {{project_name}}.{{project_type}};

{% if global_inputs.framework == 'net5.0' %}
namespace {{project_name}}.Application.SayHelloGrpc
{
{% else %}
namespace {{project_name}}.Application.SayHelloGrpc;{% endif %}

public class SayHelloCommand : IRequest<HelloReply>
{
    public SayHelloCommand(HelloRequest helloRequest)
    {
        HelloRequest = helloRequest;
    }

    public HelloRequest HelloRequest { get; set; }
}
{% if global_inputs.framework == 'net5.0' %}
}{% endif %}