using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasFechamentoFinal : IConsultasFechamentoFinal
    {
        private readonly IRepositorioFechamentoFinal repositorioFechamentoFinal;
        private readonly IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina;
        private readonly IRepositorioNotaConceitoBimestre repositorioNotaConceitoBimestre;
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IServicoEOL servicoEOL;
        private readonly IServicoUsuario servicoUsuario;

        public ConsultasFechamentoFinal(IRepositorioTurma repositorioTurma, IRepositorioTipoCalendario repositorioTipoCalendario,
            IRepositorioPeriodoEscolar repositorioPeriodoEscolar, IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina,
            IServicoEOL servicoEOL, IServicoUsuario servicoUsuario, IRepositorioNotaConceitoBimestre repositorioNotaConceitoBimestre, IRepositorioFechamentoFinal repositorioFechamentoFinal)
        {
            this.repositorioTurma = repositorioTurma ?? throw new System.ArgumentNullException(nameof(repositorioTurma));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new System.ArgumentNullException(nameof(repositorioTipoCalendario));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new System.ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.repositorioFechamentoTurmaDisciplina = repositorioFechamentoTurmaDisciplina ?? throw new System.ArgumentNullException(nameof(repositorioFechamentoTurmaDisciplina));
            this.servicoEOL = servicoEOL ?? throw new System.ArgumentNullException(nameof(servicoEOL));
            this.servicoUsuario = servicoUsuario ?? throw new System.ArgumentNullException(nameof(servicoUsuario));
            this.repositorioNotaConceitoBimestre = repositorioNotaConceitoBimestre ?? throw new System.ArgumentNullException(nameof(repositorioNotaConceitoBimestre));
            this.repositorioFechamentoFinal = repositorioFechamentoFinal ?? throw new System.ArgumentNullException(nameof(repositorioFechamentoFinal));
        }

        public async Task<FechamentoFinalConsultaRetornoDto> ObterFechamentos(FechamentoFinalConsultaFiltroDto filtros)
        {
            var retorno = new FechamentoFinalConsultaRetornoDto();
            var turma = repositorioTurma.ObterPorCodigo(filtros.TurmaCodigo);

            if (turma == null)
                throw new NegocioException("Não foi possível localizar a UE.");

            var tipoCalendario = repositorioTipoCalendario.BuscarPorAnoLetivoEModalidade(turma.AnoLetivo, turma.ObterModalidadeTipoCalendario());
            if (tipoCalendario == null)
                throw new NegocioException("Não foi encontrado tipo de calendário escolar, para a modalidade informada.");

            var periodosEscolares = repositorioPeriodoEscolar.ObterPorTipoCalendario(tipoCalendario.Id);
            if (periodosEscolares == null || !periodosEscolares.Any())
                throw new NegocioException("Não foi encontrado período Escolar para a modalidade informada.");

            retorno.EventoData = periodosEscolares.OrderByDescending(a => a.Bimestre).FirstOrDefault().PeriodoFim;

            var alunosDaTurma = await servicoEOL.ObterAlunosPorTurma(turma.CodigoTurma);
            if (alunosDaTurma == null || !alunosDaTurma.Any())
                throw new NegocioException("Não foram encontrandos alunos para a turma informada.");

            //var disciplinas = await servicoEOL.ObterDisciplinasPorCodigoTurma(turma.CodigoTurma);
            //if (disciplinas == null || !disciplinas.Any())
            //    throw new NegocioException("Não foi encontrado componentes curriculares para a turma informada.");

            //foreach (var disciplina in disciplinas)
            //{
            //    if (disciplina.Regencia)
            //    {
            //        var disciplinasRegencia = await servicoEOL.ObterDisciplinasParaPlanejamento(long.Parse(turma.CodigoTurma), servicoUsuario.ObterLoginAtual(), servicoUsuario.ObterPerfilAtual());
            //        if (disciplinasRegencia == null || !disciplinasRegencia.Any())
            //            throw new NegocioException("Não foi encontrado componentes curriculares para a regencia informada.");

            //    }
            //
            //}

            //Obter os fechamentos ~~

            //Primeira lista de notas dos bimestres
            //Codigo aluno / NotaConceito / Código Disciplina / bimestre

            var listaAlunosNotas = new List<(string, string, long, int)>();

            foreach (var periodo in periodosEscolares)
            {
                //if (filtros.EhRegencia)
                //{
                //    var disciplinasRegencia = await servicoEOL.ObterDisciplinasParaPlanejamento(long.Parse(turma.CodigoTurma), servicoUsuario.ObterLoginAtual(), servicoUsuario.ObterPerfilAtual());
                //    if (disciplinasRegencia == null || !disciplinasRegencia.Any())
                //        throw new NegocioException("Não foi encontrado componentes curriculares para a regencia informada.");

                //    foreach (var disciplina in disciplinasRegencia)
                //    {
                //    }

                //}
                var fechamentoDoBimestre = await repositorioFechamentoTurmaDisciplina.ObterFechamentoTurmaDisciplina(turma.CodigoTurma, filtros.DisciplinaCodigo, periodo.Bimestre);
                if (fechamentoDoBimestre == null)
                    throw new NegocioException($"Não foi encontrado fechamento para o bimestre {periodo.Bimestre}.");

                var notasEConceitos = await repositorioNotaConceitoBimestre.ObterPorFechamentoTurma(fechamentoDoBimestre.Id);
                if (notasEConceitos == null || !notasEConceitos.Any())
                    throw new NegocioException($"Não foram encontrados notas e conceitos para a turma e bimestre {periodo.Bimestre}.");

                foreach (var notaEConceito in notasEConceitos)
                {
                    var fechamentoFinalConsultaRetornoAlunoDto = new FechamentoFinalConsultaRetornoAlunoDto();
                    listaAlunosNotas.Add((notaEConceito.CodigoAluno, (notaEConceito.ConceitoId == 0 ? notaEConceito.Nota.ToString() : notaEConceito.ConceitoId.ToString()), notaEConceito.DisciplinaId, periodo.Bimestre));
                }
            }

            //TODO: Transformar essa lista na lista do DTO;
            ////var aluno = alunosDaTurma.FirstOrDefault(a => a.CodigoAluno == notaEConceito.CodigoAluno);
            ////if (aluno == null)
            ////    throw new NegocioException($"Não foi encontrado o aluno de código {notaEConceito.CodigoAluno}");

            ////fechamentoFinalConsultaRetornoAlunoDto.Nome = aluno.NomeAluno;
            ////if (notaEConceito.ConceitoId == 0)
            ////    fechamentoFinalConsultaRetornoAlunoDto.nota
            ///

            //Conceito Final
            var fechamentos = await repositorioFechamentoFinal.ObterPorFiltros(turma.CodigoTurma);
            var idsDisciplinas = listaAlunosNotas.Select(a => a.Item3).Distinct().ToList();
            if (filtros.EhRegencia)
                idsDisciplinas.Add(filtros.DisciplinaCodigo);

            var disciplinas = servicoEOL.ObterDisciplinasPorIds(idsDisciplinas.ToArray());

            foreach (var aluno in alunosDaTurma)
            {
                var fechamentoFinalAluno = new FechamentoFinalConsultaRetornoAlunoDto();
                fechamentoFinalAluno.Nome = aluno.NomeAluno;
                //fechamentoFinalAluno.Frequencia
                fechamentoFinalAluno.NumeroChamada = aluno.NumeroAlunoChamada;
                //fechamentoFinalAluno.TotalAusenciasCompensadas

                var notasDosBimestres = listaAlunosNotas.Where(a => a.Item1 == aluno.CodigoAluno).ToList();
                foreach (var notaDoBimestre in notasDosBimestres)
                {
                    var alunoNotaConceito = new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = notaDoBimestre.Item4, };
                    fechamentoFinalAluno.NotasConceitoBimestre.Add()
                }

                retorno.Alunos.Add(fechamentoFinalAluno);
            }

            return retorno;
        }
    }
}