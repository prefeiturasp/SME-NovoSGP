﻿using MediatR;
using SME.SGP.Aplicacao;
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
        private readonly IRepositorioAulaConsulta repositorioAula;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IServicoAbrangencia servicoAbrangencia;
        private readonly IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IMediator mediator;

        public ServicoAtribuicaoCJ(IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ, IServicoAbrangencia servicoAbrangencia, IRepositorioTurma repositorioTurma,
            IRepositorioAbrangencia repositorioAbrangencia, IRepositorioAulaConsulta repositorioAula, IServicoUsuario servicoUsuario, IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular, IMediator mediator)
        {
            this.repositorioAtribuicaoCJ = repositorioAtribuicaoCJ ?? throw new ArgumentNullException(nameof(repositorioAtribuicaoCJ));
            this.servicoAbrangencia = servicoAbrangencia ?? throw new ArgumentNullException(nameof(servicoAbrangencia));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new ArgumentNullException(nameof(repositorioAbrangencia));
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new ArgumentNullException(nameof(repositorioComponenteCurricular));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task Salvar(AtribuicaoCJ atribuicaoCJ,IEnumerable<ProfessorTitularDisciplinaEol> professoresTitularesDisciplinasEol, IEnumerable<AtribuicaoCJ> atribuicoesAtuais = null)
        {
            await ValidaComponentesCurricularesQueNaoPodemSerSubstituidos(atribuicaoCJ);

            if (professoresTitularesDisciplinasEol.NaoEhNulo() && professoresTitularesDisciplinasEol.Any(c => c.ProfessorRf.Contains(atribuicaoCJ.ProfessorRf) && c.DisciplinasId.Contains(atribuicaoCJ.DisciplinaId)))
                throw new NegocioException("Não é possível realizar substituição na turma onde o professor já é o titular.");

            if (atribuicoesAtuais.EhNulo())
                atribuicoesAtuais = await repositorioAtribuicaoCJ.ObterPorFiltros(atribuicaoCJ.Modalidade, atribuicaoCJ.TurmaId,
                    atribuicaoCJ.UeId, 0, atribuicaoCJ.ProfessorRf, string.Empty, null);

            var atribuicaoJaCadastrada = atribuicoesAtuais.FirstOrDefault(a => a.DisciplinaId == atribuicaoCJ.DisciplinaId);

            if (atribuicaoJaCadastrada.EhNulo())
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
            var perfil = atribuicaoCJ.Modalidade == Modalidade.EducacaoInfantil ? Perfis.PERFIL_CJ_INFANTIL : Perfis.PERFIL_CJ;

            var abrangenciasAtuais = await repositorioAbrangencia.ObterAbrangenciaSintetica(atribuicaoCJ.ProfessorRf, perfil, atribuicaoCJ.TurmaId);

            if (atribuicaoCJ.Substituir)
            {
                var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(atribuicaoCJ.TurmaId));
              
                if (abrangenciasAtuais.NaoEhNulo() && !abrangenciasAtuais.Any())
                {
                 
                    if (turma.EhNulo())
                        throw new NegocioException($"Não foi possível localizar a turma {atribuicaoCJ.TurmaId} da abrangência.");
                 
                    var abrangencias = new Abrangencia[] { new Abrangencia() { Perfil = perfil, TurmaId = turma.Id , Historico  = turma.Historica } };

                    await servicoAbrangencia.SalvarAbrangencias(abrangencias, atribuicaoCJ.ProfessorRf);
                }

                if (abrangenciasAtuais.NaoEhNulo())
                {
                    var abrangenciaDaTurma = abrangenciasAtuais.Where(x => x.TurmaId == turma.Id).FirstOrDefault();
                    if (abrangenciaDaTurma.NaoEhNulo() && abrangenciaDaTurma.Historico != turma.Historica)
                    {
                        var abangencia = new long[] { abrangenciaDaTurma.Id };
                        await repositorioAbrangencia.AtualizaAbrangenciaHistorica(abangencia);
                    }

                }


            }
            else if ((abrangenciasAtuais.NaoEhNulo() && abrangenciasAtuais.Any()) &&
                     (!atribuicoesAtuais.Any(a => a.Id != atribuicaoCJ.Id && a.Substituir)))
            {
                await servicoAbrangencia.RemoverAbrangencias(abrangenciasAtuais.Select(a => a.Id).ToArray());

                await repositorioAtribuicaoCJ.RemoverRegistros(atribuicaoCJ.DreId, atribuicaoCJ.UeId, atribuicaoCJ.TurmaId, atribuicaoCJ.ProfessorRf, atribuicaoCJ.DisciplinaId);
            }

           


        }

        private async Task ValidaComponentesCurricularesQueNaoPodemSerSubstituidos(AtribuicaoCJ atribuicaoCJ)
        {
            if (componentesQueNaoPodemSerSubstituidos.Any(a => a == atribuicaoCJ.DisciplinaId))
            {
                var nomeComponenteCurricular = await mediator.Send(new ObterComponenteCurricularPorIdQuery(atribuicaoCJ.DisciplinaId));
                if (nomeComponenteCurricular.NaoEhNulo())
                {
                    throw new NegocioException($"O componente curricular {nomeComponenteCurricular.Nome} não pode ser substituido.");
                }
                else throw new NegocioException($"Não foi possível localizar o nome do componente curricular de identificador {atribuicaoCJ.DisciplinaId} no EOL.");
            }
        }

        private async Task ValidaSePerfilPodeIncluir()
        {
            var usuarioAtual = await servicoUsuario.ObterUsuarioLogado();

            if (usuarioAtual.EhNulo())
                throw new NegocioException("Não foi possível obter o usuário logado.");

            if (usuarioAtual.PerfilAtual == Perfis.PERFIL_CP || usuarioAtual.PerfilAtual == Perfis.PERFIL_DIRETOR)
                throw new NegocioException("Este perfil não pode fazer substituição.");
        }

        private async Task ValidaSeTemAulaCriada(AtribuicaoCJ atribuicaoCJ)
        {
            if (atribuicaoCJ.Id > 0 && !atribuicaoCJ.Substituir)
            {
                var aulas = await repositorioAula.ObterAulas(atribuicaoCJ.TurmaId, atribuicaoCJ.UeId, atribuicaoCJ.ProfessorRf, null, atribuicaoCJ.DisciplinaId.ToString());
                if (aulas.NaoEhNulo() && aulas.Any())
                {
                    var componenteCurricular = await mediator.Send(new ObterComponenteCurricularPorIdQuery(atribuicaoCJ.DisciplinaId)); 
                    var nomeComponenteCurricular = componenteCurricular?.Nome ?? atribuicaoCJ.DisciplinaId.ToString();
                    throw new NegocioException($"Não é possível remover a substituição da turma {atribuicaoCJ.Turma.Nome} no componente curricular {nomeComponenteCurricular} porque existem aulas cadastradas.");
                }
            }
        }
    }
}