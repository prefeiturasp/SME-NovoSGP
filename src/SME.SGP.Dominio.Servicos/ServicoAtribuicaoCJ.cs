using SME.SGP.Aplicacao.Servicos;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoAtribuicaoCJ : IServicoAtribuicaoCJ
    {
        private readonly IRepositorioAbrangencia repositorioAbrangencia;
        private readonly IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IServicoAbrangencia servicoAbrangencia;

        public ServicoAtribuicaoCJ(IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ, IServicoAbrangencia servicoAbrangencia, IRepositorioTurma repositorioTurma,
            IRepositorioAbrangencia repositorioAbrangencia)
        {
            this.repositorioAtribuicaoCJ = repositorioAtribuicaoCJ ?? throw new ArgumentNullException(nameof(repositorioAtribuicaoCJ));
            this.servicoAbrangencia = servicoAbrangencia ?? throw new ArgumentNullException(nameof(servicoAbrangencia));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new ArgumentNullException(nameof(repositorioAbrangencia));
        }

        public async Task Salvar(AtribuicaoCJ atribuicaoCJ)
        {
            await repositorioAtribuicaoCJ.SalvarAsync(atribuicaoCJ);
            TratarAbrangencia(atribuicaoCJ);
        }

        private async void TratarAbrangencia(AtribuicaoCJ atribuicaoCJ)
        {
            if (atribuicaoCJ.Substituir)
            {
                var turma = repositorioTurma.ObterPorId(atribuicaoCJ.TurmaId);
                if (turma == null)
                    throw new NegocioException($"Não foi possível localizar a turma {atribuicaoCJ.TurmaId} da abrangência.");

                var abrangencias = new Abrangencia[] { new Abrangencia() { Perfil = Perfis.PERFIL_CJ, TurmaId = turma.Id } };

                servicoAbrangencia.SalvarAbrangencias(abrangencias, atribuicaoCJ.ProfessorRf);
            }
            else
            {
                var abrangencias = await repositorioAbrangencia.ObterAbrangenciaSintetica(atribuicaoCJ.ProfessorRf, Perfis.PERFIL_CJ, atribuicaoCJ.TurmaId);

                if (abrangencias != null && abrangencias.Any())
                {
                    servicoAbrangencia.RemoverAbrangencias(abrangencias.Select(a => a.Id).ToArray());
                }
            }
        }
    }
}