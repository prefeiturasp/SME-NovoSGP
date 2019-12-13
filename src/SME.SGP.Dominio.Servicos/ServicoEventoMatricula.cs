using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoEventoMatricula : IServicoEventoMatricula
    {
        private readonly IRepositorioEventoMatricula repositorioEventoMatricula;
        private readonly IServicoEOL servicoEOL;

        public ServicoEventoMatricula(IServicoEOL servicoEOL,
                                      IRepositorioEventoMatricula repositorioEventoMatricula)
        {
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.repositorioEventoMatricula = repositorioEventoMatricula ?? throw new ArgumentNullException(nameof(repositorioEventoMatricula));
        }

        public void ExecutaCargaEventos()
        {
            CargaDres();
        }

        private void CargaAlunos(string codigoTurma)
        {
            var alunos = servicoEOL.ObterAlunosPorTurma(codigoTurma).Result;
            if (alunos != null)
                foreach (var aluno in alunos)
                {
                    try
                    {
                        if (aluno.EstaInativo())
                        {
                            // Verifica Evento Matricula Gerado
                            if (!repositorioEventoMatricula.CheckarEventoExistente(aluno.CodigoSituacaoMatricula, aluno.DataSituacao, aluno.CodigoAluno))
                                // Inclui o Evento Matricula do aluno
                                IncluirEventoMatricula(aluno);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Aluno [{aluno.CodigoAluno}]: erro ao gerar evento matricula: {ex.Message}");
                    }
                }
        }

        private void CargaDres()
        {
            var dres = servicoEOL.ObterDres();
            if (dres != null)
                foreach (var dre in dres)
                {
                    try
                    {
                        CargaUes(dre.CodigoDRE);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"DRE [{dre.CodigoDRE}]: {ex.Message}");
                    }
                }
        }

        private void CargaTurmas(string codigoEscola)
        {
            var turmas = servicoEOL.ObterTurmasPorUE(codigoEscola, DateTime.Now.Year.ToString()).Result;
            if (turmas != null)
                foreach (var turma in turmas)
                {
                    try
                    {
                        CargaAlunos(turma.CodigoTurma);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Turma [{turma.CodigoTurma}]: {ex.Message}");
                    }
                }
        }

        private void CargaUes(string codigoDRE)
        {
            var ues = servicoEOL.ObterEscolasPorDre(codigoDRE);
            if (ues != null)
                foreach (var ue in ues)
                {
                    try
                    {
                        CargaTurmas(ue.CodigoEscola);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Escola [{ue.CodigoEscola}]: {ex.Message}");
                    }
                }
        }

        private void IncluirEventoMatricula(AlunoPorTurmaResposta aluno)
        {
            repositorioEventoMatricula.Salvar(new EventoMatricula()
            {
                CodigoAluno = aluno.CodigoAluno,
                DataEvento = aluno.DataSituacao,
                Tipo = aluno.CodigoSituacaoMatricula,
                // Tranferencia interna e Remanejamento detalham escola e turma
                NomeEscola = aluno.EscolaTransferencia,
                NomeTurma = !string.IsNullOrEmpty(aluno.TurmaTransferencia) ?
                            aluno.TurmaTransferencia : aluno.TurmaRemanejamento
            });
        }
    }
}