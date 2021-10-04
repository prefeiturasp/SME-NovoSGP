using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class InserirConsolidacaoDashBoardFrequenciaCommandHandler : IRequestHandler<InserirConsolidacaoDashBoardFrequenciaCommand, bool>
    {
        private readonly IRepositorioDashBoardFrequencia repositorioDashBoardFrequencia;
        private readonly IMediator mediator;

        public InserirConsolidacaoDashBoardFrequenciaCommandHandler(IRepositorioDashBoardFrequencia repositorioDashBoardFrequencia, IMediator mediator)
        {
            this.repositorioDashBoardFrequencia = repositorioDashBoardFrequencia ?? throw new ArgumentNullException(nameof(repositorioDashBoardFrequencia));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(InserirConsolidacaoDashBoardFrequenciaCommand request, CancellationToken cancellationToken)
        {
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(request.TurmaId));

            if (turma == null)
                return false;

            var anoLetivo = request.DataAula.Year;
            var dataInicioSemena = DateTime.Now;
            var dataFimSemena = DateTime.Now;

            var consolidacao = await mediator.Send(new ObterDadosParaConsolidacaoDashBoardFrequenciaPorTurmaQuery(anoLetivo,
                                                                                                                  request.TurmaId,
                                                                                                                  turma.ModalidadeCodigo,
                                                                                                                  request.TipoPeriodo,
                                                                                                                  request.DataAula.Month,
                                                                                                                  dataInicioSemena,
                                                                                                                  dataFimSemena));

            return await repositorioDashBoardFrequencia.Inserir(MapearParaEntidade(turma,
                                                                                   consolidacao,
                                                                                   request.DataAula,
                                                                                   (int)request.TipoPeriodo)) != 0;
        }

        private ConsolidacaoDashBoardFrequencia MapearParaEntidade(Turma turma, DadosParaConsolidacaoDashBoardFrequenciaDto dados, DateTime dataAula, int tipoPeriodo)
        {
            return new ConsolidacaoDashBoardFrequencia()
            {
                AnoLetivo = dataAula.Year,
                TurmaId = turma.Id,
                TurmaNome = turma.NomeComModalidade(),
                TurmaAno = turma.Ano,
                DataAula = dataAula,
                ModalidadeCodigo = (int)turma.ModalidadeCodigo,
                Tipo = tipoPeriodo,
                DreId = turma.Ue.DreId,
                UeId = turma.UeId,
                DreAbreviacao = turma.Ue.Dre.Abreviacao,
                QuantidadePresencas = dados.QuantidadePresentes,
                QuantidadeRemotos = dados.QuantidadeRemotos,
                QuantidadeAusentes = dados.QuantidadeAusentes
            };
        }
    }
}
