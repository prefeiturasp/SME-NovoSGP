using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosProcessoExecutando: IComandosProcessoExecutando
    {
        private readonly IRepositorioProcessoExecutando repositorio;

        public ComandosProcessoExecutando(IRepositorioProcessoExecutando repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task Incluir(ProcessoExecutando processo)
            => await repositorio.SalvarAsync(processo);

        public void Excluir(ProcessoExecutando processo)
            => repositorio.Remover(processo);

        public async Task IncluirCalculoFrequencia(string turmaId, string disciplinaId, int bimestre)
            => await Incluir(new ProcessoExecutando()
            {
                TipoProcesso = TipoProcesso.CalculoFrequencia,
                TurmaId = turmaId,
                DisciplinaId = disciplinaId,
                Bimestre = bimestre
            });

        public async Task ExcluirCalculoFrequencia(string turmaId, string disciplinaId, int bimestre)
        {
            var processo = await repositorio.ObterProcessoCalculoFrequenciaAsync(turmaId, disciplinaId, bimestre, TipoProcesso.CalculoFrequencia);
            if (processo != null)
                Excluir(processo);
        }
    }
}
