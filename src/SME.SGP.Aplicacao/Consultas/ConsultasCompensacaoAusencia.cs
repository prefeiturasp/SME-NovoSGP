using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasCompensacaoAusencia : ConsultasBase, IConsultasCompensacaoAusencia
    {
        private readonly IRepositorioCompensacaoAusenciaConsulta repositorioCompensacaoAusencia;
        private readonly IConsultasCompensacaoAusenciaAluno consultasCompensacaoAusenciaAluno;
        private readonly IConsultasCompensacaoAusenciaDisciplinaRegencia consultasCompensacaoAusenciaDisciplinaRegencia;
        private readonly IConsultasUe consultasUe;
        private readonly IRepositorioTurmaConsulta repositorioTurmaConsulta;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IMediator mediator;

        public ConsultasCompensacaoAusencia(IRepositorioCompensacaoAusenciaConsulta repositorioCompensacaoAusencia,
                                            IConsultasCompensacaoAusenciaAluno consultasCompensacaoAusenciaAluno,
                                            IConsultasCompensacaoAusenciaDisciplinaRegencia consultasCompensacaoAusenciaDisciplinaRegencia,
                                            IRepositorioTurmaConsulta repositorioTurmaConsulta,
                                            IServicoUsuario servicoUsuario,
                                            IContextoAplicacao contextoAplicacao,
                                            IConsultasUe consultasUe,
                                            IMediator mediator) : base(contextoAplicacao)
        {
            this.repositorioCompensacaoAusencia = repositorioCompensacaoAusencia ?? throw new ArgumentNullException(nameof(repositorioCompensacaoAusencia));
            this.consultasCompensacaoAusenciaAluno = consultasCompensacaoAusenciaAluno ?? throw new ArgumentNullException(nameof(consultasCompensacaoAusenciaAluno));
            this.consultasCompensacaoAusenciaDisciplinaRegencia = consultasCompensacaoAusenciaDisciplinaRegencia ?? throw new ArgumentNullException(nameof(consultasCompensacaoAusenciaDisciplinaRegencia));
            this.repositorioTurmaConsulta = repositorioTurmaConsulta ?? throw new ArgumentNullException(nameof(repositorioTurmaConsulta));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.consultasUe = consultasUe ?? throw new ArgumentNullException(nameof(consultasUe));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<PaginacaoResultadoDto<CompensacaoAusenciaListagemDto>> ListarPaginado(string turmaId, string disciplinaId, int bimestre, string nomeAtividade, string nomeAluno)
        {
            var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance);
            var listaCompensacoesDto = new List<CompensacaoAusenciaListagemDto>();
            var codigosComponentesConsiderados = new List<string>() { disciplinaId };

            var listaCompensacoes = await repositorioCompensacaoAusencia
                .Listar(Paginacao, turmaId, codigosComponentesConsiderados.ToArray(), bimestre, nomeAtividade);

            // Busca os nomes de alunos do EOL por turma
            var alunos = await mediator.Send(new ObterAlunosEolPorTurmaQuery(turmaId, true));
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(turmaId));
            var matriculadosTurmaPAP = alunos.Any() ? await BuscarAlunosTurmaPAP(alunos.Select(x => x.CodigoAluno).ToArray(), turma) : Enumerable.Empty<AlunosTurmaProgramaPapDto>();

            foreach (var compensacaoAusencia in listaCompensacoes.Items)
            {
                var compensacaoDto = MapearParaDto(compensacaoAusencia);
                compensacaoAusencia.Alunos = await consultasCompensacaoAusenciaAluno.ObterPorCompensacao(compensacaoAusencia.Id);

                if (compensacaoAusencia.Alunos.Any())
                {
                    foreach (var aluno in compensacaoAusencia.Alunos)
                    {
                        // Adiciona nome do aluno no Dto de retorno
                        var alunoEol = alunos.FirstOrDefault(a => a.CodigoAluno == aluno.CodigoAluno);
                        if (alunoEol.NaoEhNulo())
                            compensacaoDto.Alunos.Add(new CompensacaoAusenciaListagemAlunosDto(alunoEol.NomeAluno,matriculadosTurmaPAP.Any(x => x.CodigoAluno.ToString() == aluno.CodigoAluno)));
                    }
                }

                listaCompensacoesDto.Add(compensacaoDto);
            };

            if (!string.IsNullOrEmpty(nomeAluno))
                listaCompensacoesDto = listaCompensacoesDto.Where(c => c.Alunos.Exists(a => a.Nome.ToLower().Contains(nomeAluno.ToLower()))).ToList();

            // Mostrar apenas 3 alunos
            foreach (var compensacaoDto in listaCompensacoesDto.Where(c => c.Alunos.Count > 3))
            {
                var qtd = compensacaoDto.Alunos.Count;
                compensacaoDto.Alunos = compensacaoDto.Alunos.GetRange(0, 3);
                compensacaoDto.Alunos.Add(new CompensacaoAusenciaListagemAlunosDto($"mais {qtd - 3} alunos"));
            }

            var resultado = new PaginacaoResultadoDto<CompensacaoAusenciaListagemDto>();
            resultado.TotalPaginas = listaCompensacoes.TotalPaginas;
            resultado.TotalRegistros = listaCompensacoes.TotalRegistros;
            resultado.Items = listaCompensacoesDto;

            return resultado;
        }

        private async Task<IEnumerable<AlunosTurmaProgramaPapDto>> BuscarAlunosTurmaPAP(string[] alunosCodigos, Turma turma)
        {
            return await mediator.Send(new ObterAlunosAtivosTurmaProgramaPapEolQuery(turma.AnoLetivo, alunosCodigos));
        }
        public async Task<CompensacaoAusenciaCompletoDto> ObterPorId(long id)
        {
            var compensacao = repositorioCompensacaoAusencia.ObterPorId(id);
            if (compensacao.EhNulo())
                throw new NegocioException("Compensação de ausencia não localizada.");

            var compensacaoDto = MapearParaDtoCompleto(compensacao);
            compensacao.Alunos = await consultasCompensacaoAusenciaAluno.ObterPorCompensacao(compensacao.Id);

            // Busca os nomes de alunos do EOL por turma
            var turma = await repositorioTurmaConsulta.ObterPorId(compensacao.TurmaId);
            compensacaoDto.TurmaId = turma.CodigoTurma;

            var alunos = await mediator.Send(new ObterAlunosEolPorTurmaQuery(turma.CodigoTurma));
            if (alunos.EhNulo())
                throw new NegocioException("Alunos não localizados para a turma.");

            var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance);
            var codigosComponentesConsiderados = new List<string>() { compensacao.DisciplinaId };

            var disciplinasEOL = await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(codigosComponentesConsiderados.Select(c => long.Parse(c)).ToArray()));
            if (disciplinasEOL.EhNulo() || !disciplinasEOL.Any())
                throw new NegocioException("Componente curricular informado na compensação não localizado.");

            var quantidadeMaximaCompensacoes = int.Parse(await mediator.Send(new ObterValorParametroSistemaTipoEAnoQuery(TipoParametroSistema.QuantidadeMaximaCompensacaoAusencia, DateTime.Today.Year)));
            var percentualFrequenciaAlerta = int.Parse(await mediator.Send(new ObterValorParametroSistemaTipoEAnoQuery(disciplinasEOL.First().Regencia ? TipoParametroSistema.CompensacaoAusenciaPercentualRegenciaClasse : TipoParametroSistema.CompensacaoAusenciaPercentualFund2, DateTime.Today.Year)));
            var alunosCodigos = compensacao.Alunos.Select(x => x.CodigoAluno).ToArray();

            var compensacoes =
                (alunosCodigos.Any() ? await mediator.Send(new ObterAusenciaParaCompensacaoPorAlunosQuery(alunosCodigos, codigosComponentesConsiderados.ToArray(), compensacao.Bimestre, turma.CodigoTurma)) : null) ??
                new List<CompensacaoDataAlunoDto>();

            var matriculadosTurmaPAP = alunosCodigos.Any() ? await BuscarAlunosTurmaPAP(alunosCodigos, turma) : Enumerable.Empty<AlunosTurmaProgramaPapDto>();

            foreach (var aluno in compensacao.Alunos)
            {
                // Adiciona nome do aluno no Dto de retorno
                var alunoEol = alunos.FirstOrDefault(a => a.CodigoAluno == aluno.CodigoAluno);
                if (alunoEol.NaoEhNulo())
                {
                    var alunoDto = MapearParaDtoAlunos(aluno);
                    alunoDto.Nome = alunoEol.NomeAluno;
                    alunoDto.Compensacoes = compensacoes.Where(x => x.CodigoAluno == aluno.CodigoAluno);
                    alunoDto.EhMatriculadoTurmaPAP = matriculadosTurmaPAP.Any(x => x.CodigoAluno.ToString() == aluno.CodigoAluno);

                    var frequenciaAluno = await mediator
                        .Send(new ObterFrequenciaAlunoPorBimestreTurmaDisciplinaTipoQuery(aluno.CodigoAluno, compensacao.Bimestre, TipoFrequenciaAluno.PorDisciplina, turma.CodigoTurma, codigosComponentesConsiderados.ToArray()));

                    if (frequenciaAluno.NaoEhNulo())
                    {
                        alunoDto.QuantidadeFaltasTotais = int.Parse((frequenciaAluno.NumeroFaltasNaoCompensadas + alunoDto.QuantidadeFaltasCompensadas).ToString());
                        alunoDto.PercentualFrequencia = frequenciaAluno.PercentualFrequencia;
                        alunoDto.MaximoCompensacoesPermitidas = quantidadeMaximaCompensacoes > alunoDto.QuantidadeFaltasTotais ? alunoDto.QuantidadeFaltasTotais : quantidadeMaximaCompensacoes;
                        alunoDto.Alerta = frequenciaAluno.PercentualFrequencia <= percentualFrequenciaAlerta;
                    }

                    compensacaoDto.Alunos.Add(alunoDto);
                }
            }

            if (disciplinasEOL.First().Regencia)
            {
                var disciplinasRegencia = await consultasCompensacaoAusenciaDisciplinaRegencia.ObterPorCompensacao(compensacao.Id);
                var disciplinasIds = disciplinasRegencia.Select(x => long.Parse(x.DisciplinaId));

                if (!disciplinasIds.Any())
                    return compensacaoDto;
                disciplinasEOL = await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(disciplinasIds.ToArray()));
                    
                foreach (var disciplinaEOL in disciplinasEOL)
                {
                    compensacaoDto.DisciplinasRegencia.Add(new DisciplinaNomeDto()
                    {
                        Codigo = disciplinaEOL.CodigoComponenteCurricular.ToString(),
                        Nome = disciplinaEOL.Nome
                    });
                }
            }

            return compensacaoDto;
        }

        private CompensacaoAusenciaAlunoCompletoDto MapearParaDtoAlunos(CompensacaoAusenciaAluno aluno)
            => aluno.EhNulo() ? null :
            new CompensacaoAusenciaAlunoCompletoDto()
            {
                Id = aluno.CodigoAluno,
                QuantidadeFaltasCompensadas = aluno.QuantidadeFaltasCompensadas
            };

        private CompensacaoAusenciaListagemDto MapearParaDto(CompensacaoAusencia compensacaoAusencia)
            => compensacaoAusencia.EhNulo() ? null :
            new CompensacaoAusenciaListagemDto()
            {
                Id = compensacaoAusencia.Id,
                Bimestre = compensacaoAusencia.Bimestre,
                AtividadeNome = compensacaoAusencia.Nome,
                Alunos = new List<CompensacaoAusenciaListagemAlunosDto>()
            };

        private CompensacaoAusenciaCompletoDto MapearParaDtoCompleto(CompensacaoAusencia compensacaoAusencia)
            => compensacaoAusencia.EhNulo() ? null :
            new CompensacaoAusenciaCompletoDto()
            {
                Id = compensacaoAusencia.Id,
                DisciplinaId = compensacaoAusencia.DisciplinaId,
                Bimestre = compensacaoAusencia.Bimestre,
                Atividade = compensacaoAusencia.Nome,
                Descricao = compensacaoAusencia.Descricao,
                DisciplinasRegencia = new List<DisciplinaNomeDto>(),
                Alunos = new List<CompensacaoAusenciaAlunoCompletoDto>(),

                CriadoPor = compensacaoAusencia.CriadoPor,
                CriadoRf = compensacaoAusencia.CriadoRF,
                CriadoEm = compensacaoAusencia.CriadoEm,
                AlteradoPor = compensacaoAusencia.AlteradoPor,
                AlteradoRf = compensacaoAusencia.AlteradoRF,
                AlteradoEm = compensacaoAusencia.AlteradoEm,
                Migrado = compensacaoAusencia.Migrado
            };

        public async Task<IEnumerable<TurmaRetornoDto>> ObterTurmasParaCopia(string turmaOrigemId)
        {
            var usuario = await servicoUsuario.ObterUsuarioLogado();
            var turmaOrigem = await mediator.Send(new ObterTurmaPorCodigoQuery(turmaOrigemId));

            var ue = consultasUe.ObterPorId(turmaOrigem.UeId);

            if (usuario.PerfilAtual == Perfis.PERFIL_CP)
                return await ObtemTurmasUsuarioCPParaCopiaCompensacao(turmaOrigem, usuario.CodigoRf);
            else
            {
                var turmas = await mediator.Send(new ObterTurmasDoProfessorQuery(usuario.CodigoRf));
                return turmas.Where(t => t.CodTurma.ToString() != turmaOrigem.CodigoTurma
                                && t.CodEscola == ue.CodigoUe
                                && t.AnoLetivo == turmaOrigem.AnoLetivo
                                && t.Ano == turmaOrigem.Ano)
                        .Select(t => new TurmaRetornoDto()
                        {
                            Codigo = t.CodTurma.ToString(),
                            Nome = t.NomeTurma
                        });
            }
        }

        private async Task<IEnumerable<TurmaRetornoDto>> ObtemTurmasUsuarioCPParaCopiaCompensacao(Turma turmaOrigem, string usuarioRf)
        {
            var turmasUsuarioCP = await mediator.Send(new ObterAbrangenciaTurmasCPParaCopiaAvaliacaoQuery(turmaOrigem.AnoLetivo, usuarioRf, (int)turmaOrigem.ModalidadeCodigo, turmaOrigem.Ano, turmaOrigem.Id));

            return turmasUsuarioCP.Where(t => t.CodigoTurma.ToString() != turmaOrigem.CodigoTurma
                            && t.UeId == turmaOrigem.UeId
                            && t.AnoLetivo == turmaOrigem.AnoLetivo
                            && t.Ano == turmaOrigem.Ano)
                    .Select(t => new TurmaRetornoDto()
                    {
                        Codigo = t.CodigoTurma.ToString(),
                        Nome = t.Nome
                    });
        }
    }
}
