using MediatR;
using SME.SGP.Infra.Dtos.Relatorios;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Infra;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesRegenciaPorTurmaUseCase : IObterComponentesCurricularesRegenciaPorTurmaUseCase
    {
        private readonly IMediator mediator;

        public ObterComponentesCurricularesRegenciaPorTurmaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<DisciplinaDto>> Executar(long turmaId)
        {
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(turmaId));
            
            if(turma == null)
                throw new NegocioException("Turma não encontrada.");

            var turno = turma.ModalidadeCodigo == Modalidade.Fundamental ? turma.QuantidadeDuracaoAula : 0;
            var ano = turma.ModalidadeCodigo == Modalidade.Fundamental && !string.IsNullOrEmpty(turma.Ano) && int.TryParse(turma.Ano, out _) ? Convert.ToInt64(turma?.Ano) : 0;

            return await mediator.Send(new ObterComponentesCurricularesRegenciaPorAnoETurnoQuery(ano, turno));

        }
    }
}
