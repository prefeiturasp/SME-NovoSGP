import { store } from '~/redux';
import {
  limparDadosParaSalvarRelatorioSemestral,
  setAuditoriaRelatorioSemestral,
  setRelatorioSemestralEmEdicao,
  setDadosRelatorioSemestral,
} from '~/redux/modulos/relatorioSemestral/actions';
import { confirmar, erro, erros, sucesso } from '~/servicos/alertas';
import ServicoRelatorioSemestral from '~/servicos/Paginas/Relatorios/PAP/RelatorioSemestral/ServicoRelatorioSemestral';

class ServicoSalvarRelatorioSemestral {
  validarSalvarRelatorioSemestral = async (salvarSemValidar = false) => {
    const { dispatch } = store;
    const state = store.getState();

    const { relatorioSemestral } = state;

    const {
      dadosRelatorioSemestral,
      relatorioSemestralEmEdicao,
      dadosParaSalvarRelatorioSemestral,
    } = relatorioSemestral;

    const {
      relatorioSemestralId,
      relatorioSemestralAlunoId,
      turmaCodigo,
      semestreConsulta,
      alunoCodigo,
    } = dadosRelatorioSemestral;

    const perguntaDescartarRegistros = async () => {
      return confirmar(
        'Atenção',
        '',
        'Os registros deste aluno ainda não foram salvos, deseja descartar os registros?'
      );
    };

    const todosCamposValidos = (mostrarErro = false) => {
      const { secoes } = dadosRelatorioSemestral;
      for (let index = 0; index < secoes.length; index += 1) {
        const secao = secoes[index];

        if (secao.obrigatorio) {
          const itemAlterado = dadosParaSalvarRelatorioSemestral.find(
            campo => campo.id == secao.id
          );
          if (itemAlterado) {
            if (!itemAlterado.valor) {
              if (mostrarErro) erro(`É obrigatório informar ${secao.nome}`);
              return false;
            }
          } else if (!secao.valor) {
            if (mostrarErro) erro(`É obrigatório informar ${secao.nome}`);
            return false;
          }
        }
      }
      return true;
    };

    const salvar = async (limparTodosOsDados = false) => {
      const params = {
        relatorioSemestralId: !relatorioSemestralId || 0,
        relatorioSemestralAlunoId: !relatorioSemestralAlunoId || 0,
        secoes: dadosParaSalvarRelatorioSemestral,
      };

      const temRegistrosInvalidos = !todosCamposValidos(true);
      if (temRegistrosInvalidos) {
        return false;
      }

      const retorno = await ServicoRelatorioSemestral.salvarServicoRelatorioSemestral(
        turmaCodigo,
        semestreConsulta,
        alunoCodigo,
        params
      ).catch(e => erros(e));

      if (retorno && retorno.status === 200) {
        const { auditoria } = retorno.data;

        dispatch(setAuditoriaRelatorioSemestral(auditoria));
        dispatch(setRelatorioSemestralEmEdicao(false));

        if (limparTodosOsDados) {
          dispatch(setDadosRelatorioSemestral());
          dispatch(limparDadosParaSalvarRelatorioSemestral());
        } else {
          dispatch(limparDadosParaSalvarRelatorioSemestral());
        }

        sucesso('Suas informações foram salvas com sucesso.');
        return true;
      }
      return false;
    };

    if (salvarSemValidar && relatorioSemestralEmEdicao) {
      return salvar();
    }

    if (relatorioSemestralEmEdicao) {
      const temRegistrosInvalidos = !todosCamposValidos();

      let descartarRegistros = false;
      if (temRegistrosInvalidos) {
        descartarRegistros = await perguntaDescartarRegistros();
      }

      // Voltar para a tela continua e executa a ação!
      if (descartarRegistros) {
        dispatch(setRelatorioSemestralEmEdicao(false));
        return true;
      }

      // Voltar para a tela e não executa a ação!
      if (!descartarRegistros && temRegistrosInvalidos) {
        return false;
      }

      // Tenta salvar os registros se estão válidos e continuar para executação a ação!
      return salvar(true);
    }
    return true;
  };
}

export default new ServicoSalvarRelatorioSemestral();
