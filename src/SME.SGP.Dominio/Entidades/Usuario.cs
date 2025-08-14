using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SME.SGP.Dominio
{
    public class Usuario : EntidadeBase
    {
        private const string MENSAGEM_ERRO_USUARIO_SEM_ACESSO = "Usuário sem perfis de acesso.";

        public string CodigoRf { get; set; }

        public DateTime? ExpiracaoRecuperacaoSenha { get; set; }

        public string Login { get; set; }

        public string Nome { get; set; }

        public Guid PerfilAtual { get; set; }

        public IEnumerable<PrioridadePerfil> Perfis { get; private set; }

        public Guid? TokenRecuperacaoSenha { get; set; }

        public DateTime UltimoLogin { get; set; }

        private string Email { get; set; }

        public void AtualizaUltimoLogin()
            => UltimoLogin = DateTime.Now;

        public void DefinirEmail(string novoEmail)
        {
            if ((PossuiPerfilDre() || PossuiPerfilSme()) && !novoEmail.Contains("@sme.prefeitura.sp.gov.br"))
                throw new NegocioException("Usuários da SME ou DRE devem utilizar e-mail profissional. Ex: usuario@sme.prefeitura.sp.gov.br");

            Email = novoEmail;
        }

        public void DefinirPerfilAtual(Guid perfilAtual)
            => PerfilAtual = perfilAtual;

        public IEnumerable<Aula> ObterAulasQuePodeVisualizar(IEnumerable<Aula> aulas, IList<ComponenteCurricularEol> componentesUsuario)
        {
            if (TemPerfilGestaoUes() || TemPerfilAdmUE())
                return aulas;
            else
            {
                if (EhProfessorCj())
                    return aulas.Where(a => a.ProfessorRf == CodigoRf && a.AulaCJ);
                else
                {
                    return (from a in aulas
                            from ccp in componentesUsuario
                            where ((!ccp.TerritorioSaber && a.DisciplinaId == (ccp.CodigoComponenteCurricularPai ?? ccp.Codigo).ToString()) ||
                                   (ccp.TerritorioSaber && (a.DisciplinaId == ccp.Codigo.ToString() || a.DisciplinaId == ccp.CodigoComponenteTerritorioSaber.ToString()))) ||
                                  a.ProfessorRf == CodigoRf
                            select a).Distinct();
                }
            }
        }

        public bool EhProfessorInfantilOuCjInfantil()
            => EhProfessorInfantil() || EhProfessorCjInfantil();

        public IEnumerable<AtividadeAvaliativa> ObterAtividadesAvaliativasQuePodeVisualizar(IEnumerable<AtividadeAvaliativa> atividades, string[] componentesCurricularesProfessor)
        {
            if (TemPerfilGestaoUes())
                return atividades;

            else
            {
                if (EhProfessorCj())
                    return atividades.Where(a => a.ProfessorRf == CodigoRf);

                else
                    return atividades.Where(a => (componentesCurricularesProfessor.Intersect(a.Disciplinas.Select(d => d.DisciplinaId))
                                                                                  .Any() && !a.EhCj || a.Disciplinas.Select(item => item.DisciplinaId)
                                                                                                                    .Any() && a.EhCj));
            }
        }

        public void DefinirPerfis(IEnumerable<PrioridadePerfil> perfisUsuario)
         => Perfis = perfisUsuario;

        public bool EhPerfilDRE()
         => Perfis.Any(c => c.Tipo == TipoPerfil.DRE && c.CodigoPerfil == PerfilAtual);

        public bool EhPerfilSME()
         => Perfis.Any(c => c.Tipo == TipoPerfil.SME && c.CodigoPerfil == PerfilAtual);

        public bool EhPerfilUE()
         => Perfis.Any(c => c.Tipo == TipoPerfil.UE && c.CodigoPerfil == PerfilAtual);

        public bool EhPerfilAD()
         => Perfis.Any(c => c.CodigoPerfil == PerfilAtual && PerfilAtual == Dominio.Perfis.PERFIL_AD);

        public bool EhCoordenadorCEFAI()
         => PerfilAtual == Dominio.Perfis.PERFIL_CEFAI;

        public bool EhPerfilPaai()
            => PerfilAtual == Dominio.Perfis.PERFIL_PAAI;

        public bool EhPerfilProfessor()
         => EhProfessor()
         || EhProfessorCj()
         || EhProfessorInfantil()
         || EhProfessorCjInfantil()
         || EhProfessorPoa()
         || EhProfessorPaee()
         || EhProfessorPap()
         || EhProfessorPoei()
         || EhProfessorPoed()
         || EhProfessorPosl();

        public bool EhProfessorPaee()
         => PerfilAtual == Dominio.Perfis.PERFIL_PAEE;

        public bool EhProfessorPap()
         => PerfilAtual == Dominio.Perfis.PERFIL_PAP;

        public bool EhProfessorPoei()
         => PerfilAtual == Dominio.Perfis.PERFIL_POEI;

        public bool EhProfessorPoed()
         => PerfilAtual == Dominio.Perfis.PERFIL_POED;

        public bool EhProfessorPosl()
         => PerfilAtual == Dominio.Perfis.PERFIL_POSL;

        public bool EhProfessor()
         => PerfilAtual == Dominio.Perfis.PERFIL_PROFESSOR
         || PerfilAtual == Dominio.Perfis.PERFIL_PROFESSOR_INFANTIL;

        public bool EhAbrangenciaUEECP()
         => Perfis.Any(x => x.Tipo == TipoPerfil.UE && x.CodigoPerfil == Dominio.Perfis.PERFIL_CP);

        public bool EhAbrangenciaSomenteUE()
         => Perfis.Any(x => x.Tipo == TipoPerfil.UE) && !PossuiPerfilSmeOuDre();

        public bool EhProfessorCj()
         => PerfilAtual == Dominio.Perfis.PERFIL_CJ
                || PerfilAtual == Dominio.Perfis.PERFIL_CJ_INFANTIL;

        public bool EhSomenteProfessorCj()
                 => PerfilAtual == Dominio.Perfis.PERFIL_CJ;

        public bool EhGestorEscolar()
            => EhCP()
            || EhAD()
            || EhDiretor()
            || EhCCELP();

        public bool PossuiPerfilGestorEscolar()
            => Perfis.Any(p => p.CodigoPerfil == Dominio.Perfis.PERFIL_AD
                                || p.CodigoPerfil == Dominio.Perfis.PERFIL_CP
                                || p.CodigoPerfil == Dominio.Perfis.PERFIL_DIRETOR
                                || p.CodigoPerfil == Dominio.Perfis.PERFIL_COORDENADOR_CELP);

        public bool EhGestorCIEJA()
            => PerfilAtual == Dominio.Perfis.PERFIL_COORDENADOR_PEDAGOGICO_CIEJA
            || PerfilAtual == Dominio.Perfis.PERFIL_ASSISTENTE_COORDENADOR_GERAL_CIEJA
            || PerfilAtual == Dominio.Perfis.PERFIL_COORDENADOR_GERAL_CIEJA;

        public bool EhCP()
            => PerfilAtual == Dominio.Perfis.PERFIL_CP;

        public bool EhCCELP()
            => PerfilAtual == Dominio.Perfis.PERFIL_COORDENADOR_CELP;

        private bool EhAD()
            => PerfilAtual == Dominio.Perfis.PERFIL_AD;

        private bool EhDiretor()
            => PerfilAtual == Dominio.Perfis.PERFIL_DIRETOR;

        public bool EhProfessorPoa()
            => PerfilAtual.EhPerfilPOA();


        public void FinalizarRecuperacaoSenha()
        {
            TokenRecuperacaoSenha = null;
            ExpiracaoRecuperacaoSenha = null;
        }

        public void IniciarRecuperacaoDeSenha()
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                throw new NegocioException("Você não tem um e-mail cadastrado para recuperar sua senha. Para restabelecer o seu acesso, procure o Diretor da sua UE ou Administrador do SGP da sua unidade.");
            }

            TokenRecuperacaoSenha = Guid.NewGuid();
            ExpiracaoRecuperacaoSenha = DateTime.Now.AddHours(6);
        }

        public Guid ObterPerfilPrioritario(bool possuiTurmaAtiva, bool possuiTurmaInfantilAtiva, Guid perfilCJPrioritario)
        {
            if (Perfis.EhNulo() || !Perfis.Any())
                throw new NegocioException(MENSAGEM_ERRO_USUARIO_SEM_ACESSO);

            if (perfilCJPrioritario != Guid.Empty)
            {
                VerificarOrdenacaoPerfilInfantil(perfilCJPrioritario);
                return perfilCJPrioritario;
            }

            var possuiPerfilPrioritario = Perfis.Any(c => c.CodigoPerfil == Dominio.Perfis.PERFIL_PROFESSOR_INFANTIL && possuiTurmaInfantilAtiva);
            if (possuiPerfilPrioritario)
                return Dominio.Perfis.PERFIL_PROFESSOR_INFANTIL;

            possuiPerfilPrioritario = Perfis.Any(c => c.CodigoPerfil == Dominio.Perfis.PERFIL_PROFESSOR && possuiTurmaAtiva);
            if (possuiPerfilPrioritario)
                return Dominio.Perfis.PERFIL_PROFESSOR;

            possuiPerfilPrioritario = Perfis.Any(c => c.CodigoPerfil == Dominio.Perfis.PERFIL_PROFESSOR_INFANTIL && possuiTurmaAtiva);
            if (possuiPerfilPrioritario)
                return Dominio.Perfis.PERFIL_PROFESSOR_INFANTIL;

            possuiPerfilPrioritario = PossuiPerfilProfessor() && PossuiPerfilCJInfantil() && !possuiTurmaAtiva;
            if (possuiPerfilPrioritario)
                return Dominio.Perfis.PERFIL_AD;

            possuiPerfilPrioritario = PossuiPerfilProfessor() && PossuiPerfilComunicadosUe() && !possuiTurmaAtiva;
            if (possuiPerfilPrioritario)
                return Dominio.Perfis.PERFIL_COMUNICADOS_UE;

            return Perfis.FirstOrDefault().CodigoPerfil;
        }

        private void VerificarOrdenacaoPerfilInfantil(Guid perfil)
        {
            if (perfil == Dominio.Perfis.PERFIL_CJ_INFANTIL ||
                perfil == Dominio.Perfis.PERFIL_PROFESSOR_INFANTIL)
                Perfis = Perfis.OrderByDescending(o => o.EhPerfilInfantil());
        }

        public TipoPerfil? ObterTipoPerfilAtual()
        {
            return Perfis.FirstOrDefault(a => a.CodigoPerfil == PerfilAtual).Tipo;
        }

        public void PodeAlterarEvento(Evento evento)
        {
            if (evento.CriadoRF != this.CodigoRf)
            {
                ConsistirEdicaoEventoSME(evento);
                if (!PossuiPerfilSme())
                {
                    if (evento.TipoEvento.LocalOcorrencia == EventoLocalOcorrencia.DRE)
                    {
                        if (evento.TipoPerfilCadastro == TipoPerfil.SME)
                            ConsistirEdicaoEventoPerfilAtualDiferenteSME();
                        else ConsistirEdicaoEventoPerfilAtualDiferenteGestao();
                    }
                }
            }
        }

        private void ConsistirEdicaoEventoPerfilAtualDiferenteSME()
        {
            if (TipoPerfil.SME != ObterTipoPerfilAtual())
                throw new NegocioException("Você não tem permissão para alterar este evento.");
        }
        private void ConsistirEdicaoEventoPerfilAtualDiferenteGestao()
        {
            if (PerfilAtual != Dominio.Perfis.PERFIL_DIRETOR && PerfilAtual != Dominio.Perfis.PERFIL_AD
                                 && PerfilAtual != Dominio.Perfis.PERFIL_CP && PerfilAtual != Dominio.Perfis.PERFIL_COORDENADOR_CELP)
                throw new NegocioException("Você não tem permissão para alterar este evento.");
        }
        private void ConsistirEdicaoEventoSME(Evento evento)
        {
            if (string.IsNullOrEmpty(evento.DreId) 
                && string.IsNullOrEmpty(evento.UeId) 
                && !PossuiPerfilSme())
                throw new NegocioException("Evento da SME só pode ser editado por usuario com perfil SME.");
        }

        public void PodeCriarEvento(Evento evento)
        {
            if (!PossuiPerfilSme() && string.IsNullOrWhiteSpace(evento.DreId))
                throw new NegocioException("É necessário informar a DRE.");

            if (!PossuiPerfilSmeOuDre() && string.IsNullOrWhiteSpace(evento.UeId))
                throw new NegocioException("É necessário informar a UE.");

            if (evento.TipoEvento.LocalOcorrencia == EventoLocalOcorrencia.SME && !PossuiPerfilSme())
                throw new NegocioException("Somente usuários da SME podem criar este tipo de evento.");

            if (evento.TipoEvento.LocalOcorrencia == EventoLocalOcorrencia.DRE && (!PossuiPerfilDre() && !PossuiPerfilSme()))
                throw new NegocioException("Somente usuários da SME ou da DRE podem criar este tipo de evento.");
        }

        public void PodeCriarEventoComDataPassada(Evento evento)
        {
            if ((evento.DataInicio.Date < DateTime.Today)
             && (ObterTipoPerfilAtual() != TipoPerfil.SME && ObterTipoPerfilAtual() != TipoPerfil.DRE))
                throw new NegocioException("Não é possível criar evento com data passada.");
        }

        public bool PodeRegistrarFrequencia(Aula aula)
        {
            if (aula.PermiteSubstituicaoFrequencia)
                return true;

            else
                return aula.CriadoRF == CodigoRf;
        }

        public bool PodeReiniciarSenha()
        {
            return !string.IsNullOrEmpty(Email);
        }

        public bool PodeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme()
        {
            return EhAdmGestao();
        }

        public bool EhAdmGestao()
        {
            return (PerfilAtual == Dominio.Perfis.PERFIL_AD
                 || PerfilAtual == Dominio.Perfis.PERFIL_CP
                 || PerfilAtual == Dominio.Perfis.PERFIL_DIRETOR
                 || PerfilAtual == Dominio.Perfis.PERFIL_COORDENADOR_CELP
                 || EhPerfilSME()
                 || EhPerfilDRE());
        }

        public bool PodeVisualizarEventosOcorrenciaDre()
        {
            var perfilAtual = Perfis.FirstOrDefault(a => a.CodigoPerfil == PerfilAtual);

            if (perfilAtual.NaoEhNulo() && perfilAtual.Tipo == TipoPerfil.UE)
                return (PerfilAtual == Dominio.Perfis.PERFIL_DIRETOR 
                                        || PerfilAtual == Dominio.Perfis.PERFIL_AD 
                                        || PerfilAtual == Dominio.Perfis.PERFIL_CP 
                                        || PerfilAtual == Dominio.Perfis.PERFIL_COORDENADOR_CELP 
                                        || PerfilAtual == Dominio.Perfis.PERFIL_SECRETARIO);

            else return true;
        }

        public bool PossuiPerfilAdmUE()
           => Perfis.NaoEhNulo() && Perfis.Any(c => c.CodigoPerfil == Dominio.Perfis.PERFIL_ADMUE);

        public bool PossuiPerfilCJ()
            => Perfis.NaoEhNulo() && Perfis.Any(c => c.CodigoPerfil == Dominio.Perfis.PERFIL_CJ);

        public bool PossuiPerfilCJInfantil()
           => Perfis.NaoEhNulo() && Perfis.Any(c => c.CodigoPerfil == Dominio.Perfis.PERFIL_CJ_INFANTIL);

        public bool PossuiPerfilProfessor()
           => Perfis.NaoEhNulo() && Perfis.Any(c => c.CodigoPerfil == Dominio.Perfis.PERFIL_PROFESSOR);

        public bool PossuiPerfilComunicadosUe()
           => Perfis.NaoEhNulo() && Perfis.Any(c => c.CodigoPerfil == Dominio.Perfis.PERFIL_COMUNICADOS_UE);

        public bool PossuiPerfilProfessorInfantil()
           => Perfis.NaoEhNulo() && Perfis.Any(c => c.CodigoPerfil == Dominio.Perfis.PERFIL_PROFESSOR_INFANTIL);

        public bool PossuiPerfilDre()
         => Perfis.NaoEhNulo() && Perfis.Any(c => c.Tipo == TipoPerfil.DRE);

        public bool PossuiPerfilDreOuUe()
        {
            if (Perfis.EhNulo() || !Perfis.Any())
                throw new NegocioException(MENSAGEM_ERRO_USUARIO_SEM_ACESSO);

            return PossuiPerfilDre() || PossuiPerfilUe();
        }

        public bool PossuiPerfilCJPrioritario()
        {
            if (Perfis.NaoEhNulo() && PossuiPerfilCJ() && PossuiPerfilProfessor())
            {
                var perfilCj = Perfis.FirstOrDefault(x => x.CodigoPerfil == Dominio.Perfis.PERFIL_CJ);

                return !Perfis.Any(x => x.Ordem < perfilCj.Ordem);
            }

            return false;
        }

        public bool PossuiPerfilCJInfantilPrioritario()
        {
            if (Perfis.NaoEhNulo() && PossuiPerfilCJInfantil() && PossuiPerfilProfessorInfantil())
            {
                var perfilCjInfantil = Perfis.FirstOrDefault(x => x.CodigoPerfil == Dominio.Perfis.PERFIL_CJ_INFANTIL);

                return !Perfis.Any(x => x.Ordem < perfilCjInfantil.Ordem);
            }

            return false;
        }

        public bool PossuiPerfilSme()
         => Perfis.NaoEhNulo() && Perfis.Any(c => c.Tipo == TipoPerfil.SME);

        public bool EhProfessorInfantil()
         => PerfilAtual == Dominio.Perfis.PERFIL_PROFESSOR_INFANTIL;

        public bool EhProfessorCjInfantil()
         => PerfilAtual == Dominio.Perfis.PERFIL_CJ_INFANTIL;

        public bool PossuiPerfilSmeOuDre()
        {
            if (Perfis.EhNulo() || !Perfis.Any())
                throw new NegocioException(MENSAGEM_ERRO_USUARIO_SEM_ACESSO);

            return PossuiPerfilSme() || PossuiPerfilDre();
        }

        public bool PossuiPerfilUe()
         => Perfis.NaoEhNulo() && Perfis.Any(c => c.Tipo == TipoPerfil.UE);

        public bool TemPerfilGestaoUes() => (PerfilAtual == Dominio.Perfis.PERFIL_DIRETOR
                                          || PerfilAtual == Dominio.Perfis.PERFIL_AD
                                          || PerfilAtual == Dominio.Perfis.PERFIL_SECRETARIO
                                          || PerfilAtual == Dominio.Perfis.PERFIL_CP
                                          || PerfilAtual == Dominio.Perfis.PERFIL_COORDENADOR_CELP
                                          || EhPerfilSME()
                                          || EhPerfilDRE());

        public bool TemPerfilAdmUE()
         => PerfilAtual == Dominio.Perfis.PERFIL_ADMUE;

        public bool TemPerfilSupervisorOuDiretor()
         => (PerfilAtual == Dominio.Perfis.PERFIL_DIRETOR || PerfilAtual == Dominio.Perfis.PERFIL_SUPERVISOR);

        public bool TokenRecuperacaoSenhaEstaValido()
         => ExpiracaoRecuperacaoSenha > DateTime.Now;

        public void ValidarSenha(string novaSenha)
        {
            if (novaSenha.Length < 8)
                throw new NegocioException("A senha deve conter no minimo 8 caracteres.");

            if (novaSenha.Length > 12)
                throw new NegocioException("A senha deve conter no máximo 12 caracteres.");

            if (novaSenha.Contains(" "))
                throw new NegocioException("A senhão não pode conter espaço em branco.");

            var regexSenha = new Regex(@"(?=.*?[A-Z])(?=.*?[a-z])(?=((?=.*[!@#$\-%&/\\\[\]|*()_=+])|(?=.*?[0-9]+)))");

            if (!regexSenha.IsMatch(novaSenha))
                throw new NegocioException("A senha deve conter pelo menos 1 letra maiúscula, 1 minúscula, 1 número e/ou 1 caractere especial.");
        }

        public string ObterCodigoRfLogin()
        {
            return CodigoRf ?? Login;
        }
    }
}