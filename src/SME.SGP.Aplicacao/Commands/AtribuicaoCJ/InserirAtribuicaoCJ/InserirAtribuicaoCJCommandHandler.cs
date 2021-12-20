using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class InserirAtribuicaoCJCommandHandler : IRequestHandler<InserirAtribuicaoCJCommand>
    {
        private static readonly long[] componentesQueNaoPodemSerSubstituidos = { 1033, 1051, 1052, 1053, 1054, 1030 };

        private readonly IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ;
        private readonly IRepositorioAbrangencia repositorioAbrangencia;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular;
        private readonly IRepositorioAulaConsulta repositorioAula;
        private readonly IMediator mediator;

        public InserirAtribuicaoCJCommandHandler(IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ, IRepositorioAbrangencia repositorioAbrangencia,
                                                 IRepositorioTurma repositorioTurma, IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular,
                                                 IRepositorioAulaConsulta repositorioAula, IMediator mediator)
        {
            this.repositorioAtribuicaoCJ = repositorioAtribuicaoCJ ?? throw new ArgumentNullException(nameof(repositorioAtribuicaoCJ));
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new ArgumentNullException(nameof(repositorioAbrangencia));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new ArgumentNullException(nameof(repositorioComponenteCurricular));
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<Unit> Handle(InserirAtribuicaoCJCommand request, CancellationToken cancellationToken)
        {
            var atribuicaoCJ = request.AtribuicaoCJ;
            var atribuicoesAtuais = request.AtribuicoesAtuais;

            await ValidaComponentesCurricularesQueNaoPodemSerSubstituidos(atribuicaoCJ);

            if (request.ProfessoresTitulares != null && request.ProfessoresTitulares.Any(c => c.ProfessorRf.Contains(atribuicaoCJ.ProfessorRf) && c.DisciplinaId == atribuicaoCJ.DisciplinaId))
                throw new NegocioException("Não é possível realizar substituição na turma onde o professor já é o titular.");

            if (request.AtribuicoesAtuais == null)
                request.AtribuicoesAtuais = await repositorioAtribuicaoCJ.ObterPorFiltros(atribuicaoCJ.Modalidade, atribuicaoCJ.TurmaId,
                    atribuicaoCJ.UeId, 0, atribuicaoCJ.ProfessorRf, string.Empty, null);

            var atribuicaoJaCadastrada = atribuicoesAtuais.FirstOrDefault(a => a.DisciplinaId == atribuicaoCJ.DisciplinaId);

            if (atribuicaoJaCadastrada == null)
            {
                if (!atribuicaoCJ.Substituir)
                    return Unit.Value;
            }
            else
            {
                if (atribuicaoCJ.Substituir == atribuicaoJaCadastrada.Substituir)
                    return Unit.Value;

                atribuicaoJaCadastrada.Substituir = atribuicaoCJ.Substituir;
                atribuicaoCJ = atribuicaoJaCadastrada;

                if (!atribuicaoCJ.Substituir)
                    await ValidaSeTemAulaCriada(atribuicaoCJ);
            }

            ValidaSePerfilPodeIncluir(request.Usuario);

            await repositorioAtribuicaoCJ.SalvarAsync(atribuicaoCJ);
            await TratarAbrangencia(atribuicaoCJ, atribuicoesAtuais, request.EhHistorico);

            return Unit.Value;
        }

        private async Task TratarAbrangencia(AtribuicaoCJ atribuicaoCJ, IEnumerable<AtribuicaoCJ> atribuicoesAtuais, bool ehHistorico)
        {
            var perfil = atribuicaoCJ.Modalidade == Modalidade.EducacaoInfantil ? Perfis.PERFIL_CJ_INFANTIL : Perfis.PERFIL_CJ;

            var abrangenciasAtuais = await repositorioAbrangencia.ObterAbrangenciaSintetica(atribuicaoCJ.ProfessorRf, perfil, atribuicaoCJ.TurmaId, ehHistorico);

            if (atribuicaoCJ.Substituir)
            {
                if (abrangenciasAtuais != null && !abrangenciasAtuais.Any())
                {
                    var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(atribuicaoCJ.TurmaId));
                    if (turma == null)
                        throw new NegocioException($"Não foi possível localizar a turma {atribuicaoCJ.TurmaId} da abrangência.");

                    var abrangencias = new Abrangencia[] { new Abrangencia() { Perfil = perfil, TurmaId = turma.Id, Historico = turma.Historica } };

                    repositorioAbrangencia.InserirAbrangencias(abrangencias, atribuicaoCJ.ProfessorRf);
                }
            }
            else if ((abrangenciasAtuais != null && abrangenciasAtuais.Any()) &&
                     (!atribuicoesAtuais.Any(a => a.Id != atribuicaoCJ.Id && a.Substituir)))
            {
                if (ehHistorico)
                    repositorioAbrangencia.ExcluirAbrangenciasHistoricas(abrangenciasAtuais.Select(a => a.Id).ToArray());
                else
                    repositorioAbrangencia.ExcluirAbrangencias(abrangenciasAtuais.Select(a => a.Id).ToArray());

                await repositorioAtribuicaoCJ.RemoverRegistros(atribuicaoCJ.DreId, atribuicaoCJ.UeId, atribuicaoCJ.TurmaId, atribuicaoCJ.ProfessorRf, atribuicaoCJ.DisciplinaId);

            }
        }

        private async Task ValidaComponentesCurricularesQueNaoPodemSerSubstituidos(AtribuicaoCJ atribuicaoCJ)
        {
            if (componentesQueNaoPodemSerSubstituidos.Any(a => a == atribuicaoCJ.DisciplinaId))
            {
                var nomeComponenteCurricular = await repositorioComponenteCurricular.ObterDisciplinasPorIds(new long[] { atribuicaoCJ.DisciplinaId });
                if (nomeComponenteCurricular != null && nomeComponenteCurricular.Any())
                {
                    throw new NegocioException($"O componente curricular {nomeComponenteCurricular.FirstOrDefault().Nome} não pode ser substituido.");
                }
                else throw new NegocioException($"Não foi possível localizar o nome do componente curricular de identificador {atribuicaoCJ.DisciplinaId} no EOL.");
            }
        }

        private void ValidaSePerfilPodeIncluir(Usuario usuario)
        {
            if (usuario == null)
                throw new NegocioException("Não foi possível obter o usuário logado.");

            if (usuario.PerfilAtual == Perfis.PERFIL_CP || usuario.PerfilAtual == Perfis.PERFIL_DIRETOR)
                throw new NegocioException("Este perfil não pode fazer substituição.");
        }

        private async Task ValidaSeTemAulaCriada(AtribuicaoCJ atribuicaoCJ)
        {
            if (atribuicaoCJ.Id > 0 && !atribuicaoCJ.Substituir)
            {
                var aulas = await repositorioAula.ObterAulas(atribuicaoCJ.TurmaId, atribuicaoCJ.UeId, atribuicaoCJ.ProfessorRf, null, atribuicaoCJ.DisciplinaId.ToString());
                if (aulas != null && aulas.Any())
                {
                    var componenteCurricular = await repositorioComponenteCurricular.ObterDisciplinasPorIds(new long[] { atribuicaoCJ.DisciplinaId });
                    var nomeComponenteCurricular = componenteCurricular?.FirstOrDefault()?.Nome ?? atribuicaoCJ.DisciplinaId.ToString();
                    throw new NegocioException($"Não é possível remover a substituição da turma {atribuicaoCJ.Turma.Nome} no componente curricular {nomeComponenteCurricular} porque existem aulas cadastradas.");
                }
            }
        }
    }
}
