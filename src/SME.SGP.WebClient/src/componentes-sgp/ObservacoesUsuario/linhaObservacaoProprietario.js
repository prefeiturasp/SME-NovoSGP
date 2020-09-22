import { Tooltip } from 'antd';
import PropTypes from 'prop-types';
import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import Button from '~/componentes/button';
import { Colors } from '~/componentes/colors';
import { setObservacaoEmEdicao } from '~/redux/modulos/observacoesUsuario/actions';
import { confirmar } from '~/servicos/alertas';
import { ContainerCampoObservacao } from './observacoesUsuario.css';

const LinhaObservacaoProprietario = props => {
  const { dados, onClickSalvarEdicao, onClickExcluir, children } = props;

  const dispatch = useDispatch();

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
      }
    });
  };

  const onClickCancelar = async () => {
    const confirmou = await confirmar(
      'Atenção',
      'Você não salvou as informações preenchidas.',
      'Deseja realmente cancelar as alterações?'
    );

    if (confirmou) {
      dispatch(setObservacaoEmEdicao());
    }
  };

  const onChangeObs = ({ target: { value } }) => {
    const obs = { ...observacaoEmEdicao };
    obs.observacao = value;
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

  const btnEditarExcluir = () => {
    return (
      <div className="d-flex mt-2">
        <Tooltip title="Editar">
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
              disabled={
                !!(observacaoEmEdicao && observacaoEmEdicao.id !== dados.id) ||
                !!novaObservacao
              }
            />
          </span>
        </Tooltip>
        <Tooltip title="Excluir">
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
              disabled={
                !!(observacaoEmEdicao && observacaoEmEdicao.id !== dados.id) ||
                !!novaObservacao
              }
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
};

LinhaObservacaoProprietario.defaultProps = {
  dados: {},
  onClickSalvarEdicao: () => {},
  onClickExcluir: () => {},
  children: () => {},
};

export default LinhaObservacaoProprietario;
