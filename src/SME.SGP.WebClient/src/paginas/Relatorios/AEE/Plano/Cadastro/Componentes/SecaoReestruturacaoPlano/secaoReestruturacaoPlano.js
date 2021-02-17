import React, { useCallback, useEffect, useState } from 'react';
import PropTypes from 'prop-types';
import moment from 'moment';

import { useSelector } from 'react-redux';
import { CardCollapse } from '~/componentes';

import ServicoPlanoAEE from '~/servicos/Paginas/Relatorios/AEE/ServicoPlanoAEE';

import ReestruturacaoTabela from '../ReestruturacaoTabela/reestruturacaoTabela';

const SecaoReestruturacaoPlano = ({ match }) => {
  const [listaPrimeiroSemestre, setListaPrimeiroSemestre] = useState([]);
  const [listaSegundoSemestre, setListaSegundoSemestre] = useState([]);
  const [listaVersao, setListaVersao] = useState([]);
  const [listaReestruturacao, setListaReestruturacao] = useState([]);

  const keyPrimeiroSemestre = 'secao-1-semestre-plano-collapse';
  const keySegundoSemestre = 'secao-2-semestre-plano-collapse';

  const reestruturacaoDados = useSelector(
    store => store.planoAEE.reestruturacaoDados
  );

  const obterVersoes = useCallback(async () => {
    const resposta = await ServicoPlanoAEE.obterVersoes(match?.params?.id);
    if (resposta?.data) {
      setListaVersao(resposta.data);
    }
  }, [match]);

  useEffect(() => {
    obterVersoes();
  }, [obterVersoes]);

  const FormatarDados = dados =>
    dados.map(item => {
      const dataFormatada = moment(item.data).format('DD/MM/YYYY');
      return {
        ...item,
        data: dataFormatada,
      };
    });

  const separarDados = useCallback(dados => {
    if (dados) {
      const dadosPrimeiroSemestre = dados.filter(item => item.semestre === 1);
      const dadosSegundoSemestre = dados.filter(item => item.semestre === 2);
      setListaReestruturacao(estadoAntigo => [...estadoAntigo, ...dados]);
      setListaPrimeiroSemestre(estadoAntigo => [
        ...estadoAntigo,
        ...FormatarDados(dadosPrimeiroSemestre),
      ]);
      setListaSegundoSemestre(estadoAntigo => [
        ...estadoAntigo,
        ...FormatarDados(dadosSegundoSemestre),
      ]);
    }
  }, []);

  const obterReestruturacoes = useCallback(async () => {
    const resposta = await ServicoPlanoAEE.obterReestruturacoes(
      match?.params?.id
    );
    if (resposta?.data) {
      separarDados(resposta?.data);
    }
  }, [match, separarDados]);

  useEffect(() => {
    obterReestruturacoes();
  }, [obterReestruturacoes]);

  useEffect(() => {
    if (reestruturacaoDados) {
      separarDados(reestruturacaoDados);
    }
  }, [reestruturacaoDados, separarDados]);

  return (
    <>
      <CardCollapse
        key={`${keyPrimeiroSemestre}-key`}
        titulo="Reestruturações do 1º Semestre"
        show
        indice={`${keyPrimeiroSemestre}-indice`}
        alt="secao-informacoes-plano-alt"
      >
        <ReestruturacaoTabela
          key={keyPrimeiroSemestre}
          semestre={1}
          listaDados={listaPrimeiroSemestre}
          match={match}
          listaVersao={listaVersao}
        />
      </CardCollapse>
      <CardCollapse
        key={`${keySegundoSemestre}-key`}
        titulo="Reestruturações do 2º Semestre"
        show
        indice={`${keySegundoSemestre}-indice`}
        alt="secao-informacoes-plano-alt"
      >
        <ReestruturacaoTabela
          key={keySegundoSemestre}
          semestre={2}
          listaDados={listaSegundoSemestre}
          match={match}
          listaVersao={listaVersao}
        />
      </CardCollapse>
    </>
  );
};

SecaoReestruturacaoPlano.defaultProps = {
  match: {},
};

SecaoReestruturacaoPlano.propTypes = {
  match: PropTypes.oneOfType([PropTypes.object]),
};

export default SecaoReestruturacaoPlano;
