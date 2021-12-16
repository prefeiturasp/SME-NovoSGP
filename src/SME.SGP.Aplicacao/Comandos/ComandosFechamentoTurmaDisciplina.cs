using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosFechamentoTurmaDisciplina : IComandosFechamentoTurmaDisciplina
    {
        private readonly IServicoFechamentoTurmaDisciplina servicoFechamentoTurmaDisciplina;
        private readonly IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina;
        private readonly IRepositorioFechamentoTurmaConsulta repositorioFechamentoTurma;
        private readonly IMediator mediator;

        public ComandosFechamentoTurmaDisciplina(IServicoFechamentoTurmaDisciplina servicoFechamentoTurmaDisciplina,
                                                 IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina,
                                                 IRepositorioFechamentoTurmaConsulta repositorioFechamentoTurma,
                                                 IMediator mediator)
        {
            this.servicoFechamentoTurmaDisciplina = servicoFechamentoTurmaDisciplina ?? throw new ArgumentNullException(nameof(servicoFechamentoTurmaDisciplina));
            this.repositorioFechamentoTurmaDisciplina = repositorioFechamentoTurmaDisciplina ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurmaDisciplina));
            this.repositorioFechamentoTurma = repositorioFechamentoTurma ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurma));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task Reprocessar(long fechamentoId, Usuario usuario = null)
            => await servicoFechamentoTurmaDisciplina.Reprocessar(fechamentoId, usuario);

        public void Reprocessar(IEnumerable<long> fechamentoId, Usuario usuario = null)
        {
            fechamentoId.ToList().ForEach(f => Reprocessar(f, usuario).Wait());
        }

        public async Task<IEnumerable<AuditoriaPersistenciaDto>> Salvar(IEnumerable<FechamentoTurmaDisciplinaDto> fechamentosTurma, bool componenteSemNota = false)
        {
            var listaAuditoria = new List<AuditoriaPersistenciaDto>();
            foreach (var fechamentoTurma in fechamentosTurma)
            {
                try
                {
                    if (fechamentoTurma?.Justificativa != null)
                    {
                        int tamanhoJustificativa = fechamentoTurma.Justificativa.Length;
                        int limite = int.Parse(FechamentoTurmaDisciplinaEnum.TamanhoCampoJustificativa.Description());
                        if (tamanhoJustificativa > limite)
                            throw new NegocioException("Justificativa não pode ter mais que " + limite.ToString() + " caracteres");
                    }
                    listaAuditoria.Add(await servicoFechamentoTurmaDisciplina.Salvar(fechamentoTurma.Id, fechamentoTurma, componenteSemNota));
                }
                catch (Exception e)
                {
                    listaAuditoria.Add(new AuditoriaPersistenciaDto() { Sucesso = false, MensagemConsistencia = $"{fechamentoTurma.Bimestre}º Bimestre: {e.Message}" });
                }
            }

            if (!listaAuditoria.Any(a => a.Sucesso))
                throw new NegocioException("Erro ao salvar o fechamento da turma: " + string.Join(", ", listaAuditoria.Select(s => s.MensagemConsistencia)));

            return listaAuditoria;
        }

        public async Task ProcessarPendentes(int anoLetivo)
        {
            var fechamentosPendentes = await repositorioFechamentoTurmaDisciplina
                .ObterFechamentosComSituacaoEmProcessamentoPorAnoLetivo(anoLetivo);

            foreach (var fechamento in fechamentosPendentes)
            {
                var fechamentoTurma = await repositorioFechamentoTurma.ObterPorIdAsync(fechamento.FechamentoTurmaId);

                if (fechamentoTurma.PeriodoEscolarId.HasValue)
                {
                    var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(fechamentoTurma.TurmaId));                    
                    var periodoEscolar = await mediator.Send(new ObterPeriodoEscolarePorIdQuery(fechamentoTurma.PeriodoEscolarId.Value));
                    var usuario = await mediator.Send(new ObterUsuarioPorRfQuery(fechamento.AlteradoEm.HasValue ? fechamento.AlteradoRF : fechamento.CriadoRF));
                    // TODO trocara para a rotina no rabbit
                    //await servicoFechamentoTurmaDisciplina.GerarPendenciasFechamento(fechamento.DisciplinaId, turma, periodoEscolar, fechamento, usuario);
                }                
            }
        }

       
    }
}
