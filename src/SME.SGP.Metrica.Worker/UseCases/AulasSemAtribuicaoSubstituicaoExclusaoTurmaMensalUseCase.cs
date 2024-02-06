using MediatR;
using SME.SGP.Infra;
using SME.SGP.Metrica.Worker.Queries;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System;
using System.Threading.Tasks;
using System.Linq;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using Nest;
using SME.SGP.Infra.Utilitarios;
using SME.SGP.Metrica.Worker.Commands;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class AulasSemAtribuicaoSubstituicaoExclusaoTurmaMensalUseCase : IAulasSemAtribuicaoSubstituicaoExclusaoTurmaMensalUseCase
    {
        private readonly IRepositorioSGPConsulta repositorioSGP;
        private readonly IRepositorioAulasSemAtribuicaoSubstituicaoMensal repositorioAulas;
        private readonly IMediator mediator;

        public AulasSemAtribuicaoSubstituicaoExclusaoTurmaMensalUseCase(IRepositorioSGPConsulta repositorioSGP,
                                                                IRepositorioAulasSemAtribuicaoSubstituicaoMensal repositorioAulas,
                                                                IMediator mediator)
        {
            this.repositorioSGP = repositorioSGP ?? throw new ArgumentNullException(nameof(repositorioSGP));
            this.repositorioAulas = repositorioAulas ?? throw new ArgumentNullException(nameof(repositorioAulas));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtroTurma = mensagem.ObterObjetoMensagem<FiltroComponentesSemAtribuicaoCodigoDataMetricasDto>();
            var turma = await repositorioSGP.ObterTurmaComUeEDrePorCodigo(filtroTurma.CodigoTurma);
            await ExcluirDocumentos(turma, filtroTurma.Data, filtroTurma.CodigosComponentesSemAtribuicao);
            return true;
        }

        private async Task ExcluirDocumentos(Turma turma, DateTime dataJob, string[] codigosComponentesCurricularesTurmaSemAtribuicao)
        {
            var componentesCurricularesTurma = await mediator.Send(new ObterComponentesCurricularesVinculadosTurmaQuery(turma.CodigoTurma));
            var codigosComponentesCurricularesAtribuidos = componentesCurricularesTurma.Except(codigosComponentesCurricularesTurmaSemAtribuicao);
            foreach (var codigoComponenteCurricular in codigosComponentesCurricularesAtribuidos)
            {
                var ehRegencia = await repositorioSGP.ComponenteCurriculareEhRegencia(long.Parse(codigoComponenteCurricular));
                var semana = UtilData.ObterSemanaDoAno(dataJob);
                var id = $"{turma.CodigoTurma}-{codigoComponenteCurricular}-{(ehRegencia ? dataJob.ToString("yyyyMMdd") : $"{dataJob.ToString("yyyyMM")}-{semana}")}";
                await repositorioAulas.ExcluirPorId(id);
            }
        }
    }
}
