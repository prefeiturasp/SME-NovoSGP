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
  const { dados, onClickSalvarEdicao, onClickExcluir, index, children } = props;

  const dispatch = useDispatch();

  const observacaoEmEdicao = useSelector(
    store => store.observacoesUsuario.observacaoEmEdicao
  );

  const novaObservacao = useSelector(
    store => store.observacoesUsuario.novaObservacao
  );

  const onClickEditar = () => {
    const obs = [...observacaoEmEdicao];
    obs[index] = { ...dados };
    dispatch(setObservacaoEmEdicao([...obs]));
  };
  const onClickSalvar = () => {
    onClickSalvarEdicao(observacaoEmEdicao[index]).then(resultado => {
      if (resultado && resultado.status === 200) {
        dispatch(setObservacaoEmEdicao([]));
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
      dispatch(setObservacaoEmEdicao([]));
    }
  };

  const onChangeObs = ({ target: { value } }) => {
    const obs = [...observacaoEmEdicao];
    obs[index].observacao = value;
    dispatch(setObservacaoEmEdicao([...obs]));
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
                !!(
                  observacaoEmEdicao &&
                  observacaoEmEdicao.length &&
                  !observacaoEmEdicao[index]
                ) || !!novaObservacao
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
                !!(
                  observacaoEmEdicao &&
                  observacaoEmEdicao.length &&
                  !observacaoEmEdicao[index]
                ) || !!novaObservacao
              }
            />
          </span>
        </Tooltip>
      </div>
    );
  };

  return (
    <>
      {observacaoEmEdicao &&
      observacaoEmEdicao[index] &&
      observacaoEmEdicao[index].id ? (
        <>
          <ContainerCampoObservacao
            id="editando-observacao"
            autoSize={{ minRows: 3 }}
            value={observacaoEmEdicao[index].observacao}
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
  index: PropTypes.number,
  children: PropTypes.node,
};

LinhaObservacaoProprietario.defaultProps = {
  dados: {},
  onClickSalvarEdicao: () => {},
  onClickExcluir: () => {},
  index: null,
  children: () => {},
};

export default LinhaObservacaoProprietario;
