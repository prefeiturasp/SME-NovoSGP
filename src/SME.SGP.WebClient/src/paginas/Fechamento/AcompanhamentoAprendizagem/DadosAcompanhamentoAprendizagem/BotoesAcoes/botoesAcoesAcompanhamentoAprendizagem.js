import PropTypes from 'prop-types';
import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import Button from '~/componentes/button';
import { Colors } from '~/componentes/colors';
import { URL_HOME } from '~/constantes/url';
import {
  setAcompanhamentoAprendizagemEmEdicao,
  setApanhadoGeralEmEdicao,
} from '~/redux/modulos/acompanhamentoAprendizagem/actions';
import { confirmar } from '~/servicos';
import history from '~/servicos/history';
import ServicoAcompanhamentoAprendizagem from '~/servicos/Paginas/Relatorios/AcompanhamentoAprendizagem/ServicoAcompanhamentoAprendizagem';

const BotoesAcoesAcompanhamentoAprendizagem = props => {
  const { semestreSelecionado } = props;

  const dispatch = useDispatch();

  const desabilitarCamposAcompanhamentoAprendizagem = useSelector(
    store =>
      store.acompanhamentoAprendizagem
        .desabilitarCamposAcompanhamentoAprendizagem
  );

  const acompanhamentoAprendizagemEmEdicao = useSelector(
    store => store.acompanhamentoAprendizagem.acompanhamentoAprendizagemEmEdicao
  );

  const apanhadoGeralEmEdicao = useSelector(
    store => store.acompanhamentoAprendizagem.apanhadoGeralEmEdicao
  );

  const dadosAlunoObjectCard = useSelector(
    store => store.acompanhamentoAprendizagem.dadosAlunoObjectCard
  );

  const { codigoEOL } = dadosAlunoObjectCard;

  const usuario = useSelector(store => store.usuario);
  const { turmaSelecionada } = usuario;

  const onClickSalvar = async () => {
    const salvouApanhadoGeral = await ServicoAcompanhamentoAprendizagem.salvarDadosApanhadoGeral(
      semestreSelecionado
    );

    const salvouCompanhamento = await ServicoAcompanhamentoAprendizagem.salvarDadosAcompanhamentoAprendizagem(
      semestreSelecionado
    );

    return salvouApanhadoGeral && salvouCompanhamento;
  };

  const perguntaAoSalvar = async () => {
    return confirmar(
      'Atenção',
      '',
      'Suas alterações não foram salvas, deseja salvar agora?'
    );
  };

  const onClickVoltar = async () => {
    if (
      !desabilitarCamposAcompanhamentoAprendizagem &&
      (acompanhamentoAprendizagemEmEdicao || apanhadoGeralEmEdicao)
    ) {
      const confirmado = await perguntaAoSalvar();
      if (confirmado) {
        const continuar = await onClickSalvar();
        if (continuar) {
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
    if (acompanhamentoAprendizagemEmEdicao) {
      dispatch(setAcompanhamentoAprendizagemEmEdicao(false));
      ServicoAcompanhamentoAprendizagem.obterAcompanhamentoEstudante(
        turmaSelecionada?.id,
        codigoEOL,
        semestreSelecionado
      );
    }

    if (apanhadoGeralEmEdicao) {
      dispatch(setApanhadoGeralEmEdicao(false));
      ServicoAcompanhamentoAprendizagem.obterDadosApanhadoGeral(
        turmaSelecionada?.id,
        semestreSelecionado
      );
    }
  };

  const onClickCancelar = async () => {
    if (acompanhamentoAprendizagemEmEdicao || apanhadoGeralEmEdicao) {
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
        className="mr-2"
        onClick={onClickCancelar}
        disabled={
          desabilitarCamposAcompanhamentoAprendizagem ||
          (!acompanhamentoAprendizagemEmEdicao && !apanhadoGeralEmEdicao)
        }
      />
      <Button
        id="btn-salvar"
        label="Salvar"
        color={Colors.Roxo}
        border
        bold
        onClick={onClickSalvar}
        disabled={
          desabilitarCamposAcompanhamentoAprendizagem ||
          (!acompanhamentoAprendizagemEmEdicao && !apanhadoGeralEmEdicao)
        }
      />
    </>
  );
};

BotoesAcoesAcompanhamentoAprendizagem.propTypes = {
  semestreSelecionado: PropTypes.string,
};

BotoesAcoesAcompanhamentoAprendizagem.defaultProps = {
  semestreSelecionado: '',
};

export default BotoesAcoesAcompanhamentoAprendizagem;
