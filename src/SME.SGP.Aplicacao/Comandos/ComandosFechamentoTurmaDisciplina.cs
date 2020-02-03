using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
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

        public async Task<AuditoriaDto> Alterar(long id, FechamentoTurmaDisciplinaDto fechamentoTurma)
            => await servicoFechamentoTurmaDisciplina.Salvar(id, fechamentoTurma);

        public async Task<AuditoriaDto> Inserir(FechamentoTurmaDisciplinaDto fechamentoTurma)
            => await servicoFechamentoTurmaDisciplina.Salvar(0, fechamentoTurma);
    }
}
