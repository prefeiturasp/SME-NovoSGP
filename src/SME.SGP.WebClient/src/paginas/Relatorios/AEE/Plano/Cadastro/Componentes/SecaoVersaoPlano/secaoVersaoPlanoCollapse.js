import * as moment from 'moment';
import React, { useState } from 'react';
import { useSelector } from 'react-redux';
import CardCollapse from '~/componentes/cardCollapse';
import MontarDadosPorSecaoVersao from './montarDadosPorSecaoVersao';

const SecaoVersaoPlanoCollapse = () => {
  const planoAEEDados = useSelector(store => store.planoAEE.planoAEEDados);

  const { versoes, questionarioId, turma } = planoAEEDados;

  const [exibir, setExibir] = useState(false);

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
          show={exibir}
          onClick={() => {
            setExibir(!exibir);
          }}
        >
          <MontarDadosPorSecaoVersao
            dados={{
              questionarioId: plano.id,
            }}
            versao={plano.id}
            questionarioId={questionarioId}
            exibir={exibir}
            turmaCodigo={turma?.codigo}
          />
        </CardCollapse>
      ))}
    </>
  );
};

export default SecaoVersaoPlanoCollapse;
