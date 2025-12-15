using MediatR;
using SME.SGP.Aplicacao.Commands.NovoEncaminhamentoNAAPA.AlterarSituacaoNovoEncaminhamentoNaapa;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.NovoEncaminhamentoNAAPA;
using SME.SGP.Aplicacao.Queries.NovoEncaminhamentoNAAPA.ObterNovoEncaminhamentoNAAPAComTurmaPorId;
using SME.SGP.Aplicacao.Queries.NovoEncaminhamentoNAAPA.VerificaSituacaoNovoEncaminhamentoNAAPASeEstaAguardandoAtendimentoIndevidamente;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Dtos.NovoEncaminhamentoNAAPA;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.NovoEncaminhamentoNAAPA
{
    public class ObterNovoEncaminhamentoNAAPAPorIdUseCase : IObterNovoEncaminhamentoNAAPAPorIdUseCase
    {
        private readonly IMediator mediator;
        private readonly IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo;

        public ObterNovoEncaminhamentoNAAPAPorIdUseCase(
            IMediator mediator,
            IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorio)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioFrequenciaAlunoDisciplinaPeriodo = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<NovoEncaminhamentoNAAPARespostaDto> Executar(long id)
        {
            var encaminhamento = await mediator.Send(new ObterNovoEncaminhamentoNAAPAComTurmaPorIdQuery(id));

            if (encaminhamento.EhNulo())
                throw new NegocioException(MensagemNegocioNovoEncaminhamentoNAAPA.ENCAMINHAMENTO_NAO_ENCONTRADO);

            var tipoEncaminhamento = (TipoQuestionario)encaminhamento.Tipo;

            if (encaminhamento.Situacao == Dominio.Enumerados.SituacaoNovoEncaminhamentoNAAPA.AguardandoAtendimento)
                await VerificaSeEstaEmAguardandoAtendimentoIndevidamente(encaminhamento);

            return tipoEncaminhamento == TipoQuestionario.EncaminhamentoNAAPAIndividual
                ? await MontarRespostaIndividual(encaminhamento)
                : await MontarRespostaInstitucional(encaminhamento);
        }

        private async Task<NovoEncaminhamentoNAAPARespostaDto> MontarRespostaIndividual(EncaminhamentoEscolar encaminhamento)
        {
            if (encaminhamento.Turma.EhNulo())
                throw new NegocioException("Encaminhamento individual deve ter turma associada.");

            var aluno = await ObterAlunoPorCodigoETurma(
                encaminhamento.AlunoCodigo,
                encaminhamento.Turma.CodigoTurma,
                encaminhamento.Turma.AnoLetivo);

            var nomeUe = encaminhamento.Turma.Ue.TipoEscola == TipoEscola.Nenhum
                ? encaminhamento.Turma.Ue.Nome
                : $"{encaminhamento.Turma.Ue.TipoEscola.ObterNomeCurto()} {encaminhamento.Turma.Ue.Nome}";

            return new NovoEncaminhamentoNAAPARespostaDto()
            {
                Tipo = encaminhamento.Tipo,
                DescricaoTipo = ((TipoQuestionario)encaminhamento.Tipo).Name(),
                Aluno = aluno,
                DreId = encaminhamento.Turma.Ue.Dre.Id,
                DreCodigo = encaminhamento.Turma.Ue.Dre.CodigoDre,
                DreNome = encaminhamento.Turma.Ue.Dre.Nome,
                UeId = encaminhamento.Turma.Ue.Id,
                UeCodigo = encaminhamento.Turma.Ue.CodigoUe,
                UeNome = nomeUe,
                TurmaId = encaminhamento.Turma.Id,
                TurmaCodigo = encaminhamento.Turma.CodigoTurma,
                TurmaNome = encaminhamento.Turma.Nome,
                AnoLetivo = encaminhamento.Turma.AnoLetivo,
                Situacao = (int)encaminhamento.Situacao,
                DescricaoSituacao = encaminhamento.Situacao.Name(),
                Modalidade = (int)encaminhamento.Turma.ModalidadeCodigo,
                MotivoEncerramento = encaminhamento.MotivoEncerramento
            };
        }

        private async Task<NovoEncaminhamentoNAAPARespostaDto> MontarRespostaInstitucional(EncaminhamentoEscolar encaminhamento)
        {
            if (encaminhamento.Ue.EhNulo() || encaminhamento.Dre.EhNulo())
                throw new NegocioException("Encaminhamento institucional deve ter UE e DRE associadas.");

            var nomeUe = encaminhamento.Ue.TipoEscola == TipoEscola.Nenhum
                ? encaminhamento.Ue.Nome
                : $"{encaminhamento.Ue.TipoEscola.ObterNomeCurto()} {encaminhamento.Ue.Nome}";

            return new NovoEncaminhamentoNAAPARespostaDto()
            {
                Tipo = encaminhamento.Tipo,
                DescricaoTipo = ((TipoQuestionario)encaminhamento.Tipo).Name(),
                DreId = encaminhamento.Dre.Id,
                DreCodigo = encaminhamento.Dre.CodigoDre,
                DreNome = encaminhamento.Dre.Nome,
                UeId = encaminhamento.Ue.Id,
                UeCodigo = encaminhamento.Ue.CodigoUe,
                UeNome = nomeUe,
                TurmaId = null,
                TurmaCodigo = null,
                TurmaNome = null,
                Aluno = null,
                Modalidade = null,
                AnoLetivo = encaminhamento.CriadoEm.Year,
                Situacao = (int)encaminhamento.Situacao,
                DescricaoSituacao = encaminhamento.Situacao.Name(),
                MotivoEncerramento = encaminhamento.MotivoEncerramento
            };
        }

        private async Task<IEnumerable<AlunosTurmaProgramaPapDto>> BuscarAlunosTurmaPAP(string[] alunosCodigos, int anoLetivo)
        {
            return await mediator.Send(new ObterAlunosAtivosTurmaProgramaPapEolQuery(anoLetivo, alunosCodigos));
        }

        private async Task<AlunoTurmaReduzidoDto> ObterAlunoPorCodigoETurma(string alunoCodigo, string turmaCodigo, int anoLetivo)
        {
            var aluno = await mediator.Send(new ObterAlunoPorCodigoEolQuery(alunoCodigo, anoLetivo, false, true, turmaCodigo));
            if (aluno.EhNulo()) throw new NegocioException(MensagemNegocioEOL.NAO_LOCALIZADO_INFORMACOES_ALUNO_TURMA_EOL);

            var frequencia = await mediator.Send(new ObterConsultaFrequenciaGeralAlunoQuery(alunoCodigo, turmaCodigo));
            var frequenciaAluno = await repositorioFrequenciaAlunoDisciplinaPeriodo.ObterFrequenciaGeralAluno(alunoCodigo, turmaCodigo);
            var frequenciaPeriodoEspecifico = frequenciaAluno?.Where(f => f.PeriodoInicio.Year <= anoLetivo && f.PeriodoFim.Year >= anoLetivo).FirstOrDefault();
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(turmaCodigo));
            var dadosBuscaAtiva = await mediator.Send(new ObterDadosBuscaAtivaPorAlunoQuery(alunoCodigo, long.Parse(turmaCodigo), anoLetivo));
            var matriculadosTurmaPAP = await BuscarAlunosTurmaPAP(new[] { aluno.CodigoAluno }, anoLetivo);

            return new AlunoTurmaReduzidoDto()
            {
                Nome = !string.IsNullOrEmpty(aluno.NomeAluno) ? aluno.NomeAluno : aluno.NomeSocialAluno,
                NumeroAlunoChamada = aluno.ObterNumeroAlunoChamada(),
                DataNascimento = aluno.DataNascimento,
                DataSituacao = aluno.DataSituacao,
                Idade = aluno.Idade,
                DocumentoCpf = aluno.DocumentoCpf,
                CodigoAluno = aluno.CodigoAluno,
                CodigoSituacaoMatricula = aluno.CodigoSituacaoMatricula,
                Situacao = aluno.SituacaoMatricula,
                TurmaEscola = await ObterNomeTurmaFormatado(aluno.CodigoTurma.ToString()),
                CodigoTurma = aluno.CodigoTurma.ToString(),
                CelularResponsavel = aluno.CelularResponsavel,
                NomeResponsavel = aluno.NomeResponsavel,
                DataAtualizacaoContato = aluno.DataAtualizacaoContato,
                TipoResponsavel = aluno.TipoResponsavel,
                Frequencia = frequencia,
                EhMatriculadoTurmaPAP = matriculadosTurmaPAP.Any(x => x.CodigoAluno.ToString() == aluno.CodigoAluno),
                NumeroAulasDadas = frequenciaPeriodoEspecifico?.TotalAulas ?? 0,
                NumeroAulasFrequentadas = frequenciaPeriodoEspecifico?.TotalPresencas ?? 0,
                CicloEnsino = turma?.SerieEnsino ?? string.Empty,
                EhBuscaAtiva = dadosBuscaAtiva?.TemRegistroBuscaAtiva ?? false,
                NumeroVisitas = dadosBuscaAtiva?.NumeroVisitas ?? 0,
                NumeroLigacoes = dadosBuscaAtiva?.NumeroLigacoes ?? 0
            };
        }

        private async Task<string> ObterNomeTurmaFormatado(string turmaCodigo)
        {
            var turmaNome = "";
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(turmaCodigo));

            if (turma.NaoEhNulo())
            {
                var nomeTurno = "";
                if (Enum.IsDefined(typeof(TipoTurnoEOL), turma.TipoTurno))
                {
                    TipoTurnoEOL tipoTurno = (TipoTurnoEOL)turma.TipoTurno;
                    nomeTurno = $"- {tipoTurno.GetAttribute<DisplayAttribute>()?.GetName()}";
                }
                turmaNome = $"{turma.ModalidadeCodigo.ShortName()} - {turma.Nome} {nomeTurno}";
            }

            return turmaNome;
        }

        public async Task VerificaSeEstaEmAguardandoAtendimentoIndevidamente(EncaminhamentoEscolar encaminhamento)
        {
            bool necessitaAlteracao = await mediator.Send(new VerificaSituacaoNovoEncaminhamentoNAAPASeEstaAguardandoAtendimentoIndevidamenteQuery(encaminhamento.Id));
            if (necessitaAlteracao)
                await mediator.Send(new AlterarSituacaoNovoEncaminhamentoNaapaCommand(encaminhamento, Dominio.Enumerados.SituacaoNovoEncaminhamentoNAAPA.AguardandoAtendimento));
        }
    }
}