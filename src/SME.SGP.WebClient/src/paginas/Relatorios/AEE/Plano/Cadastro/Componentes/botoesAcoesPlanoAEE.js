import PropTypes from 'prop-types';
import React from 'react';
import { useSelector } from 'react-redux';
import Button from '~/componentes/button';
import { Colors } from '~/componentes';
import { history, sucesso } from '~/servicos';
import { RotasDto } from '~/dtos';
import ServicoPlanoAEE from '~/servicos/Paginas/Relatorios/AEE/ServicoPlanoAEE';

const BotoesAcoesPlanoAEE = props => {
  const { match } = props;

  const questionarioDinamicoEmEdicao = useSelector(
    store => store.questionarioDinamico.questionarioDinamicoEmEdicao
  );

  const desabilitarCamposPlanoAEE = useSelector(
    store => store.planoAEE.desabilitarCamposPlanoAEE
  );

  const onClickVoltar = async () => {
    history.push(RotasDto.RELATORIO_AEE_PLANO);
  };

  const onClickCancelar = async () => {
    history.push(RotasDto.RELATORIO_AEE_PLANO);
  };

  const onClickSalvar = async () => {
    const salvou = await ServicoPlanoAEE.salvarPlano();
    const planoId = match?.params?.id;

    if (salvou) {
      let mensagem = 'Registro salvo com sucesso';
      if (planoId) {
        mensagem = 'Registro alterado com sucesso';
      }
      sucesso(mensagem);
      history.push(RotasDto.RELATORIO_AEE_PLANO);
    }
  };

  return (
    <>
      <Button
        id="btn-voltar"
        label="Voltar"
        icon="arrow-left"
        color={Colors.Azul}
        border
        className="mr-2"
        onClick={onClickVoltar}
      />
      <Button
        id="btn-cancelar"
        label="Cancelar"
        color={Colors.Roxo}
        border
        className="mr-3"
        onClick={onClickCancelar}
      />
      <Button
        id="btn-salvar"
        label={match?.params?.id ? 'Alterar' : 'Salvar'}
        color={Colors.Azul}
        border
        bold
        onClick={onClickSalvar}
        disabled={desabilitarCamposPlanoAEE || !questionarioDinamicoEmEdicao}
      />
    </>
  );
};

BotoesAcoesPlanoAEE.propTypes = {
  match: PropTypes.oneOfType([PropTypes.object]),
};

BotoesAcoesPlanoAEE.defaultProps = {
  match: {},
};

export default BotoesAcoesPlanoAEE;
