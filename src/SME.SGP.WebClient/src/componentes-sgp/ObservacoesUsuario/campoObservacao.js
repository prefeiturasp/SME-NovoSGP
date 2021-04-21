import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import shortid from 'shortid';
import { Label } from '~/componentes';
import Button from '~/componentes/button';
import { Colors } from '~/componentes/colors';
import { ModalNotificarUsuarios } from '~/paginas/DiarioClasse/DiarioBordo/componentes';
import {
  setNovaObservacao,
  setListaUsuariosNotificacao,
} from '~/redux/modulos/observacoesUsuario/actions';
import { confirmar, erros } from '~/servicos/alertas';
import ServicoDiarioBordo from '~/servicos/Paginas/DiarioClasse/ServicoDiarioBordo';
import { ContainerCampoObservacao } from './observacoesUsuario.css';

const CampoObservacao = props => {
  const {
    salvarObservacao,
    esconderCaixaExterna,
    podeIncluir,
    obterUsuariosNotificadosDiarioBordo,
    usarLocalizadorFuncionario,
    parametrosLocalizadorFuncionario,
    desabilitarBotaoNotificar,
  } = props;
  const [modalVisivel, setModalVisivel] = useState(false);

  const dispatch = useDispatch();

  const turmaSelecionada = useSelector(state => state.usuario.turmaSelecionada);
  const turmaId = turmaSelecionada?.id || 0;
  const observacaoEmEdicao = useSelector(
    store => store.observacoesUsuario.observacaoEmEdicao
  );

  const novaObservacao = useSelector(
    store => store.observacoesUsuario.novaObservacao
  );

  const listaUsuarios = useSelector(
    store => store.observacoesUsuario.listaUsuariosNotificacao
  );

  const onChangeNovaObservacao = ({ target: { value } }) => {
    dispatch(setNovaObservacao(value));
  };

  const onClickCancelarNovo = async () => {
    const confirmou = await confirmar(
      'Atenção',
      'Você não salvou as informações preenchidas.',
      'Deseja realmente cancelar as alterações?'
    );

    if (confirmou) {
      dispatch(setNovaObservacao(''));
    }
  };

  const onClickSalvar = async () => {
    const retorno = await salvarObservacao({ observacao: novaObservacao });
    if (retorno?.status === 200) {
      dispatch(setNovaObservacao(''));
      if (obterUsuariosNotificadosDiarioBordo) {
        const retornoUsuarios = await ServicoDiarioBordo.obterNofiticarUsuarios(
          {
            turmaId,
          }
        ).catch(e => erros(e));

        if (retornoUsuarios?.status === 200) {
          dispatch(setListaUsuariosNotificacao(retornoUsuarios.data));
        }
      }
    }
  };

  const obterNofiticarUsuarios = useCallback(async () => {
    const retorno = await ServicoDiarioBordo.obterNofiticarUsuarios({
      turmaId,
    }).catch(e => erros(e));

    if (retorno?.status === 200) {
      dispatch(setListaUsuariosNotificacao(retorno.data));
    }
  }, [turmaId]);

  useEffect(() => {
    if (
      turmaId &&
      !listaUsuarios?.length &&
      obterUsuariosNotificadosDiarioBordo
    ) {
      obterNofiticarUsuarios();
    }
  }, [
    turmaId,
    obterNofiticarUsuarios,
    listaUsuarios,
    obterUsuariosNotificadosDiarioBordo,
  ]);

  return (
    <>
      <div className={`col-md-12 pb-2 ${esconderCaixaExterna && 'p-0'}`}>
        <Label text="Escreva uma observação" />
        <ContainerCampoObservacao
          id="nova-observacao"
          autoSize={{ minRows: 4 }}
          value={novaObservacao}
          onChange={onChangeNovaObservacao}
          disabled={!!observacaoEmEdicao || !podeIncluir}
        />
      </div>
      <div
        className="row pb-4 d-flex"
        style={{ margin: `${esconderCaixaExterna ? 0 : 15}px` }}
      >
        <div className="p-0 col-md-6 d-flex justify-content-start">
          <Button
            height="30px"
            id={shortid.generate()}
            label={`Notificar usuários (${listaUsuarios?.length})`}
            icon="bell"
            color={Colors.Azul}
            border
            onClick={() => setModalVisivel(true)}
            disabled={
              !!observacaoEmEdicao || !podeIncluir || desabilitarBotaoNotificar
            }
          />
        </div>
        <div className="p-0 col-md-6 d-flex justify-content-end">
          <Button
            id="btn-cancelar-obs-novo"
            label="Cancelar"
            color={Colors.Roxo}
            border
            bold
            className="mr-3"
            onClick={onClickCancelarNovo}
            height="30px"
            disabled={!novaObservacao || !podeIncluir}
          />
          <Button
            id="btn-salvar-obs-novo"
            label="Salvar"
            color={Colors.Roxo}
            border
            bold
            onClick={onClickSalvar}
            height="30px"
            disabled={!novaObservacao || !podeIncluir}
          />
        </div>
      </div>
      {modalVisivel && (
        <ModalNotificarUsuarios
          modalVisivel={modalVisivel}
          setModalVisivel={setModalVisivel}
          listaUsuarios={listaUsuarios}
          somenteConsulta={!podeIncluir}
          desabilitado={!novaObservacao || !podeIncluir}
          usarLocalizadorFuncionario={usarLocalizadorFuncionario}
          parametrosLocalizadorFuncionario={parametrosLocalizadorFuncionario}
        />
      )}
    </>
  );
};

CampoObservacao.propTypes = {
  salvarObservacao: PropTypes.func,
  esconderCaixaExterna: PropTypes.bool,
  podeIncluir: PropTypes.oneOfType(PropTypes.object),
  obterUsuariosNotificadosDiarioBordo: PropTypes.bool,
  usarLocalizadorFuncionario: PropTypes.bool,
  parametrosLocalizadorFuncionario: PropTypes.oneOfType(PropTypes.object),
  desabilitarBotaoNotificar: PropTypes.bool,
};

CampoObservacao.defaultProps = {
  salvarObservacao: () => {},
  esconderCaixaExterna: false,
  podeIncluir: true,
  obterUsuariosNotificadosDiarioBordo: true,
  usarLocalizadorFuncionario: false,
  parametrosLocalizadorFuncionario: {},
  desabilitarBotaoNotificar: false,
};

export default CampoObservacao;
