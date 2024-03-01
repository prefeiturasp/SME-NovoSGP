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
            var id = await repositorioEncaminhamentoAEE.SalvarAsync(encaminhamento);
            var resultado = new ResultadoEncaminhamentoAEEDto(id);
            return resultado;
        }

        private EncaminhamentoAEE MapearParaEntidade(RegistrarEncaminhamentoAeeCommand request)
            => new EncaminhamentoAEE()
            {
                TurmaId = request.TurmaId,
                Situacao = request.Situacao,
                AlunoCodigo = request.AlunoCodigo,
                AlunoNome = request.AlunoNome
            };
    }
}
