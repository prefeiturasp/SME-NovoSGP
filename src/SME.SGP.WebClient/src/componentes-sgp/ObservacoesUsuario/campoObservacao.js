import PropTypes from 'prop-types';
import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Label } from '~/componentes';
import Button from '~/componentes/button';
import { Colors } from '~/componentes/colors';
import { setNovaObservacao } from '~/redux/modulos/observacoesUsuario/actions';
import { confirmar } from '~/servicos/alertas';
import { ContainerCampoObservacao } from './observacoesUsuario.css';

const CampoObservacao = props => {
  const { salvarObservacao } = props;

  const dispatch = useDispatch();

  const observacaoEmEdicao = useSelector(
    store => store.observacoesUsuario.observacaoEmEdicao
  );

  const novaObservacao = useSelector(
    store => store.observacoesUsuario.novaObservacao
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

  const onClickSalvar = () => {
    salvarObservacao({ observacao: novaObservacao }).then(() => {
      dispatch(setNovaObservacao(''));
    });
  };

  return (
    <>
      <div className="col-md-12 pb-2">
        <Label text="Escreva uma observação" />
        <ContainerCampoObservacao
          id="nova-observacao"
          autoSize={{ minRows: 4 }}
          value={novaObservacao}
          onChange={onChangeNovaObservacao}
          disabled={!!(observacaoEmEdicao && observacaoEmEdicao.length)}
        />
      </div>
      <div className="col-md-12 d-flex justify-content-end pb-4">
        <Button
          id="btn-cancelar-obs-novo"
          label="Cancelar"
          color={Colors.Roxo}
          border
          bold
          className="mr-3"
          onClick={onClickCancelarNovo}
          height="30px"
          disabled={!novaObservacao}
        />
        <Button
          id="btn-salvar-obs-novo"
          label="Salvar"
          color={Colors.Roxo}
          border
          bold
          onClick={onClickSalvar}
          height="30px"
          disabled={!novaObservacao}
        />
      </div>
    </>
  );
};

CampoObservacao.propTypes = {
  salvarObservacao: PropTypes.func,
};

CampoObservacao.defaultProps = {
  salvarObservacao: () => {},
};

export default CampoObservacao;
