using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Integracoes
{
    public interface IServicoEOL
    {
        Task<UsuarioEolAutenticacaoRetornoDto> Autenticar(string login, string senha);

        Task<IEnumerable<DisciplinaResposta>> ObterDisciplinasPorProfessorETurma(long codigoTurma, string rfProfessor);

        IEnumerable<DreRespostaEolDto> ObterDres();

        IEnumerable<EscolasRetornoDto> ObterEscolasPorCodigo(string[] codigoUes);

        IEnumerable<EscolasRetornoDto> ObterEscolasPorDre(string dreId);

        IEnumerable<UsuarioEolRetornoDto> ObterFuncionariosPorCargoUe(string UeId, long cargoId);

        IEnumerable<ProfessorTurmaReposta> ObterListaTurmasPorProfessor(string codigoRf);

        IEnumerable<SupervisoresRetornoDto> ObterSupervisoresPorCodigo(string[] codigoUes);

        IEnumerable<SupervisoresRetornoDto> ObterSupervisoresPorDre(string dreId);

        Task<IEnumerable<TurmaDto>> ObterTurmasAtribuidasAoProfessorPorEscolaEAnoLetivo(string rfProfessor, string codigoEscola, int anoLetivo);
    }
}