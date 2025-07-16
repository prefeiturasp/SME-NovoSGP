using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Aplicacao.Queries;

namespace SME.SGP.Aplicacao
{
    public class ComandosFechamentoTurmaDisciplina : IComandosFechamentoTurmaDisciplina
    {
        private readonly IServicoFechamentoTurmaDisciplina servicoFechamentoTurmaDisciplina;
        private readonly IRepositorioFechamentoTurmaDisciplinaConsulta repositorioFechamentoTurmaDisciplina;
        private readonly IRepositorioFechamentoTurmaConsulta repositorioFechamentoTurma;
        private readonly IMediator mediator;

        public ComandosFechamentoTurmaDisciplina(IServicoFechamentoTurmaDisciplina servicoFechamentoTurmaDisciplina,                                                 
            IRepositorioFechamentoTurmaConsulta repositorioFechamentoTurma,
            IRepositorioFechamentoTurmaDisciplinaConsulta repositorioFechamentoTurmaDisciplina,                                                 
            IMediator mediator)
        {
            this.servicoFechamentoTurmaDisciplina = servicoFechamentoTurmaDisciplina ?? throw new ArgumentNullException(nameof(servicoFechamentoTurmaDisciplina));
            this.repositorioFechamentoTurmaDisciplina = repositorioFechamentoTurmaDisciplina ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurmaDisciplina));
            this.repositorioFechamentoTurma = repositorioFechamentoTurma ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurma));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task Reprocessar(long fechamentoId, Usuario usuario = null)
            => await servicoFechamentoTurmaDisciplina.Reprocessar(fechamentoId, usuario);

        public async Task Reprocessar(IEnumerable<long> fechamentoId, Usuario usuario = null)
        {
            foreach (long id in fechamentoId)
                await Reprocessar(id, usuario);
        }

        public async Task<IEnumerable<AuditoriaPersistenciaDto>> Salvar(IEnumerable<FechamentoTurmaDisciplinaDto> fechamentosTurma, bool componenteSemNota = false, bool processamento = false)
        {
            var listaAuditoria = new List<AuditoriaPersistenciaDto>();

            if (fechamentosTurma == null)
                fechamentosTurma = new List<FechamentoTurmaDisciplinaDto>();

            foreach (var fechamentoTurma in fechamentosTurma)
            {
                try
                {
                    if ((fechamentoTurma?.Justificativa).NaoEhNulo())
                    {
                        var tamanhoJustificativa = await mediator.Send(new ObterTamanhoCaracteresJustificativaNotaQuery(fechamentoTurma?.Justificativa));
                        var limite = int.Parse(FechamentoTurmaDisciplinaEnum.TamanhoCampoJustificativa.Description());
                        
                        if (tamanhoJustificativa > limite)
                            throw new NegocioException("Justificativa não pode ter mais que " + limite + " caracteres");
                    }
                    
                    listaAuditoria.Add(await servicoFechamentoTurmaDisciplina.Salvar(fechamentoTurma?.Id ?? 0, fechamentoTurma, componenteSemNota, processamento));
                    await RemoverCacheFechamento(fechamentoTurma);
                }
                catch (Exception e)
                {
                    listaAuditoria.Add(new AuditoriaPersistenciaDto() { Sucesso = false, MensagemConsistencia = $"{fechamentoTurma?.Bimestre}º Bimestre: {e.Message}" });
                }
            }

            if (!listaAuditoria.Any(a => a.Sucesso))
                throw new NegocioException("Erro ao salvar o fechamento da turma: " + string.Join(", ", listaAuditoria.Select(s => s.MensagemConsistencia)));

            return listaAuditoria;
        }

        public async Task ProcessarPendentes(int anoLetivo)
        {
            var fechamentosPendentes = await repositorioFechamentoTurmaDisciplina.ObterFechamentosComSituacaoEmProcessamentoPorAnoLetivo(anoLetivo);

            foreach (var fechamento in fechamentosPendentes)
            {
                var fechamentoTurma = await repositorioFechamentoTurma.ObterPorIdAsync(fechamento.FechamentoTurmaId);

                if (!fechamentoTurma.PeriodoEscolarId.HasValue) 
                    continue;
                
                var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(fechamentoTurma.TurmaId));                    
                var periodoEscolar = await mediator.Send(new ObterPeriodoEscolarePorIdQuery(fechamentoTurma.PeriodoEscolarId.Value));
                var usuario = await mediator.Send(new ObterUsuarioPorRfQuery(fechamento.AlteradoEm.HasValue ? fechamento.AlteradoRF : fechamento.CriadoRF));
            }
        }
        
        private async Task RemoverCache(string nomeChave)
        {
            await mediator.Send(new RemoverChaveCacheCommand(nomeChave));
        }        

        private async Task RemoverCacheFechamento(FechamentoTurmaDisciplinaDto fechamentoTurma)
        {
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(fechamentoTurma.TurmaId));
            
            var periodoEscolarId = await mediator.Send(new ObterPeriodoEscolarIdPorTurmaBimestreAnoLetivoQuery(fechamentoTurma.TurmaId,
                fechamentoTurma.Bimestre, turma.AnoLetivo));
            
            await RemoverCache(string.Format(NomeChaveCache.FECHAMENTO_NOTA_TURMA_PERIODO_COMPONENTE, turma.Id, periodoEscolarId, fechamentoTurma.DisciplinaId));
            await RemoverCache(string.Format(NomeChaveCache.FECHAMENTO_NOTA_TURMA_BIMESTRE, turma.CodigoTurma, fechamentoTurma.Bimestre));
        }
    }
}
