using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAtividadesNotasAlunoPorTurmaPeriodoQueryHandler : IRequestHandler<ObterAtividadesNotasAlunoPorTurmaPeriodoQuery, IEnumerable<AvaliacaoNotaAlunoDto>>
    {
        private readonly IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa;
        private readonly Mediator mediator;

        public ObterAtividadesNotasAlunoPorTurmaPeriodoQueryHandler(IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa, Mediator mediator)
        {
            this.repositorioAtividadeAvaliativa = repositorioAtividadeAvaliativa ?? throw new ArgumentNullException(nameof(repositorioAtividadeAvaliativa));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<AvaliacaoNotaAlunoDto>> Handle(ObterAtividadesNotasAlunoPorTurmaPeriodoQuery request, CancellationToken cancellationToken)
        {
            var retorno = (await repositorioAtividadeAvaliativa.ObterAtividadesNotasAlunoPorTurmaPeriodo(request.TurmaId, request.PeriodoEscolarId, request.AlunoCodigo, request.ComponenteCurricular)).ToList();

            retorno = await ObterAusencia(request, retorno);

            return retorno;
        }

        private async Task<List<AvaliacaoNotaAlunoDto>> ObterAusencia(ObterAtividadesNotasAlunoPorTurmaPeriodoQuery request, List<AvaliacaoNotaAlunoDto> listAtividades)
        {
            var retorno = new List<AvaliacaoNotaAlunoDto>();
            var datasDasAtividadesAvaliativas = retorno.Select(x => x.Data).ToArray();
            var turma = await mediator.Send(new ObterTurmaPorIdQuery(request.TurmaId));

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