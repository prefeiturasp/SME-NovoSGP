using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ListarAlunosDaTurmaRegistroIndividualUseCase : AbstractUseCase, IListarAlunosDaTurmaRegistroIndividualUseCase
    {
        public ListarAlunosDaTurmaRegistroIndividualUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<AlunoDadosBasicosDto>> Executar(FiltroRegistroIndividualBase filtro)
        {

            var turma = await mediator.Send(new ObterTurmaPorIdQuery(filtro.TurmaId));

            if (turma == null)
                throw new NegocioException("Não foi encontrado turma com o id informado");

            var alunosParaRegistroIndividual = await mediator.Send(new ListarAlunosDaTurmaPorComponenteRegistroIndividualQuery(turma, filtro.ComponenteCurricularId));

            //TODO: Modificar para mediatr
            var parametroDiasSemRegistroIndividual = await ObterDiasDeAusenciaParaPendenciaRegistroIndividualAsync();

            var alunosDaTurmaComPendencia = (await mediator.Send(new ListarAlunosCodigosPorTurmaComponenteComPendenciaQuery(turma.Id, filtro.ComponenteCurricularId))).ToList();

            var alunosDadosBasicosDTO = await ObterAlunoTemAtendimentoPlanoAEE(turma, alunosParaRegistroIndividual);

            if (parametroDiasSemRegistroIndividual > 0 && alunosDaTurmaComPendencia.Any())
            {
                var alunosComPendencia = alunosParaRegistroIndividual.Where(a => alunosDaTurmaComPendencia.Contains(long.Parse(a.CodigoEOL))).ToList();
                foreach (var alunoDaTurma in alunosComPendencia)
                {
                    var registroParaAlterar = alunosParaRegistroIndividual.FirstOrDefault(a => a.CodigoEOL == alunoDaTurma.CodigoEOL);
                    registroParaAlterar.MarcaComoSemRegistroPorDias(parametroDiasSemRegistroIndividual);
                }
            }

            return alunosDadosBasicosDTO;
        }

        private async Task<List<AlunoDadosBasicosDto>> ObterAlunoTemAtendimentoPlanoAEE(Turma turma, IEnumerable<AlunoDadosBasicosDto> alunosParaRegistroIndividual)
        {
            var alunosDadosBasicosDTO = new List<AlunoDadosBasicosDto>();
            foreach (var aluno in alunosParaRegistroIndividual)
            {
                aluno.EhAtendidoAEE = await mediator.Send(new VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQuery(aluno.CodigoEOL, turma.AnoLetivo));
                alunosDadosBasicosDTO.Add(aluno);
            }

            return alunosDadosBasicosDTO;
        }

        private async Task<int> ObterDiasDeAusenciaParaPendenciaRegistroIndividualAsync()
        {
            var parametroDiasSemRegistroIndividual = await mediator.Send(new ObterParametroSistemaPorTipoQuery(TipoParametroSistema.PendenciaPorAusenciaDeRegistroIndividual));
            return parametroDiasSemRegistroIndividual != null ? int.Parse(parametroDiasSemRegistroIndividual) : 0;
        }
    }

}
