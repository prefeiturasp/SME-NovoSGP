using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Contexto;
using SME.SGP.Infra.Interfaces;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao
{
    public class TesteBaseUtils : TesteBaseConstantes
    {
        protected readonly CollectionFixture collectionFixture;

        public TesteBaseUtils(CollectionFixture collectionFixture) 
        {
            this.collectionFixture = collectionFixture ?? throw new ArgumentNullException(nameof(collectionFixture));
        }

        public void CriarClaimUsuario(string perfil, string pagina = "0", string registros = "10")
        {
            var contextoAplicacao = collectionFixture.ServiceProvider.GetService<IContextoAplicacao>();

            contextoAplicacao.AdicionarVariaveis(ObterVariaveisPorPerfil(perfil, pagina, registros));
        }

        private Dictionary<string, object> ObterVariaveisPorPerfil(string perfil, string pagina, string registros)
        {
            var rfLoginPerfil = ObterRfLoginPerfil(perfil);

            return new Dictionary<string, object>
            {
                { USUARIO_CHAVE, rfLoginPerfil },
                { USUARIO_LOGADO_CHAVE, rfLoginPerfil },
                { USUARIO_RF_CHAVE, rfLoginPerfil },
                { USUARIO_LOGIN_CHAVE, rfLoginPerfil },
                { NUMERO_PAGINA, pagina },
                { NUMERO_REGISTROS, registros },
                { ADMINISTRADOR, rfLoginPerfil },
                { NOME_ADMINISTRADOR, rfLoginPerfil },
                {
                    USUARIO_CLAIMS_CHAVE,
                    new List<InternalClaim> {
                        new InternalClaim { Value = rfLoginPerfil, Type = USUARIO_CLAIM_TIPO_RF },
                        new InternalClaim { Value = perfil, Type = USUARIO_CLAIM_TIPO_PERFIL }
                    }
                }
            };
        }

        private string ObterRfLoginPerfil(string perfil)
        {
            if (perfil.Equals(ObterPerfilProfessor()) || perfil.Equals(ObterPerfilCJ()))
                return USUARIO_PROFESSOR_LOGIN_2222222;

            if (perfil.Equals(ObterPerfilDiretor()))
                return USUARIO_LOGIN_DIRETOR;

            if (perfil.Equals(ObterPerfilAD()))
                return USUARIO_LOGIN_AD;

            if (perfil.Equals(ObterPerfilPaai()))
                return USUARIO_LOGIN_PAAI;

            if (perfil.Equals(ObterPerfilPaee()))
                return USUARIO_PAAI_LOGIN_5555555;

            if (perfil.Equals(ObterPerfilAdmDre()))
                return USUARIO_LOGIN_ADM_DRE;

            if (perfil.Equals(ObterPerfilAdmSme()))
                return USUARIO_LOGIN_ADM_SME;

            if (perfil.Equals(ObterPerfilPAP()))
                return USUARIO_LOGIN_PAP;

            return USUARIO_PROFESSOR_LOGIN_2222222;
        }

        public string ObterPerfilProfessor()
        {
            return Guid.Parse(PerfilUsuario.PROFESSOR.Name()).ToString();
        }

        public string ObterPerfilCoordenadorNAAPA()
        {
            return Guid.Parse(PerfilUsuario.COORDENADOR_NAAPA.Name()).ToString();
        }

        public string ObterPerfilPsicologoEscolar()
        {
            return Guid.Parse(PerfilUsuario.PSICOLOGO_ESCOLAR.Name()).ToString();
        }

        public string ObterPerfilPsicopedagogo()
        {
            return Guid.Parse(PerfilUsuario.PSICOPEDAGOGO.Name()).ToString();
        }

        public string ObterPerfilAssistenteSocial()
        {
            return Guid.Parse(PerfilUsuario.ASSISTENTE_SOCIAL.Name()).ToString();
        }

        public string ObterPerfilCJ()
        {
            return Guid.Parse(PerfilUsuario.CJ.Name()).ToString();
        }

        public string ObterPerfilCoordenadorCefai()
        {
            return Guid.Parse(PerfilUsuario.CEFAI.Name()).ToString();
        }
        public string ObterPerfilPaai()
        {
            return Guid.Parse(PerfilUsuario.PAAI.Name()).ToString();
        }

        public string ObterPerfilPaee()
        {
            return Guid.Parse(PerfilUsuario.PAEE.Name()).ToString();
        }

        public string ObterPerfilAdmDre()
        {
            return Guid.Parse(PerfilUsuario.ADMDRE.ObterNome()).ToString();
        }

        public string ObterPerfilAdmSme()
        {
            return Guid.Parse(PerfilUsuario.ADMSME.ObterNome()).ToString();
        }

        public string ObterPerfilCJInfantil()
        {
            return Guid.Parse(PerfilUsuario.CJ_INFANTIL.Name()).ToString();
        }

        public string ObterPerfilProfessorInfantil()
        {
            return Guid.Parse(PerfilUsuario.PROFESSOR_INFANTIL.Name()).ToString();
        }

        public string ObterPerfilCP()
        {
            return Guid.Parse(PerfilUsuario.CP.Name()).ToString();
        }

        public string ObterPerfilCEFAI()
        {
            return Guid.Parse(PerfilUsuario.CEFAI.Name()).ToString();
        }

        public string ObterPerfilAD()
        {
            return Guid.Parse(PerfilUsuario.AD.Name()).ToString();
        }

        public string ObterPerfilDiretor()
        {
            return Guid.Parse(PerfilUsuario.DIRETOR.Name()).ToString();
        }
        public string ObterPerfilPAP()
        {
            return Guid.Parse(PerfilUsuario.PAP.Name()).ToString();
        }
        public string ObterPerfilPOA_Portugues()
        {
            return Guid.Parse(PerfilUsuario.POA_LINGUA_PORTUGUESA.Name()).ToString();
        }

        public async Task CriarPeriodoEscolarEncerrado()
        {
            await this.collectionFixture.InserirNaBase(new PeriodoEscolar
            {
                Id = 1,
                TipoCalendarioId = 1,
                Bimestre = BIMESTRE_2,
                PeriodoInicio = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 10),
                PeriodoFim = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 02, 5),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                Migrado = false
            });
        }

        public async Task CriarEvento(EventoLetivo letivo, DateTime dataInicioEvento, DateTime dataFimEvento, bool eventoEscolaAqui = false, long tipoEventoId = 1)
        {
            await collectionFixture.InserirNaBase(new EventoTipo
            {
                Id = tipoEventoId,
                Descricao = EVENTO_NOME_FESTA,
                Ativo = true,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                EventoEscolaAqui = eventoEscolaAqui
            });

            await collectionFixture.InserirNaBase(new Evento
            {
                Nome = EVENTO_NOME_FESTA,
                TipoCalendarioId = 1,
                TipoEventoId = tipoEventoId,
                UeId = UE_CODIGO_1,
                Letivo = letivo,
                DreId = DRE_CODIGO_1,
                DataInicio = dataInicioEvento,
                DataFim = dataFimEvento,
                Status = EntidadeStatus.Aprovado,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                Migrado = false
            });
        }

        public async Task CriarComunicadoAluno(string comunicado, int anoLetivo, string codigoDre, string codigoUe, string codigoTurma, string codigoAluno, long comunicadoId = 1)
        {
            await collectionFixture.InserirNaBase(new Comunicado
            {
                Id = comunicadoId,
                Descricao = comunicado,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                AnoLetivo = anoLetivo,
                CodigoDre = codigoDre,
                CodigoUe = codigoUe,
                Titulo = comunicado,
                TipoComunicado = TipoComunicado.ALUNO,
            });

            await collectionFixture.InserirNaBase(new ComunicadoAluno
            {
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                ComunicadoId = comunicadoId,
                AlunoCodigo = codigoAluno,
                AlunoNome = "Nome Teste Aluno",
            });

            await collectionFixture.InserirNaBase(new ComunicadoTurma
            {
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                ComunicadoId = comunicadoId,
                CodigoTurma = codigoTurma
            });
        }

        public async Task CriarEventoTipo(string descricao, EventoLocalOcorrencia localOcorrencia, bool concomitancia, EventoTipoData tipoData, bool dependencia, EventoLetivo letivo, bool ativo, bool excluido, long codigo, bool somenteLeitura, bool eventoEscolaAqui)
        {
            await collectionFixture.InserirNaBase(new EventoTipo()
            {
                Descricao = descricao,
                LocalOcorrencia = localOcorrencia,
                Concomitancia = concomitancia,
                TipoData = tipoData,
                Dependencia = dependencia,
                Letivo = letivo,
                Ativo = ativo,
                Excluido = excluido,
                Codigo = codigo,
                SomenteLeitura = somenteLeitura,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME
            });
        }

        protected async Task CriarEventoResumido(string nomeEvento, DateTime dataInicio, DateTime dataFim, EventoLetivo eventoLetivo, long tipoCalendarioId, long tipoEventoId, string dreId, string ueId, EntidadeStatus eventoStatus)
        {
            await CriarEvento(nomeEvento, dataInicio, dataFim, eventoLetivo, tipoCalendarioId, tipoEventoId, dreId, ueId, eventoStatus, null, null, null, null);
        }

        public async Task CriarEvento(string nomeEvento, DateTime dataInicio, DateTime dataFim, EventoLetivo eventoLetivo, long tipoCalendarioId, long tipoEventoId, string dreId, string ueId, EntidadeStatus eventoStatus, long? workflowAprovacaoId, TipoPerfil? tipoPerfil, long? eventoPaiId, long? feriadoId, bool migrado = false)
        {
            await collectionFixture.InserirNaBase(new Evento
            {
                Nome = nomeEvento,
                DataInicio = dataInicio,
                DataFim = dataFim,
                Letivo = eventoLetivo,
                TipoCalendarioId = tipoCalendarioId,
                TipoEventoId = tipoEventoId,
                DreId = dreId,
                UeId = ueId,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Excluido = false,
                Status = eventoStatus,
                WorkflowAprovacaoId = workflowAprovacaoId,
                Migrado = migrado,
                TipoPerfilCadastro = tipoPerfil,
                EventoPaiId = eventoPaiId,
                FeriadoId = feriadoId
            });
        }

        public async Task CriarAtribuicaoEsporadica(DateTime dataInicio, DateTime dataFim)
        {
            await collectionFixture.InserirNaBase(new AtribuicaoEsporadica
            {
                UeId = UE_CODIGO_1,
                ProfessorRf = USUARIO_PROFESSOR_LOGIN_2222222,
                AnoLetivo = ANO_LETIVO_ANO_ATUAL,
                DreId = DRE_CODIGO_1,
                DataInicio = dataInicio,
                DataFim = dataFim,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                Migrado = false
            });
        }

        public async Task CriarAtribuicaoCJ(Modalidade modalidade, long componenteCurricularId, bool substituir = true)
        {
            await collectionFixture.InserirNaBase(new AtribuicaoCJ
            {
                TurmaId = TURMA_CODIGO_1,
                DreId = DRE_CODIGO_1,
                UeId = UE_CODIGO_1,
                ProfessorRf = USUARIO_PROFESSOR_LOGIN_2222222,
                DisciplinaId = componenteCurricularId,
                Modalidade = modalidade,
                Substituir = substituir,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                Migrado = false
            });
        }

        public async Task CriarAtribuicaoCJ(Modalidade modalidade, long componenteCurricularId, string professorRf, bool substituir = true)
        {
            await collectionFixture.InserirNaBase(new AtribuicaoCJ
            {
                TurmaId = TURMA_CODIGO_1,
                DreId = DRE_CODIGO_1,
                UeId = UE_CODIGO_1,
                ProfessorRf = professorRf,
                DisciplinaId = componenteCurricularId,
                Modalidade = modalidade,
                Substituir = substituir,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                Migrado = false
            });
        }

        public async Task CriarUsuarios()
        {
            await collectionFixture.InserirNaBase(new Usuario
            {
                Login = USUARIO_PROFESSOR_LOGIN_2222222,
                CodigoRf = USUARIO_PROFESSOR_CODIGO_RF_2222222,
                Nome = USUARIO_PROFESSOR_NOME_2222222,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await collectionFixture.InserirNaBase(new Usuario
            {
                Login = USUARIO_PROFESSOR_LOGIN_1111111,
                CodigoRf = USUARIO_PROFESSOR_CODIGO_RF_1111111,
                Nome = USUARIO_PROFESSOR_NOME_1111111,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await collectionFixture.InserirNaBase(new Usuario()
            {
                CodigoRf = USUARIO_CP_LOGIN_3333333,
                Login = USUARIO_CP_LOGIN_3333333,
                Nome = USUARIO_CP_NOME_3333333,
                PerfilAtual = Guid.Parse(PerfilUsuario.CP.ObterNome()),
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
            });

            await collectionFixture.InserirNaBase(new Usuario()
            {
                CodigoRf = USUARIO_LOGIN_CP,
                Login = USUARIO_LOGIN_CP,
                Nome = USUARIO_LOGIN_CP,
                PerfilAtual = Guid.Parse(PerfilUsuario.CP.ObterNome()),
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
            });

            await collectionFixture.InserirNaBase(new Usuario()
            {
                CodigoRf = USUARIO_LOGIN_DIRETOR,
                Login = USUARIO_LOGIN_DIRETOR,
                Nome = USUARIO_LOGIN_DIRETOR,
                PerfilAtual = Guid.Parse(PerfilUsuario.DIRETOR.ObterNome()),
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
            });

            await collectionFixture.InserirNaBase(new Usuario()
            {
                CodigoRf = USUARIO_LOGIN_AD,
                Login = USUARIO_LOGIN_AD,
                Nome = USUARIO_LOGIN_AD,
                PerfilAtual = Guid.Parse(PerfilUsuario.AD.ObterNome()),
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
            });
            await collectionFixture.InserirNaBase(new Usuario()
            {
                CodigoRf = USUARIO_LOGIN_PAAI,
                Login = USUARIO_LOGIN_PAAI,
                Nome = USUARIO_LOGIN_PAAI,
                PerfilAtual = Guid.Parse(PerfilUsuario.PAAI.ObterNome()),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
            });
            await collectionFixture.InserirNaBase(new Usuario
            {
                Login = USUARIO_PAEE_LOGIN_5555555,
                CodigoRf = USUARIO_PAEE_LOGIN_5555555,
                PerfilAtual = Guid.Parse(PerfilUsuario.PAEE.ObterNome()),
                Nome = USUARIO_PAEE_LOGIN_5555555,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
            await collectionFixture.InserirNaBase(new Usuario
            {
                Login = USUARIO_LOGIN_PAP,
                CodigoRf = USUARIO_LOGIN_PAP,
                PerfilAtual = Guid.Parse(PerfilUsuario.PAP.ObterNome()),
                Nome = USUARIO_LOGIN_PAP,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
            await collectionFixture.InserirNaBase(new Usuario
            {
                Login = SISTEMA_NOME,
                CodigoRf = SISTEMA_NOME,
                PerfilAtual = Guid.Parse(PerfilUsuario.ADMSME.ObterNome()),
                Nome = SISTEMA_NOME,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        public async Task CriarTurma(Modalidade modalidade, bool turmaHistorica = false, bool turmasMesmaUe = false, int tipoTurnoEol = 0)
        {
            await collectionFixture.InserirNaBase(new Dominio.Turma
            {
                UeId = 1,
                Ano = TURMA_ANO_2,
                CodigoTurma = TURMA_CODIGO_1,
                Historica = turmaHistorica,
                ModalidadeCodigo = modalidade,
                AnoLetivo = turmaHistorica ? ANO_LETIVO_ANO_ANTERIOR : ANO_LETIVO_ANO_ATUAL,
                Semestre = SEMESTRE_1,
                Nome = TURMA_NOME_1,
                TipoTurma = TipoTurma.Regular,
                TipoTurno = tipoTurnoEol
            });

            await collectionFixture.InserirNaBase(new Dominio.Turma
            {
                UeId = turmasMesmaUe ? 1 : 2,
                Ano = TURMA_ANO_2,
                CodigoTurma = TURMA_CODIGO_2,
                Historica = turmaHistorica,
                ModalidadeCodigo = modalidade,
                AnoLetivo = turmaHistorica ? ANO_LETIVO_ANO_ANTERIOR : ANO_LETIVO_ANO_ATUAL,
                Semestre = SEMESTRE_1,
                Nome = TURMA_NOME_2,
                TipoTurma = TipoTurma.Regular,
                TipoTurno = tipoTurnoEol
            });

            await collectionFixture.InserirNaBase(new Dominio.Turma
            {
                UeId = turmasMesmaUe ? 1 : 3,
                Ano = TURMA_ANO_3,
                CodigoTurma = TURMA_CODIGO_3,
                Historica = turmaHistorica,
                ModalidadeCodigo = modalidade,
                AnoLetivo = turmaHistorica ? ANO_LETIVO_ANO_ANTERIOR : ANO_LETIVO_ANO_ATUAL,
                Semestre = SEMESTRE_1,
                Nome = TURMA_NOME_3,
                TipoTurma = TipoTurma.Regular,
                TipoTurno = tipoTurnoEol
            });
        }

        public async Task CriarTurma(Modalidade modalidade, string anoTurma, bool turmaHistorica = false,
            TipoTurma tipoTurma = TipoTurma.Regular, int tipoTurno = 0)
        {
            await collectionFixture.InserirNaBase(new Dominio.Turma
            {
                UeId = 1,
                Ano = anoTurma,
                CodigoTurma = TURMA_CODIGO_1,
                Historica = turmaHistorica,
                ModalidadeCodigo = modalidade,
                AnoLetivo = turmaHistorica ? ANO_LETIVO_ANO_ANTERIOR : ANO_LETIVO_ANO_ATUAL,
                Semestre = SEMESTRE_1,
                Nome = TURMA_NOME_1,
                TipoTurma = tipoTurma,
                TipoTurno = tipoTurno
            });
        }

        public async Task CriarTurma(Modalidade modalidade, string anoTurma, string codigoTurma, bool turmaHistorica = false)
        {
            await collectionFixture.InserirNaBase(new Dominio.Turma
            {
                UeId = 1,
                Ano = anoTurma,
                CodigoTurma = codigoTurma,
                Historica = turmaHistorica,
                ModalidadeCodigo = modalidade,
                AnoLetivo = turmaHistorica ? ANO_LETIVO_ANO_ANTERIOR : ANO_LETIVO_ANO_ATUAL,
                Semestre = SEMESTRE_1,
                Nome = TURMA_NOME_1
            });
        }
        public async Task CriarTurma(Modalidade modalidade, string anoTurma, string codigoTurma, TipoTurma tipoTurma, bool turmaHistorica = false)
        {
            await collectionFixture.InserirNaBase(new Dominio.Turma
            {
                UeId = 1,
                Ano = anoTurma,
                CodigoTurma = codigoTurma,
                Historica = turmaHistorica,
                ModalidadeCodigo = modalidade,
                AnoLetivo = turmaHistorica ? ANO_LETIVO_ANO_ANTERIOR : ANO_LETIVO_ANO_ATUAL,
                Semestre = SEMESTRE_1,
                Nome = TURMA_NOME_1,
                TipoTurma = tipoTurma
            });
        }
        public async Task CriarTurma(Modalidade modalidade, string anoTurma, string codigoTurma, TipoTurma tipoTurma, long ueId, int anoLetivo, bool turmaHistorica = false, string nomeTurma = null)
        {
            await collectionFixture.InserirNaBase(new Dominio.Turma
            {
                UeId = ueId,
                Ano = anoTurma,
                CodigoTurma = codigoTurma,
                Historica = turmaHistorica,
                ModalidadeCodigo = modalidade,
                AnoLetivo = anoLetivo,
                Semestre = SEMESTRE_1,
                Nome = nomeTurma ?? TURMA_NOME_1,
                TipoTurma = tipoTurma
            });
        }

        public async Task CriarDreUe(string codigoDre, string codigoUe)
        {
            await collectionFixture.InserirNaBase(new Dre
            {
                CodigoDre = codigoDre,
                Abreviacao = DRE_NOME_1,
                Nome = DRE_NOME_1
            });

            await collectionFixture.InserirNaBase(new Ue
            {
                CodigoUe = codigoUe,
                DreId = 1,
                Nome = UE_NOME_1,
            });

            await collectionFixture.InserirNaBase(new Ue
            {
                CodigoUe = codigoUe,
                DreId = 1,
                Nome = UE_NOME_2,
            });
        }

        public async Task CriarDreUe(string codigoDre, string nomeDre, string codigoUe, string nomeUe)
        {
            var dres = collectionFixture.ObterTodos<Dre>();
            long? idDre = dres.Where(d => d.CodigoDre.Equals(codigoDre)).FirstOrDefault()?.Id;

            if (!idDre.HasValue)
                await collectionFixture.InserirNaBase(new Dre
                {
                    CodigoDre = codigoDre,
                    Abreviacao = nomeDre,
                    Nome = nomeDre
                });

            await collectionFixture.InserirNaBase(new Ue
            {
                CodigoUe = codigoUe,
                DreId = idDre ?? dres.Count + 1,
                Nome = nomeUe,
            });
        }
        public async Task CriarAtividadeAvaliativaFundamental(DateTime dataAvaliacao)
        {
            await CrieTipoAtividade();
            await CriarAtividadeAvaliativa(dataAvaliacao, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), USUARIO_PROFESSOR_CODIGO_RF_2222222, false, ATIVIDADE_AVALIATIVA_1);
        }

        public async Task CrieTipoAtividade()
        {
            await collectionFixture.InserirNaBase(new TipoAvaliacao
            {
                Nome = "Avaliação bimestral",
                Descricao = "Avaliação bimestral",
                Situacao = true,
                AvaliacoesNecessariasPorBimestre = 1,
                Codigo = TipoAvaliacaoCodigo.AvaliacaoBimestral,
                CriadoPor = "Sistema",
                CriadoRF = "1",
                CriadoEm = DateTime.Now
            });
        }

        public async Task CriarAtividadeAvaliativa(DateTime dataAvaliacao, string componente, string rf, bool ehCj, long idAtividade)
        {
            await collectionFixture.InserirNaBase(new AtividadeAvaliativa
            {
                DreId = "1",
                UeId = "1",
                ProfessorRf = rf,
                TurmaId = TURMA_CODIGO_1,
                Categoria = CategoriaAtividadeAvaliativa.Normal,
                EhCj = ehCj,
                TipoAvaliacaoId = 1,
                NomeAvaliacao = "Avaliação 04",
                DescricaoAvaliacao = "Avaliação 04",
                DataAvaliacao = dataAvaliacao,
                CriadoPor = "Sistema",
                CriadoRF = "1",
                CriadoEm = DateTime.Now
            });

            await collectionFixture.InserirNaBase(new AtividadeAvaliativaDisciplina
            {
                AtividadeAvaliativaId = idAtividade,
                DisciplinaId = componente,
                CriadoPor = "Sistema",
                CriadoRF = "1",
                CriadoEm = DateTime.Now
            });
        }

        public async Task CriarAtividadeAvaliativaFundamental(
                                    DateTime dataAvaliacao,
                                    string componente,
                                    TipoAvaliacaoCodigo tipoAvalicao = TipoAvaliacaoCodigo.AvaliacaoBimestral,
                                    bool ehRegencia = false,
                                    bool ehCj = false,
                                    string rf = USUARIO_PROFESSOR_CODIGO_RF_2222222)
        {
            await CriaTipoAvaliacao(tipoAvalicao);

            await collectionFixture.InserirNaBase(new AtividadeAvaliativa
            {
                Id = 1,
                DreId = "1",
                UeId = "1",
                ProfessorRf = rf,
                TurmaId = TURMA_CODIGO_1,
                Categoria = CategoriaAtividadeAvaliativa.Normal,
                TipoAvaliacaoId = 1,
                NomeAvaliacao = "Avaliação 04",
                DescricaoAvaliacao = "Avaliação 04",
                DataAvaliacao = dataAvaliacao,
                EhRegencia = ehRegencia,
                EhCj = ehCj,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await collectionFixture.InserirNaBase(new AtividadeAvaliativaDisciplina
            {
                Id = 1,
                AtividadeAvaliativaId = 1,
                DisciplinaId = componente,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
        }
        public async Task CriarAtividadeAvaliativaRegencia(string componente, string nomeComponente)
        {

            await collectionFixture.InserirNaBase(new AtividadeAvaliativaRegencia
            {
                Id = 1,
                AtividadeAvaliativaId = 1,
                DisciplinaContidaRegenciaId = componente,
                DisciplinaContidaRegenciaNome = nomeComponente,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
        }

        public async Task CriarTipoCalendario(ModalidadeTipoCalendario tipoCalendario, bool considerarAnoAnterior = false, int semestre = SEMESTRE_1)
        {
            await collectionFixture.InserirNaBase(new TipoCalendario
            {
                AnoLetivo = considerarAnoAnterior ? ANO_LETIVO_ANO_ANTERIOR : ANO_LETIVO_ANO_ATUAL,
                Nome = considerarAnoAnterior ? NOME_TIPO_CALENDARIO_ANO_ANTERIOR : NOME_TIPO_CALENDARIO_ANO_ATUAL,
                Periodo = Periodo.Semestral,
                Modalidade = tipoCalendario,
                Situacao = true,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Excluido = false,
                Migrado = false,
                Semestre = tipoCalendario.EhEjaOuCelp() ? semestre : null
            });
        }

        public async Task CriarItensComuns(bool criarPeriodo, DateTime dataInicio, DateTime dataFim, int bimestre, long tipoCalendarioId = 1)
        {
            await CriarDreUePerfil();
            if (criarPeriodo) await CriarPeriodoEscolar(dataInicio, dataFim, bimestre, tipoCalendarioId);
            await CriarComponenteCurricular();
        }

        public async Task CriarDreUePerfilComponenteCurricular()
        {
            await CriarDreUePerfil();
            await CriarComponenteCurricular();
        }

        public async Task CriaTipoAvaliacao(TipoAvaliacaoCodigo tipoAvalicao)
        {
            await collectionFixture.InserirNaBase(new TipoAvaliacao
            {
                Id = 1,
                Nome = "Avaliação bimestral",
                Descricao = "Avaliação bimestral",
                Situacao = true,
                AvaliacoesNecessariasPorBimestre = 1,
                Codigo = tipoAvalicao,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
        }

        public async Task CriarAtividadeAvaliativa(DateTime dataAvaliacao, string componente, TipoAvaliacaoCodigo tipoAvalicao = TipoAvaliacaoCodigo.AvaliacaoBimestral, bool ehRegencia = false,
                                                      bool ehCj = false, string rf = USUARIO_PROFESSOR_CODIGO_RF_2222222)
        {
            await CriaTipoAvaliacao(tipoAvalicao);

            await collectionFixture.InserirNaBase(new AtividadeAvaliativa
            {
                Id = 1,
                DreId = "1",
                UeId = "1",
                ProfessorRf = rf,
                TurmaId = TURMA_CODIGO_1,
                Categoria = CategoriaAtividadeAvaliativa.Normal,
                TipoAvaliacaoId = 1,
                NomeAvaliacao = "Avaliação 04",
                DescricaoAvaliacao = "Avaliação 04",
                DataAvaliacao = dataAvaliacao,
                EhRegencia = ehRegencia,
                EhCj = ehCj,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await collectionFixture.InserirNaBase(new AtividadeAvaliativaDisciplina
            {
                Id = 1,
                AtividadeAvaliativaId = 1,
                DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
        }

        public async Task CriarDreUePerfil()
        {
            await collectionFixture.InserirNaBase(new Dre
            {
                CodigoDre = DRE_CODIGO_1,
                Abreviacao = DRE_NOME_1,
                Nome = DRE_NOME_1
            });

            await collectionFixture.InserirNaBase(new Ue
            {
                CodigoUe = UE_CODIGO_1,
                DreId = 1,
                Nome = UE_NOME_1,
                TipoEscola = TipoEscola.EMEF
            });

            await collectionFixture.InserirNaBase(new Dre
            {
                CodigoDre = DRE_CODIGO_2,
                Abreviacao = DRE_NOME_2,
                Nome = DRE_NOME_2
            });

            await collectionFixture.InserirNaBase(new Ue
            {
                CodigoUe = UE_CODIGO_2,
                DreId = 2,
                Nome = UE_NOME_2,
                TipoEscola = TipoEscola.EMEF
            });

            await collectionFixture.InserirNaBase(new Ue
            {
                CodigoUe = UE_CODIGO_3,
                DreId = 2,
                Nome = UE_NOME_3,
                TipoEscola = TipoEscola.EMEF
            });

            var teste = collectionFixture.ObterTodos<PrioridadePerfil>();

            await collectionFixture.InserirNaBase(new PrioridadePerfil
            {
                CodigoPerfil = Guid.Parse(PerfilUsuario.PROFESSOR.Name()),
                NomePerfil = PROFESSOR,
                Ordem = ORDEM_290,
                Tipo = TipoPerfil.UE,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await collectionFixture.InserirNaBase(new PrioridadePerfil
            {
                CodigoPerfil = Guid.Parse(PerfilUsuario.CJ.Name()),
                NomePerfil = PROFESSOR_CJ,
                Ordem = ORDEM_320,
                Tipo = TipoPerfil.UE,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await collectionFixture.InserirNaBase(new PrioridadePerfil()
            {
                Ordem = ORDEM_240,
                Tipo = TipoPerfil.UE,
                NomePerfil = CP,
                CodigoPerfil = Perfis.PERFIL_CP,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await collectionFixture.InserirNaBase(new PrioridadePerfil()
            {
                Ordem = 230,
                Tipo = TipoPerfil.UE,
                NomePerfil = "AD",
                CodigoPerfil = Perfis.PERFIL_AD,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await collectionFixture.InserirNaBase(new PrioridadePerfil()
            {
                Ordem = 220,
                Tipo = TipoPerfil.UE,
                NomePerfil = "DIRETOR",
                CodigoPerfil = Perfis.PERFIL_DIRETOR,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await collectionFixture.InserirNaBase("tipo_escola", new string[] { "cod_tipo_escola_eol", "descricao", "criado_em", "criado_por", "criado_rf" }, new string[] { "1", "'EMEF'", "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'", "'" + SISTEMA_NOME + "'", "'" + SISTEMA_CODIGO_RF + "'" });
        }

        public async Task CriarPeriodoEscolar(DateTime dataInicio, DateTime dataFim, int bimestre, long tipoCalendarioId = 1, bool considerarAnoAnterior = false)
        {
            await collectionFixture.InserirNaBase(new PeriodoEscolar
            {
                TipoCalendarioId = tipoCalendarioId,
                Bimestre = bimestre,
                PeriodoInicio = considerarAnoAnterior ? dataInicio.AddYears(-1) : dataInicio,
                PeriodoFim = considerarAnoAnterior ? dataFim.AddYears(-1) : dataFim,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                Migrado = false
            });
        }
        public async Task CriarComponenteRegenciaInfantil()
        {
            await collectionFixture.InserirNaBase(COMPONENTE_CURRICULAR_GRUPO_MATRIZ, CODIGO_1, GRUPO_MATRIZ_1);
            await collectionFixture.InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_CURRICULAR_512.ToString(), COMPONENTE_CURRICULAR_512.ToString(), CODIGO_1, NULO, COMPONENTE_ED_INF_EMEI_4HS_NOME, TRUE, FALSE, FALSE, FALSE, TRUE, FALSE, COMPONENTE_REGENCIA_CLASSE_INFANTIL_NOME, COMPONENTE_REGENCIA_INFANTIL_EMEI_4H_NOME);
            await collectionFixture.InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_CURRICULAR_513.ToString(), COMPONENTE_CURRICULAR_513.ToString(), CODIGO_1, NULO, COMPONENTE_ED_INF_EMEI_2HS_NOME, TRUE, FALSE, FALSE, FALSE, TRUE, FALSE, COMPONENTE_REGENCIA_CLASSE_INFANTIL_NOME, COMPONENTE_REGENCIA_INFANTIL_EMEI_2H_NOME);
        }

        public async Task CriarComponenteCurricular()
        {
            await collectionFixture.InserirNaBase(COMPONENTE_CURRICULAR_GRUPO_MATRIZ, CODIGO_1, GRUPO_MATRIZ_1);
            await collectionFixture.InserirNaBase(COMPONENTE_CURRICULAR_GRUPO_MATRIZ, CODIGO_2, GRUPO_MATRIZ_2);
            await collectionFixture.InserirNaBase(COMPONENTE_CURRICULAR_GRUPO_MATRIZ, CODIGO_3, GRUPO_MATRIZ_3);
            await collectionFixture.InserirNaBase(COMPONENTE_CURRICULAR_GRUPO_MATRIZ, CODIGO_4, GRUPO_MATRIZ_4);
            await collectionFixture.InserirNaBase(COMPONENTE_CURRICULAR_GRUPO_MATRIZ, CODIGO_8, GRUPO_MATRIZ_8);
            await collectionFixture.InserirNaBase(COMPONENTE_CURRICULAR_GRUPO_MATRIZ, CODIGO_7, GRUPO_MATRIZ_7);


            await collectionFixture.InserirNaBase(COMPONENTE_CURRICULAR_AREA_CONHECIMENTO, CODIGO_1, AREA_DE_CONHECIMENTO_1);
            await collectionFixture.InserirNaBase(COMPONENTE_CURRICULAR_AREA_CONHECIMENTO, CODIGO_2, AREA_DE_CONHECIMENTO_2);
            await collectionFixture.InserirNaBase(COMPONENTE_CURRICULAR_AREA_CONHECIMENTO, CODIGO_3, AREA_DE_CONHECIMENTO_3);
            await collectionFixture.InserirNaBase(COMPONENTE_CURRICULAR_AREA_CONHECIMENTO, CODIGO_4, AREA_DE_CONHECIMENTO_4);
            await collectionFixture.InserirNaBase(COMPONENTE_CURRICULAR_AREA_CONHECIMENTO, CODIGO_5, AREA_DE_CONHECIMENTO_5);
            await collectionFixture.InserirNaBase(COMPONENTE_CURRICULAR_AREA_CONHECIMENTO, CODIGO_8, AREA_DE_CONHECIMENTO_8);
            await collectionFixture.InserirNaBase(COMPONENTE_CURRICULAR_AREA_CONHECIMENTO, CODIGO_10, AREA_DE_CONHECIMENTO_10);

            await collectionFixture.InserirNaBase(COMPONENTE_CURRICULAR_GRUPO_AREA_ORDENACAO, CODIGO_1, CODIGO_1, CODIGO_1);
            await collectionFixture.InserirNaBase(COMPONENTE_CURRICULAR_GRUPO_AREA_ORDENACAO, CODIGO_2, CODIGO_2, CODIGO_2);
            await collectionFixture.InserirNaBase(COMPONENTE_CURRICULAR_GRUPO_AREA_ORDENACAO, CODIGO_3, CODIGO_3, CODIGO_3);
            await collectionFixture.InserirNaBase(COMPONENTE_CURRICULAR_GRUPO_AREA_ORDENACAO, CODIGO_4, CODIGO_4, CODIGO_4);
            await collectionFixture.InserirNaBase(COMPONENTE_CURRICULAR_GRUPO_AREA_ORDENACAO, CODIGO_4, CODIGO_5, CODIGO_5);
            await collectionFixture.InserirNaBase(COMPONENTE_CURRICULAR_GRUPO_AREA_ORDENACAO, CODIGO_8, CODIGO_8, CODIGO_8);
            await collectionFixture.InserirNaBase(COMPONENTE_CURRICULAR_GRUPO_AREA_ORDENACAO, CODIGO_8, CODIGO_10, CODIGO_10);

            await collectionFixture.InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), NULO, CODIGO_1, CODIGO_1, COMPONENTE_CURRICULAR_LINGUA_PORTUGUESA_NOME, FALSE, FALSE, FALSE, TRUE, TRUE, TRUE, COMPONENTE_CURRICULAR_LINGUA_PORTUGUESA_NOME, NULO);
            await collectionFixture.InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_CURRICULAR_ARTES_ID_139.ToString(), NULO, CODIGO_1, CODIGO_1, COMPONENTE_CURRICULAR_ARTES_NOME, FALSE, FALSE, FALSE, TRUE, TRUE, TRUE, COMPONENTE_CURRICULAR_ARTES_NOME, NULO);
            await collectionFixture.InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_MATEMATICA_ID_2.ToString(), NULO, CODIGO_1, CODIGO_2, COMPONENTE_CURRICULAR_MATEMATICA_NOME, FALSE, FALSE, FALSE, TRUE, TRUE, TRUE, COMPONENTE_CURRICULAR_MATEMATICA_NOME, NULO);
            await collectionFixture.InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_GEOGRAFIA_ID_8.ToString(), NULO, CODIGO_1, CODIGO_1, COMPONENTE_GEOGRAFIA_NOME, FALSE, FALSE, FALSE, TRUE, TRUE, TRUE, COMPONENTE_GEOGRAFIA_NOME, NULO);
            await collectionFixture.InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213.ToString(), NULO, CODIGO_1, NULO, COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_EOL, TRUE, FALSE, FALSE, FALSE, TRUE, TRUE, COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_NOME, NULO);
            await collectionFixture.InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105.ToString(), NULO, CODIGO_1, NULO, COMPONENTE_REG_CLASSE_CICLO_ALFAB_INTERD_5HRS_EOL_1105, TRUE, FALSE, FALSE, FALSE, TRUE, TRUE, COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_NOME_1105, NULO);
            await collectionFixture.InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_REGENCIA_CLASSE_EJA_BASICA_ID_1114.ToString(), NULO, CODIGO_1, CODIGO_8, COMPONENTE_REG_CLASSE_EJA_ETAPA_BASICA_EOL_1114, TRUE, FALSE, FALSE, FALSE, TRUE, TRUE, COMPONENTE_REGENCIA_CLASSE_EJA_BASICA_NOME_1114, NULO);
            await collectionFixture.InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_CURRICULAR_AULA_COMPARTILHADA.ToString(), COMPONENTE_CURRICULAR_AULA_COMPARTILHADA.ToString(), CODIGO_1, CODIGO_1, COMPONENTE_CURRICULAR_AULA_COMPARTILHADA_NOME, FALSE, TRUE, FALSE, FALSE, FALSE, TRUE, COMPONENTE_CURRICULAR_AULA_COMPARTILHADA_NOME, COMPONENTE_CURRICULAR_AULA_COMPARTILHADA_NOME);
            await collectionFixture.InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_CURRICULAR_AEE_COLABORATIVO.ToString(), COMPONENTE_CURRICULAR_AEE_COLABORATIVO.ToString(), CODIGO_1, CODIGO_1, COMPONENTE_CURRICULAR_AEE_COLABORATIVO_NOME, FALSE, TRUE, FALSE, FALSE, FALSE, TRUE, COMPONENTE_CURRICULAR_AEE_COLABORATIVO_NOME, COMPONENTE_CURRICULAR_AEE_COLABORATIVO_NOME);
            await collectionFixture.InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_HISTORIA_ID_7.ToString(), NULO, CODIGO_1, CODIGO_4, COMPONENTE_HISTORIA_NOME, FALSE, FALSE, FALSE, TRUE, TRUE, TRUE, COMPONENTE_HISTORIA_NOME, NULO);
            await collectionFixture.InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_CURRICULAR_LEITURA_OSL_ID_1061.ToString(), NULO, CODIGO_3, CODIGO_8, COMPONENTE_LEITURA_OSL_NOME, FALSE, FALSE, FALSE, FALSE, TRUE, FALSE, COMPONENTE_LEITURA_OSL_NOME, NULO);
            await collectionFixture.InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_CURRICULAR_INGLES_ID_9.ToString(), NULO, CODIGO_2, CODIGO_5, COMPONENTE_CURRICULAR_INGLES_NOME, FALSE, FALSE, FALSE, FALSE, TRUE, TRUE, COMPONENTE_CURRICULAR_INGLES_NOME, NULO);

            await collectionFixture.InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_EDUCACAO_FISICA_ID_6.ToString(), NULO, CODIGO_1, CODIGO_1, COMPONENTE_EDUCACAO_FISICA_NOME, FALSE, FALSE, FALSE, TRUE, TRUE, TRUE, COMPONENTE_EDUCACAO_FISICA_NOME, NULO);
            await collectionFixture.InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_CIENCIAS_ID_89.ToString(), NULO, CODIGO_1, CODIGO_3, COMPONENTE_CIENCIAS_NOME, FALSE, FALSE, FALSE, TRUE, TRUE, TRUE, COMPONENTE_CIENCIAS_NOME, NULO);
            await collectionFixture.InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_CURRICULAR_INFORMATICA_OIE_ID_1060.ToString(), NULO, CODIGO_3, NULO, COMPONENTE_CURRICULAR_INFORMATICA_OIE_NOME, FALSE, FALSE, FALSE, FALSE, TRUE, FALSE, COMPONENTE_CURRICULAR_INFORMATICA_OIE_NOME, NULO);

            await collectionFixture.InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_TERRITORIO_SABER_EXP_PEDAG_ID_1214.ToString(), NULO, CODIGO_4, NULO, COMPONENTE_TERRITORIO_SABER_EXP_PEDAG_NOME, FALSE, FALSE, TRUE, FALSE, TRUE, FALSE, COMPONENTE_TERRITORIO_SABER_EXP_PEDAG_NOME, NULO);
            await collectionFixture.InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_CURRICULAR_512.ToString(), COMPONENTE_CURRICULAR_512.ToString(), CODIGO_1, NULO, COMPONENTE_ED_INF_EMEI_4HS_NOME, TRUE, FALSE, FALSE, FALSE, TRUE, FALSE, COMPONENTE_REGENCIA_CLASSE_INFANTIL_NOME, COMPONENTE_REGENCIA_INFANTIL_EMEI_4H_NOME);

            await collectionFixture.InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_CURRICULAR_PAP_PROJETO_COLABORATIVO.ToString(), COMPONENTE_CURRICULAR_PAP_PROJETO_COLABORATIVO.ToString(), CODIGO_1, CODIGO_1, COMPONENTE_CURRICULAR_PAP_PROJETO_COLABORATIVO_NOME, FALSE, FALSE, FALSE, FALSE, TRUE, TRUE, COMPONENTE_CURRICULAR_PAP_PROJETO_COLABORATIVO_NOME, COMPONENTE_CURRICULAR_PAP_PROJETO_COLABORATIVO_NOME);
            await collectionFixture.InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_CURRICULAR_513.ToString(), COMPONENTE_CURRICULAR_512.ToString(), CODIGO_1, NULO, COMPONENTE_ED_INF_EMEI_2HS_NOME, TRUE, FALSE, FALSE, FALSE, TRUE, FALSE, COMPONENTE_REGENCIA_CLASSE_INFANTIL_NOME, COMPONENTE_REGENCIA_INFANTIL_EMEI_2H_NOME);
            await collectionFixture.InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_CURRICULAR_TEC_APRENDIZAGEM.ToString(), NULO, CODIGO_7, CODIGO_10, COMPONENTE_CURRICULAR_TEC_APRENDIZAGEM_NOME, FALSE, FALSE, FALSE, FALSE, TRUE, FALSE, COMPONENTE_CURRICULAR_TEC_APRENDIZAGEM_NOME, NULO);

            await collectionFixture.InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_CURRICULAR_LIBRAS_COMPARTILHADA_ID_1116.ToString(), NULO, CODIGO_1, CODIGO_1, COMPONENTE_CURRICULAR_LIBRAS_COMPARTILHADA_NOME, FALSE, FALSE, FALSE, FALSE, FALSE, TRUE, COMPONENTE_CURRICULAR_LIBRAS_COMPARTILHADA_NOME, NULO);
        }

        public async Task CriarPeriodoEscolarCustomizadoQuartoBimestre(bool periodoEscolarValido = false)
        {
            var dataReferencia = DateTimeExtension.HorarioBrasilia().Date;

            await CriarPeriodoEscolar(dataReferencia.AddDays(-285), dataReferencia.AddDays(-210), BIMESTRE_1);

            await CriarPeriodoEscolar(dataReferencia.AddDays(-200), dataReferencia.AddDays(-125), BIMESTRE_2);

            await CriarPeriodoEscolar(dataReferencia.AddDays(-115), dataReferencia.AddDays(-40), BIMESTRE_3);

            await CriarPeriodoEscolar(dataReferencia.AddDays(-20), periodoEscolarValido ? dataReferencia : dataReferencia.AddDays(-5), BIMESTRE_4);
        }

        public async Task CriarAula(DateTime dataAula, RecorrenciaAula recorrenciaAula, TipoAula tipoAula, string professorRf, string turmaCodigo, string ueCodigo, string disciplinaCodigo, long tipoCalendarioId, bool aulaCJ = false)
        {
            await collectionFixture.InserirNaBase(new Dominio.Aula()
            {
                UeId = ueCodigo,
                DisciplinaId = disciplinaCodigo,
                TurmaId = turmaCodigo,
                TipoCalendarioId = tipoCalendarioId,
                ProfessorRf = professorRf,
                Quantidade = 1,
                DataAula = dataAula.Date,
                RecorrenciaAula = recorrenciaAula,
                TipoAula = tipoAula,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Excluido = false,
                AulaCJ = aulaCJ
            });
        }

        public async Task CriarPeriodoEscolarReabertura(long tipoCalendarioId)
        {
            await CriarPeriodoEscolar(DATA_03_01, DATA_28_04, BIMESTRE_1, tipoCalendarioId);
            await CriarPeriodoEscolar(DATA_02_05, DATA_08_07, BIMESTRE_2, tipoCalendarioId);
            await CriarPeriodoEscolar(DATA_25_07, DATA_30_09, BIMESTRE_3, tipoCalendarioId);
            await CriarPeriodoEscolar(DATA_03_10, DATA_22_12, BIMESTRE_4, tipoCalendarioId);

            await CriarPeriodoReabertura(tipoCalendarioId);
        }

        public async Task CriarPeriodoReabertura(long tipoCalendarioId, DateTime? dataInicio = null, DateTime? dataFim = null)
        {
            await collectionFixture.InserirNaBase(new FechamentoReabertura()
            {
                Descricao = REABERTURA_GERAL,
                Inicio = dataInicio.HasValue ? dataInicio.Value : DATA_01_01,
                Fim = dataFim.HasValue ? dataFim.Value : DATA_31_12,
                TipoCalendarioId = tipoCalendarioId,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await collectionFixture.InserirNaBase(new FechamentoReaberturaBimestre()
            {
                FechamentoAberturaId = 1,
                Bimestre = BIMESTRE_1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await collectionFixture.InserirNaBase(new FechamentoReaberturaBimestre()
            {
                FechamentoAberturaId = 1,
                Bimestre = BIMESTRE_2,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await collectionFixture.InserirNaBase(new FechamentoReaberturaBimestre()
            {
                FechamentoAberturaId = 1,
                Bimestre = BIMESTRE_3,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await collectionFixture.InserirNaBase(new FechamentoReaberturaBimestre()
            {
                FechamentoAberturaId = 1,
                Bimestre = BIMESTRE_4,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });
        }

        public async Task CrieConceitoValores()
        {
            await collectionFixture.InserirNaBase(new Conceito()
            {
                Valor = PLENAMENTE_SATISFATORIO,
                InicioVigencia = DATA_01_01,
                Ativo = true,
                Descricao = ConceitoValores.P.GetAttribute<DisplayAttribute>().Name,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });
            await collectionFixture.InserirNaBase(new Conceito()
            {
                Valor = SATISFATORIO,
                InicioVigencia = DATA_01_01,
                Ativo = true,
                Descricao = ConceitoValores.S.GetAttribute<DisplayAttribute>().Name,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });
            await collectionFixture.InserirNaBase(new Conceito()
            {
                Valor = NAO_SATISFATORIO,
                InicioVigencia = DATA_01_01,
                Ativo = true,
                Descricao = ConceitoValores.NS.GetAttribute<DisplayAttribute>().Name,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });
        }

        public async Task CriarParametrosSistema(int ano)
        {
            await collectionFixture.InserirNaBase(new ParametrosSistema()
            {
                Nome = ConstantesTeste.PERCENTUAL_FREQUENCIA_CRITICO_TIPO_4_NOME,
                Descricao = ConstantesTeste.PERCENTUAL_FREQUENCIA_CRITICO_TIPO_4_DESCRICAO,
                Tipo = TipoParametroSistema.PercentualFrequenciaCritico,
                Valor = ConstantesTeste.VALOR_75,
                Ano = ano,
                Ativo = true,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoPor = SISTEMA_NOME
            });

            await collectionFixture.InserirNaBase(new ParametrosSistema()
            {
                Nome = ConstantesTeste.PERCENTUAL_FREQUENCIA_MINIMO_INFANTIL_TIPO_27_NOME,
                Descricao = ConstantesTeste.PERCENTUAL_FREQUENCIA_MINIMO_INFANTIL_TIPO_27_DESCRICAO,
                Tipo = TipoParametroSistema.PercentualFrequenciaMinimaInfantil,
                Valor = ConstantesTeste.VALOR_60,
                Ano = ano,
                Ativo = true,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoPor = SISTEMA_NOME
            });
        }

        public async Task CriarVariosTiposCalendariosEPeriodosEscolaresParaEjaCelpFundamental()
        {
            await CriarTipoCalendario(ModalidadeTipoCalendario.EJA, true, SEMESTRE_1);
            await CriarPeriodoEscolar(DATA_01_01_INICIO_BIMESTRE_1, DATA_01_05_FIM_BIMESTRE_1, BIMESTRE_1, TIPO_CALENDARIO_1, true);
            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_24_07_FIM_BIMESTRE_2, BIMESTRE_2, TIPO_CALENDARIO_1, true);

            await CriarTipoCalendario(ModalidadeTipoCalendario.EJA, true, SEMESTRE_2);
            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_02_10_FIM_BIMESTRE_3, BIMESTRE_1, TIPO_CALENDARIO_2, true);
            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_2, TIPO_CALENDARIO_2, true);

            await CriarTipoCalendario(ModalidadeTipoCalendario.CELP, true, SEMESTRE_1);
            await CriarPeriodoEscolar(DATA_01_01_INICIO_BIMESTRE_1, DATA_01_05_FIM_BIMESTRE_1, BIMESTRE_1, TIPO_CALENDARIO_3, true);
            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_24_07_FIM_BIMESTRE_2, BIMESTRE_2, TIPO_CALENDARIO_3, true);

            await CriarTipoCalendario(ModalidadeTipoCalendario.CELP, true, SEMESTRE_2);
            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_02_10_FIM_BIMESTRE_3, BIMESTRE_1, TIPO_CALENDARIO_4, true);
            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_2, TIPO_CALENDARIO_4, true);

            await CriarTipoCalendario(ModalidadeTipoCalendario.FundamentalMedio, true);
            await CriarPeriodoEscolar(DATA_01_01_INICIO_BIMESTRE_1, DATA_01_05_FIM_BIMESTRE_1, BIMESTRE_1, TIPO_CALENDARIO_5, true);
            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_24_07_FIM_BIMESTRE_2, BIMESTRE_2, TIPO_CALENDARIO_5, true);
            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_02_10_FIM_BIMESTRE_3, BIMESTRE_3, TIPO_CALENDARIO_5, true);
            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_4, TIPO_CALENDARIO_5, true);

            await CriarTipoCalendario(ModalidadeTipoCalendario.EJA, semestre: 1);
            await CriarPeriodoEscolar(DATA_01_01_INICIO_BIMESTRE_1, DATA_01_05_FIM_BIMESTRE_1, BIMESTRE_1, TIPO_CALENDARIO_6);
            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_24_07_FIM_BIMESTRE_2, BIMESTRE_2, TIPO_CALENDARIO_6);

            await CriarTipoCalendario(ModalidadeTipoCalendario.EJA, semestre: 2);
            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_02_10_FIM_BIMESTRE_3, BIMESTRE_1, TIPO_CALENDARIO_7);
            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_2, TIPO_CALENDARIO_7);

            await CriarTipoCalendario(ModalidadeTipoCalendario.CELP, semestre: 1);
            await CriarPeriodoEscolar(DATA_01_01_INICIO_BIMESTRE_1, DATA_01_05_FIM_BIMESTRE_1, BIMESTRE_1, TIPO_CALENDARIO_8);
            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_24_07_FIM_BIMESTRE_2, BIMESTRE_2, TIPO_CALENDARIO_8);

            await CriarTipoCalendario(ModalidadeTipoCalendario.CELP, semestre: 2);
            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_02_10_FIM_BIMESTRE_3, BIMESTRE_1, TIPO_CALENDARIO_9);
            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_2, TIPO_CALENDARIO_9);

            await CriarTipoCalendario(ModalidadeTipoCalendario.FundamentalMedio);
            await CriarPeriodoEscolar(DATA_01_01_INICIO_BIMESTRE_1, DATA_01_05_FIM_BIMESTRE_1, BIMESTRE_1, TIPO_CALENDARIO_10);
            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_24_07_FIM_BIMESTRE_2, BIMESTRE_2, TIPO_CALENDARIO_10);
            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_02_10_FIM_BIMESTRE_3, BIMESTRE_3, TIPO_CALENDARIO_10);
            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_4, TIPO_CALENDARIO_10);
        }
        public async Task CriarConselhoClasseConsolidadoTurmaAlunos()
        {
            await collectionFixture.InserirNaBase(new ConselhoClasseConsolidadoTurmaAluno()
            {
                Id = 1,
                Status = SituacaoConselhoClasse.NaoIniciado,
                AlunoCodigo = "1",
                TurmaId = 1,
                ParecerConclusivoId = null,
                CriadoRF = "1",
                CriadoPor = "1",
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            await collectionFixture.InserirNaBase(new ConselhoClasseConsolidadoTurmaAluno()
            {
                Id = 2,
                Status = SituacaoConselhoClasse.NaoIniciado,
                AlunoCodigo = "2",
                TurmaId = 1,
                ParecerConclusivoId = null,
                CriadoRF = "1",
                CriadoPor = "1",
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            await collectionFixture.InserirNaBase(new ConselhoClasseConsolidadoTurmaAluno()
            {
                Id = 3,
                Status = SituacaoConselhoClasse.NaoIniciado,
                AlunoCodigo = "3",
                TurmaId = 1,
                ParecerConclusivoId = null,
                CriadoRF = "1",
                CriadoPor = "1",
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            await collectionFixture.InserirNaBase(new ConselhoClasseConsolidadoTurmaAluno()
            {
                Id = 4,
                Status = SituacaoConselhoClasse.NaoIniciado,
                AlunoCodigo = "4",
                TurmaId = 1,
                ParecerConclusivoId = null,
                CriadoRF = "1",
                CriadoPor = "1",
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            await collectionFixture.InserirNaBase(new ConselhoClasseConsolidadoTurmaAluno()
            {
                Id = 5,
                Status = SituacaoConselhoClasse.NaoIniciado,
                AlunoCodigo = "5",
                TurmaId = 1,
                ParecerConclusivoId = null,
                CriadoRF = "1",
                CriadoPor = "1",
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            await collectionFixture.InserirNaBase(new ConselhoClasseConsolidadoTurmaAluno()
            {
                Id = 6,
                Status = SituacaoConselhoClasse.EmAndamento,
                AlunoCodigo = "6",
                TurmaId = 1,
                ParecerConclusivoId = null,
                CriadoRF = "1",
                CriadoPor = "1",
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            await collectionFixture.InserirNaBase(new ConselhoClasseConsolidadoTurmaAluno()
            {
                Id = 7,
                Status = SituacaoConselhoClasse.EmAndamento,
                AlunoCodigo = "7",
                TurmaId = 1,
                ParecerConclusivoId = null,
                CriadoRF = "1",
                CriadoPor = "1",
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            await collectionFixture.InserirNaBase(new ConselhoClasseConsolidadoTurmaAluno()
            {
                Id = 8,
                Status = SituacaoConselhoClasse.EmAndamento,
                AlunoCodigo = "8",
                TurmaId = 1,
                ParecerConclusivoId = null,
                CriadoRF = "1",
                CriadoPor = "1",
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            await collectionFixture.InserirNaBase(new ConselhoClasseConsolidadoTurmaAluno()
            {
                Id = 9,
                Status = SituacaoConselhoClasse.Concluido,
                AlunoCodigo = "9",
                TurmaId = 1,
                ParecerConclusivoId = null,
                CriadoRF = "1",
                CriadoPor = "1",
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
        }
        public DateTime ObterTerceiroSabadoDoMes(int ano, int mes)
        {
            // Define a data do primeiro dia do mês
            DateTime primeiroDiaDoMes = new DateTime(ano, mes, 1);

            // Obtém o primeiro sábado do mês
            DateTime primeiroSabado = primeiroDiaDoMes.AddDays((DayOfWeek.Saturday - primeiroDiaDoMes.DayOfWeek + 7) % 7);

            // Calcula o terceiro sábado adicionando 14 dias ao primeiro sábado
            DateTime terceiroSabado = primeiroSabado.AddDays(14);

            return terceiroSabado;
        }
    }
}
