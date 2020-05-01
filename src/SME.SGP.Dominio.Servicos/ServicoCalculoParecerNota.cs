using SME.SGP.Aplicacao;
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
    [DisplayName("Nota")]
    public class ServicoCalculoParecerNota : ServicoCalculoParecerConclusivo, IServicoCalculoParecerNota
    {
        private readonly IRepositorioFechamentoNota repositorioFechamentoNota;
        private readonly IRepositorioParametrosSistema repositorioParametrosSistema;
        private readonly IRepositorioConceito repositorioConceito;

        public ServicoCalculoParecerNota(IRepositorioFechamentoNota repositorioFechamentoNota,
                                         IRepositorioParametrosSistema repositorioParametrosSistema,
                                         IRepositorioConceito repositorioConceito)
        {
            this.repositorioFechamentoNota = repositorioFechamentoNota ?? throw new ArgumentNullException(nameof(repositorioFechamentoNota));
            this.repositorioParametrosSistema = repositorioParametrosSistema ?? throw new ArgumentNullException(nameof(repositorioParametrosSistema));
            this.repositorioConceito = repositorioConceito ?? throw new ArgumentNullException(nameof(repositorioConceito));

            quandoFalso = Activator.CreateInstance<IServicoCalculoParecerConselho>();
        }

        protected override IEnumerable<ConselhoClasseParecerConclusivo> FiltrarPareceresDoServico(IEnumerable<ConselhoClasseParecerConclusivo> pareceresDaTurma)
            => pareceresDaTurma.Where(c => c.Nota);

        protected override async Task<bool> ValidarParecer(string alunoCodigo, string turmaCodigo)
        {
            var notasFechamentoAluno = await repositorioFechamentoNota.ObterNotasFinaisAlunoAsync(turmaCodigo, alunoCodigo);
            if (notasFechamentoAluno == null || !notasFechamentoAluno.Any())
                return true;

            var tipoNota = notasFechamentoAluno.First().ConceitoId.HasValue ? TipoNota.Conceito : TipoNota.Nota;
            return tipoNota == TipoNota.Nota ?
                ValidarParecerPorNota(notasFechamentoAluno) :
                ValidarParecerPorConceito(notasFechamentoAluno);
        }

        private bool ValidarParecerPorNota(IEnumerable<NotaConceitoBimestreComponenteDto> notasFechamentoAluno)
        {
            var notaMedia = double.Parse(repositorioParametrosSistema.ObterValorPorTipoEAno(TipoParametroSistema.MediaBimestre));
            foreach (var notaFechamentoAluno in notasFechamentoAluno)
                if (notaFechamentoAluno.Nota < notaMedia)
                    return false;

            return true;
        }

        private bool ValidarParecerPorConceito(IEnumerable<NotaConceitoBimestreComponenteDto> conceitosFechamentoAluno)
        {
            var conceitosVigentes = repositorioConceito.ObterPorData(DateTime.Today);
            foreach (var conceitoFechamentoAluno in conceitosFechamentoAluno)
            {
                var conceitoAluno = conceitosVigentes.FirstOrDefault(c => c.Id == conceitoFechamentoAluno.ConceitoId);
                if (!conceitoAluno.Aprovado)
                    return false;
            }

            return true;
        }
    }
}
