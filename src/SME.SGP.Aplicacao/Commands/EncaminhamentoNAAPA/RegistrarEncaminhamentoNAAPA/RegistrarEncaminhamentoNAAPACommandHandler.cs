using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RegistrarEncaminhamentoNAAPACommandHandler : IRequestHandler<RegistrarEncaminhamentoNAAPACommand, ResultadoEncaminhamentoNAAPADto>
    {
        private readonly IRepositorioEncaminhamentoNAAPA repositorioEncaminhamentoNAAPA;

        public RegistrarEncaminhamentoNAAPACommandHandler(IRepositorioEncaminhamentoNAAPA repositorioEncaminhamentoNAAPA)
        {
            this.repositorioEncaminhamentoNAAPA = repositorioEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNAAPA));
        }

        public async Task<ResultadoEncaminhamentoNAAPADto> Handle(RegistrarEncaminhamentoNAAPACommand request, CancellationToken cancellationToken)
        {
            var encaminhamento = MapearParaEntidade(request);
            var id = await repositorioEncaminhamentoNAAPA.SalvarAsync(encaminhamento);
            var resultado = new ResultadoEncaminhamentoNAAPADto(id);
            resultado.Auditoria = (AuditoriaDto)encaminhamento;
            return resultado;
        }

        private EncaminhamentoNAAPA MapearParaEntidade(RegistrarEncaminhamentoNAAPACommand request)
            => new ()
            {
                TurmaId = request.TurmaId,
                Situacao = request.Situacao,
                AlunoCodigo = request.AlunoCodigo,
                AlunoNome = request.AlunoNome
            };
    }
}
