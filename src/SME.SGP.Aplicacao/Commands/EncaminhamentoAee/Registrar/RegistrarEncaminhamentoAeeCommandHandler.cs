using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands
{
    public class RegistrarEncaminhamentoAeeCommandHandler : IRequestHandler<RegistrarEncaminhamentoAeeCommand, ResultadoEncaminhamentoAEEDto>
    {
        private readonly IRepositorioEncaminhamentoAEE repositorioEncaminhamentoAEE;

        public RegistrarEncaminhamentoAeeCommandHandler(IRepositorioEncaminhamentoAEE repositorioEncaminhamentoAEE)
        {
            this.repositorioEncaminhamentoAEE = repositorioEncaminhamentoAEE ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoAEE));
        }

        public async Task<ResultadoEncaminhamentoAEEDto> Handle(RegistrarEncaminhamentoAeeCommand request, CancellationToken cancellationToken)
        {
            var encaminhamento = MapearParaEntidade(request);
            await repositorioEncaminhamentoAEE.SalvarAsync(encaminhamento);
            var resultado = new ResultadoEncaminhamentoAEEDto();
            return resultado;
        }

        private EncaminhamentoAEE MapearParaEntidade(RegistrarEncaminhamentoAeeCommand request)
            => new EncaminhamentoAEE()
            {
                TurmaId = request.TurmaId
            };
    }
}
