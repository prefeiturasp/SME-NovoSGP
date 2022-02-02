using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ListarFechamentoTurmaBimestreUseCase : IListarFechamentoTurmaBimestreUseCase
    {
        private readonly IMediator mediator;

        public ListarFechamentoTurmaBimestreUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<FechamentoNotaConceitoTurmaDto> Executar(string turmaCodigo, long componenteCurricularCodigo, int bimestre, int? semestre)
        {
            var fechamentoNotaConceitoTurma = new FechamentoNotaConceitoTurmaDto();

            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(turmaCodigo));
            if (turma == null)
                throw new NegocioException("Não foi possível localizar a turma.");

            var alunos = await mediator.Send(new ObterAlunosPorTurmaEAnoLetivoQuery(turmaCodigo));
            if (alunos == null || !alunos.Any())
                throw new NegocioException("Não foi encontrado alunos para a turma informada");

            var tipoNotaTurma = await mediator.Send(new ObterTipoNotaPorTurmaIdQuery(turma.Id, turma.TipoTurma));
            if (tipoNotaTurma == null)
                throw new NegocioException("Não foi possível localizar o tipo de nota para esta turma.");

            var tipoCalendario = await mediator.Send(new ObterTipoCalendarioPorAnoLetivoEModalidadeQuery(turma.AnoLetivo, turma.ObterModalidadeTipoCalendario(), semestre.HasValue ? semestre.Value : 0));
            if (tipoCalendario == null)
                throw new NegocioException("Não foi encontrado tipo de calendário escolar, para a modalidade informada.");

            var periodosEscolares = await mediator.Send(new ObterPeriodosEscolaresPorTipoCalendarioIdQuery(tipoCalendario.Id));
            if (periodosEscolares == null || !periodosEscolares.Any())
                throw new NegocioException("Não foi encontrado período Escolar para a modalidade informada.");

            var componentesCurriculares = await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(new long[] { componenteCurricularCodigo }));
            if (!componentesCurriculares.Any())
                throw new NegocioException("Não foi possível localizar dados do componente curricular selecionado.");

            var componenteCurricularSelecionado = componentesCurriculares.FirstOrDefault();
            var usuarioAtual = await mediator.Send(new ObterUsuarioLogadoQuery());

            var ultimoPeriodoEscolar = periodosEscolares.OrderByDescending(a => a.Bimestre).FirstOrDefault();
            var bimestreAtual = bimestre;

            bool temPeriodoAberto =await mediator.Send(new TurmaEmPeriodoAbertoQuery(turma, DateTime.Today, ultimoPeriodoEscolar.Bimestre, turma.AnoLetivo == DateTime.Today.Year));
            fechamentoNotaConceitoTurma.PeriodoAberto = temPeriodoAberto;
            fechamentoNotaConceitoTurma.NotaTipo = tipoNotaTurma.TipoNota;
            fechamentoNotaConceitoTurma.MediaAprovacaoBimestre = double.Parse(await mediator.Send(new ObterValorParametroSistemaTipoEAnoQuery(TipoParametroSistema.MediaBimestre, DateTime.Today.Year)));

            var disciplinas = new List<DisciplinaDto>();
            IEnumerable<DisciplinaDto> disciplinasRegencia = null;

            if (componenteCurricularSelecionado.Regencia)
            {
                fechamentoNotaConceitoTurma.EhRegencia = true;
                disciplinasRegencia = await mediator.Send(new ObterComponentesCurricularesRegenciaPorTurmaCodigoQuery(turmaCodigo));

                if (disciplinasRegencia == null || !disciplinasRegencia.Any())
                    throw new NegocioException("Não foram encontrados componentes curriculares para a regência informada.");

                disciplinas.AddRange(disciplinasRegencia);
            }
            else
                disciplinas.Add(new DisciplinaDto() { Nome = componenteCurricularSelecionado.Nome, CodigoComponenteCurricular = componenteCurricularSelecionado.CodigoComponenteCurricular });

            var fechamentosTurma = await ObterFechamentosTurmaDisciplina(turmaCodigo, componenteCurricularCodigo.ToString(), bimestre);
            if (fechamentosTurma != null && fechamentosTurma.Any())
            {
                fechamentoNotaConceitoTurma.FechamentoId = fechamentosTurma.First().Id;
                fechamentoNotaConceitoTurma.DataFechamento = fechamentosTurma.First().AlteradoEm.HasValue ? fechamentosTurma.First().AlteradoEm.Value : fechamentosTurma.First().CriadoEm;
            }

            if (bimestre > 0)
            {
                var periodoAtual = periodosEscolares.FirstOrDefault(x => x.Bimestre == bimestreAtual);
                if (periodoAtual == null)
                    throw new NegocioException("Não foi encontrado período escolar para o bimestre solicitado.");

                var bimestreDoPeriodo = await mediator.Send(new ObterPeriodoEscolarPorCalendarioEDataQuery(tipoCalendario.Id, periodoAtual.PeriodoFim));
                var alunosValidosComOrdenacao = alunos.Where(a => (a.NumeroAlunoChamada > 0 ||
                                                                 a.CodigoSituacaoMatricula.Equals(SituacaoMatriculaAluno.Ativo) ||
                                                                 a.CodigoSituacaoMatricula.Equals(SituacaoMatriculaAluno.Concluido)) &&
                                                                 a.DataMatricula.Date <= bimestreDoPeriodo.PeriodoFim.Date)
                                                           .OrderBy(a => a.NumeroAlunoChamada)
                                                           .ThenBy(a => a.NomeValido());

                var turmaPossuiFrequenciaRegistrada = await mediator.Send(new ExisteFrequenciaRegistradaPorTurmaComponenteCurricularQuery(turma.CodigoTurma, componenteCurricularCodigo.ToString(), bimestreDoPeriodo.Id));
                
                fechamentoNotaConceitoTurma.Alunos = await RetornaListagemAlunosFechamentoBimestreEspecifico(alunosValidosComOrdenacao, fechamentosTurma, periodoAtual, turma,
                                                           componenteCurricularCodigo.ToString(), turmaPossuiFrequenciaRegistrada, componenteCurricularSelecionado, disciplinasRegencia, 
                                                           periodosEscolares, usuarioAtual);
            } 
            else if (bimestre == 0)
            {
                var alunosValidosComOrdenacao = alunos.Where(a => (a.NumeroAlunoChamada > 0 ||
                                                                a.CodigoSituacaoMatricula.Equals(SituacaoMatriculaAluno.Ativo) ||
                                                                a.CodigoSituacaoMatricula.Equals(SituacaoMatriculaAluno.Concluido)))
                                                              .OrderBy(a => a.NumeroAlunoChamada)
                                                              .ThenBy(a => a.NomeValido());

                fechamentoNotaConceitoTurma.Alunos = await RetornaListagemAlunosFechamentoFinal(alunosValidosComOrdenacao, disciplinas, fechamentosTurma, turma,
                                                            componenteCurricularCodigo, periodosEscolares, tipoNotaTurma, usuarioAtual);
            }

            fechamentoNotaConceitoTurma.AuditoriaAlteracao = MontaTextoAuditoriaAlteracao(fechamentosTurma.Any() ? fechamentosTurma.FirstOrDefault() : null, tipoNotaTurma.EhNota());
            fechamentoNotaConceitoTurma.AuditoriaInclusao = MontaTextoAuditoriaInclusao(fechamentosTurma.Any() ? fechamentosTurma.FirstOrDefault() : null, tipoNotaTurma.EhNota());

            return fechamentoNotaConceitoTurma;
        }

        public async Task<IList<AlunosFechamentoNotaConceitoTurmaDto>> RetornaListagemAlunosFechamentoBimestreEspecifico(IEnumerable<AlunoPorTurmaResposta> alunos,
            IEnumerable<FechamentoTurmaDisciplina> fechamentosTurma, PeriodoEscolar periodoAtual, Turma turma, string componenteCurricularCodigo,
            bool turmaPossuiFrequenciaRegistrada, DisciplinaDto disciplina, IEnumerable<DisciplinaDto> disciplinasRegencia, IEnumerable<PeriodoEscolar> periodosEscolares, Usuario usuarioAtual)
        {
            var alunosFechamentoNotaConceito = new List<AlunosFechamentoNotaConceitoTurmaDto>();
            
            var ultimoPeriodoEscolar = periodosEscolares.OrderByDescending(a => a.Bimestre).FirstOrDefault();
            var usuarioEPeriodoPodeEditar = await PodeEditarNotaOuConceitoPeriodoUsuario(usuarioAtual, ultimoPeriodoEscolar, turma, componenteCurricularCodigo, ultimoPeriodoEscolar.PeriodoFim);

            foreach (var aluno in alunos)
            {
                var fechamentoTurma = (from ft in fechamentosTurma
                                       from fa in ft.FechamentoAlunos
                                       where fa.AlunoCodigo.Equals(aluno.CodigoAluno)
                                       select ft).FirstOrDefault();

                var alunoDto = new AlunosFechamentoNotaConceitoTurmaDto
                {
                    CodigoAluno = aluno.CodigoAluno,
                    NumeroChamada = aluno.NumeroAlunoChamada,
                    Nome = aluno.NomeAluno,
                    EhAtendidoAEE = await mediator.Send(new VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQuery(aluno.CodigoAluno, turma.AnoLetivo))
                };

                alunoDto.Marcador = await mediator.Send(new ObterMarcadorAlunoQuery(aluno, periodoAtual.PeriodoFim, turma.EhTurmaInfantil));

                var frequenciaAluno = await mediator.Send(new ObterFrequenciaAlunosPorAlunoDisciplinaPeriodoEscolarTipoTurmaQuery(aluno.CodigoAluno, componenteCurricularCodigo, periodoAtual.Id, TipoFrequenciaAluno.PorDisciplina, turma.CodigoTurma));
                if (frequenciaAluno != null)
                    alunoDto.Frequencia = frequenciaAluno.PercentualFrequencia.ToString();
                else
                    alunoDto.Frequencia = turmaPossuiFrequenciaRegistrada ? "100" : string.Empty;

                alunoDto.PodeEditar = usuarioEPeriodoPodeEditar ? aluno.PodeEditarNotaConceito() : false;

                if (aluno.CodigoAluno != null)
                {
                    var notasConceitoBimestre = await ObterNotasBimestre(aluno.CodigoAluno, fechamentoTurma != null ? fechamentoTurma.Id : 0);

                    if (notasConceitoBimestre.Any())
                        alunoDto.NotasConceitoBimestre = new List<FechamentoFinalConsultaRetornoAlunoNotaConceitoDto>();

                    if (turma.EhEJA() && notasConceitoBimestre != null)
                        notasConceitoBimestre = notasConceitoBimestre.Where(n => n.DisciplinaId != 6);

                    foreach (var notaConceitoBimestre in notasConceitoBimestre)
                    {
                        string nomeDisciplina;
                        if (disciplina.Regencia)
                            nomeDisciplina = disciplinasRegencia.FirstOrDefault(a => a.CodigoComponenteCurricular == notaConceitoBimestre.DisciplinaId)?.Nome;
                        else nomeDisciplina = disciplina.Nome;

                        string notaConceito = string.Empty;

                        if (notaConceitoBimestre.ConceitoId.HasValue)
                        {
                            var valorConceito = await ObterConceito(notaConceitoBimestre.ConceitoId.Value);
                            notaConceito = valorConceito.ToString();
                        }
                        else
                            notaConceito = notaConceitoBimestre.Nota.ToString();

                        var nota = new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto()
                        {
                            Bimestre = periodoAtual.Bimestre,
                            DisciplinaCodigo = notaConceitoBimestre.DisciplinaId,
                            Disciplina = nomeDisciplina,
                            NotaConceito = notaConceito
                        };

                        await VerificaNotaEmAprovacao(aluno.CodigoAluno, fechamentoTurma.FechamentoTurmaId, fechamentoTurma.DisciplinaId, nota);

                        ((List<FechamentoFinalConsultaRetornoAlunoNotaConceitoDto>)alunoDto.NotasConceitoBimestre).Add(nota);
                    }

                    alunosFechamentoNotaConceito.Add(alunoDto);
                }
            }

            return alunosFechamentoNotaConceito;
        }

        public async Task<IList<AlunosFechamentoNotaConceitoTurmaDto>> RetornaListagemAlunosFechamentoFinal(IEnumerable<AlunoPorTurmaResposta> alunos, List<DisciplinaDto> disciplinas,
            IEnumerable<FechamentoTurmaDisciplina> fechamentosTurma, Turma turma, long componenteCurricularCodigo, IEnumerable<PeriodoEscolar> periodosEscolares, 
            NotaTipoValor tipoNota, Usuario usuarioAtual)
        {
            var alunosFechamentoNotaConceito = new List<AlunosFechamentoNotaConceitoTurmaDto>();
            var ultimoPeriodoEscolar = periodosEscolares.OrderByDescending(a => a.Bimestre).FirstOrDefault();
            var usuarioEPeriodoPodeEditar = await PodeEditarNotaOuConceitoPeriodoUsuario(usuarioAtual, ultimoPeriodoEscolar, turma, componenteCurricularCodigo.ToString(), ultimoPeriodoEscolar.PeriodoFim);
            var notasFechamentosBimestres = await ObterNotasFechamentosBimestres(componenteCurricularCodigo, turma, periodosEscolares, tipoNota.EhNota());

            var notasFechamentosFinais = Enumerable.Empty<FechamentoNotaAlunoAprovacaoDto>();
            if (fechamentosTurma != null && fechamentosTurma.Any())
                notasFechamentosFinais = await mediator.Send(new ObterPorFechamentosTurmaQuery(fechamentosTurma.Select(ftd => ftd.Id).ToArray())); 

            foreach (var aluno in alunos)
            {
                AlunosFechamentoNotaConceitoTurmaDto fechamentoFinalAluno = await TrataFrequenciaAluno(componenteCurricularCodigo.ToString(), aluno, turma);

                fechamentoFinalAluno.Marcador = await mediator.Send(new ObterMarcadorAlunoQuery(aluno, ultimoPeriodoEscolar.PeriodoFim, turma.EhTurmaInfantil));

                foreach (var periodo in periodosEscolares.OrderBy(a => a.Bimestre))
                {
                    foreach (var disciplinaParaAdicionar in disciplinas)
                    {
                        var nota = notasFechamentosBimestres.FirstOrDefault(a => a.Bimestre == periodo.Bimestre
                                                                            && a.DisciplinaId == disciplinaParaAdicionar.CodigoComponenteCurricular
                                                                            && a.AlunoCodigo == aluno.CodigoAluno);
                        var notaParaAdicionar = nota?.NotaConceito ?? "";

                        fechamentoFinalAluno.NotasConceitoBimestre.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto()
                        {
                            Bimestre = periodo.Bimestre,
                            Disciplina = disciplinaParaAdicionar.Nome,
                            DisciplinaCodigo = disciplinaParaAdicionar.CodigoComponenteCurricular,
                            NotaConceito = notaParaAdicionar,

                        });
                    }
                }

                foreach (var disciplinaParaAdicionar in disciplinas)
                {
                    var nota = notasFechamentosFinais.FirstOrDefault(a => a.ComponenteCurricularId == disciplinaParaAdicionar.CodigoComponenteCurricular
                                                                    && a.AlunoCodigo == aluno.CodigoAluno);

                    string notaParaAdicionar = nota == null ? string.Empty :
                                            tipoNota.EhNota() ?
                                                nota.Nota :
                                                nota.ConceitoId;

                    fechamentoFinalAluno.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto()
                    {
                        Disciplina = disciplinaParaAdicionar.Nome,
                        DisciplinaCodigo = disciplinaParaAdicionar.CodigoComponenteCurricular,
                        NotaConceito = notaParaAdicionar,
                        EmAprovacao = nota?.EmAprovacao ?? false
                    });
                }

                fechamentoFinalAluno.PodeEditar = usuarioEPeriodoPodeEditar ? aluno.PodeEditarNotaConceito() : false;
                fechamentoFinalAluno.CodigoAluno = aluno.CodigoAluno;
                alunosFechamentoNotaConceito.Add(fechamentoFinalAluno);
            }

            return alunosFechamentoNotaConceito;
        }
        public async Task<IEnumerable<FechamentoTurmaDisciplina>> ObterFechamentosTurmaDisciplina(string turmaCodigo, string disciplinaId, int bimestre)
        {
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(turmaCodigo));
            return await mediator.Send(new ObterFechamentosTurmaComponentesQuery(turma.Id, new long[] { Convert.ToInt64(disciplinaId) }, bimestre));
        }

        private string MontaTextoAuditoriaAlteracao(FechamentoTurmaDisciplina fechamentoTurmaDisciplina, bool EhNota)
        {
            if (fechamentoTurmaDisciplina != null && !string.IsNullOrEmpty(fechamentoTurmaDisciplina.AlteradoPor))
                return $"{(EhNota ? "Notas" : "Conceitos")} finais {(EhNota ? "alteradas" : "alterados")} por {fechamentoTurmaDisciplina.AlteradoPor}({fechamentoTurmaDisciplina.AlteradoRF}) em {fechamentoTurmaDisciplina.AlteradoEm.Value.ToString("dd/MM/yyyy")},às {fechamentoTurmaDisciplina.AlteradoEm.Value.ToString("HH:mm")}.";
            else return string.Empty;
        }

        private string MontaTextoAuditoriaInclusao(FechamentoTurmaDisciplina fechamentoTurmaDisciplina, bool EhNota)
        {
            var criadorRf = fechamentoTurmaDisciplina != null && fechamentoTurmaDisciplina.CriadoRF != "0" && !string.IsNullOrEmpty(fechamentoTurmaDisciplina.CriadoRF) ?
                $"({fechamentoTurmaDisciplina.CriadoRF})" : "";

            if (fechamentoTurmaDisciplina != null)
                return $"{(EhNota ? "Notas" : "Conceitos")} finais {(EhNota ? "incluídas" : "incluídos")} por {fechamentoTurmaDisciplina.CriadoPor}{criadorRf} em {fechamentoTurmaDisciplina.CriadoEm.ToString("dd/MM/yyyy")},às {fechamentoTurmaDisciplina.CriadoEm.ToString("HH:mm")}.";

            else return string.Empty;
        }

        private async Task<bool> PodeEditarNotaOuConceitoPeriodoUsuario(Usuario usuarioLogado, PeriodoEscolar periodoEscolar, Turma turma, string codigoComponenteCurricular, DateTime data)
        {
            if (!usuarioLogado.EhGestorEscolar() && !usuarioLogado.EhPerfilDRE() && !usuarioLogado.EhPerfilSME())
            {
                var usuarioPodeEditar = await mediator.Send(new PodePersistirTurmaDisciplinaQuery(usuarioLogado.CodigoRf, turma.CodigoTurma, codigoComponenteCurricular, data.Ticks)); 
                if (!usuarioPodeEditar)
                    return false;
            }

            var temPeriodoAberto = await mediator.Send(new TurmaEmPeriodoFechamentoQuery(turma, periodoEscolar.Bimestre, DateTimeExtension.HorarioBrasilia().Date));

            return temPeriodoAberto;
        }

        private async Task<IEnumerable<FechamentoNotaAlunoDto>> ObterNotasFechamentosBimestres(long disciplinaCodigo, Turma turma, IEnumerable<PeriodoEscolar> periodosEscolares, bool ehNota)
        {
            var listaRetorno = new List<FechamentoNotaAlunoDto>();
            var fechamentosTurmaDisciplina = new List<FechamentoTurmaDisciplina>();
            foreach(var periodo in periodosEscolares)
            {
                var fechamentoBimestreTurma = await mediator.Send(new ObterFechamentoTurmaDisciplinaPorTurmaIdDisciplinasIdBimestreQuery(turma.Id, new long[] { disciplinaCodigo },periodo.Bimestre));
                
                if(fechamentoBimestreTurma.Any())
                    fechamentosTurmaDisciplina.AddRange(fechamentoBimestreTurma);
            }

            
            var fechamentosIds = fechamentosTurmaDisciplina?.Select(a => a.Id).ToArray() ?? new long[] { };
            var notasBimestrais = await mediator.Send(new ObterPorFechamentosTurmaQuery(fechamentosIds)); 

            foreach (var nota in notasBimestrais.Where(a => a.Bimestre.HasValue))
            {
                var notaParaAdicionar = ehNota ?
                                            nota?.Nota :
                                            nota?.ConceitoId;

                listaRetorno.Add(new FechamentoNotaAlunoDto(nota.Bimestre.Value,
                                                            notaParaAdicionar,
                                                            nota.ComponenteCurricularId,
                                                            nota.AlunoCodigo));
            }

            return listaRetorno;
        }

        private async Task<AlunosFechamentoNotaConceitoTurmaDto> TrataFrequenciaAluno(string componenteCurricularCodigo, AlunoPorTurmaResposta aluno, Turma turma)
        {
            var frequenciaAluno = await mediator.Send(new ObterFrequenciaGeralAlunoPorTurmaEComponenteQuery(aluno.CodigoAluno, turma.CodigoTurma, componenteCurricularCodigo));

            var percentualFrequencia = frequenciaAluno?.PercentualFrequencia ?? 100;

            if (frequenciaAluno != null && turma.AnoLetivo.Equals(2020))
                percentualFrequencia = frequenciaAluno.PercentualFrequenciaFinal;

            var fechamentoFinalAluno = new AlunosFechamentoNotaConceitoTurmaDto
            {
                Nome = aluno.NomeAluno,
                Frequencia = percentualFrequencia.ToString(),
                NumeroChamada = aluno.NumeroAlunoChamada,
                EhAtendidoAEE = await mediator.Send(new VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQuery(aluno.CodigoAluno, turma.AnoLetivo))
            };
            return fechamentoFinalAluno;
        }

        private async Task VerificaNotaEmAprovacao(string codigoAluno, long turmaFechamentoId, long disciplinaId, FechamentoFinalConsultaRetornoAlunoNotaConceitoDto notasConceito)
        {
            double nota = await mediator.Send(new ObterNotaEmAprovacaoQuery(codigoAluno, turmaFechamentoId, disciplinaId));

            if (nota > 0)
            {
                notasConceito.NotaConceito = nota.ToString();
                notasConceito.EmAprovacao = true;
            }
            else
            {
                notasConceito.EmAprovacao = false;
            }
        }

        private async Task<double> ObterConceito(long id)
        {
            var conceito = await mediator.Send(new ObterConceitoPorIdQuery(id));
            return conceito != null ? conceito.Id : 0;
        }

        public async Task<IEnumerable<FechamentoNotaDto>> ObterNotasBimestre(string codigoAluno, long fechamentoTurmaId)
           => await mediator.Send(new ObterNotasBimestrePorCodigoAlunoFechamentoIdQuery(codigoAluno, fechamentoTurmaId));

        private int ObterBimestreAtual(IEnumerable<PeriodoEscolar> periodosEscolares)
        {
            var dataPesquisa = DateTime.Now;

            var periodoEscolar = periodosEscolares.FirstOrDefault(x => x.PeriodoInicio.Date <= dataPesquisa.Date && x.PeriodoFim.Date >= dataPesquisa.Date);

            if (periodoEscolar == null)
                return 1;
            else return periodoEscolar.Bimestre;
        }

    }
}
