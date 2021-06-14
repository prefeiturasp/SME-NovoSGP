import React, { useCallback, useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Loader } from '~/componentes';
import ObservacoesUsuario from '~/componentes-sgp/ObservacoesUsuario/observacoesUsuario';
import { RotasDto } from '~/dtos';
import {
  limparDadosObservacoesUsuario,
  setDadosObservacoesUsuario,
  setListaUsuariosNotificacao,
} from '~/redux/modulos/observacoesUsuario/actions';
import { confirmar, erros, sucesso } from '~/servicos';
import ServicoPlanoAEEObservacoes from '~/servicos/Paginas/Relatorios/AEE/ServicoPlanoAEEObservacoes';

const MontarDadosObservacoesPlanoAEE = () => {
  const [desabilitarBotaoNotificar, setDesabilitarBotaoNotificar] = useState(
    true
  );

  const usuario = useSelector(store => store.usuario);

  const permissoesTela = usuario.permissoes[RotasDto.RELATORIO_AEE_PLANO];

  const planoAEEDados = useSelector(store => store.planoAEE.planoAEEDados);

  const listaUsuariosNotificacao = useSelector(
    store => store.observacoesUsuario.listaUsuariosNotificacao
  );

  const novaObservacao = useSelector(
    store => store.observacoesUsuario.novaObservacao
  );

  const dispatch = useDispatch();

  const [carregandoGeral, setCarregandoGeral] = useState(false);

  const obterDadosObservacoes = useCallback(
    async id => {
      setCarregandoGeral(true);

      const retorno = await ServicoPlanoAEEObservacoes.obterDadosObservacoes(id)
        .catch(e => erros(e))
        .finally(() => setCarregandoGeral(false));

      if (retorno?.data) {
        const lista = retorno.data.map(item => {
          const obs = { ...item };
          if (item?.usuarios?.length) {
            obs.usuariosNotificacao = item?.usuarios;
          }
          return obs;
        });

        dispatch(setDadosObservacoesUsuario([...lista]));
      } else {
        dispatch(setDadosObservacoesUsuario([]));
      }

      dispatch(setListaUsuariosNotificacao([]));
    },
    [dispatch]
  );

  useEffect(() => {
    if (planoAEEDados?.id) {
      dispatch(limparDadosObservacoesUsuario());
      obterDadosObservacoes(planoAEEDados.id);
    }
  }, [planoAEEDados, dispatch, obterDadosObservacoes]);

  const salvarEditarObservacao = async obs => {
    setCarregandoGeral(true);

    const params = {
      planoAEEId: planoAEEDados?.id || 0,
      id: obs?.id || 0,
      observacao: obs?.observacao || 0,
    };

    if (obs?.id && obs?.usuarios?.length) {
      params.usuarios = obs.usuarios
        .filter(item => !!item?.usuarioId)
        .map(u => {
          return u.usuarioId;
        });
    } else if (!obs?.id && listaUsuariosNotificacao?.length) {
      params.usuariosNotificacao = [...listaUsuariosNotificacao];
      params.usuarios = params.usuariosNotificacao
        .filter(item => !!item?.usuarioId)
        .map(u => {
          return u.usuarioId;
        });
    }

    return ServicoPlanoAEEObservacoes.salvarEditarObservacao(params)
      .then(resultado => {
        if (resultado?.status === 200) {
          const msg = `Observação ${
            obs.id ? 'alterada' : 'inserida'
          } com sucesso.`;
          sucesso(msg);
        }
        setCarregandoGeral(false);

        obterDadosObservacoes(planoAEEDados.id);
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
      const resultado = await ServicoPlanoAEEObservacoes.excluirObservacao(
        obs?.id
      )
        .catch(e => {
          erros(e);
          setCarregandoGeral(false);
        })
        .finally(() => setCarregandoGeral(false));

      if (resultado?.status === 200) {
        sucesso('Registro excluído com sucesso');
        obterDadosObservacoes(planoAEEDados.id);
      }
    }
  };

  useEffect(() => {
    let desabilitaBotao = true;
    if (novaObservacao) {
      desabilitaBotao = false;
    }
    setDesabilitarBotaoNotificar(desabilitaBotao);
  }, [novaObservacao]);

  return (
    <Loader loading={carregandoGeral}>
      <ObservacoesUsuario
        esconderLabel
        mostrarListaNotificacao
        desabilitarBotaoNotificar={desabilitarBotaoNotificar}
        salvarObservacao={obs => salvarEditarObservacao(obs)}
        editarObservacao={obs => salvarEditarObservacao(obs)}
        excluirObservacao={obs => excluirObservacao(obs)}
        verificaProprietario
        obterUsuariosNotificadosDiarioBordo={false}
        usarLocalizadorFuncionario
        parametrosLocalizadorFuncionario={{
          codigoUe: planoAEEDados?.turma?.codigoUE,
        }}
      />
    </Loader>
  );
};

export default MontarDadosObservacoesPlanoAEE;
