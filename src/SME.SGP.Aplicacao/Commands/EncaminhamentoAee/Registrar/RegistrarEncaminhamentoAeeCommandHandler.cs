using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands
{
    public class RegistrarEncaminhamentoAeeCommandHandler : IRequestHandler<RegistrarEncaminhamentoAeeCommand, ResultadoEncaminhamentoAeeDto>
    {
        private readonly IRepositorioEncaminhamentoAEE repositorioEncaminhamentoAEE;

        public RegistrarEncaminhamentoAeeCommandHandler(IRepositorioEncaminhamentoAEE repositorioEncaminhamentoAEE)
        {
            this.repositorioEncaminhamentoAEE = repositorioEncaminhamentoAEE ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoAEE));
        }

        public async Task<ResultadoEncaminhamentoAeeDto> Handle(RegistrarEncaminhamentoAeeCommand request, CancellationToken cancellationToken)
        {
            var encaminhamento = MapearParaEntidade(request);
            await repositorioEncaminhamentoAEE.SalvarAsync(encaminhamento);
            var resultado = new ResultadoEncaminhamentoAeeDto();
            return resultado;
        }

        private EncaminhamentoAEE MapearParaEntidade(RegistrarEncaminhamentoAeeCommand request)
            => new EncaminhamentoAEE()
            {
                TurmaId = request.TurmaId
            };
    }
}
