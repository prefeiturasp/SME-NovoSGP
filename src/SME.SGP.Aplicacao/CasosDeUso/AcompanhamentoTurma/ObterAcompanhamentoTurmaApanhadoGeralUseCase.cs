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

        public async Task<AcompanhamentoTurmaDto> Executar(FiltroAcompanhamentoTurmaApanhadoGeral param)
        {
            var turma = await mediator.Send(new ObterTurmaPorIdQuery(param.TurmaId));
            if (turma.EhNulo())
                throw new NegocioException("Turma não encontrada");

            var acompanhamentoTurma = await mediator.Send(new ObterApanhadoGeralPorTurmaIdESemestreQuery(param.TurmaId, param.Semestre));

            return MapearEntidadeParaDTO(acompanhamentoTurma);
        }

        private AcompanhamentoTurmaDto MapearEntidadeParaDTO(AcompanhamentoTurma acompanhamentoTurma)
        {
            if (acompanhamentoTurma.EhNulo())
                return new AcompanhamentoTurmaDto();
           
            var acompanhamentoTurmaDto = new AcompanhamentoTurmaDto
            {
                AcompanhamentoTurmaId = acompanhamentoTurma.Id,
                ApanhadoGeral = acompanhamentoTurma.ApanhadoGeral,
                Auditoria = (AuditoriaDto)acompanhamentoTurma
            };
            return acompanhamentoTurmaDto;
        }
    }
}
