using MediatR;
using SME.Background.Core;

namespace SME.SGP.Aplicacao
{
    public static class MediatRExtension
    {
        public static void Enfileirar<T>(this IMediator mediator, IRequest<T> request)
        {
            Cliente.Executar<HangfireMediator>(mediatr => mediatr.SendCommand(request));
        }
    }


}
