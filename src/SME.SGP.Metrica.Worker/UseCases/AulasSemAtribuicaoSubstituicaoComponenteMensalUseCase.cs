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

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class AulasSemAtribuicaoSubstituicaoComponenteMensalUseCase : IAulasSemAtribuicaoSubstituicaoComponenteMensalUseCase
    {
        private readonly IRepositorioSGPConsulta repositorioSGP;
        private readonly IRepositorioAulasSemAtribuicaoSubstituicaoMensal repositorioAulas;
        private readonly IMediator mediator;

        public AulasSemAtribuicaoSubstituicaoComponenteMensalUseCase(IRepositorioSGPConsulta repositorioSGP, 
                                                                IRepositorioAulasSemAtribuicaoSubstituicaoMensal repositorioAulas,
                                                                IMediator mediator)
        {
            this.repositorioSGP = repositorioSGP ?? throw new ArgumentNullException(nameof(repositorioSGP));
            this.repositorioAulas = repositorioAulas ?? throw new ArgumentNullException(nameof(repositorioAulas));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtroComponente = mensagem.ObterObjetoMensagem<FiltroComponenteCodigoDataMetricasDto>();
            var turma = await repositorioSGP.ObterTurmaComUeEDrePorCodigo(filtroComponente.CodigoTurma);

            var ehRegencia = await repositorioSGP.ComponenteCurriculareEhRegencia(long.Parse(filtroComponente.Codigo));
            var horasAulaGrade = await mediator.Send(new ObterHorasGradeComponenteCurricularTurmaQuery(turma, filtroComponente.Codigo, ehRegencia));
            var horasAulaLancadas = await mediator.Send(new ObterHorasCadastradasComponenteCurricularTurmaQuery(turma, filtroComponente.Codigo, ehRegencia, filtroComponente.Data));
            var diferencaAulasLancadasXAulasGrade = Math.Max(horasAulaGrade - horasAulaLancadas, 0);
            if (diferencaAulasLancadasXAulasGrade > 0)
                await repositorioAulas.InserirAsync(new Entidade.AulasSemAtribuicaoSubstituicaoMensal(filtroComponente.CodigoTurma, filtroComponente.Codigo, (int)turma.ModalidadeCodigo,
                                                                                                        filtroComponente.Data, diferencaAulasLancadasXAulasGrade,
                                                                                                          ehRegencia));
            return true;
        }
    }
}
