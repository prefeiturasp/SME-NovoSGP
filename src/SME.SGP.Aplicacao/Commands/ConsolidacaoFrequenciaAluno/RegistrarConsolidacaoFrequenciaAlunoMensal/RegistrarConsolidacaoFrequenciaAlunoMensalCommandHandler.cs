using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RegistrarConsolidacaoFrequenciaAlunoMensalCommandHandler : IRequestHandler<RegistrarConsolidacaoFrequenciaAlunoMensalCommand, long>
    {
        private readonly IRepositorioConsolidacaoFrequenciaAlunoMensal repositorioConsolidacaoFrequenciaAlunoMensal;

        public RegistrarConsolidacaoFrequenciaAlunoMensalCommandHandler(IRepositorioConsolidacaoFrequenciaAlunoMensal repositorioConsolidacaoFrequenciaAlunoMensal)
        {
            this.repositorioConsolidacaoFrequenciaAlunoMensal = repositorioConsolidacaoFrequenciaAlunoMensal ?? throw new ArgumentNullException(nameof(repositorioConsolidacaoFrequenciaAlunoMensal));
        }

        public async Task<long> Handle(RegistrarConsolidacaoFrequenciaAlunoMensalCommand request, CancellationToken cancellationToken)
        {
            return await repositorioConsolidacaoFrequenciaAlunoMensal.Inserir(new ConsolidacaoFrequenciaAlunoMensal()
            {
                TurmaId = request.TurmaId,
                AlunoCodigo = request.AlunoCodigo,
                Mes = request.Mes,
                Percentual = request.Percentual,
                QuantidadeAulas = request.QuantidadeAulas,
                QuantidadeAusencias = request.QuantidadeAusencias,
                QuantidadeCompensacoes = request.QuantidadeCompensacoes
            });
        }
    }
}
