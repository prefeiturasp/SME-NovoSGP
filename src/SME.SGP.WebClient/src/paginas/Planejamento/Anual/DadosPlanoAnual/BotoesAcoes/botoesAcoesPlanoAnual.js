import PropTypes from 'prop-types';
import React from 'react';
import { useSelector } from 'react-redux';
import Button from '~/componentes/button';
import { Colors } from '~/componentes/colors';
import { URL_HOME } from '~/constantes/url';
import history from '~/servicos/history';
import { ehTurmaInfantil } from '~/servicos/Validacoes/validacoesInfatil';

const BotoesAcoesPlanoAnual = props => {
  const { onClickCancelar, onClickSalvar } = props;

  const usuario = useSelector(store => store.usuario);
  const { turmaSelecionada } = usuario;

  const modalidadesFiltroPrincipal = useSelector(
    store => store.filtro.modalidades
  );

  const onSalvar = async () => {
    onClickSalvar();
  };

  const onClickVoltar = async () => {
    history.push(URL_HOME);
  };

  const onCancelar = async () => {
    onClickCancelar();
  };

  return (
    <>
      <Button
        id="btn-voltar-plano-anual"
        label="Voltar"
        icon="arrow-left"
        color={Colors.Azul}
        border
        className="mr-3"
        onClick={onClickVoltar}
      />
      <Button
        id="btn-cancelar-plano-anual"
        label="Cancelar"
        color={Colors.Roxo}
        border
        className="mr-3"
        onClick={onCancelar}
        disabled={ehTurmaInfantil(modalidadesFiltroPrincipal, turmaSelecionada)}
      />
      <Button
        id="btn-salvar-plano-anual"
        label="Salvar"
        color={Colors.Roxo}
        border
        bold
        onClick={onSalvar}
        disabled={ehTurmaInfantil(modalidadesFiltroPrincipal, turmaSelecionada)}
      />
    </>
  );
};

BotoesAcoesPlanoAnual.propTypes = {
  onClickCancelar: PropTypes.func,
  onClickSalvar: PropTypes.func,
};

BotoesAcoesPlanoAnual.defaultProps = {
  onClickCancelar: () => {},
  onClickSalvar: () => {},
};

export default BotoesAcoesPlanoAnual;
