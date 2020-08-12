import PropTypes from 'prop-types';
import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import Button from '~/componentes/button';
import { Colors } from '~/componentes/colors';
import { URL_HOME } from '~/constantes/url';
import { setCarregandoCartaIntencoes } from '~/redux/modulos/cartaIntencoes/actions';
import { confirmar } from '~/servicos/alertas';
import history from '~/servicos/history';
import { ehTurmaInfantil } from '~/servicos/Validacoes/validacoesInfatil';
import servicoSalvarCartaIntencoes from '../../servicoSalvarCartaIntencoes';

const BotoesAcoesCartaIntencoes = props => {
  const dispatch = useDispatch();

  const {
    onClickCancelar,
    onClickSalvar,
    componenteCurricularId,
    codigoTurma,
    somenteConsulta,
  } = props;

  const cartaIntencoesEmEdicao = useSelector(
    store => store.cartaIntencoes.cartaIntencoesEmEdicao
  );

  const onSalvar = async () => {
    if (cartaIntencoesEmEdicao && ehTurmaInfantil) {
      dispatch(setCarregandoCartaIntencoes(true));
      const salvou = await servicoSalvarCartaIntencoes.validarSalvarCartaIntencoes(
        componenteCurricularId,
        codigoTurma
      );
      dispatch(setCarregandoCartaIntencoes(false));
      if (salvou) {
        onClickSalvar();
      }
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
    if (!somenteConsulta && cartaIntencoesEmEdicao && ehTurmaInfantil) {
      const confirmado = await perguntaAoSalvar();
      if (confirmado) {
        dispatch(setCarregandoCartaIntencoes(true));
        const salvou = await servicoSalvarCartaIntencoes.validarSalvarCartaIntencoes(
          componenteCurricularId,
          codigoTurma
        );
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
    if (cartaIntencoesEmEdicao && ehTurmaInfantil) {
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
        disabled={
          !ehTurmaInfantil || somenteConsulta || !cartaIntencoesEmEdicao
        }
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
          !ehTurmaInfantil || somenteConsulta || !cartaIntencoesEmEdicao
        }
      />
    </>
  );
};

BotoesAcoesCartaIntencoes.propTypes = {
  onClickCancelar: PropTypes.func,
  onClickSalvar: PropTypes.func,
  componenteCurricularId: PropTypes.string,
  codigoTurma: PropTypes.oneOfType([PropTypes.any]),
  somenteConsulta: PropTypes.bool,
};

BotoesAcoesCartaIntencoes.defaultProps = {
  onClickCancelar: () => {},
  onClickSalvar: () => {},
  componenteCurricularId: '',
  codigoTurma: '',
  somenteConsulta: false,
};

export default BotoesAcoesCartaIntencoes;
