using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificarCompensacaoAusenciaUseCase : AbstractUseCase, INotificarCompensacaoAusenciaUseCase
    {
        private readonly IRepositorioCompensacaoAusenciaAlunoConsulta repositorioCompensacaoAusenciaAluno;
        private readonly IRepositorioCompensacaoAusencia repositorioCompensacaoAusencia;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IRepositorioNotificacaoCompensacaoAusencia repositorioNotificacaoCompensacaoAusencia;
        private readonly IRepositorioComponenteCurricular repositorioComponenteCurricular;
        private readonly IServicoEol servicoEOL;

        public NotificarCompensacaoAusenciaUseCase(IMediator mediator,
                                                   IRepositorioCompensacaoAusenciaAlunoConsulta repositorioCompensacaoAusenciaAluno,
                                                   IRepositorioCompensacaoAusencia repositorioCompensacaoAusencia,
                                                   IRepositorioTurma repositorioTurma,
                                                   IRepositorioNotificacaoCompensacaoAusencia repositorioNotificacaoCompensacaoAusencia,
                                                   IRepositorioComponenteCurricular repositorioComponenteCurricular,
                                                   IServicoEol servicoEOL) : base(mediator)
        {
            this.repositorioCompensacaoAusenciaAluno = repositorioCompensacaoAusenciaAluno ?? throw new ArgumentNullException(nameof(repositorioCompensacaoAusenciaAluno));
            this.repositorioCompensacaoAusencia = repositorioCompensacaoAusencia ?? throw new ArgumentNullException(nameof(repositorioCompensacaoAusencia));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.repositorioNotificacaoCompensacaoAusencia = repositorioNotificacaoCompensacaoAusencia ?? throw new ArgumentNullException(nameof(repositorioNotificacaoCompensacaoAusencia));
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new ArgumentNullException(nameof(repositorioComponenteCurricular));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtro = mensagem.ObterObjetoMensagem<FiltroNotificacaoCompensacaoAusenciaDto>();
            var compensacaoId = filtro.CompensacaoId;

            // Verifica se compensação possui alunos vinculados
            var alunos = await repositorioCompensacaoAusenciaAluno.ObterPorCompensacao(compensacaoId);
            if (alunos == null || !alunos.Any())
                return true;

            // Verifica se possui aluno não notificado na compensação
            if (!alunos.Any(a => !a.Notificado && a.QuantidadeFaltasCompensadas > 0))
                return true;

            // Carrega dados da compensacao a notificar
            var compensacao = repositorioCompensacaoAusencia.ObterPorId(compensacaoId);

            var turma = await repositorioTurma.ObterTurmaComUeEDrePorId(compensacao.TurmaId);

            var disciplinaEOL = await ObterNomeDisciplina(compensacao.DisciplinaId);

            MeusDadosDto professor = await servicoEOL.ObterMeusDados(compensacao.CriadoRF);

            var possuirPeriodoAberto = await mediator.Send(new TurmaEmPeriodoAbertoQuery(turma, DateTimeExtension.HorarioBrasilia(), compensacao.Bimestre, true));
            var parametroAtivo = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.PermiteCompensacaoForaPeriodo, turma.AnoLetivo));

            // Carrega dados dos alunos não notificados
            var alunosTurma = await servicoEOL.ObterAlunosPorTurma(turma.CodigoTurma);
            var alunosDto = new List<CompensacaoAusenciaAlunoQtdDto>();
            foreach (var aluno in alunos)
            {
                var alunoEol = alunosTurma.FirstOrDefault(a => a.CodigoAluno == aluno.CodigoAluno);
                alunosDto.Add(new CompensacaoAusenciaAlunoQtdDto()
                {
                    NumeroAluno = alunoEol.NumeroAlunoChamada,
                    CodigoAluno = aluno.CodigoAluno,
                    NomeAluno = alunoEol.NomeAluno,
                    QuantidadeCompensacoes = aluno.QuantidadeFaltasCompensadas
                });
            }

            repositorioNotificacaoCompensacaoAusencia.Excluir(compensacaoId);

            var cargos = new Cargo[] { Cargo.CP };
            if (GerarNotificacaoExtemporanea(possuirPeriodoAberto, parametroAtivo != null ? parametroAtivo.Ativo : false))
            {

                await NotificarCompensacaoExtemporanea(
                     professor.Nome,
                     professor.CodigoRf,
                     disciplinaEOL,
                     turma.CodigoTurma,
                     turma.Nome,
                     turma.ModalidadeCodigo.ObterNomeCurto(),
                     turma.Ue.CodigoUe,
                     turma.Ue.Nome,
                     turma.Ue.TipoEscola.ObterNomeCurto(),
                     turma.Ue.Dre.CodigoDre,
                     turma.Ue.Dre.Nome,
                     compensacao.Bimestre,
                     compensacao.Nome,
                     alunosDto, cargos);
            }
            else
            {
                await NotificarCompensacaoAusencia(
                         professor.Nome
                        , professor.CodigoRf
                        , disciplinaEOL
                        , turma.CodigoTurma
                        , turma.Nome
                        , turma.ModalidadeCodigo.ObterNomeCurto()
                        , turma.Ue.CodigoUe
                        , turma.Ue.Nome
                        , turma.Ue.TipoEscola.ObterNomeCurto()
                        , turma.Ue.Dre.CodigoDre
                        , turma.Ue.Dre.Nome
                        , compensacao.Bimestre
                        , compensacao.Nome
                        , alunosDto, cargos);
            }
            // Marca aluno como notificado
            alunosDto.ForEach(alunoDto =>
            {
                var aluno = alunos.FirstOrDefault(a => a.CodigoAluno == alunoDto.CodigoAluno);
                aluno.Notificado = true;
                repositorioCompensacaoAusenciaAluno.Salvar(aluno);
            });

            return true;
        }

        private async Task<string> ObterNomeDisciplina(string codigoDisciplina)
        {
            long[] disciplinaId = { long.Parse(codigoDisciplina) };
            var disciplina = await repositorioComponenteCurricular.ObterDisciplinasPorIds(disciplinaId);

            if (!disciplina.Any())
                throw new NegocioException("Componente curricular não encontrado no EOL.");

            return disciplina.FirstOrDefault().Nome;
        }

        private bool GerarNotificacaoExtemporanea(bool periodoAberto, bool parametroAtivo)
        {
            if (periodoAberto)
                return false;
            else if (parametroAtivo && !periodoAberto)
                return true;
            else if (!parametroAtivo && !periodoAberto)
                throw new NegocioException("Compensação de ausência não permitida, É necessário que o período esteja aberto");
            else if (!parametroAtivo)
                return false;

            return false;
        }

        private async Task<long> NotificarCompensacaoExtemporanea(string professor, string professorRf, string disciplina, string codigoTurma, string turma, string modalidade, string codigoUe, string escola, string tipoEscola, string codigoDre, string dre, int bimestre, string atividade, List<CompensacaoAusenciaAlunoQtdDto> alunos, Cargo[] cargos)
        {
            var tituloMensagem = $"Atividade de compensação de ausência extemporânea - {modalidade}-{turma} - {disciplina}";

            StringBuilder mensagemUsuario = new StringBuilder();
            mensagemUsuario.AppendLine($"<p>A atividade de compensação <b>'{atividade}'</b> do componente curricular de <b>{disciplina}</b> foi cadastrada para a turma <b>{turma} {modalidade}</b> da <b>{tipoEscola} {escola} ({dre})</b> no <b>{bimestre}º</b> Bimestre pelo professor <b>{professor} ({professorRf})</b> de forma extemporânea (fora do período escolar).</p>");
            mensagemUsuario.AppendLine("<p>O(s) seguinte(s) aluno(s) foi(ram) vinculado(s) a atividade:</p>");

            mensagemUsuario.AppendLine("<table style='margin-left: auto; margin-right: auto;' border='2' cellpadding='5'>");
            mensagemUsuario.AppendLine("<tr>");
            mensagemUsuario.AppendLine("<td style='padding: 5px;'>Nº</td>");
            mensagemUsuario.AppendLine("<td style='padding: 5px;'>Nome do aluno</td>");
            mensagemUsuario.AppendLine("<td style='padding: 5px;'>Quantidade de aulas compensadas</td>");
            mensagemUsuario.AppendLine("</tr>");
            foreach (var aluno in alunos)
            {
                mensagemUsuario.AppendLine("<tr>");
                mensagemUsuario.Append($"<td style='padding: 5px;'>{aluno.NumeroAluno}</td>");
                mensagemUsuario.Append($"<td style='padding: 5px;'>{aluno.NomeAluno}</td>");
                mensagemUsuario.Append($"<td style='text-align: center;'>{aluno.QuantidadeCompensacoes}</td>");
                mensagemUsuario.AppendLine("</tr>");
            }
            mensagemUsuario.AppendLine("</table>");

            return await mediator.Send(new EnviarNotificacaoCommand(tituloMensagem, mensagemUsuario.ToString(), NotificacaoCategoria.Alerta, NotificacaoTipo.Frequencia, cargos, codigoDre, codigoUe, codigoTurma));

        }


        private async Task<long> NotificarCompensacaoAusencia(string professor, string professorRf, string disciplina,
            string codigoTurma, string turma, string modalidade, string codigoUe, string escola, string tipoEscola, string codigoDre, string dre,
            int bimestre, string atividade, List<CompensacaoAusenciaAlunoQtdDto> alunos, Cargo[] cargos)
        {
            var tituloMensagem = $"Atividade de compensação da turma {turma}";

            StringBuilder mensagemUsuario = new StringBuilder();
            mensagemUsuario.AppendLine($"<p>A atividade de compensação <b>'{atividade}'</b> do componente curricular de <b>{disciplina}</b> foi cadastrada para a turma <b>{turma} {modalidade}</b> da <b>{tipoEscola} {escola} ({dre})</b> no <b>{bimestre}º</b> Bimestre pelo professor <b>{professor} ({professorRf})</b>.</p>");
            mensagemUsuario.AppendLine("<p>O(s) seguinte(s) aluno(s) foi(ram) vinculado(s) a atividade:</p>");

            mensagemUsuario.AppendLine("<table style='margin-left: auto; margin-right: auto;' border='2' cellpadding='5'>");
            mensagemUsuario.AppendLine("<tr>");
            mensagemUsuario.AppendLine("<td style='padding: 5px;'>Nº</td>");
            mensagemUsuario.AppendLine("<td style='padding: 5px;'>Nome do aluno</td>");
            mensagemUsuario.AppendLine("<td style='padding: 5px;'>Quantidade de aulas compensadas</td>");
            mensagemUsuario.AppendLine("</tr>");
            foreach (var aluno in alunos)
            {
                mensagemUsuario.AppendLine("<tr>");
                mensagemUsuario.Append($"<td style='padding: 5px;'>{aluno.NumeroAluno}</td>");
                mensagemUsuario.Append($"<td style='padding: 5px;'>{aluno.NomeAluno}</td>");
                mensagemUsuario.Append($"<td style='text-align: center;'>{aluno.QuantidadeCompensacoes}</td>");
                mensagemUsuario.AppendLine("</tr>");
            }
            mensagemUsuario.AppendLine("</table>");
            mensagemUsuario.Append("Para consultar os detalhes desta atividade acesse 'Diário de classe > Compensação de ausência'");

            return await mediator.Send(new EnviarNotificacaoCommand(tituloMensagem, mensagemUsuario.ToString(), NotificacaoCategoria.Aviso, NotificacaoTipo.Frequencia, cargos, codigoDre, codigoUe, codigoTurma));
        }
    }
}
