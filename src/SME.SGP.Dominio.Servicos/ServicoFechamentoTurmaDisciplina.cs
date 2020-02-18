using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoFechamentoTurmaDisciplina : IServicoFechamentoTurmaDisciplina
    {
        private readonly IConsultasDisciplina consultasDisciplina;
        private readonly IRepositorioAtividadeAvaliativaDisciplina repositorioAtividadeAvaliativaDisciplina;
        private readonly IRepositorioAtividadeAvaliativaRegencia repositorioAtividadeAvaliativaRegencia;
        private readonly IRepositorioPeriodoFechamento repositorioFechamento;
        private readonly IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina;
        private readonly IRepositorioNotaConceitoBimestre repositorioNotaConceitoBimestre;
        private readonly IRepositorioTipoAvaliacao repositorioTipoAvaliacao;
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IRepositorioUe repositorioUe;
        private readonly IServicoEOL servicoEOL;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IUnitOfWork unitOfWork;

        public ServicoFechamentoTurmaDisciplina(IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina,
                                                IRepositorioNotaConceitoBimestre repositorioNotaConceitoBimestre,
                                                IRepositorioTurma repositorioTurma,
                                                IRepositorioUe repositorioUe,
                                                IRepositorioPeriodoFechamento repositorioFechamento,
                                                IRepositorioTipoCalendario repositorioTipoCalendario,
                                                IRepositorioTipoAvaliacao repositorioTipoAvaliacao,
                                                IRepositorioAtividadeAvaliativaRegencia repositorioAtividadeAvaliativaRegencia,
                                                IRepositorioAtividadeAvaliativaDisciplina repositorioAtividadeAvaliativaDisciplina,
                                                IConsultasDisciplina consultasDisciplina,
                                                IServicoEOL servicoEOL,
                                                IServicoUsuario servicoUsuario,
                                                IUnitOfWork unitOfWork)
        {
            this.repositorioFechamentoTurmaDisciplina = repositorioFechamentoTurmaDisciplina ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurmaDisciplina));
            this.repositorioNotaConceitoBimestre = repositorioNotaConceitoBimestre ?? throw new ArgumentNullException(nameof(repositorioNotaConceitoBimestre));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
            this.repositorioFechamento = repositorioFechamento ?? throw new ArgumentNullException(nameof(repositorioFechamento));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
            this.repositorioTipoAvaliacao = repositorioTipoAvaliacao ?? throw new ArgumentNullException(nameof(repositorioTipoAvaliacao));
            this.repositorioAtividadeAvaliativaRegencia = repositorioAtividadeAvaliativaRegencia ?? throw new ArgumentNullException(nameof(repositorioAtividadeAvaliativaRegencia));
            this.repositorioAtividadeAvaliativaDisciplina = repositorioAtividadeAvaliativaDisciplina ?? throw new ArgumentNullException(nameof(repositorioAtividadeAvaliativaDisciplina));
            this.consultasDisciplina = consultasDisciplina ?? throw new ArgumentNullException(nameof(consultasDisciplina));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<AuditoriaFechamentoTurmaDto> Salvar(long id, FechamentoTurmaDisciplinaDto entidadeDto)
        {
            var fechamentoTurma = MapearParaEntidade(id, entidadeDto);

            // Valida periodo de fechamento
            var tipoCalendario = repositorioTipoCalendario.BuscarPorAnoLetivoEModalidade(fechamentoTurma.Turma.AnoLetivo
                                                                , fechamentoTurma.Turma.ModalidadeCodigo == Modalidade.EJA ? ModalidadeTipoCalendario.EJA : ModalidadeTipoCalendario.FundamentalMedio
                                                                , DateTime.Now.Month > 6 ? 2 : 1);

            var ue = repositorioUe.ObterPorId(fechamentoTurma.Turma.UeId);
            var fechamento = repositorioFechamento.ObterPorFiltros(tipoCalendario.Id, ue.DreId, ue.Id, null);
            var fechamentoBimestre = fechamento?.FechamentosBimestre.FirstOrDefault(x => x.PeriodoEscolar.Bimestre == entidadeDto.Bimestre);

            if (fechamento == null || fechamentoBimestre == null)
                throw new NegocioException($"Não localizado período de fechamento em aberto para turma informada no {entidadeDto.Bimestre}º Bimestre");

            // Valida Permissão do Professor na Turma/Disciplina
            VerificaSeProfessorPodePersistirTurma(servicoUsuario.ObterRf(), entidadeDto.TurmaId, fechamentoBimestre.PeriodoEscolar.PeriodoFim);

            fechamentoTurma.PeriodoFechamentoBimestreId = fechamentoBimestre.Id;

            // Carrega notas alunos
            var notasConceitosBimestre = await MapearParaEntidade(id, entidadeDto.NotaConceitoAlunos);

            unitOfWork.IniciarTransacao();
            try
            {
                await repositorioFechamentoTurmaDisciplina.SalvarAsync(fechamentoTurma);
                foreach (var notaBimestre in notasConceitosBimestre)
                {
                    notaBimestre.FechamentoTurmaDisciplinaId = fechamentoTurma.Id;
                    repositorioNotaConceitoBimestre.Salvar(notaBimestre);
                }
                unitOfWork.PersistirTransacao();

                return (AuditoriaFechamentoTurmaDto)fechamentoTurma;
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                throw e;
            }
        }

        private async Task<IEnumerable<NotaConceitoBimestre>> MapearParaEntidade(long id, IEnumerable<NotaConceitoBimestreDto> notasConceitosAlunosDto)
        {
            var notasConceitosBimestre = new List<NotaConceitoBimestre>();

            if (id > 0)
            {
                // Edita as notas existentes
                notasConceitosBimestre = (await repositorioNotaConceitoBimestre.ObterPorFechamentoTurma(id)).ToList();

                foreach (var notaConceitoAlunoDto in notasConceitosAlunosDto)
                {
                    var notaConceitoBimestre = notasConceitosBimestre.FirstOrDefault(x => x.CodigoAluno == notaConceitoAlunoDto.CodigoAluno && x.DisciplinaId == notaConceitoAlunoDto.DisciplinaId);
                    if (notaConceitoBimestre != null)
                    {
                        if (notaConceitoAlunoDto.Nota.HasValue)
                        {
                            notaConceitoBimestre.Nota = notaConceitoAlunoDto.Nota.Value;
                            notaConceitoBimestre.ConceitoId = null;
                        }
                        else
                        {
                            notaConceitoBimestre.ConceitoId = notaConceitoAlunoDto.ConceitoId.Value;
                            notaConceitoBimestre.Nota = 0;
                        }
                    }
                    else
                        notasConceitosBimestre.Add(MapearParaEntidade(notaConceitoAlunoDto));
                }
            }
            else
            {
                foreach (var notaConceitoAlunoDto in notasConceitosAlunosDto)
                {
                    notasConceitosBimestre.Add(MapearParaEntidade(notaConceitoAlunoDto));
                }
            }

            return notasConceitosBimestre;
        }

        private NotaConceitoBimestre MapearParaEntidade(NotaConceitoBimestreDto notaConceitoAlunoDto)
            => notaConceitoAlunoDto == null ? null :
              new NotaConceitoBimestre()
              {
                  CodigoAluno = notaConceitoAlunoDto.CodigoAluno,
                  DisciplinaId = notaConceitoAlunoDto.DisciplinaId,
                  Nota = notaConceitoAlunoDto.Nota.HasValue ? notaConceitoAlunoDto.Nota.Value : 0,
                  ConceitoId = notaConceitoAlunoDto.ConceitoId
              };

        private FechamentoTurmaDisciplina MapearParaEntidade(long id, FechamentoTurmaDisciplinaDto fechamentoDto)
        {
            var fechamento = new FechamentoTurmaDisciplina();
            if (id > 0)
                fechamento = repositorioFechamentoTurmaDisciplina.ObterPorId(id);

            fechamento.Turma = repositorioTurma.ObterPorCodigo(fechamentoDto.TurmaId);
            fechamento.TurmaId = fechamento.Turma.Id;
            fechamento.DisciplinaId = fechamentoDto.DisciplinaId;

            return fechamento;
        }

        private async Task<string> ValidaMinimoAvaliacoesBimestre(long tipoCalendarioId, string turmaId, long disciplinaId, int bimestre)
        {
            var validacoes = new StringBuilder();
            var tipoAvaliacaoBimestral = await repositorioTipoAvaliacao.ObterTipoAvaliacaoBimestral();

            var disciplinasEOL = servicoEOL.ObterDisciplinasPorIds(new long[] { disciplinaId });

            if (disciplinasEOL == null || !disciplinasEOL.Any())
                throw new NegocioException("Não foi possível localizar a disciplina no EOL.");

            if (disciplinasEOL.First().Regencia)
            {
                // Disciplinas Regencia de Classe
                disciplinasEOL = await consultasDisciplina.ObterDisciplinasParaPlanejamento(new FiltroDisciplinaPlanejamentoDto()
                {
                    CodigoTurma = long.Parse(turmaId),
                    CodigoDisciplina = int.Parse(disciplinaId.ToString()),
                    Regencia = true
                });

                foreach (var disciplina in disciplinasEOL)
                {
                    var avaliacoes = await repositorioAtividadeAvaliativaRegencia.ObterAvaliacoesBimestrais(tipoCalendarioId, turmaId, disciplina.CodigoComponenteCurricular.ToString(), bimestre);
                    if ((avaliacoes == null) || (avaliacoes.Count() < tipoAvaliacaoBimestral.AvaliacoesNecessariasPorBimestre))
                        validacoes.AppendLine($"A disciplina [{disciplina.Nome}] não tem o número mínimo de avaliações bimestrais: Necessário {tipoAvaliacaoBimestral.AvaliacoesNecessariasPorBimestre}");
                }
            }
            else
            {
                var disciplinaEOL = disciplinasEOL.First();
                var avaliacoes = await repositorioAtividadeAvaliativaDisciplina.ObterAvaliacoesBimestrais(tipoCalendarioId, turmaId, disciplinaEOL.CodigoComponenteCurricular.ToString(), bimestre);
                if ((avaliacoes == null) || (avaliacoes.Count() < tipoAvaliacaoBimestral.AvaliacoesNecessariasPorBimestre))
                    validacoes.AppendLine($"A disciplina [{disciplinaEOL.Nome}] não tem o número mínimo de avaliações bimestrais: Necessário {tipoAvaliacaoBimestral.AvaliacoesNecessariasPorBimestre}");
            }

            return validacoes.ToString();
        }

        private void VerificaSeProfessorPodePersistirTurma(string codigoRf, string turmaId, DateTime data)
        {
            if (!servicoUsuario.PodePersistirTurma(codigoRf, turmaId, data).Result)
                throw new NegocioException("Você não pode fazer alterações ou inclusões nesta turma e data.");
        }
    }
}