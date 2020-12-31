import React, { useCallback, useEffect, useState } from 'react';
import PropTypes from 'prop-types';
import shortid from 'shortid';

import { useDispatch, useSelector } from 'react-redux';
import { CardCollapse } from '~/componentes';

import { ServicoRegistroIndividual } from '~/servicos';

import { setDadosPrincipaisRegistroIndividual } from '~/redux/modulos/registroIndividual/actions';

import NovoRegistroIndividualItem from './novoRegistroIndividualItem/novoRegistroIndividualItem';

import { CONFIG_COLLAPSE_REGISTRO_INDIVIDUAL } from '~/constantes';

const NovoRegistroIndividual = ({ dadosAlunoObjectCard }) => {
  const idCollapse = shortid.generate();
  const [expandir, setExpandir] = useState(false);
  const [exibirCollapse, setExibirCollapse] = useState(false);

  const componenteCurricularSelecionado = useSelector(
    state => state.registroIndividual.componenteCurricularSelecionado
  );
  const dadosPrincipaisRegistroIndividual = useSelector(
    store => store.registroIndividual.dadosPrincipaisRegistroIndividual
  );
  const { turmaSelecionada } = useSelector(state => state.usuario);
  const turmaId = turmaSelecionada?.id || 0;

  const dispatch = useDispatch();

  const obterRegistroIndividualPorData = useCallback(async () => {
    const dataAtual = window.moment().format('MM-DD-YYYY');
    if (dataAtual) {
      const retorno = await ServicoRegistroIndividual.obterRegistroIndividualPorData(
        {
          alunoCodigo: dadosAlunoObjectCard.codigoEOL,
          componenteCurricular: componenteCurricularSelecionado,
          data: dataAtual,
          turmaId,
        }
      );
      if (retorno?.data) {
        dispatch(setDadosPrincipaisRegistroIndividual(retorno.data));
      }
    }
  }, [
    dispatch,
    componenteCurricularSelecionado,
    dadosAlunoObjectCard,
    turmaId,
  ]);

  useEffect(() => {
    const temDadosAlunos = Object.keys(dadosAlunoObjectCard).length;
    if (temDadosAlunos) {
      obterRegistroIndividualPorData();
    }
  }, [dadosAlunoObjectCard, obterRegistroIndividualPorData]);

  useEffect(() => {
    const temDadosRegistros = Object.keys(dadosPrincipaisRegistroIndividual)
      .length;
    if (temDadosRegistros) {
      setExpandir(true);
      // retirar a exclamação para o funcionamento correto
      setExibirCollapse(
        !dadosPrincipaisRegistroIndividual?.podeRealizarNovoRegistro
      );
    }
  }, [setExibirCollapse, dadosPrincipaisRegistroIndividual]);

  return (
    <>
      {exibirCollapse && (
        <div key={shortid.generate()} className="px-4 pt-4">
          <CardCollapse
            configCabecalho={CONFIG_COLLAPSE_REGISTRO_INDIVIDUAL}
            styleCardBody={{ paddingTop: 12 }}
            key={`${idCollapse}-collapse-key`}
            titulo="Novo registro individual"
            indice={`${idCollapse}-collapse-indice`}
            alt={`${idCollapse}-alt`}
            show={expandir}
          >
            <NovoRegistroIndividualItem
              dadosAlunoObjectCard={dadosAlunoObjectCard}
            />
          </CardCollapse>
        </div>
      )}
    </>
  );
};

NovoRegistroIndividual.propTypes = {
  dadosAlunoObjectCard: PropTypes.oneOfType([
    PropTypes.object,
    PropTypes.string,
  ]),
};

NovoRegistroIndividual.defaultProps = {
  dadosAlunoObjectCard: {},
};

export default NovoRegistroIndividual;
