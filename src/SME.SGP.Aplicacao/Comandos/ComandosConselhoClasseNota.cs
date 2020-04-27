using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosConselhoClasseNota : IComandosConselhoClasseNota
    {
        private readonly IServicoConselhoClasse servicoConselhoClasse;

        public ComandosConselhoClasseNota(IServicoConselhoClasse servicoConselhoClasse)
        {
            this.servicoConselhoClasse = servicoConselhoClasse ?? throw new ArgumentNullException(nameof(servicoConselhoClasse));
        }

        public async Task<AuditoriaDto> Salvar(ConselhoClasseNotaDto conselhoClasseNotaDto, string alunoCodigo, long conselhoClasseId, long fechamentoTurmaId)
        {
            return await servicoConselhoClasse.SalvarConselhoClasseAlunoNota(conselhoClasseNotaDto, alunoCodigo, conselhoClasseId, fechamentoTurmaId);
        }
    }
}