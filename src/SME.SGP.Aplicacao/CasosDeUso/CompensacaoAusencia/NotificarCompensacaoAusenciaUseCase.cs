using MediatR;
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
        private readonly IRepositorioCompensacaoAusenciaAlunoConsulta repositorioCompensacaoAusenciaAlunoConsulta;
        private readonly IRepositorioCompensacaoAusenciaAluno repositorioCompensacaoAusenciaAluno;
        private readonly IRepositorioCompensacaoAusencia repositorioCompensacaoAusencia;
        private readonly IRepositorioTurmaConsulta repositorioTurmaConsulta;
        
        public NotificarCompensacaoAusenciaUseCase(IMediator mediator,
                                                   IRepositorioCompensacaoAusenciaAlunoConsulta repositorioCompensacaoAusenciaAlunoConsulta,
                                                   IRepositorioCompensacaoAusenciaAluno repositorioCompensacaoAusenciaAluno,
                                                   IRepositorioCompensacaoAusencia repositorioCompensacaoAusencia,
                                                   IRepositorioTurmaConsulta repositorioTurmaConsulta) : base(mediator)
        {
            this.repositorioCompensacaoAusenciaAlunoConsulta = repositorioCompensacaoAusenciaAlunoConsulta ?? throw new ArgumentNullException(nameof(repositorioCompensacaoAusenciaAlunoConsulta));
            this.repositorioCompensacaoAusenciaAluno = repositorioCompensacaoAusenciaAluno ?? throw new ArgumentNullException(nameof(repositorioCompensacaoAusenciaAluno));
            this.repositorioCompensacaoAusencia = repositorioCompensacaoAusencia ?? throw new ArgumentNullException(nameof(repositorioCompensacaoAusencia));
            this.repositorioTurmaConsulta = repositorioTurmaConsulta ?? throw new ArgumentNullException(nameof(repositorioTurmaConsulta));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtro = mensagem.ObterObjetoMensagem<FiltroNotificacaoCompensacaoAusenciaDto>();
            var compensacaoId = filtro.CompensacaoId;

            // Verifica se compensação possui alunos vinculados
            var alunos = await repositorioCompensacaoAusenciaAlunoConsulta.ObterPorCompensacao(compensacaoId);
            if (alunos.EhNulo() || !alunos.Any())
                return true;

            // Verifica se possui aluno não notificado na compensação
            if (!alunos.Any(a => !a.Notificado && a.QuantidadeFaltasCompensadas > 0))
                return true;

            // Carrega dados da compensacao a notificar
            var compensacao = repositorioCompensacaoAusencia.ObterPorId(compensacaoId);

            var turma = await repositorioTurmaConsulta.ObterTurmaComUeEDrePorId(compensacao.TurmaId);

            var disciplinaEOL = await ObterNomeDisciplina(compensacao.DisciplinaId);

            MeusDadosDto professor = await mediator.Send(new ObterUsuarioCoreSSOQuery(compensacao.CriadoRF));

            var possuirPeriodoAberto = await mediator.Send(new TurmaEmPeriodoAbertoQuery(turma, DateTimeExtension.HorarioBrasilia(), compensacao.Bimestre, true));
            var parametroAtivo = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.PermiteCompensacaoForaPeriodo, turma.AnoLetivo));

            // Carrega dados dos alunos não notificados
            var alunosTurma = await mediator.Send(new ObterAlunosEolPorTurmaQuery(turma.CodigoTurma, true));
            var alunosDto = new List<CompensacaoAusenciaAlunoQtdDto>();
            foreach (var aluno in alunos)
            {
                var alunoEol = alunosTurma.FirstOrDefault(a => a.CodigoAluno == aluno.CodigoAluno);
                alunosDto.Add(new CompensacaoAusenciaAlunoQtdDto()
                {
                    NumeroAluno = alunoEol.ObterNumeroAlunoChamada(),
                    CodigoAluno = aluno.CodigoAluno,
                    NomeAluno = alunoEol.NomeAluno,
                    QuantidadeCompensacoes = aluno.QuantidadeFaltasCompensadas
                });
            }

            await mediator.Send(new ExcluirNotificacaoCompensacaoAusenciaCommand(compensacaoId));

            var cargos = new Cargo[] { Cargo.CP };
            var dtoNotificacao = new NotificarCompensacaoDto(professor, disciplinaEOL, turma, compensacao, alunosDto, cargos);

            if (GerarNotificacaoExtemporanea(possuirPeriodoAberto, parametroAtivo.NaoEhNulo() ? parametroAtivo.Ativo : false))
            {
                await NotificarCompensacaoExtemporanea(dtoNotificacao);
            }
            else
            {
                await NotificarCompensacaoAusencia(dtoNotificacao);
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
            var disciplina = await mediator.Send(new ObterComponenteCurricularPorIdQuery(long.Parse(codigoDisciplina)));
            if (disciplina is null)
                throw new NegocioException("Componente curricular não encontrado no EOL.");
            return disciplina.Nome;
        }

        private bool GerarNotificacaoExtemporanea(bool periodoAberto, bool parametroAtivo)
        {
            if (periodoAberto)
                return false;
            else if (parametroAtivo)
                return true;
            else
                throw new NegocioException("Compensação de ausência não permitida, É necessário que o período esteja aberto");
        }

        private async Task<long> NotificarCompensacaoExtemporanea(NotificarCompensacaoDto dto)
        {
            var tituloMensagem = $"Atividade de compensação de ausência extemporânea - {dto.Modalidade}-{dto.Turma} - {dto.Disciplina}";

            StringBuilder mensagemUsuario = new StringBuilder();
            mensagemUsuario.AppendLine($"<p>A atividade de compensação <b>'{dto.Atividade}'</b> do componente curricular de <b>{dto.Disciplina}</b> foi cadastrada para a turma <b>{dto.Turma} {dto.Modalidade}</b> da <b>{dto.TipoEscola} {dto.Escola} ({dto.Dre})</b> no <b>{dto.Bimestre}º</b> Bimestre pelo professor <b>{dto.Professor} ({dto.ProfessorRf})</b> de forma extemporânea (fora do período escolar).</p>");
            mensagemUsuario.AppendLine("<p>O(s) seguinte(s) aluno(s) foi(ram) vinculado(s) a atividade:</p>");

            mensagemUsuario.AppendLine("<table style='margin-left: auto; margin-right: auto;' border='2' cellpadding='5'>");
            mensagemUsuario.AppendLine("<tr>");
            mensagemUsuario.AppendLine("<td style='padding: 5px;'>Nº</td>");
            mensagemUsuario.AppendLine("<td style='padding: 5px;'>Nome do aluno</td>");
            mensagemUsuario.AppendLine("<td style='padding: 5px;'>Quantidade de aulas compensadas</td>");
            mensagemUsuario.AppendLine("</tr>");
            foreach (var aluno in dto.Alunos)
            {
                mensagemUsuario.AppendLine("<tr>");
                mensagemUsuario.Append($"<td style='padding: 5px;'>{aluno.NumeroAluno}</td>");
                mensagemUsuario.Append($"<td style='padding: 5px;'>{aluno.NomeAluno}</td>");
                mensagemUsuario.Append($"<td style='text-align: center;'>{aluno.QuantidadeCompensacoes}</td>");
                mensagemUsuario.AppendLine("</tr>");
            }
            mensagemUsuario.AppendLine("</table>");

            return await mediator.Send(new EnviarNotificacaoCommand(tituloMensagem, mensagemUsuario.ToString(), NotificacaoCategoria.Alerta, NotificacaoTipo.Frequencia, dto.Cargos, dto.CodigoDre, dto.CodigoUe, dto.CodigoTurma));

        }

        private async Task<long> NotificarCompensacaoAusencia(NotificarCompensacaoDto dto)
        {
            var tituloMensagem = $"Atividade de compensação da turma {dto.Turma}";

            StringBuilder mensagemUsuario = new StringBuilder();
            mensagemUsuario.AppendLine($"<p>A atividade de compensação <b>'{dto.Atividade}'</b> do componente curricular de <b>{dto.Disciplina}</b> foi cadastrada para a turma <b>{dto.Turma} {dto.Modalidade}</b> da <b>{dto.TipoEscola} {dto.Escola} ({dto.Dre})</b> no <b>{dto.Bimestre}º</b> Bimestre pelo professor <b>{dto.Professor} ({dto.ProfessorRf})</b>.</p>");
            mensagemUsuario.AppendLine("<p>O(s) seguinte(s) aluno(s) foi(ram) vinculado(s) a atividade:</p>");

            mensagemUsuario.AppendLine("<table style='margin-left: auto; margin-right: auto;' border='2' cellpadding='5'>");
            mensagemUsuario.AppendLine("<tr>");
            mensagemUsuario.AppendLine("<td style='padding: 5px;'>Nº</td>");
            mensagemUsuario.AppendLine("<td style='padding: 5px;'>Nome do aluno</td>");
            mensagemUsuario.AppendLine("<td style='padding: 5px;'>Quantidade de aulas compensadas</td>");
            mensagemUsuario.AppendLine("</tr>");
            foreach (var aluno in dto.Alunos)
            {
                mensagemUsuario.AppendLine("<tr>");
                mensagemUsuario.Append($"<td style='padding: 5px;'>{aluno.NumeroAluno}</td>");
                mensagemUsuario.Append($"<td style='padding: 5px;'>{aluno.NomeAluno}</td>");
                mensagemUsuario.Append($"<td style='text-align: center;'>{aluno.QuantidadeCompensacoes}</td>");
                mensagemUsuario.AppendLine("</tr>");
            }
            mensagemUsuario.AppendLine("</table>");
            mensagemUsuario.Append("Para consultar os detalhes desta atividade acesse 'Diário de classe > Compensação de ausência'");

            return await mediator.Send(new EnviarNotificacaoCommand(tituloMensagem, mensagemUsuario.ToString(), NotificacaoCategoria.Aviso, NotificacaoTipo.Frequencia, dto.Cargos, dto.CodigoDre, dto.CodigoUe, dto.CodigoTurma));
        }
    }
}
