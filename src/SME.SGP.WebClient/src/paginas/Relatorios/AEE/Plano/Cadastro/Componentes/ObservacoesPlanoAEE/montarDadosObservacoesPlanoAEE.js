import React, { useCallback, useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Loader } from '~/componentes';
import ObservacoesUsuario from '~/componentes-sgp/ObservacoesUsuario/observacoesUsuario';
import ServicoObservacoesUsuario from '~/componentes-sgp/ObservacoesUsuario/ServicoObservacoesUsuario';
import { RotasDto } from '~/dtos';
import {
  limparDadosObservacoesUsuario,
  setDadosObservacoesUsuario,
} from '~/redux/modulos/observacoesUsuario/actions';
import { confirmar, erros, sucesso } from '~/servicos';
import ServicoPlanoAEE from '~/servicos/Paginas/Relatorios/AEE/ServicoPlanoAEE';

const MontarDadosObservacoesPlanoAEE = () => {
  const usuario = useSelector(store => store.usuario);
  const { turmaSelecionada } = usuario;

  const permissoesTela = usuario.permissoes[RotasDto.RELATORIO_AEE_PLANO];

  const planoAEEDados = useSelector(store => store.planoAEE.planoAEEDados);

  const dispatch = useDispatch();

  const [carregandoGeral, setCarregandoGeral] = useState(false);

  const obterUsuarioPorObservacao = useCallback(
    dadosObservacoes => {
      const promises = dadosObservacoes.map(async observacao => {
        const retorno = await ServicoPlanoAEE.obterNofiticarUsuarios({
          turmaId: turmaSelecionada?.id,
          observacaoId: observacao.id,
        }).catch(e => erros(e));

        if (retorno?.data) {
          return {
            ...observacao,
            usuariosNotificacao: retorno.data,
          };
        }
        return observacao;
      });
      return Promise.all(promises);
    },
    [turmaSelecionada]
  );

  const obterDadosObservacoes = useCallback(
    async id => {
      dispatch(limparDadosObservacoesUsuario());
      setCarregandoGeral(true);

      const retorno = await ServicoPlanoAEE.obterDadosObservacoes(id)
        .catch(e => erros(e))
        .finally(() => setCarregandoGeral(false));

      if (retorno && retorno.data) {
        const dadosObservacoes = await obterUsuarioPorObservacao(retorno.data);
        dispatch(setDadosObservacoesUsuario([...dadosObservacoes]));
      } else {
        dispatch(setDadosObservacoesUsuario([]));
      }
    },
    [dispatch, obterUsuarioPorObservacao]
  );

  useEffect(() => {
    if (planoAEEDados?.id) {
      obterDadosObservacoes(planoAEEDados.id);
    }
  }, [planoAEEDados, obterDadosObservacoes]);

  const salvarEditarObservacao = async obs => {
    setCarregandoGeral(true);

    return ServicoPlanoAEE.salvarEditarObservacao(9999)
      .then(resultado => {
        if (resultado?.status === 200) {
          const msg = `Observação ${
            obs.id ? 'alterada' : 'inserida'
          } com sucesso.`;
          sucesso(msg);
        }
        setCarregandoGeral(false);

        ServicoObservacoesUsuario.atualizarSalvarEditarDadosObservacao(
          obs,
          resultado.data
        );
        return resultado;
      })
      .catch(e => {
        erros(e);
        setCarregandoGeral(false);
        return e;
      });
  };

  const excluirObservacao = async obs => {
    const confirmado = await confirmar(
      'Excluir',
      '',
      'Você tem certeza que deseja excluir este registro?'
    );

    if (confirmado) {
      setCarregandoGeral(true);
      const resultado = await ServicoPlanoAEE.excluirObservacao(obs?.id)
        .catch(e => {
          erros(e);
          setCarregandoGeral(false);
        })
        .finally(() => setCarregandoGeral(false));

      if (resultado?.status === 200) {
        sucesso('Registro excluído com sucesso');
        ServicoObservacoesUsuario.atualizarExcluirDadosObservacao(obs);
      }
    }
  };

  return (
    <Loader loading={carregandoGeral}>
      <ObservacoesUsuario
        esconderLabel
        mostrarListaNotificacao
        salvarObservacao={obs => salvarEditarObservacao(obs)}
        editarObservacao={obs => salvarEditarObservacao(obs)}
        excluirObservacao={obs => excluirObservacao(obs)}
        permissoes={permissoesTela}
      />
    </Loader>
  );
};

export default MontarDadosObservacoesPlanoAEE;
