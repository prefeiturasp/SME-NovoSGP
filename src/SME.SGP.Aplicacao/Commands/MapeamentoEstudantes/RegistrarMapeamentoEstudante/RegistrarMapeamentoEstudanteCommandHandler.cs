using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RegistrarMapeamentoEstudanteCommandHandler : IRequestHandler<RegistrarMapeamentoEstudanteCommand, ResultadoMapeamentoEstudanteDto>
    {
        private readonly IRepositorioMapeamentoEstudante repositorioMapeamento;

        public RegistrarMapeamentoEstudanteCommandHandler(IRepositorioMapeamentoEstudante repositorioMapeamento)
        {
            this.repositorioMapeamento = repositorioMapeamento ?? throw new ArgumentNullException(nameof(repositorioMapeamento));
        }

        public async Task<ResultadoMapeamentoEstudanteDto> Handle(RegistrarMapeamentoEstudanteCommand request, CancellationToken cancellationToken)
        {
            var registroAcao = MapearParaEntidade(request);
            var id = await repositorioMapeamento.SalvarAsync(registroAcao);
            var resultado = new ResultadoMapeamentoEstudanteDto(id);
            resultado.Auditoria = (AuditoriaDto)registroAcao;
            return resultado;
        }

        private MapeamentoEstudante MapearParaEntidade(RegistrarMapeamentoEstudanteCommand request)
            => new ()
            {
                TurmaId = request.TurmaId,
                AlunoCodigo = request.AlunoCodigo,
                AlunoNome = request.AlunoNome,
                Bimestre = request.Bimestre,
            };
    }
}
