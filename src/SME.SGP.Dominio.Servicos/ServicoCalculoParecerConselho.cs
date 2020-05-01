using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Servicos
{
    [DisplayName("Conselho de Classe")]
    public class ServicoCalculoParecerConselho : ServicoCalculoParecerConclusivo, IServicoCalculoParecerConselho
    {
        private readonly IRepositorioConselhoClasseNota repositorioConselhoClasseNota;
        private readonly IRepositorioParametrosSistema repositorioParametrosSistema;
        private readonly IRepositorioConceito repositorioConceito;

        public ServicoCalculoParecerConselho(IRepositorioConselhoClasseNota repositorioConselhoClasseNota,
                                             IRepositorioParametrosSistema repositorioParametrosSistema,
                                             IRepositorioConceito repositorioConceito)
        {
            this.repositorioConselhoClasseNota = repositorioConselhoClasseNota ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseNota));
            this.repositorioParametrosSistema = repositorioParametrosSistema ?? throw new ArgumentNullException(nameof(repositorioParametrosSistema));
            this.repositorioConceito = repositorioConceito ?? throw new ArgumentNullException(nameof(repositorioConceito));
        }

        protected override IEnumerable<ConselhoClasseParecerConclusivo> FiltrarPareceresDoServico(IEnumerable<ConselhoClasseParecerConclusivo> pareceresDaTurma)
            => pareceresDaTurma.Where(c => c.Conselho);

        protected override async Task<bool> ValidarParecer(string alunoCodigo, string turmaCodigo)
        {
            var notasConselhoClasse = await repositorioConselhoClasseNota.ObterNotasFinaisAlunoAsync(alunoCodigo, turmaCodigo);
            if (notasConselhoClasse == null || !notasConselhoClasse.Any())
                return true;

            var tipoNota = notasConselhoClasse.First().ConceitoId.HasValue ? TipoNota.Conceito : TipoNota.Nota;
            return tipoNota == TipoNota.Nota ?
                ValidarParecerPorNota(notasConselhoClasse) :
                ValidarParecerPorConceito(notasConselhoClasse);

        }

        private bool ValidarParecerPorNota(IEnumerable<NotaConceitoBimestreComponenteDto> notasConselhoClasse)
        {
            var notaMedia = double.Parse(repositorioParametrosSistema.ObterValorPorTipoEAno(TipoParametroSistema.MediaBimestre));
            foreach (var notaConcelhoClasse in notasConselhoClasse)
                if (notaConcelhoClasse.Nota < notaMedia)
                    return false;

            return true;
        }

        private bool ValidarParecerPorConceito(IEnumerable<NotaConceitoBimestreComponenteDto> notasConselhoClasse)
        {
            var conceitosVigentes = repositorioConceito.ObterPorData(DateTime.Today);
            foreach (var conceitoConselhoClasseAluno in notasConselhoClasse)
            {
                var conceitoAluno = conceitosVigentes.FirstOrDefault(c => c.Id == conceitoConselhoClasseAluno.ConceitoId);
                if (!conceitoAluno.Aprovado)
                    return false;
            }

            return true;
        }
    }
}
