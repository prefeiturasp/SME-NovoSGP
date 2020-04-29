import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import Button from '~/componentes/button';
import { Colors } from '~/componentes/colors';
import { URL_HOME } from '~/constantes/url';
import { setRelatorioSemestralEmEdicao } from '~/redux/modulos/relatorioSemestral/actions';
import { confirmar } from '~/servicos/alertas';
import history from '~/servicos/history';
import servicoSalvarRelatorioSemestral from '../../servicoSalvarRelatorioSemestral';

const BotoesAcoesRelatorioSemestral = () => {
  const dispatch = useDispatch();

  const alunosRelatorioSemestral = useSelector(
    store => store.relatorioSemestral.alunosRelatorioSemestral
  );

  const relatorioSemestralEmEdicao = useSelector(
    store => store.relatorioSemestral.relatorioSemestralEmEdicao
  );

  const desabilitarCampos = useSelector(
    store => store.relatorioSemestral.desabilitarCampos
  );

  const onClickSalvar = () => {
    return servicoSalvarRelatorioSemestral.validarSalvarRelatorioSemestral(
      true
    );
  };

  const perguntaAoSalvar = async () => {
    return confirmar(
      'Atenção',
      '',
      'Suas alterações não foram salvas, deseja salvar agora?'
    );
  };

  const onClickVoltar = async () => {
    if (!desabilitarCampos && relatorioSemestralEmEdicao) {
      const confirmado = await perguntaAoSalvar();
      if (confirmado) {
        const salvou = await onClickSalvar();
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

  const recarregarDados = () => {
    // TODO Recarregar os dados em tela!!
    dispatch(setRelatorioSemestralEmEdicao(false));
  };

  const onClickCancelar = async () => {
    if (relatorioSemestralEmEdicao) {
      const confirmou = await confirmar(
        'Atenção',
        'Você não salvou as informações preenchidas.',
        'Deseja realmente cancelar as alterações?'
      );
      if (confirmou) {
        recarregarDados();
      }
    }
  };
  return (
    <>
      <Button
        id="btn-voltar-relatorio-semestral"
        label="Voltar"
        icon="arrow-left"
        color={Colors.Azul}
        border
        className="mr-2"
        onClick={onClickVoltar}
      />
      <Button
        id="btn-cancelar-relatorio-semestral"
        label="Cancelar"
        color={Colors.Roxo}
        border
        className="mr-2"
        onClick={onClickCancelar}
        disabled={
          desabilitarCampos ||
          !relatorioSemestralEmEdicao ||
          !alunosRelatorioSemestral ||
          alunosRelatorioSemestral.length < 1 ||
          !alunosRelatorioSemestral
        }
      />
      <Button
        id="btn-salvar-relatorio-semestral"
        label="Salvar"
        color={Colors.Roxo}
        border
        bold
        className="mr-2"
        onClick={onClickSalvar}
        disabled={desabilitarCampos || !relatorioSemestralEmEdicao}
      />
    </>
  );
};

export default BotoesAcoesRelatorioSemestral;
