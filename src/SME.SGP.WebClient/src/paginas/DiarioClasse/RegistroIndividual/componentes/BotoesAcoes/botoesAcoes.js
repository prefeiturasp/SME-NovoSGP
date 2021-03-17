import React from 'react';
import PropTypes from 'prop-types';
import { useDispatch, useSelector } from 'react-redux';

import { Button, Colors } from '~/componentes';

import { history } from '~/servicos';
import { URL_HOME } from '~/constantes';

import { setDadosAlunoObjectCard } from '~/redux/modulos/conselhoClasse/actions';
import { limparDadosRegistroIndividual } from '~/redux/modulos/registroIndividual/actions';

import MetodosRegistroIndividual from '~/componentes-sgp/RegistroIndividual/metodosRegistroIndividual';

const BotoesAcoes = ({ turmaInfantil }) => {
  const registroIndividualEmEdicao = useSelector(
    state => state.registroIndividual.registroIndividualEmEdicao
  );
  const desabilitarCampos = useSelector(
    state => state.registroIndividual.desabilitarCampos
  );

  const dispatch = useDispatch();

  const onClickVoltar = async () => {
    let validouSalvarRegistro = true;
    if (registroIndividualEmEdicao && turmaInfantil && desabilitarCampos) {
      validouSalvarRegistro = await MetodosRegistroIndividual.salvarRegistroIndividual();
    }

    if (validouSalvarRegistro) {
      history.push(URL_HOME);
      MetodosRegistroIndividual.resetarInfomacoes();
      dispatch(setDadosAlunoObjectCard({}));
    }
  };

  const onClickCancelar = () => {
    dispatch(limparDadosRegistroIndividual());
  };

  const onClickCadastrar = () => {
    MetodosRegistroIndividual.verificarSalvarRegistroIndividual(true);
  };

  return (
    <>
      <Button
        label="Voltar"
        icon="arrow-left"
        color={Colors.Azul}
        border
        className="mr-2"
        onClick={onClickVoltar}
      />
      <Button
        label="Cancelar"
        color={Colors.Roxo}
        border
        className="mr-2"
        onClick={onClickCancelar}
        disabled={
          !registroIndividualEmEdicao || !turmaInfantil || !desabilitarCampos
        }
      />
      <Button
        label="Cadastrar"
        color={Colors.Roxo}
        bold
        className="mr-2"
        onClick={onClickCadastrar}
        disabled={
          !registroIndividualEmEdicao || !turmaInfantil || !desabilitarCampos
        }
      />
    </>
  );
};

BotoesAcoes.propTypes = {
  turmaInfantil: PropTypes.bool,
};

BotoesAcoes.defaultProps = {
  turmaInfantil: false,
};
export default BotoesAcoes;
