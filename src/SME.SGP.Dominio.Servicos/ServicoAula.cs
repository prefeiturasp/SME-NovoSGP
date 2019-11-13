using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoAula : IServicoAula
    {
        private readonly IRepositorioAula repositorioAula;
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;
        private readonly IServicoEOL servicoEOL;

        public ServicoAula(IRepositorioAula repositorioAula,
                           IServicoEOL servicoEOL,
                           IRepositorioTipoCalendario repositorioTipoCalendario)
        {
            this.repositorioAula = repositorioAula ?? throw new System.ArgumentNullException(nameof(repositorioAula));
            this.servicoEOL = servicoEOL ?? throw new System.ArgumentNullException(nameof(servicoEOL));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new System.ArgumentNullException(nameof(repositorioTipoCalendario));
        }

        public async Task Salvar(Aula aula, Usuario usuario)
        {
            var disciplinasProfessor = await servicoEOL.ObterDisciplinasPorCodigoTurmaLoginEPerfil(aula.TurmaId, usuario.Login, usuario.ObterPerfilPrioritario());
            if (!disciplinasProfessor.Any(c => c.CodigoComponenteCurricular.ToString() == aula.DisciplinaId) ||
                !repositorioAula.UsuarioPodeCriarAulaNaUeETurma(aula))
            {
                throw new NegocioException("Você não pode criar aulas para essa UE/Turma/Disciplina.");
            }
            var tipoCalendario = repositorioTipoCalendario.ObterPorId(aula.TipoCalendarioId);
            if (tipoCalendario == null)
                throw new NegocioException("O tipo de calendário não foi encontrado.");

            repositorioAula.Salvar(aula);
        }
    }
}