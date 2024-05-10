using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class VerificarFechamentoTurmaComponenteEmAndamentoQueryHandler : IRequestHandler<VerificarFechamentoTurmaComponenteEmAndamentoQuery, bool>
    {
        private readonly IRepositorioFechamentoTurmaDisciplinaConsulta repositorioFechamentoTurmaDisciplina;

        public VerificarFechamentoTurmaComponenteEmAndamentoQueryHandler(IRepositorioFechamentoTurmaDisciplinaConsulta repositorioFechamentoTurmaDisciplina)
        {
            this.repositorioFechamentoTurmaDisciplina = repositorioFechamentoTurmaDisciplina ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurmaDisciplina));
        }

        public Task<bool> Handle(VerificarFechamentoTurmaComponenteEmAndamentoQuery request, CancellationToken cancellationToken)
        {
            var situacoesFechamento = new SituacaoFechamento[] { SituacaoFechamento.EmProcessamento, SituacaoFechamento.ProcessadoComErro, SituacaoFechamento.ProcessadoComSucesso, SituacaoFechamento.ProcessadoComPendencias };
            return repositorioFechamentoTurmaDisciplina.VerificaExistenciaFechamentoTurmaDisciplinPorTurmaDisciplinaBimestreSituacao(request.TurmaId, request.ComponenteCurricularId, request.PeriodoEscolarId, situacoesFechamento);
        }
    }
}
