using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Queries.Funcionario;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.Github.ObterVersaoRelease
{
    public class ObterFuncionariosQueryHandler : IRequestHandler<ObterFuncionariosQuery, FiltroFuncionariosDto>
    {
        private readonly IServicoEol servicoEOL;

        public ObterFuncionariosQueryHandler(IServicoEol servicoEOL)
        {
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
        }

        Task<FiltroFuncionariosDto> IRequestHandler<ObterFuncionariosQuery, FiltroFuncionariosDto>.Handle(ObterFuncionariosQuery request, CancellationToken cancellationToken)
        {
            // TODO: Fazer chamada do servico do EOL
            throw new NotImplementedException();
        }
    }
}
