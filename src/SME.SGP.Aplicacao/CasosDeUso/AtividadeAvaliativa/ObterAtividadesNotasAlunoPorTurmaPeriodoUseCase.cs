using System;
using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterAtividadesNotasAlunoPorTurmaPeriodoUseCase : AbstractUseCase, IObterAtividadesNotasAlunoPorTurmaPeriodoUseCase
    {
        public ObterAtividadesNotasAlunoPorTurmaPeriodoUseCase(IMediator mediator) : base(mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        private readonly IMediator mediator;

        public async Task<IEnumerable<AvaliacaoNotaAlunoDto>> Executar(FiltroTurmaAlunoPeriodoEscolarDto param)
        {
          var retorno =  (await mediator.Send(new ObterAtividadesNotasAlunoPorTurmaPeriodoQuery(param.TurmaId, param.PeriodoEscolarId, param.AlunoCodigo, param.ComponenteCurricular))).ToList();

          retorno = await ObterAusencia(param, retorno);
          return retorno;
        }
        
        private async Task<List<AvaliacaoNotaAlunoDto>> ObterAusencia(FiltroTurmaAlunoPeriodoEscolarDto request, List<AvaliacaoNotaAlunoDto> listAtividades)
        {
            var retorno = new List<AvaliacaoNotaAlunoDto>();
            var datasDasAtividadesAvaliativas = retorno.Select(x => x.Data).ToArray();
            var turma = await mediator.Send(new ObterTurmaPorIdQuery(request.TurmaId));
            
            if(turma == null)
                throw new NegocioException("Turma não encontrada");

            var ausenciasDasAtividadesAvaliativas = (await mediator.Send(new ObterAusenciasDaAtividadesAvaliativasPorAlunoQuery(turma.CodigoTurma, datasDasAtividadesAvaliativas, request.ComponenteCurricular, request.AlunoCodigo))).ToList();

            foreach (var atividade in listAtividades)
            {
                var ausente = ausenciasDasAtividadesAvaliativas
                    .Any(a => a.AlunoCodigo == request.AlunoCodigo && a.AulaData.Date == atividade.Data.Date);

                atividade.Ausente = ausente;
                retorno.Add(atividade);
            }

            return retorno;
        }
    }
}
