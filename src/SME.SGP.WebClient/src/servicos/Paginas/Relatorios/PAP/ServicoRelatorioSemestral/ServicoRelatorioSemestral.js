import api from '~/servicos/api';
import { RelatorioSemestralMock, RelatorioSemestralMock2 } from './mock';

class ServicoRelatorioSemestral {
  obterListaAlunos = (turmaCodigo, anoLetivo, periodo, semestreConsulta) => {
    // TODO Ver com backend se vai mudar o endpoint!
    // const url = `v1/fechamentos/turmas/${turmaCodigo}/alunos/anos/${anoLetivo}/semestres/${periodo}`;
    // return api.get(url);
    console.log('<--- CONSULTA ALONOS --->');
    console.log(`turmaCodigo: ${turmaCodigo}`);
    console.log(`anoLetivo: ${anoLetivo}`);
    console.log(`periodo: ${periodo}`);
    console.log(`acompanhamento semestre: ${semestreConsulta}`);

    if (semestreConsulta == '1') {
      return Promise.resolve(RelatorioSemestralMock);
    }
    return Promise.resolve(RelatorioSemestralMock2);
  };

  obterFrequenciaAluno = alunoCodigo => {
    const url = `v1/calendarios/frequencias/alunos/${alunoCodigo}/geral`;
    return api.get(url);
  };

  salvarServicoRelatorioSemestral = params => {
    // TODO
    console.log('<--- SALVAR RELATORIO SEMESTRAL --->');
    console.log(params);
    return Promise.resolve(true);
  };

  obterListaSemestres = () => {
    const semestres = {
      data: [
        { valor: '1', descricao: 'Acompanhamento 1º Semestre' },
        { valor: '2', descricao: 'Acompanhamento 2º Semestre' },
      ],
    };
    return Promise.resolve(semestres);
  };
}

export default new ServicoRelatorioSemestral();
