using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Servicos;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoAtribuicaoCJ : IServicoAtribuicaoCJ
    {
        private static readonly long[] componentesQueNaoPodemSerSubstituidos = { 1033, 1051, 1052, 1053, 1054, 1030 };
        private readonly IRepositorioAbrangencia repositorioAbrangencia;
        private readonly IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ;
        private readonly IRepositorioAula repositorioAula;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IServicoAbrangencia servicoAbrangencia;
        private readonly IServicoEol servicoEOL;
        private readonly IServicoUsuario servicoUsuario;

        public ServicoAtribuicaoCJ(IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ, IServicoAbrangencia servicoAbrangencia, IRepositorioTurma repositorioTurma,
            IRepositorioAbrangencia repositorioAbrangencia, IServicoEol servicoEOL, IRepositorioAula repositorioAula, IServicoUsuario servicoUsuario)
        {
            this.repositorioAtribuicaoCJ = repositorioAtribuicaoCJ ?? throw new ArgumentNullException(nameof(repositorioAtribuicaoCJ));
            this.servicoAbrangencia = servicoAbrangencia ?? throw new ArgumentNullException(nameof(servicoAbrangencia));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new ArgumentNullException(nameof(repositorioAbrangencia));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
        }

        public async Task Salvar(AtribuicaoCJ atribuicaoCJ,IEnumerable<ProfessorTitularDisciplinaEol> professoresTitularesDisciplinasEol, IEnumerable<AtribuicaoCJ> atribuicoesAtuais = null)
        {
            ValidaComponentesCurricularesQueNaoPodemSerSubstituidos(atribuicaoCJ);

            if (professoresTitularesDisciplinasEol != null && professoresTitularesDisciplinasEol.Any(c => c.ProfessorRf.Contains(atribuicaoCJ.ProfessorRf) && c.DisciplinaId == atribuicaoCJ.DisciplinaId))
                throw new NegocioException("Não é possível realizar substituição na turma onde o professor já é o titular.");

            if (atribuicoesAtuais == null)
                atribuicoesAtuais = await repositorioAtribuicaoCJ.ObterPorFiltros(atribuicaoCJ.Modalidade, atribuicaoCJ.TurmaId,
                    atribuicaoCJ.UeId, 0, atribuicaoCJ.ProfessorRf, string.Empty, null);

            var atribuicaoJaCadastrada = atribuicoesAtuais.FirstOrDefault(a => a.DisciplinaId == atribuicaoCJ.DisciplinaId);

            if (atribuicaoJaCadastrada == null)
            {
                if (!atribuicaoCJ.Substituir)
                    return;
            }
            else
            {
                if (atribuicaoCJ.Substituir == atribuicaoJaCadastrada.Substituir)
                    return;

                atribuicaoJaCadastrada.Substituir = atribuicaoCJ.Substituir;
                atribuicaoCJ = atribuicaoJaCadastrada;

                if (!atribuicaoCJ.Substituir)
                    await ValidaSeTemAulaCriada(atribuicaoCJ);
            }
            await ValidaSePerfilPodeIncluir();
            await repositorioAtribuicaoCJ.SalvarAsync(atribuicaoCJ);
            await TratarAbrangencia(atribuicaoCJ, atribuicoesAtuais);
        }

        private async Task TratarAbrangencia(AtribuicaoCJ atribuicaoCJ, IEnumerable<AtribuicaoCJ> atribuicoesAtuais)
        {
            var perfil = atribuicaoCJ.Modalidade == Modalidade.Infantil ? Perfis.PERFIL_CJ_INFANTIL : Perfis.PERFIL_CJ;

            var abrangenciasAtuais = await repositorioAbrangencia.ObterAbrangenciaSintetica(atribuicaoCJ.ProfessorRf, perfil, atribuicaoCJ.TurmaId);

            if (atribuicaoCJ.Substituir)
            {
                if (abrangenciasAtuais != null && !abrangenciasAtuais.Any())
                {
                    var turma = await repositorioTurma.ObterPorCodigo(atribuicaoCJ.TurmaId);
                    if (turma == null)
                        throw new NegocioException($"Não foi possível localizar a turma {atribuicaoCJ.TurmaId} da abrangência.");

                    var abrangencias = new Abrangencia[] { new Abrangencia() { Perfil = perfil, TurmaId = turma.Id } };

                    servicoAbrangencia.SalvarAbrangencias(abrangencias, atribuicaoCJ.ProfessorRf);
                }
            }
            else if ((abrangenciasAtuais != null && abrangenciasAtuais.Any()) &&
                     (!atribuicoesAtuais.Any(a => a.Id != atribuicaoCJ.Id && a.Substituir)))
            {
                servicoAbrangencia.RemoverAbrangencias(abrangenciasAtuais.Select(a => a.Id).ToArray());
            }
        }

        private void ValidaComponentesCurricularesQueNaoPodemSerSubstituidos(AtribuicaoCJ atribuicaoCJ)
        {
            if (componentesQueNaoPodemSerSubstituidos.Any(a => a == atribuicaoCJ.DisciplinaId))
            {
                var nomeComponenteCurricular = servicoEOL.ObterDisciplinasPorIds(new long[] { atribuicaoCJ.DisciplinaId });
                if (nomeComponenteCurricular != null && nomeComponenteCurricular.Any())
                {
                    throw new NegocioException($"O componente curricular {nomeComponenteCurricular.FirstOrDefault().Nome} não pode ser substituido.");
                }
                else throw new NegocioException($"Não foi possível localizar o nome do componente curricular de identificador {atribuicaoCJ.DisciplinaId} no EOL.");
            }
        }

        private async Task ValidaSePerfilPodeIncluir()
        {
            var usuarioAtual = await servicoUsuario.ObterUsuarioLogado();

            if (usuarioAtual == null)
                throw new NegocioException("Não foi possível obter o usuário logado.");

            if (usuarioAtual.PerfilAtual == Perfis.PERFIL_CP || usuarioAtual.PerfilAtual == Perfis.PERFIL_DIRETOR)
                throw new NegocioException("Este perfil não pode fazer substituição.");
        }

        private async Task ValidaSeTemAulaCriada(AtribuicaoCJ atribuicaoCJ)
        {
            if (atribuicaoCJ.Id > 0 && !atribuicaoCJ.Substituir)
            {
                var aulas = await repositorioAula.ObterAulas(atribuicaoCJ.TurmaId, atribuicaoCJ.UeId, atribuicaoCJ.ProfessorRf, null, atribuicaoCJ.DisciplinaId.ToString());
                if (aulas != null && aulas.Any())
                    throw new NegocioException($"Não é possível tirar a substituição da turma {atribuicaoCJ.TurmaId} para o componente curricular {atribuicaoCJ.DisciplinaId}");
            }
        }
    }
}