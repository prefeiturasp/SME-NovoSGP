using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasNotaConceitoBimestre : IConsultasNotaConceitoBimestre
    {
        private readonly IRepositorioNotaConceitoBimestre repositorioNotaConceitoBimestre;
        public ConsultasNotaConceitoBimestre(IRepositorioNotaConceitoBimestre repositorioNotaConceitoBimestre)
        {
            this.repositorioNotaConceitoBimestre = repositorioNotaConceitoBimestre ?? throw new ArgumentNullException(nameof(repositorioNotaConceitoBimestre));
        }

        public async Task<string> ObterAnotacaoPorAlunoEFechamento(long fechamentoId, string codigoAluno)
            => await repositorioNotaConceitoBimestre.ObterAnotacaoAlunoPorFechamento(fechamentoId, codigoAluno);
    }
}