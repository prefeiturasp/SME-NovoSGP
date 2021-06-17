using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterCodigosDresQuery : IRequest<string[]>
    {
        public ObterCodigosDresQuery()
        {
        }
    }
}
