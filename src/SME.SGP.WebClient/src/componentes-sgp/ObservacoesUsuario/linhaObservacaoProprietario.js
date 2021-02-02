import { Tooltip } from 'antd';
import PropTypes from 'prop-types';
import React, { useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import Button from '~/componentes/button';
import { Colors } from '~/componentes/colors';
import { setObservacaoEmEdicao } from '~/redux/modulos/observacoesUsuario/actions';
import { confirmar } from '~/servicos/alertas';
import { ContainerCampoObservacao } from './observacoesUsuario.css';

const LinhaObservacaoProprietario = props => {
  const {
    dados,
    onClickSalvarEdicao,
    onClickExcluir,
    children,
    podeAlterar,
    podeExcluir,
    proprietario,
  } = props;

  const dispatch = useDispatch();
  const [modoEdicao, setModoEdicao] = useState(false);

  const observacaoEmEdicao = useSelector(
    store => store.observacoesUsuario.observacaoEmEdicao
  );

  const novaObservacao = useSelector(
    store => store.observacoesUsuario.novaObservacao
  );

  const onClickEditar = () => {
    dispatch(setObservacaoEmEdicao({ ...dados }));
  };
  const onClickSalvar = () => {
    onClickSalvarEdicao(observacaoEmEdicao).then(resultado => {
      if (resultado && resultado.status === 200) {
        dispatch(setObservacaoEmEdicao());
        setModoEdicao(false);
      }
    });
  };

  const onClickCancelar = async () => {
    if (modoEdicao) {
      const confirmou = await confirmar(
        'Atenção',
        'Você não salvou as informações preenchidas.',
        'Deseja realmente cancelar as alterações?'
      );

      if (confirmou) {
        dispatch(setObservacaoEmEdicao());
        setModoEdicao(false);
      }
    } else {
      dispatch(setObservacaoEmEdicao());
    }
  };

  const onChangeObs = ({ target: { value } }) => {
    const obs = { ...observacaoEmEdicao };
    obs.observacao = value;
    setModoEdicao(true);
    dispatch(setObservacaoEmEdicao({ ...obs }));
  };

  const btnSalvarCancelar = () => {
    return (
      <div className="d-flex mt-2">
        <Button
          id="btn-cancelar-obs-novo"
          label="Cancelar"
          color={Colors.Roxo}
          border
          bold
          className="mr-3"
          onClick={onClickCancelar}
          height="30px"
        />
        <Button
          id="btn-salvar-obs-novo"
          label="Salvar"
          color={Colors.Roxo}
          border
          bold
          onClick={onClickSalvar}
          height="30px"
        />
      </div>
    );
  };

  const desabilitaEditar = () => {
    return (
      !!(observacaoEmEdicao && observacaoEmEdicao.id !== dados.id) ||
      !!novaObservacao ||
      !podeAlterar ||
      !proprietario
    );
  };

  const desabilitaExcluir = () => {
    return (
      !!(observacaoEmEdicao && observacaoEmEdicao.id !== dados.id) ||
      !!novaObservacao ||
      !podeExcluir ||
      !proprietario
    );
  };

  const btnEditarExcluir = () => {
    return (
      <div className="d-flex mt-2">
        <Tooltip title={desabilitaEditar() ? '' : 'Editar'}>
          <span>
            <Button
              id="btn-editar"
              icon="edit"
              iconType="far"
              color={Colors.Azul}
              border
              className="btn-acao mr-2"
              onClick={onClickEditar}
              height="30px"
              width="30px"
              disabled={desabilitaEditar()}
            />
          </span>
        </Tooltip>
        <Tooltip title={desabilitaExcluir() ? '' : 'Excluir'}>
          <span>
            <Button
              id="btn-excluir"
              icon="trash-alt"
              iconType="far"
              color={Colors.Azul}
              border
              className="btn-acao"
              onClick={() => onClickExcluir(dados)}
              height="30px"
              width="30px"
              disabled={desabilitaExcluir()}
            />
          </span>
        </Tooltip>
      </div>
    );
  };

  return (
    <>
      {dados && observacaoEmEdicao && observacaoEmEdicao.id === dados.id ? (
        <>
          <ContainerCampoObservacao
            id="editando-observacao"
            autoSize={{ minRows: 3 }}
            value={observacaoEmEdicao.observacao}
            onChange={onChangeObs}
          />
          <div className="d-flex justify-content-between">
            {children}
            {btnSalvarCancelar()}
          </div>
        </>
      ) : (
        <>
          <ContainerCampoObservacao
            style={{ cursor: 'not-allowed' }}
            className="col-md-12"
            readOnly
            autoSize={{ minRows: 3 }}
            value={dados.observacao}
          />
          <div className="d-flex justify-content-between">
            {children}
            {btnEditarExcluir()}
          </div>
        </>
      )}
    </>
  );
};

LinhaObservacaoProprietario.propTypes = {
  dados: PropTypes.oneOfType([PropTypes.object]),
  onClickSalvarEdicao: PropTypes.func,
  onClickExcluir: PropTypes.func,
  children: PropTypes.node,
  podeAlterar: PropTypes.bool,
  podeExcluir: PropTypes.bool,
  proprietario: PropTypes.bool,
};

LinhaObservacaoProprietario.defaultProps = {
  dados: {},
  onClickSalvarEdicao: () => {},
  onClickExcluir: () => {},
  children: () => {},
  podeAlterar: true,
  podeExcluir: true,
  proprietario: true,
};

export default LinhaObservacaoProprietario;
