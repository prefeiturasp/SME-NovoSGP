using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ConsultasCompensacaoAusencia : ConsultasBase, IConsultasCompensacaoAusencia
    {
        private readonly IRepositorioCompensacaoAusencia repositorioCompensacaoAusencia;
        private readonly IConsultasCompensacaoAusenciaAluno consultasCompensacaoAusenciaAluno;
        private readonly IConsultasCompensacaoAusenciaDisciplinaRegencia consultasCompensacaoAusenciaDisciplinaRegencia;
        private readonly IConsultasFrequencia consultasFrequencia;
        private readonly IConsultasProfessor consultasProfessor;
        private readonly IConsultasUe consultasUe;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IRepositorioParametrosSistema repositorioParametrosSistema;
        private readonly IServicoEol servicoEOL;
        private readonly IServicoUsuario servicoUsuario;

        public ConsultasCompensacaoAusencia(IRepositorioCompensacaoAusencia repositorioCompensacaoAusencia, 
                                            IConsultasCompensacaoAusenciaAluno consultasCompensacaoAusenciaAluno,
                                            IConsultasCompensacaoAusenciaDisciplinaRegencia consultasCompensacaoAusenciaDisciplinaRegencia,
                                            IConsultasFrequencia consultasFrequencia,
                                            IRepositorioTurma repositorioTurma,
                                            IRepositorioParametrosSistema repositorioParametrosSistema,
                                            IServicoEol servicoEOL, 
                                            IServicoUsuario servicoUsuario,
                                            IContextoAplicacao contextoAplicacao, 
                                            IConsultasProfessor consultasProfessor, 
                                            IConsultasUe consultasUe) : base(contextoAplicacao)
        {
            this.repositorioCompensacaoAusencia = repositorioCompensacaoAusencia ?? throw new ArgumentNullException(nameof(repositorioCompensacaoAusencia));
            this.consultasCompensacaoAusenciaAluno = consultasCompensacaoAusenciaAluno ?? throw new ArgumentNullException(nameof(consultasCompensacaoAusenciaAluno));
            this.consultasCompensacaoAusenciaDisciplinaRegencia = consultasCompensacaoAusenciaDisciplinaRegencia ?? throw new ArgumentNullException(nameof(consultasCompensacaoAusenciaDisciplinaRegencia));
            this.consultasFrequencia = consultasFrequencia ?? throw new ArgumentNullException(nameof(consultasFrequencia));
            this.consultasProfessor = consultasProfessor ?? throw new ArgumentNullException(nameof(consultasProfessor));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.repositorioParametrosSistema = repositorioParametrosSistema ?? throw new ArgumentNullException(nameof(repositorioParametrosSistema));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.consultasUe = consultasUe ?? throw new ArgumentNullException(nameof(consultasUe));
        }

        public async Task<PaginacaoResultadoDto<CompensacaoAusenciaListagemDto>> ListarPaginado(string turmaId, string disciplinaId, int bimestre, string nomeAtividade, string nomeAluno)
        {
            var listaCompensacoesDto = new List<CompensacaoAusenciaListagemDto>();
            var listaCompensacoes = await repositorioCompensacaoAusencia.Listar(Paginacao, turmaId, disciplinaId, bimestre, nomeAtividade);

            // Busca os nomes de alunos do EOL por turma
            var alunos = await servicoEOL.ObterAlunosPorTurma(turmaId);

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
                        if (alunoEol != null)
                            compensacaoDto.Alunos.Add(alunoEol.NomeAluno);
                    }
                }

                listaCompensacoesDto.Add(compensacaoDto);
            };

            if (!string.IsNullOrEmpty(nomeAluno))
                listaCompensacoesDto = listaCompensacoesDto.Where(c => c.Alunos.Exists(a => a.ToLower().Contains(nomeAluno.ToLower()))).ToList();

            // Mostrar apenas 3 alunos
            foreach (var compensacaoDto in listaCompensacoesDto.Where(c => c.Alunos.Count > 3))
            {
                var qtd = compensacaoDto.Alunos.Count();
                compensacaoDto.Alunos = compensacaoDto.Alunos.GetRange(0, 3);
                compensacaoDto.Alunos.Add($"mais {qtd - 3} alunos");
            }



            var resultado = new PaginacaoResultadoDto<CompensacaoAusenciaListagemDto>();
            resultado.TotalPaginas = listaCompensacoes.TotalPaginas;
            resultado.TotalRegistros = listaCompensacoes.TotalRegistros;
            resultado.Items = listaCompensacoesDto;

            return resultado;
        }

        public async Task<CompensacaoAusenciaCompletoDto> ObterPorId(long id)
        {
            var compensacao = repositorioCompensacaoAusencia.ObterPorId(id);
            if (compensacao == null)
                throw new NegocioException("Compensação de ausencia não localizada.");

            var compensacaoDto = MapearParaDtoCompleto(compensacao);
            compensacao.Alunos = await consultasCompensacaoAusenciaAluno.ObterPorCompensacao(compensacao.Id);

            // Busca os nomes de alunos do EOL por turma
            var turma = await repositorioTurma.ObterPorId(compensacao.TurmaId);
            compensacaoDto.TurmaId = turma.CodigoTurma;

            var alunos = await servicoEOL.ObterAlunosPorTurma(turma.CodigoTurma);
            if (alunos == null)
                throw new NegocioException("Alunos não localizados para a turma.");

            var disciplinasEOL = servicoEOL.ObterDisciplinasPorIds(new long[] { long.Parse(compensacao.DisciplinaId) });
            if (disciplinasEOL == null || !disciplinasEOL.Any())
                throw new NegocioException("Disciplina informada na compensação não localizada no EOL.");

            var quantidadeMaximaCompensacoes = int.Parse(repositorioParametrosSistema.ObterValorPorTipoEAno(TipoParametroSistema.QuantidadeMaximaCompensacaoAusencia));
            var percentualFrequenciaAlerta = int.Parse(repositorioParametrosSistema.ObterValorPorTipoEAno(disciplinasEOL.First().Regencia ? TipoParametroSistema.CompensacaoAusenciaPercentualRegenciaClasse : TipoParametroSistema.CompensacaoAusenciaPercentualFund2));

            foreach (var aluno in compensacao.Alunos)
            {
                // Adiciona nome do aluno no Dto de retorno
                var alunoEol = alunos.FirstOrDefault(a => a.CodigoAluno == aluno.CodigoAluno);
                if (alunoEol != null)
                {
                    var alunoDto = MapearParaDtoAlunos(aluno);
                    alunoDto.Nome = alunoEol.NomeAluno;

                    var frequenciaAluno = consultasFrequencia.ObterPorAlunoDisciplinaData(aluno.CodigoAluno, compensacao.DisciplinaId, DateTime.Now);
                    if (frequenciaAluno != null)
                    {
                        alunoDto.QuantidadeFaltasTotais = int.Parse((frequenciaAluno.NumeroFaltasNaoCompensadas + alunoDto.QuantidadeFaltasCompensadas).ToString());
                        alunoDto.PercentualFrequencia = frequenciaAluno.PercentualFrequencia;
                        alunoDto.MaximoCompensacoesPermitidas = quantidadeMaximaCompensacoes > alunoDto.QuantidadeFaltasTotais ? alunoDto.QuantidadeFaltasTotais : quantidadeMaximaCompensacoes;
                        alunoDto.Alerta = frequenciaAluno.PercentualFrequencia <= percentualFrequenciaAlerta;
                    }
                    else
                        alunoDto.PercentualFrequencia = 100;

                    compensacaoDto.Alunos.Add(alunoDto);
                }
            }

            if (disciplinasEOL.First().Regencia)
            {
                var disciplinasRegencia = await consultasCompensacaoAusenciaDisciplinaRegencia.ObterPorCompensacao(compensacao.Id);
                var disciplinasIds = disciplinasRegencia.Select(x => long.Parse(x.DisciplinaId));

                if(!disciplinasIds.Any())
                    return compensacaoDto;

                disciplinasEOL = servicoEOL.ObterDisciplinasPorIds(disciplinasIds.ToArray());

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
            => aluno == null ? null :
            new CompensacaoAusenciaAlunoCompletoDto()
            { 
                Id = aluno.CodigoAluno,
                QuantidadeFaltasCompensadas = aluno.QuantidadeFaltasCompensadas
            };

        private CompensacaoAusenciaListagemDto MapearParaDto(CompensacaoAusencia compensacaoAusencia)
            => compensacaoAusencia == null ? null : 
            new CompensacaoAusenciaListagemDto()
            {
                Id = compensacaoAusencia.Id,
                Bimestre = compensacaoAusencia.Bimestre,
                AtividadeNome = compensacaoAusencia.Nome,
                Alunos = new List<string>()
            };

        private CompensacaoAusenciaCompletoDto MapearParaDtoCompleto(CompensacaoAusencia compensacaoAusencia)
            => compensacaoAusencia == null ? null :
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
            var professorRf = servicoUsuario.ObterRf();
            var turmaOrigem = await repositorioTurma.ObterPorCodigo(turmaOrigemId);

            var ue = consultasUe.ObterPorId(turmaOrigem.UeId);
            var turmas = servicoEOL.ObterListaTurmasPorProfessor(professorRf);

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
}
