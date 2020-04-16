import React from 'react';
import { useSelector, useDispatch } from 'react-redux';
import Button from '~/componentes/button';
import { Colors } from '~/componentes/colors';
import { confirmar } from '~/servicos/alertas';
import history from '~/servicos/history';
import { URL_HOME } from '~/constantes/url';
import {
  setBimestreAtual,
  setConselhoClasseEmEdicao,
} from '~/redux/modulos/conselhoClasse/actions';
import servicoSalvarConselhoClasse from '../../servicoSalvarConselhoClasse';

const BotoesAcoesConselhoClasse = () => {
  const dispatch = useDispatch();

  const alunosConselhoClasse = useSelector(
    store => store.conselhoClasse.alunosConselhoClasse
  );

  const conselhoClasseEmEdicao = useSelector(
    store => store.conselhoClasse.conselhoClasseEmEdicao
  );

  const bimestreAtual = useSelector(
    store => store.conselhoClasse.bimestreAtual
  );

  const onClickSalvar = () => {
    return servicoSalvarConselhoClasse.validarSalvarRecomendacoesAlunoFamilia(
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
    if (conselhoClasseEmEdicao) {
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

  // Setar valor para renderizar a tela novamente!
  const recarregarDados = () => {
    dispatch(setConselhoClasseEmEdicao(false));
    dispatch(setBimestreAtual(bimestreAtual.valor));
  };

  const onClickCancelar = async () => {
    if (conselhoClasseEmEdicao) {
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
          !alunosConselhoClasse ||
          alunosConselhoClasse.length < 1 ||
          !conselhoClasseEmEdicao
        }
      />
      <Button
        id="btn-salvar-conselho-classe"
        label="Salvar"
        color={Colors.Roxo}
        border
        bold
        className="mr-2"
        onClick={onClickSalvar}
        disabled={!conselhoClasseEmEdicao}
      />
    </>
  );
};

export default BotoesAcoesConselhoClasse;
