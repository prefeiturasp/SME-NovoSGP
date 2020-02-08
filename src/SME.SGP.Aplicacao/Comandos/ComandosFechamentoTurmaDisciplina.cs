using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosFechamentoTurmaDisciplina: IComandosFechamentoTurmaDisciplina
    {
        private readonly IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina;
        private readonly IServicoFechamentoTurmaDisciplina servicoFechamentoTurmaDisciplina;

        public ComandosFechamentoTurmaDisciplina(IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina,
                                                IServicoFechamentoTurmaDisciplina servicoFechamentoTurmaDisciplina)
        {
            this.repositorioFechamentoTurmaDisciplina = repositorioFechamentoTurmaDisciplina ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurmaDisciplina));
            this.servicoFechamentoTurmaDisciplina = servicoFechamentoTurmaDisciplina ?? throw new ArgumentNullException(nameof(servicoFechamentoTurmaDisciplina));
        }

        public async Task<IEnumerable<AuditoriaDto>> Alterar(IEnumerable<FechamentoTurmaDisciplinaDto> fechamentosTurma)
        {
            var listaAuditoria = new List<AuditoriaDto>();
            foreach(var fechamentoTurma in fechamentosTurma)
            {
                listaAuditoria.Add(await servicoFechamentoTurmaDisciplina.Salvar(fechamentoTurma.Id, fechamentoTurma));
            }

            return listaAuditoria;
        }

        public async Task<IEnumerable<AuditoriaDto>> Inserir(IEnumerable<FechamentoTurmaDisciplinaDto> fechamentosTurma)
        {
            var listaAuditoria = new List<AuditoriaDto>();
            foreach (var fechamentoTurma in fechamentosTurma)
            {
                listaAuditoria.Add(await servicoFechamentoTurmaDisciplina.Salvar(0, fechamentoTurma));
            }

            return listaAuditoria;
        }
    }
}
