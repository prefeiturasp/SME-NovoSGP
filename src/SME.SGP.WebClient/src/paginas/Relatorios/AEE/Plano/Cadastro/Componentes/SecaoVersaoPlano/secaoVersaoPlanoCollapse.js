import * as moment from 'moment';
import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import CardCollapse from '~/componentes/cardCollapse';
import { setExibirCollapseVersao } from '~/redux/modulos/planoAEE/actions';
import MontarDadosPorSecaoVersao from './montarDadosPorSecaoVersao';

const SecaoVersaoPlanoCollapse = () => {
  const dispatch = useDispatch();

  const planoAEEDados = useSelector(store => store.planoAEE.planoAEEDados);
  const exibirCollapseVersao = useSelector(
    store => store.planoAEE.exibirCollapseVersao
  );

  const { versoes, questionarioId, turma } = planoAEEDados;

  return (
    <>
      <div className="col-md-12 mb-2">
        <strong>Planos anteriores para consulta</strong>
      </div>
      {versoes.map(plano => (
        <CardCollapse
          key={`secao-informacoes-plano-${plano.id}-collapse-key`}
          titulo={`Informações do Plano - v${plano.numero} (${moment(
            plano.criadoEm
          ).format('DD/MM/YYYY')})`}
          indice={`secao-informacoes-plano-${plano.id}-collapse-indice`}
          alt={`secao-informacoes-plano-${plano.id}-alt`}
          show={exibirCollapseVersao === plano.numero}
          onClick={() => {
            dispatch(setExibirCollapseVersao(plano.numero));
          }}
        >
          <MontarDadosPorSecaoVersao
            dados={{
              questionarioId: plano.id,
            }}
            versao={plano.id}
            questionarioId={questionarioId}
            exibir={exibirCollapseVersao === plano.numero}
            turmaCodigo={turma?.codigo}
          />
        </CardCollapse>
      ))}
    </>
  );
};

export default SecaoVersaoPlanoCollapse;
