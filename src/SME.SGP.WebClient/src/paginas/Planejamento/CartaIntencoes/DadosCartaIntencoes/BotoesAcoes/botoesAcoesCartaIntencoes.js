import PropTypes from 'prop-types';
import React from 'react';
import { useSelector, useDispatch } from 'react-redux';
import Button from '~/componentes/button';
import { Colors } from '~/componentes/colors';
import { URL_HOME } from '~/constantes/url';
import { confirmar } from '~/servicos/alertas';
import history from '~/servicos/history';
// import { ehTurmaInfantil } from '~/servicos/Validacoes/validacoesInfatil';
import servicoSalvarCartaIntencoes from '../../servicoSalvarCartaIntencoes';
import { setCarregandoCartaIntencoes } from '~/redux/modulos/cartaIntencoes/actions';

const BotoesAcoesCartaIntencoes = props => {
  // const usuario = useSelector(store => store.usuario);
  // const { turmaSelecionada } = usuario;
  const dispatch = useDispatch();

  const { onClickCancelar, onClickSalvar } = props;

  // const modalidadesFiltroPrincipal = useSelector(
  //   store => store.filtro.modalidades
  // );

  const cartaIntencoesEmEdicao = useSelector(
    store => store.cartaIntencoes.cartaIntencoesEmEdicao
  );

  const desabilitarCampos = useSelector(
    store => store.cartaIntencoes.desabilitarCampos
  );

  const onSalvar = async () => {
    dispatch(setCarregandoCartaIntencoes(true));
    const salvou = await servicoSalvarCartaIntencoes.validarSalvarCartaIntencoes();
    dispatch(setCarregandoCartaIntencoes(false));
    if (salvou) {
      onClickSalvar();
    }
  };

  const perguntaAoSalvar = async () => {
    return confirmar(
      'Atenção',
      '',
      'Suas alterações não foram salvas, deseja salvar agora?'
    );
  };

  const onClickVoltar = async () => {
    if (!desabilitarCampos && cartaIntencoesEmEdicao) {
      const confirmado = await perguntaAoSalvar();
      if (confirmado) {
        dispatch(setCarregandoCartaIntencoes(true));
        const salvou = await servicoSalvarCartaIntencoes.validarSalvarCartaIntencoes();
        dispatch(setCarregandoCartaIntencoes(false));
        if (salvou) {
          history.push(URL_HOME);
        }
      } else {
        history.push(URL_HOME);
      }
    } else {
      history.push(URL_HOME);
    }
  };

  const onCancelar = async () => {
    if (cartaIntencoesEmEdicao) {
      const confirmou = await confirmar(
        'Atenção',
        'Você não salvou as informações preenchidas.',
        'Deseja realmente cancelar as alterações?'
      );
      if (confirmou) {
        onClickCancelar();
      }
    }
  };
  return (
    <>
      <Button
        id="btn-voltar-carta-intencoes"
        label="Voltar"
        icon="arrow-left"
        color={Colors.Azul}
        border
        className="mr-2"
        onClick={onClickVoltar}
      />
      <Button
        id="btn-cancelar-carta-intencoes"
        label="Cancelar"
        color={Colors.Roxo}
        border
        className="mr-2"
        onClick={onCancelar}
        disabled={desabilitarCampos || !cartaIntencoesEmEdicao}
      />
      <Button
        id="btn-salvar-carta-intencoes"
        label="Salvar"
        color={Colors.Roxo}
        border
        bold
        className="mr-2"
        onClick={onSalvar}
        disabled={
          // ehTurmaInfantil(modalidadesFiltroPrincipal, turmaSelecionada) ||
          desabilitarCampos || !cartaIntencoesEmEdicao
        }
      />
    </>
  );
};

BotoesAcoesCartaIntencoes.propTypes = {
  onClickCancelar: PropTypes.func,
  onClickSalvar: PropTypes.func,
};

BotoesAcoesCartaIntencoes.defaultProps = {
  onClickCancelar: () => {},
  onClickSalvar: () => {},
};

export default BotoesAcoesCartaIntencoes;
