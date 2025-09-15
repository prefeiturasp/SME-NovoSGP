using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;
using SME.SGP.Infra.Utilitarios;
using System;
using System.Collections.Generic;
using System.Linq;
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
            if (turma.EhNulo())
                throw new NegocioException("Não foi possível localizar a turma.");

            var alunos = await mediator.Send(new ObterAlunosPorTurmaEAnoLetivoQuery(turmaCodigo));
            if (alunos.EhNulo() || !alunos.Any())
                throw new NegocioException("Não foi encontrado alunos para a turma informada");
            
            var tipoNotaTurma = await mediator.Send(new ObterNotaTipoValorPorTurmaIdQuery(turma));
            if (tipoNotaTurma.EhNulo())
                throw new NegocioException("Não foi possível localizar o tipo de nota para esta turma.");

            var tipoCalendario = await mediator.Send(new ObterTipoCalendarioPorAnoLetivoEModalidadeQuery(turma.AnoLetivo, turma.ModalidadeCodigo.ObterModalidadeTipoCalendario(), semestre.HasValue ? semestre.Value : 0));
            if (tipoCalendario.EhNulo())
                throw new NegocioException("Não foi encontrado tipo de calendário escolar, para a modalidade informada.");

            var periodosEscolares = await mediator.Send(new ObterPeriodosEscolaresPorTipoCalendarioIdQuery(tipoCalendario.Id));
            if (periodosEscolares.EhNulo() || !periodosEscolares.Any())
                throw new NegocioException("Não foi encontrado período Escolar para a modalidade informada.");

            var componentesCurriculares = await mediator.Send(new ObterComponentesCurricularesPorIdsUsuarioLogadoQuery(new long[] { componenteCurricularCodigo }, codigoTurma: turmaCodigo));
            if (!componentesCurriculares.Any())
                throw new NegocioException("Não foi possível localizar dados do componente curricular selecionado.");

            fechamentoNotaConceitoTurma.PercentualAlunosInsuficientes = double.Parse(await mediator.Send(new ObterValorParametroSistemaTipoEAnoQuery(TipoParametroSistema.PercentualAlunosInsuficientes, DateTime.Today.Year)));

            var componenteCurricularSelecionado = componentesCurriculares.FirstOrDefault();
            var usuarioAtual = await mediator.Send(ObterUsuarioLogadoQuery.Instance);

            var ultimoPeriodoEscolar = periodosEscolares.OrderByDescending(a => a.Bimestre).FirstOrDefault();
            var bimestreAtual = bimestre;

            fechamentoNotaConceitoTurma.NotaTipo = tipoNotaTurma.TipoNota;
            fechamentoNotaConceitoTurma.MediaAprovacaoBimestre = double.Parse(await mediator.Send(new ObterValorParametroSistemaTipoEAnoQuery(TipoParametroSistema.MediaBimestre, DateTime.Today.Year)));

            var disciplinas = new List<DisciplinaDto>();
            IEnumerable<DisciplinaDto> disciplinasRegencia = null;

            if (componenteCurricularSelecionado.Regencia)
            {
                disciplinasRegencia = await mediator.Send(new ObterComponentesCurricularesRegenciaPorTurmaCodigoQuery(turmaCodigo));

                if (disciplinasRegencia.EhNulo() || !disciplinasRegencia.Any())
                    throw new NegocioException("Não foram encontrados componentes curriculares para a regência informada.");

                if (turma.EhEJA() && disciplinasRegencia.NaoEhNulo())
                    disciplinasRegencia = disciplinasRegencia.Where(n => n.CodigoComponenteCurricular != MensagemNegocioComponentesCurriculares.COMPONENTE_CURRICULAR_CODIGO_ED_FISICA);

                if (turma.Ue.TipoEscola == TipoEscola.EMEBS && (TipoTurnoEOL)turma.TipoTurno == TipoTurnoEOL.Integral)
                {
                    disciplinasRegencia = disciplinasRegencia.Append(new DisciplinaDto() { Nome = "Libras", CodigoComponenteCurricular = MensagemNegocioComponentesCurriculares.COMPONENTE_CURRICULAR_CODIGO_LIBRAS });
                }

                disciplinas.AddRange(disciplinasRegencia);
            }
            else
                disciplinas.Add(new DisciplinaDto() { Nome = componenteCurricularSelecionado.Nome, CodigoComponenteCurricular = componenteCurricularSelecionado.CodigoComponenteCurricular });

            var alunosComAnotacao = Enumerable.Empty<string>();
            var fechamentosTurma = await ObterFechamentosTurmaDisciplina(turmaCodigo, componenteCurricularCodigo.ToString(), bimestre, tipoCalendario.Id);
            if (fechamentosTurma.NaoEhNulo() && fechamentosTurma.Any())
            {
                fechamentoNotaConceitoTurma.FechamentoId = fechamentosTurma.First().Id;
                fechamentoNotaConceitoTurma.DataFechamento = fechamentosTurma.First().AlteradoEm.HasValue ? fechamentosTurma.First().AlteradoEm.Value : fechamentosTurma.First().CriadoEm;
                fechamentoNotaConceitoTurma.Situacao = fechamentosTurma.First().Situacao;
                fechamentoNotaConceitoTurma.SituacaoNome = fechamentosTurma.First().Situacao.Name();

                alunosComAnotacao = await ObterAlunosComAnotacaoNoFechamento(fechamentoNotaConceitoTurma.FechamentoId);
            }
            else
            {
                fechamentoNotaConceitoTurma.Situacao = SituacaoFechamento.NaoIniciado;
                fechamentoNotaConceitoTurma.SituacaoNome = SituacaoFechamento.NaoIniciado.Name();
            }

            var listagemAlunoDto = new ListagemAlunosFechamentoDto(fechamentosTurma, turma, componenteCurricularCodigo.ToString(), 
                                                                   componenteCurricularSelecionado, periodosEscolares, usuarioAtual, 
                                                                   alunosComAnotacao);

            IOrderedEnumerable <AlunoPorTurmaResposta> alunosValidosComOrdenacao = null;
            if (bimestre > 0)
            {
                var tipoAvaliacaoBimestral = await mediator.Send(ObterTipoAvaliacaoBimestralQuery.Instance);
                await ValidaMinimoAvaliacoesBimestrais(componenteCurricularSelecionado, disciplinasRegencia, tipoCalendario.Id, turma.CodigoTurma, bimestreAtual, tipoAvaliacaoBimestral, fechamentoNotaConceitoTurma);
                var periodoAtual = periodosEscolares.FirstOrDefault(x => x.Bimestre == bimestreAtual);
                if (periodoAtual.EhNulo())
                    throw new NegocioException("Não foi encontrado período escolar para o bimestre solicitado.");
                fechamentoNotaConceitoTurma.PeriodoFim = periodoAtual.PeriodoFim;

                var bimestreDoPeriodo = await mediator.Send(new ObterPeriodoEscolarPorCalendarioEDataQuery(tipoCalendario.Id, periodoAtual.PeriodoFim));

                alunos = alunos.Where(a => a.VerificaSeMatriculaEstaDentroDoPeriodoSelecionado(bimestreDoPeriodo.PeriodoFim));

                alunosValidosComOrdenacao = alunos.Where(a => a.DeveMostrarNaChamada(bimestreDoPeriodo.PeriodoFim, bimestreDoPeriodo.PeriodoInicio))
                                                      .OrderBy(a => a.NomeAluno)
                                                      .ThenBy(a => a.NomeValido());

                //var turmaPossuiFrequenciaRegistrada = await mediator.Send(new ExisteFrequenciaRegistradaPorTurmaComponenteCurricularQuery(turma.CodigoTurma, new string[] { componenteCurricularCodigo.ToString() }, bimestreDoPeriodo.Id));

                fechamentoNotaConceitoTurma.Alunos = await RetornaListagemAlunosFechamentoBimestreEspecifico(alunosValidosComOrdenacao, 
                                                                                                             periodoAtual,
                                                                                                             disciplinasRegencia,
                                                                                                             listagemAlunoDto);

                fechamentoNotaConceitoTurma.PossuiAvaliacao = await mediator.Send(new TurmaPossuiAvaliacaoNoPeriodoQuery(turma.Id, periodoAtual.Id, componenteCurricularCodigo));
                fechamentoNotaConceitoTurma.PeriodoEscolarId = periodoAtual.Id;
            }
            else if (bimestre == 0)
            {
                fechamentoNotaConceitoTurma.PeriodoFim = ultimoPeriodoEscolar.PeriodoFim;

                alunosValidosComOrdenacao = alunos.Where(a => a.DeveMostrarNaChamada(ultimoPeriodoEscolar.PeriodoFim, ultimoPeriodoEscolar.PeriodoInicio))
                                                  .OrderBy(a => a.NomeAluno)
                                                  .ThenBy(a => a.NomeValido());

                fechamentoNotaConceitoTurma.Alunos = await RetornaListagemAlunosFechamentoFinal(alunosValidosComOrdenacao, 
                                                                                                disciplinas, 
                                                                                                tipoNotaTurma,
                                                                                                listagemAlunoDto);
            }

            fechamentoNotaConceitoTurma.AuditoriaAlteracao = AuditoriaUtil.MontarTextoAuditoriaAlteracao(fechamentosTurma.FirstOrDefault(), tipoNotaTurma.EhNota());
            fechamentoNotaConceitoTurma.AuditoriaInclusao = AuditoriaUtil.MontarTextoAuditoriaInclusao(fechamentosTurma.FirstOrDefault(), tipoNotaTurma.EhNota());

            await AtribuirDadosDoArredondamento(fechamentoNotaConceitoTurma);

            return fechamentoNotaConceitoTurma;
        }

        private async Task AtribuirDadosDoArredondamento(FechamentoNotaConceitoTurmaDto fechamentoNotaConceitoTurma) 
            => fechamentoNotaConceitoTurma.DadosArredondamento = await mediator.Send(new ObterParametrosArredondamentoNotaPorDataAvaliacaoQuery(fechamentoNotaConceitoTurma.PeriodoFim));

        private Task<IEnumerable<string>> ObterAlunosComAnotacaoNoFechamento(long fechamentoId)
            => mediator.Send(new ObterCodigosAlunosComAnotacaoNoFechamentoQuery(fechamentoId));
        
        private async Task<IEnumerable<AlunosTurmaProgramaPapDto>> BuscarAlunosTurmaPAP(string[] alunosCodigos, int anoLetivo)
        {
            return  await mediator.Send(new ObterAlunosAtivosTurmaProgramaPapEolQuery(anoLetivo, alunosCodigos));
        }
        
        public async Task<IList<AlunosFechamentoNotaConceitoTurmaDto>> RetornaListagemAlunosFechamentoBimestreEspecifico(IEnumerable<AlunoPorTurmaResposta> alunos,
                                                                                                                         PeriodoEscolar periodoAtual,
                                                                                                                         IEnumerable<DisciplinaDto> disciplinasRegencia,
                                                                                                                         ListagemAlunosFechamentoDto dto)
        {
            var alunosFechamentoNotaConceito = new List<AlunosFechamentoNotaConceitoTurmaDto>();
            var usuarioEPeriodoPodeEditar = await PodeEditarNotaOuConceitoPeriodoUsuario(dto.UsuarioAtual, periodoAtual, dto.Turma, dto.ComponenteCurricularCodigo.ToString(), periodoAtual.PeriodoInicio);
            var exigeAprovacao = await mediator.Send(new ExigeAprovacaoDeNotaQuery(dto.Turma));
            var matriculadosTurmaPAP = await BuscarAlunosTurmaPAP(alunos.Select(x => x.CodigoAluno).ToArray(), dto.Turma.AnoLetivo);
            PeriodoFechamentoVigenteDto periodoFechamentoBimestre = null;

            if (dto.Turma.AnoLetivo >= DateTime.Now.Year)
                periodoFechamentoBimestre = await mediator.Send(new ObterPeriodoFechamentoVigentePorTurmaDataBimestreQuery(dto.Turma, DateTimeExtension.HorarioBrasilia().Date, periodoAtual.Bimestre));
            else
                periodoFechamentoBimestre = await mediator.Send(new ObterPeriodoFechamentoAnoAnteriorPorTurmaBimestreQuery(dto.Turma, periodoAtual.Bimestre));

            foreach (var aluno in alunos)
            {
                var fechamentoTurma = (from ft in dto.FechamentosTurma
                                       from fa in ft.FechamentoAlunos
                                       where fa.AlunoCodigo.Equals(aluno.CodigoAluno)
                                       select ft).FirstOrDefault();

                var alunoDto = new AlunosFechamentoNotaConceitoTurmaDto
                {
                    CodigoAluno = aluno.CodigoAluno,
                    NumeroChamada = aluno.ObterNumeroAlunoChamada(),
                    Nome = aluno.NomeAluno,
                    EhAtendidoAEE = await mediator.Send(new VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQuery(aluno.CodigoAluno, dto.Turma.AnoLetivo)),
                    EhMatriculadoTurmaPAP = matriculadosTurmaPAP.Any(x => x.CodigoAluno.ToString() == aluno.CodigoAluno)
                };

                alunoDto.Marcador = await mediator.Send(new ObterMarcadorAlunoQuery(aluno, periodoAtual.PeriodoInicio, dto.Turma.EhTurmaInfantil));
                alunoDto.PodeEditar = usuarioEPeriodoPodeEditar ? AlunoEstaAtivoLancamentoNotaFechamento(aluno, periodoFechamentoBimestre, periodoAtual) : false;

                var frequenciaAluno = await mediator.Send(new ObterFrequenciaAlunosPorAlunoDisciplinaPeriodoEscolarTipoTurmaQuery(aluno.CodigoAluno, dto.ComponenteCurricularCodigo, periodoAtual.Id, TipoFrequenciaAluno.PorDisciplina, dto.Turma.CodigoTurma));
                if (frequenciaAluno.NaoEhNulo())
                    alunoDto.Frequencia = frequenciaAluno.PercentualFrequenciaFormatado;

                if (aluno.CodigoAluno.NaoEhNulo())
                {
                    var notasConceitoBimestre = await ObterNotasBimestre(aluno.CodigoAluno, fechamentoTurma.NaoEhNulo() ? fechamentoTurma.Id : 0);

                    if (notasConceitoBimestre.Any())
                        alunoDto.NotasConceitoBimestre = new List<FechamentoConsultaNotaConceitoTurmaListaoDto>();

                    if (notasConceitoBimestre.Any())
                    {
                        foreach (var notaConceitoBimestre in notasConceitoBimestre)
                        {
                            string nomeDisciplina;
                            if (dto.Disciplina.Regencia)
                                nomeDisciplina = disciplinasRegencia.FirstOrDefault(a => a.CodigoComponenteCurricular == notaConceitoBimestre.DisciplinaId)?.Nome;
                            else nomeDisciplina = dto.Disciplina.Nome;

                            double? notaConceito;

                            if (notaConceitoBimestre.ConceitoId.HasValue)
                            {
                                var valorConceito = await ObterConceito(notaConceitoBimestre.ConceitoId.Value);
                                notaConceito = valorConceito;
                            }
                            else
                                notaConceito = notaConceitoBimestre.Nota;

                            var nota = new FechamentoConsultaNotaConceitoTurmaListaoDto()
                            {
                                Bimestre = periodoAtual.Bimestre,
                                DisciplinaCodigo = notaConceitoBimestre.DisciplinaId,
                                Disciplina = nomeDisciplina,
                                NotaConceito = notaConceito
                            };

                            if (exigeAprovacao)
                                await VerificaNotaEmAprovacao(aluno.CodigoAluno, fechamentoTurma.FechamentoTurmaId, nota.DisciplinaCodigo, nota);

                            ((List<FechamentoConsultaNotaConceitoTurmaListaoDto>)alunoDto.NotasConceitoBimestre).Add(nota);
                        }

                        if (dto.Disciplina.Regencia)
                        {
                            var listaDisciplinasSemNotaAluno = disciplinasRegencia.Where(d => !notasConceitoBimestre.Select(n => n.DisciplinaId).Contains(d.CodigoComponenteCurricular));

                            if (listaDisciplinasSemNotaAluno.Any())
                            {
                                foreach (var disciplinasSemNota in listaDisciplinasSemNotaAluno)
                                {
                                    var nota = new FechamentoConsultaNotaConceitoTurmaListaoDto()
                                    {
                                        Bimestre = periodoAtual.Bimestre,
                                        DisciplinaCodigo = disciplinasSemNota.CodigoComponenteCurricular,
                                        Disciplina = disciplinasSemNota.Nome
                                    };

                                    ((List<FechamentoConsultaNotaConceitoTurmaListaoDto>)alunoDto.NotasConceitoBimestre).Add(nota);
                                }
                            }

                        }
                    }
                    else
                    {
                        if (disciplinasRegencia.NaoEhNulo())
                        {
                            foreach (var disciplinaReg in disciplinasRegencia)
                            {
                                var nota = new FechamentoConsultaNotaConceitoTurmaListaoDto()
                                {
                                    Bimestre = periodoAtual.Bimestre,
                                    DisciplinaCodigo = disciplinaReg.CodigoComponenteCurricular,
                                    Disciplina = disciplinaReg.Nome
                                };

                                if (fechamentoTurma.NaoEhNulo() && nota.DisciplinaCodigo > 0)
                                    await VerificaNotaEmAprovacao(aluno.CodigoAluno, fechamentoTurma.FechamentoTurmaId, nota.DisciplinaCodigo, nota);

                                ((List<FechamentoConsultaNotaConceitoTurmaListaoDto>)alunoDto.NotasConceitoBimestre).Add(nota);
                            }
                        }
                        else
                        {
                            var nota = new FechamentoConsultaNotaConceitoTurmaListaoDto()
                            {
                                Bimestre = periodoAtual.Bimestre,
                                DisciplinaCodigo = dto.Disciplina.Id,
                                Disciplina = dto.Disciplina.Nome
                            };

                            if (fechamentoTurma.NaoEhNulo() && nota.DisciplinaCodigo > 0)
                                await VerificaNotaEmAprovacao(aluno.CodigoAluno, fechamentoTurma.FechamentoTurmaId, nota.DisciplinaCodigo, nota);

                            ((List<FechamentoConsultaNotaConceitoTurmaListaoDto>)alunoDto.NotasConceitoBimestre).Add(nota);
                        }
                    }

                    alunoDto.PossuiAnotacao = dto.AlunosComAnotacao.Any(a => a == aluno.CodigoAluno);

                    if (alunoDto.NotasConceitoBimestre.Any())
                        alunoDto.NotasConceitoBimestre = alunoDto.NotasConceitoBimestre.OrderBy(a => a.Disciplina).ToList();

                    alunosFechamentoNotaConceito.Add(alunoDto);
                }
            }

            return alunosFechamentoNotaConceito;
        }

       public bool AlunoEstaAtivoLancamentoNotaFechamento(AlunoPorTurmaResposta aluno, PeriodoFechamentoVigenteDto periodoFechamentoBimestre, PeriodoEscolar periodoAtual)
        => (aluno.Inativo == false || (aluno.Inativo && (aluno.DataSituacao >= periodoFechamentoBimestre?.PeriodoFechamentoInicio.Date ||
                       (aluno.DataSituacao >= periodoAtual.PeriodoInicio && periodoAtual.PeriodoFim <= aluno.DataSituacao))));
       

        public async Task<IList<AlunosFechamentoNotaConceitoTurmaDto>> RetornaListagemAlunosFechamentoFinal(IEnumerable<AlunoPorTurmaResposta> alunos, 
                                                                                                            List<DisciplinaDto> disciplinas,
                                                                                                            NotaTipoValor tipoNota, 
                                                                                                            ListagemAlunosFechamentoDto dto)
        {
            var alunosFechamentoNotaConceito = new List<AlunosFechamentoNotaConceitoTurmaDto>();

            if (dto.Turma.AnoLetivo != 2020)
                await VerificaSePodeFazerFechamentoFinal(dto.PeriodosEscolares, dto.Turma);

            var ultimoPeriodoEscolar = dto.PeriodosEscolares.OrderByDescending(a => a.Bimestre).FirstOrDefault();
            var usuarioEPeriodoPodeEditar = await PodeEditarNotaOuConceitoPeriodoUsuario(dto.UsuarioAtual, ultimoPeriodoEscolar, dto.Turma, dto.ComponenteCurricularCodigo, ultimoPeriodoEscolar.PeriodoInicio);
            var notasFechamentosBimestres = await ObterNotasFechamentosBimestres(long.Parse(dto.ComponenteCurricularCodigo), dto.Turma, dto.PeriodosEscolares, tipoNota.EhNota());

            var notasFechamentosFinais = Enumerable.Empty<FechamentoNotaAlunoAprovacaoDto>();
            if (dto.FechamentosTurma.NaoEhNulo() && dto.FechamentosTurma.Any())
                notasFechamentosFinais = await mediator.Send(new ObterPorFechamentosTurmaQuery(dto.FechamentosTurma.Select(ftd => ftd.Id).ToArray(), dto.Turma.CodigoTurma, dto.ComponenteCurricularCodigo));
            var matriculadosTurmaPAP = await BuscarAlunosTurmaPAP(alunos.Select(x => x.CodigoAluno).ToArray(), dto.Turma.AnoLetivo);
            foreach (var aluno in alunos)
            {
                AlunosFechamentoNotaConceitoTurmaDto fechamentoFinalAluno = await TrataFrequenciaAluno(dto.ComponenteCurricularCodigo, aluno, dto.Turma, matriculadosTurmaPAP);

                fechamentoFinalAluno.Marcador = await mediator.Send(new ObterMarcadorAlunoQuery(aluno, ultimoPeriodoEscolar.PeriodoInicio, dto.Turma.EhTurmaInfantil));

                foreach (var periodo in dto.PeriodosEscolares.OrderBy(a => a.Bimestre))
                {
                    foreach (var disciplinaParaAdicionar in disciplinas)
                    {
                        var nota = notasFechamentosBimestres.FirstOrDefault(a => a.Bimestre == periodo.Bimestre
                                                                            && a.DisciplinaId == disciplinaParaAdicionar.CodigoComponenteCurricular
                                                                            && a.AlunoCodigo == aluno.CodigoAluno);

                        var notaConceitoTurma = new FechamentoConsultaNotaConceitoTurmaListaoDto()
                        {
                            Bimestre = periodo.Bimestre,
                            Disciplina = disciplinaParaAdicionar.Nome,
                            DisciplinaCodigo = disciplinaParaAdicionar.CodigoComponenteCurricular,
                        };

                        if (nota.NaoEhNulo() && !string.IsNullOrEmpty(nota.NotaConceito))
                            notaConceitoTurma.NotaConceito = double.Parse(nota.NotaConceito);

                        fechamentoFinalAluno.NotasConceitoBimestre.Add(notaConceitoTurma);
                    }
                }

                foreach (var disciplinaParaAdicionar in disciplinas)
                {
                    var nota = notasFechamentosFinais.FirstOrDefault(a => a.ComponenteCurricularId == disciplinaParaAdicionar.CodigoComponenteCurricular
                                                                    && a.AlunoCodigo == aluno.CodigoAluno && !a.Bimestre.HasValue);

                    var notaConceitoTurma = new FechamentoConsultaNotaConceitoTurmaListaoDto()
                    {
                        Disciplina = disciplinaParaAdicionar.Nome,
                        DisciplinaCodigo = disciplinaParaAdicionar.CodigoComponenteCurricular,
                        EmAprovacao = nota?.EmAprovacao ?? false
                    };

                    if (nota.NaoEhNulo())
                    {
                        var valorNota = tipoNota.EhNota() ? nota.Nota : nota.ConceitoId;

                        if (valorNota.HasValue)
                            notaConceitoTurma.NotaConceito = valorNota;
                    }
                    fechamentoFinalAluno.NotasConceitoFinal.Add(notaConceitoTurma);
                }

                fechamentoFinalAluno.PodeEditar = usuarioEPeriodoPodeEditar ? aluno.PodeEditarNotaConceito() : false;
                fechamentoFinalAluno.CodigoAluno = aluno.CodigoAluno;

                fechamentoFinalAluno.PossuiAnotacao = dto.AlunosComAnotacao.Any(a => a == aluno.CodigoAluno);

                if (fechamentoFinalAluno.NotasConceitoBimestre.Any())
                    fechamentoFinalAluno.NotasConceitoBimestre = fechamentoFinalAluno.NotasConceitoBimestre.OrderBy(a => a.Disciplina).ToList();

                alunosFechamentoNotaConceito.Add(fechamentoFinalAluno);
            }



            return alunosFechamentoNotaConceito;
        }

        private async Task<IEnumerable<FechamentoTurmaDisciplina>> ObterFechamentosTurmaDisciplina(string turmaCodigo, string disciplinaId, int bimestre, long? tipoCalendario = null)
        {
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(turmaCodigo));
            return await mediator.Send(new ObterFechamentosTurmaComponentesQuery(turma.Id, new long[] { Convert.ToInt64(disciplinaId) }, bimestre, tipoCalendario));
        }

        private async Task VerificaSePodeFazerFechamentoFinal(IEnumerable<PeriodoEscolar> periodosEscolares, Turma turma)
        {
            var ultimoBimestre = periodosEscolares.OrderByDescending(a => a.Bimestre).FirstOrDefault().Bimestre;

            var fechamentoDoUltimoBimestre = await mediator.Send(new ObterFechamentosTurmaComponentesQuery(turma.Id, null, ultimoBimestre));

            if (fechamentoDoUltimoBimestre.EhNulo() || !fechamentoDoUltimoBimestre.Any())
                throw new NegocioException($"Para acessar este aba você precisa realizar o fechamento do {ultimoBimestre}º  bimestre.");
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
            var podeLancarNota = await mediator.Send(new ObterComponenteLancaNotaQuery(Convert.ToInt64(codigoComponenteCurricular)));

            return temPeriodoAberto && podeLancarNota;
        }

        private async Task<IEnumerable<FechamentoNotaAlunoDto>> ObterNotasFechamentosBimestres(long disciplinaCodigo, Turma turma, IEnumerable<PeriodoEscolar> periodosEscolares, bool ehNota)
        {
            var listaRetorno = new List<FechamentoNotaAlunoDto>();
            var fechamentosTurmaDisciplina = new List<FechamentoTurmaDisciplina>();
            var tipoCalendario = await mediator.Send(new ObterTipoCalendarioIdPorTurmaQuery(turma));
            IEnumerable<FechamentoNotaAlunoAprovacaoDto> notasBimestrais = new List<FechamentoNotaAlunoAprovacaoDto>();

            foreach (var periodo in periodosEscolares)
            {
                var fechamentoBimestreTurma = await mediator.Send(new ObterFechamentoTurmaDisciplinaPorTurmaIdDisciplinasIdBimestreQuery(turma.Id, new long[] { disciplinaCodigo }, periodo.Bimestre, tipoCalendario));

                if (fechamentoBimestreTurma.Any())
                    fechamentosTurmaDisciplina.AddRange(fechamentoBimestreTurma);
            }

            //Busca fechamento final
            var fechamentoFinal = await mediator.Send(new ObterFechamentoTurmaDisciplinaPorTurmaIdDisciplinasIdBimestreQuery(turma.Id, new long[] { disciplinaCodigo }, 0));
            if (fechamentoFinal.Any())
                fechamentosTurmaDisciplina.AddRange(fechamentoFinal);

            var fechamentosIds = fechamentosTurmaDisciplina?.Select(a => a.Id).ToArray() ?? new long[] { };

            if (fechamentosIds.Length > 0)
                notasBimestrais = await mediator.Send(new ObterPorFechamentosTurmaQuery(fechamentosIds, turma.CodigoTurma, disciplinaCodigo.ToString()));

            foreach (var nota in notasBimestrais.Where(a => a.Bimestre.HasValue))
            {
                var notaParaAdicionar = ehNota ?
                                            nota?.Nota.ToString() :
                                            nota?.ConceitoId.ToString();

                listaRetorno.Add(new FechamentoNotaAlunoDto(nota.Bimestre.Value,
                                                            notaParaAdicionar,
                                                            nota.ComponenteCurricularId,
                                                            nota.AlunoCodigo));
            }

            return listaRetorno;
        }

        private async Task<AlunosFechamentoNotaConceitoTurmaDto> TrataFrequenciaAluno(string componenteCurricularCodigo, AlunoPorTurmaResposta aluno, Turma turma, IEnumerable<AlunosTurmaProgramaPapDto> matriculadosTurmaPAP)
        {
            var percentualFrequencia = FrequenciaAluno.FormatarPercentual(0);
            
            var frequenciaAluno = await mediator.Send(new ObterFrequenciaGeralAlunoPorTurmaEComponenteQuery(aluno.CodigoAluno, turma.CodigoTurma, componenteCurricularCodigo));
            if (frequenciaAluno.NaoEhNulo())
            {
                percentualFrequencia = turma.AnoLetivo.Equals(2020) ? frequenciaAluno.PercentualFrequenciaFinalFormatado : frequenciaAluno.PercentualFrequenciaFormatado;
            }

            var fechamentoFinalAluno = new AlunosFechamentoNotaConceitoTurmaDto
            {
                Nome = aluno.NomeAluno,
                Frequencia = percentualFrequencia,
                NumeroChamada = aluno.ObterNumeroAlunoChamada(),
                EhAtendidoAEE = await mediator.Send(new VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQuery(aluno.CodigoAluno, turma.AnoLetivo)),
                EhMatriculadoTurmaPAP = matriculadosTurmaPAP.Any(x => x.CodigoAluno.ToString() == aluno.CodigoAluno)
            };
            return fechamentoFinalAluno;
        }

        private async Task VerificaNotaEmAprovacao(string codigoAluno, long turmaFechamentoId, long disciplinaId, FechamentoConsultaNotaConceitoTurmaListaoDto notasConceito)
        {
            double nota = await mediator.Send(new ObterNotaEmAprovacaoQuery(codigoAluno, turmaFechamentoId, disciplinaId));

            if (nota >= 0)
            {
                notasConceito.NotaConceito = nota;
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
            return conceito.NaoEhNulo() ? conceito.Id : 0;
        }

        public async Task<IEnumerable<FechamentoNotaDto>> ObterNotasBimestre(string codigoAluno, long fechamentoTurmaId)
           => await mediator.Send(new ObterNotasBimestrePorCodigoAlunoFechamentoIdQuery(codigoAluno, fechamentoTurmaId));

        private async Task ValidaMinimoAvaliacoesBimestrais(DisciplinaDto disciplinaEOL, IEnumerable<DisciplinaDto> disciplinasRegencia, long tipoCalendarioId, string turmaCodigo, int bimestre, TipoAvaliacao tipoAvaliacaoBimestral, FechamentoNotaConceitoTurmaDto fechamentoNotaConceitoTurma)
        {
            if (disciplinaEOL.Regencia)
            {
                var disciplinasObservacao = new List<string>();
                foreach (var disciplinaRegencia in disciplinasRegencia)
                {
                    var avaliacoes = await mediator.Send(new ObterAvaliacoesBimestraisRegenciaQuery(tipoCalendarioId, turmaCodigo, disciplinaRegencia.CodigoComponenteCurricular.ToString(), bimestre));
                    if ((avaliacoes.EhNulo()) || (avaliacoes.Count() < tipoAvaliacaoBimestral.AvaliacoesNecessariasPorBimestre))
                        disciplinasObservacao.Add(disciplinaRegencia.Nome);
                }
                if (disciplinasObservacao.Count > 0)
                    fechamentoNotaConceitoTurma.Observacoes.Add($"O(s) componente(s) curricular(es) [{string.Join(",", disciplinasObservacao)}] não tem o número mínimo de avaliações bimestrais no bimestre {bimestre}");
            }
            else
            {
                var avaliacoes = await mediator.Send(new ObterAvaliacoesBimestraisQuery(tipoCalendarioId, turmaCodigo, disciplinaEOL.CodigoComponenteCurricular.ToString(), bimestre));
                if ((avaliacoes.EhNulo()) || (avaliacoes.Count() < tipoAvaliacaoBimestral.AvaliacoesNecessariasPorBimestre))
                    fechamentoNotaConceitoTurma.Observacoes.Add($"O componente curricular [{disciplinaEOL.Nome}] não tem o número mínimo de avaliações bimestrais no bimestre {bimestre}");
            }
        }

    }
}
