using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAcompanhamentoTurmaApanhadoGeralUseCase : AbstractUseCase, IObterAcompanhamentoTurmaApanhadoGeralUseCase
    {
        public ObterAcompanhamentoTurmaApanhadoGeralUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<AcompanhamentoTurmaDto> Executar(FiltroAcompanhamentoTurmaApanhadoGeral filtro)
        {
            var acompanhamentoTurma = await mediator.Send(new ObterApanhadoGeralPorTurmaIdESemestreQuery(filtro.TurmaId, filtro.Semestre));
            return MapearEntidadeParaDTO(acompanhamentoTurma);
        }

        private AcompanhamentoTurmaDto MapearEntidadeParaDTO(AcompanhamentoTurma acompanhamentoTurma)
        {
            var acompanhamentoTurmaDto = new AcompanhamentoTurmaDto
            {
                AcompanhamentoTurmaId = acompanhamentoTurma.Id,
                ApanhadoGeral = acompanhamentoTurma.ApanhadoGeral,
                Semestre = acompanhamentoTurma.Semestre,
                TurmaId = acompanhamentoTurma.TurmaId,
                Auditoria = (AuditoriaDto)acompanhamentoTurma
            };
            return acompanhamentoTurmaDto;
        }
    }
}
