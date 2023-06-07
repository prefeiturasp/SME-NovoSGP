﻿using System;
using System.Collections.Generic;
using System.Linq;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.NotaFechamentoFinal.Base;
using Xunit;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes;

namespace SME.SGP.TesteIntegracao.NotaFechamentoFinal
{
    public class Ao_lancar_nota_professor_com_atribuicao : NotaFechamentoTesteBase
    {
        public Ao_lancar_nota_professor_com_atribuicao(CollectionFixture collectionFixture) : base(collectionFixture)
        { }
        
        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>), typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoPorTurmaAlunoCodigoQuery, AlunoPorTurmaResposta>), typeof(ObterAlunoPorTurmaAlunoCodigoQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Fechamento Bimestre Final - Deve lançar nota conceito pelo Professor com atribuição")]
        public async Task Deve_permitir_lancamento_nota_para_professor_com_atribuicao()
        {
            var filtroNotaFechamento = ObterFiltroNotasFechamento(
                ObterPerfilProfessor(),
                TipoNota.Nota, ANO_7,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
            
            filtroNotaFechamento.CriarPeriodoEscolar = false;
            
            filtroNotaFechamento.CriarPeriodoAbertura = false;
            
            await InserirPeriodoEscolarCustomizado(true);

            await ExecutarTeste(filtroNotaFechamento);
            
            var historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(5);
            
            var historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(5);
            
            historicoNotas.Count(w=> !w.NotaAnterior.HasValue).ShouldBe(5);
            historicoNotas.Count(w=> w.NotaNova.HasValue).ShouldBe(5);
            
            historicoNotas.Any(w=> w.Id == 1 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_6).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 2 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_5).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 3 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_8).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 4 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_10).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 5 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_2).ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Fechamento Bimestre Final - Deve lançar nota conceito pelo Gestor")]
        public async Task Deve_permitir_lancamento_nota_para_gestor()
        {
            var filtroNotaFechamento = ObterFiltroNotasFechamento(
                ObterPerfilAD(),
                TipoNota.Nota, ANO_7,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
            
            await ExecutarTeste(filtroNotaFechamento);
            
            var historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(5);
            
            var historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(5);
            
            historicoNotas.Count(w=> !w.NotaAnterior.HasValue).ShouldBe(5);
            historicoNotas.Count(w=> w.NotaNova.HasValue).ShouldBe(5);
            
            historicoNotas.Any(w=> w.Id == 1 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_6).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 2 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_5).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 3 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_8).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 4 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_10).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 5 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_2).ShouldBeTrue();
        }

        private async Task InserirPeriodoEscolarCustomizado(bool periodoEscolarValido = false)
        {
            var dataReferencia = DateTimeExtension.HorarioBrasilia();
            
            await CriarPeriodoEscolar(dataReferencia.AddDays(-285), dataReferencia.AddDays(-210), BIMESTRE_1, TIPO_CALENDARIO_1);

            await CriarPeriodoEscolar(dataReferencia.AddDays(-200), dataReferencia.AddDays(-125), BIMESTRE_2, TIPO_CALENDARIO_1);

            await CriarPeriodoEscolar(dataReferencia.AddDays(-115), dataReferencia.AddDays(-40), BIMESTRE_3, TIPO_CALENDARIO_1);

            await CriarPeriodoEscolar(dataReferencia.AddDays(-20), periodoEscolarValido ? dataReferencia : dataReferencia.AddDays(-5), BIMESTRE_4, TIPO_CALENDARIO_1);
        }

        private async Task InserirPeriodoReaberturaCustomizado()
        {
            var dataReferencia = DateTimeExtension.HorarioBrasilia();

            await InserirNaBase(new FechamentoReabertura()
            {
                Descricao = REABERTURA_GERAL,
                Inicio = DateTimeExtension.HorarioBrasilia().Date,
                Fim = DateTimeExtension.HorarioBrasilia().AddYears(1).Date,
                TipoCalendarioId = TIPO_CALENDARIO_1,
                CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new FechamentoReaberturaBimestre()
            {
                FechamentoAberturaId = NUMERO_1,
                Bimestre = BIMESTRE_1,
                CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
            
            await InserirNaBase(new FechamentoReaberturaBimestre()
            {
                FechamentoAberturaId = NUMERO_1,
                Bimestre = BIMESTRE_2,
                CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
            
            await InserirNaBase(new FechamentoReaberturaBimestre()
            {
                FechamentoAberturaId = NUMERO_1,
                Bimestre = BIMESTRE_3,
                CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
            
            await InserirNaBase(new FechamentoReaberturaBimestre()
            {
                FechamentoAberturaId = NUMERO_1,
                Bimestre = BIMESTRE_4,
                CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
        }
        
        private async Task ExecutarTeste(FiltroNotaFechamentoDto filtroNotaFechamentoDto, bool criarPeriodoReabertura = false)
        {
            await CriarDadosBase(filtroNotaFechamentoDto);

            if (criarPeriodoReabertura)
                await InserirPeriodoReaberturaCustomizado();

            var fechamentoFinalSalvarDto = ObterFechamentoFinalSalvar(filtroNotaFechamentoDto);
            
            await ExecutarComandosFechamentoFinalComValidacaoNota(fechamentoFinalSalvarDto);
        }
        
        private FechamentoFinalSalvarDto ObterFechamentoFinalSalvar(FiltroNotaFechamentoDto filtroNotaFechamento)
        {
            return new FechamentoFinalSalvarDto()
            {
                DisciplinaId = filtroNotaFechamento.ComponenteCurricular,
                EhRegencia = filtroNotaFechamento.EhRegencia,
                TurmaCodigo = TURMA_CODIGO_1,
                Itens = new List<FechamentoFinalSalvarItemDto>()
                {
                    new ()
                    {
                        AlunoRf = ALUNO_CODIGO_1,
                        ComponenteCurricularCodigo = long.Parse(filtroNotaFechamento.ComponenteCurricular),
                        Nota = NOTA_6
                    },
                    new ()
                    {
                        AlunoRf = ALUNO_CODIGO_2,
                        ComponenteCurricularCodigo = long.Parse(filtroNotaFechamento.ComponenteCurricular),
                        Nota = NOTA_5
                    },
                    new ()
                    {
                        AlunoRf = ALUNO_CODIGO_3,
                        ComponenteCurricularCodigo = long.Parse(filtroNotaFechamento.ComponenteCurricular),
                        Nota = NOTA_8
                    },
                    new ()
                    {
                        AlunoRf = ALUNO_CODIGO_4,
                        ComponenteCurricularCodigo = long.Parse(filtroNotaFechamento.ComponenteCurricular),
                        Nota = NOTA_10
                    },
                    new ()
                    {
                        AlunoRf = ALUNO_CODIGO_5,
                        ComponenteCurricularCodigo = long.Parse(filtroNotaFechamento.ComponenteCurricular),
                        Nota = NOTA_2
                    }
                }
            };
        }

        private FiltroNotaFechamentoDto ObterFiltroNotasFechamento(string perfil, TipoNota tipoNota, string anoTurma,Modalidade modalidade, ModalidadeTipoCalendario modalidadeTipoCalendario, string componenteCurricular , bool considerarAnoAnterior = false, bool ehRegencia = false)
        {
            return new FiltroNotaFechamentoDto()
            {
                Perfil = perfil,
                Modalidade = modalidade,
                TipoCalendario = modalidadeTipoCalendario,
                Bimestre = BIMESTRE_1,
                ComponenteCurricular = componenteCurricular,
                TipoCalendarioId = TIPO_CALENDARIO_1,
                CriarPeriodoEscolar = true,
                CriarPeriodoAbertura = true,
                TipoNota = tipoNota,
                AnoTurma = anoTurma,
                ConsiderarAnoAnterior = considerarAnoAnterior,
                ProfessorRf = USUARIO_PROFESSOR_LOGIN_2222222,
                EhRegencia = ehRegencia
            };
        }
    }
}