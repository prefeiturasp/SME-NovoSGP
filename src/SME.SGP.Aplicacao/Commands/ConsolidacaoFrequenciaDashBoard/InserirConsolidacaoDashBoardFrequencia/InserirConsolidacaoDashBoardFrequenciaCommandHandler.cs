using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class InserirConsolidacaoDashBoardFrequenciaCommandHandler : IRequestHandler<InserirConsolidacaoDashBoardFrequenciaCommand, bool>
    {        
        private readonly IRepositorioConsolidacaoFrequenciaTurma repositorioConsolidacaoFrequenciaTurma;
        private readonly IMediator mediator;

        public InserirConsolidacaoDashBoardFrequenciaCommandHandler(IRepositorioConsolidacaoFrequenciaTurma repositorioConsolidacaoFrequenciaTurma, IMediator mediator)
        {            
            this.repositorioConsolidacaoFrequenciaTurma = repositorioConsolidacaoFrequenciaTurma ?? throw new ArgumentNullException(nameof(repositorioConsolidacaoFrequenciaTurma));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(InserirConsolidacaoDashBoardFrequenciaCommand request, CancellationToken cancellationToken)
        {
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(request.TurmaId));

            if (turma == null)
                return false;

            var anoLetivo = request.DataAula.Year;
            int? mes = null;
            var data = request.DataAula;
            DateTime? dataInicioSemana = null;
            DateTime? dataFimSemana = null; 

            if (request.TipoPeriodo == TipoPeriodoDashboardFrequencia.Semanal)
            {
                while (data.DayOfWeek != DayOfWeek.Monday)
                {
                    data = data.AddDays(-1);
                }

                dataInicioSemana = data;
                dataFimSemana = data.AddDays(6);
            }else if(request.TipoPeriodo == TipoPeriodoDashboardFrequencia.Mensal)
            {
                mes = request.DataAula.Month;
            }
            
            var consolidacao = await mediator.Send(new ObterDadosParaConsolidacaoDashBoardFrequenciaPorTurmaQuery(anoLetivo,
                                                                                                                  request.TurmaId,
                                                                                                                  turma.ModalidadeCodigo,
                                                                                                                  request.TipoPeriodo,
                                                                                                                  request.DataAula,
                                                                                                                  mes,
                                                                                                                  dataInicioSemana,
                                                                                                                  dataFimSemana));

            if (consolidacao == null)
                return false;

            var consolidacaoJaExistente = await mediator.Send(new ObterConsolidacaoExistentePorTurmaIdAnoLetivoTipoPeriodoMesQuery(request.TurmaId, anoLetivo, request.TipoPeriodo, request.DataAula, mes,
                                                                dataInicioSemana, dataFimSemana));

            if (consolidacaoJaExistente != null)
            {
                bool validaMudancaDeInformacoes = (consolidacao.Ausentes == consolidacaoJaExistente.Ausentes && consolidacao.Presentes == consolidacaoJaExistente.Presentes
                                                   && consolidacao.Remotos == consolidacaoJaExistente.Remotos);

                if (!validaMudancaDeInformacoes)
                    await mediator.Send(new AlterarConsolidacaoDashboardFrequenciaTurmaCommand(consolidacaoJaExistente.Id, consolidacao.Remotos, consolidacao.Ausentes, consolidacao.Presentes));     
            }
            else
            {
                return await repositorioConsolidacaoFrequenciaTurma.InserirConsolidacaoDashBoard(MapearParaEntidade(turma,
                                                                                                                 consolidacao,
                                                                                                                 request.DataAula,
                                                                                                                 (int)request.TipoPeriodo,
                                                                                                                 dataInicioSemana,
                                                                                                                 dataFimSemana,
                                                                                                                 mes)) != 0;
            }

            return true;
        }

        private ConsolidacaoDashBoardFrequencia MapearParaEntidade(Turma turma, DadosParaConsolidacaoDashBoardFrequenciaDto dados, DateTime dataAula, int tipoPeriodo, DateTime? dataInicio = null, DateTime? dataFim = null, int? mes = null)
        {
            return new ConsolidacaoDashBoardFrequencia()
            {
                AnoLetivo = dataAula.Year,
                TurmaId = turma.Id,
                TurmaNome = turma.NomeComModalidade(),
                TurmaAno = turma.AnoComModalidade(),
                semestre = turma.Semestre,
                DataAula = dataAula,
                DataInicio = dataInicio,
                DataFim = dataFim,
                Mes = mes,
                ModalidadeCodigo = (int)turma.ModalidadeCodigo,
                Tipo = tipoPeriodo,
                DreId = turma.Ue.DreId,
                DreCodigo = turma.Ue.Dre.CodigoDre,
                UeId = turma.UeId,
                DreAbreviacao = AbreviacaoDreFormatado(turma.Ue.Dre.Abreviacao),
                QuantidadePresencas = dados.Presentes,
                QuantidadeRemotos = dados.Remotos,
                QuantidadeAusentes = dados.Ausentes,
                CriadoEm = DateTime.Now
            };
        }
        private static string AbreviacaoDreFormatado(string abreviacaoDre)
           => abreviacaoDre.Replace(DashboardConstants.PrefixoDreParaSerRemovido, string.Empty).Trim();        
    }
        
}
