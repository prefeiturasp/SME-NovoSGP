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
        private readonly IMediator mediator;

        public ComandosFechamentoTurmaDisciplina(IServicoFechamentoTurmaDisciplina servicoFechamentoTurmaDisciplina, IMediator mediator)
        {
            this.servicoFechamentoTurmaDisciplina = servicoFechamentoTurmaDisciplina ?? throw new ArgumentNullException(nameof(servicoFechamentoTurmaDisciplina));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task Reprocessar(long fechamentoId)
            => await servicoFechamentoTurmaDisciplina.Reprocessar(fechamentoId);

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
    }
}
