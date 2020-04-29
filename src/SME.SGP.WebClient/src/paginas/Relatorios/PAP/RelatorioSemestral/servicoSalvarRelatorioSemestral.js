import { store } from '~/redux';
import {
  setAuditoriaRelatorioSemestral,
  setRelatorioSemestralEmEdicao,
} from '~/redux/modulos/relatorioSemestral/actions';
import { confirmar, erro, erros, sucesso } from '~/servicos/alertas';
import ServicoRelatorioSemestral from '~/servicos/Paginas/Relatorios/PAP/ServicoRelatorioSemestral/ServicoRelatorioSemestral';

class ServicoSalvarRelatorioSemestral {
  validarSalvarRelatorioSemestral = async (salvarSemValidar = false) => {
    const { dispatch } = store;
    const state = store.getState();

    const { relatorioSemestral } = state;

    const {
      historicoEstudante,
      dificuldades,
      encaminhamentos,
      avancos,
      outros,
      dadosRelatorioSemestral,
      relatorioSemestralEmEdicao,
      desabilitarCampos,
    } = relatorioSemestral;

    const perguntaDescartarRegistros = async () => {
      return confirmar(
        'Atenção',
        '',
        'Os registros deste aluno ainda não foram salvos, deseja descartar os registros?'
      );
    };

    const salvar = async () => {
      // TODO Revisar os paramestros!
      const params = {
        id: dadosRelatorioSemestral.id,
        historicoEstudante,
        dificuldades,
        encaminhamentos,
        avancos,
        outros,
      };

      if (!historicoEstudante) {
        erro('É obrigatório informar Histórico do estudante');
        return false;
      }

      if (!dificuldades) {
        erro('É obrigatório informar Dificuldades do estudante');
        return false;
      }

      if (!encaminhamentos) {
        erro('É obrigatório informar Encaminhamentos do estudante');
        return false;
      }

      if (!avancos) {
        erro('É obrigatório informar Avanços do estudante');
        return false;
      }

      // TODO Revisar o post!
      const retorno = await ServicoRelatorioSemestral.salvarServicoRelatorioSemestral(
        params
      ).catch(e => erros(e));

      if (retorno && retorno.status === 200) {
        // TODO Validar retorno da auditoria!
        const auditoria = {
          criadoEm: retorno.data.criadoEm,
          criadoPor: retorno.data.criadoPor,
          criadoRF: retorno.data.criadoRF,
          alteradoEm: retorno.data.alteradoEm,
          alteradoPor: retorno.data.alteradoPor,
          alteradoRF: retorno.data.alteradoRF,
        };
        dispatch(setAuditoriaRelatorioSemestral(auditoria));
        dispatch(setRelatorioSemestralEmEdicao(false));
        sucesso('Suas informações foram salvas com sucesso.');
        return true;
      }
      return false;
    };

    if (desabilitarCampos) {
      return true;
    }

    if (salvarSemValidar && relatorioSemestralEmEdicao) {
      return salvar();
    }

    if (relatorioSemestralEmEdicao) {
      const temRegistrosInvalidos =
        !historicoEstudante ||
        !dificuldades ||
        !encaminhamentos ||
        !avancos ||
        !outros;

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
      return salvar();
    }
    return true;
  };
}

export default new ServicoSalvarRelatorioSemestral();
