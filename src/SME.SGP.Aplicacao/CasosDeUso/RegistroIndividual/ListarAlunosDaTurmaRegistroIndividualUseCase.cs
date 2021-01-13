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

            var alunosDaTurmaComPendencia = (await mediator.Send(new ListarAlunosCodigosPorTurmaComponeteComPendenciaQuery(turma.Id, filtro.ComponenteCurricularId))).ToList() ;            

            if (alunosDaTurmaComPendencia.Any())
            {
                var alunosComPendencia = alunosParaRegistroIndividual.Where(a => alunosDaTurmaComPendencia.Contains(long.Parse(a.CodigoEOL))).ToList();
                foreach (var alunoDaTurma in alunosComPendencia)
                {
                    var registroParaAlterar = alunosParaRegistroIndividual.FirstOrDefault(a => a.CodigoEOL == alunoDaTurma.CodigoEOL);
                    registroParaAlterar.MarcaComoSemRegistroPorDias(parametroDiasSemRegistroIndividual);
                }
            }

            return alunosParaRegistroIndividual;
        }
        private async Task<int> ObterDiasDeAusenciaParaPendenciaRegistroIndividualAsync()
        {
            var parametroDiasSemRegistroIndividual = await mediator.Send(new ObterParametroSistemaPorTipoQuery(TipoParametroSistema.PendenciaPorAusenciaDeRegistroIndividual));
            if (string.IsNullOrEmpty(parametroDiasSemRegistroIndividual))
                throw new NegocioException($"Não foi possível obter o parâmetro {TipoParametroSistema.PendenciaPorAusenciaDeRegistroIndividual.Name()} ");

            return int.Parse(parametroDiasSemRegistroIndividual);
        }
    }

}
