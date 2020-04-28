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
    // TODO Revisar!
    console.log('<--- SALVAR RELATORIO SEMESTRAL --->');
    console.log(params);
    const retorno = {
      status: 200,
      data: {
        id: 999999,
        criadoEm: '2010-06-19T00:00:00',
        criadoPor: 'Joao salvador',
        criadoRF: '123123',
        alteradoEm: '2010-06-19T00:00:00',
        alteradoPor: 'Joao teste alterou',
        alteradoRF: '321321',
      },
    };
    return Promise.resolve(retorno);
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

  obterDadosCamposDescritivos = (turma, codigoAluno) => {
    // TODO Revisar consulta!
    console.log('OBTER COMPOS DESCRITIVOS!');
    console.log(`turma: ${turma}`);
    console.log(`codigoAluno: ${codigoAluno}`);

    const dados = {
      data: {
        historicoEstudante: 'historicoEstudante TESTE',
        auditoria: {
          criadoEm: '2010-06-19T00:00:00',
          criadoPor: 'TESTE',
          criadoRF: '99999',
          alteradoEm: '2010-06-19T00:00:00',
          alteradoPor: 'TESTE TESTE TESTE',
          alteradoRF: '00000',
        },
      },
    };
    return Promise.resolve(dados);
  };
}

export default new ServicoRelatorioSemestral();
