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

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class AulasSemAtribuicaoSubstituicaoTurmaMensalUseCase : IAulasSemAtribuicaoSubstituicaoTurmaMensalUseCase
    {
        private readonly IRepositorioSGPConsulta repositorioSGP;
        private readonly IRepositorioAulasSemAtribuicaoSubstituicaoMensal repositorioAulas;
        private readonly IMediator mediator;

        public AulasSemAtribuicaoSubstituicaoTurmaMensalUseCase(IRepositorioSGPConsulta repositorioSGP, 
                                                                IRepositorioAulasSemAtribuicaoSubstituicaoMensal repositorioAulas,
                                                                IMediator mediator)
        {
            this.repositorioSGP = repositorioSGP ?? throw new ArgumentNullException(nameof(repositorioSGP));
            this.repositorioAulas = repositorioAulas ?? throw new ArgumentNullException(nameof(repositorioAulas));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtroTurma = mensagem.ObterObjetoMensagem<FiltroCodigoDataDto>();
            var codigosComponentesCurricularesTurmaSemAtribuicao = await mediator.Send(new ObterComponentesCurricularesSemAtribuicaoPorTurmaDataQuery(filtroTurma.Codigo, filtroTurma.Data));
            //var codigosComponentesCurricularesTurmaSemAtribuicao = new List<string>() { };
   
            foreach (var codigoComponente in codigosComponentesCurricularesTurmaSemAtribuicao)
            {
                var ehRegencia = await repositorioSGP.ComponenteCurriculareERegencia(long.Parse(codigoComponente));
                var turma = await repositorioSGP.ObterTurmaComUeEDrePorCodigo(filtroTurma.Codigo);

                var horasAulaGrade = await mediator.Send(new ObterHorasGradeComponenteCurricularTurmaQuery(turma, codigoComponente, ehRegencia));
                var horasAulaLancadas = await mediator.Send(new ObterHorasCadastradasComponenteCurricularTurmaQuery(turma, codigoComponente, ehRegencia, filtroTurma.Data));
                var diferencaAulasLancadasAulasGrade = Math.Max(horasAulaGrade - horasAulaLancadas, 0);

                await repositorioAulas.InserirAsync(new Entidade.AulasSemAtribuicaoSubstituicaoMensal(filtroTurma.Codigo, codigoComponente, (int)turma.ModalidadeCodigo,
                                                                                                      filtroTurma.Data, diferencaAulasLancadasAulasGrade,
                                                                                                      ehRegencia));
            }
                        
            return true;
        }
    }
}
