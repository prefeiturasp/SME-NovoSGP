import * as moment from 'moment';
import PropTypes from 'prop-types';
import React, { useEffect, useState } from 'react';
import CardCollapse from '~/componentes/cardCollapse';
import MontarDadosPorSecaoVersao from './montarDadosPorSecaoVersao';

const SecaoVersaoPlanoCollapse = props => {
  const { versoes, questionarioId, planoId } = props;
  const [versoesMapeado, setVersoesMapeado] = useState([]);

  useEffect(() => {
    versoes.shift();
    setVersoesMapeado(versoes);
  }, [versoes]);

  return (
    <>
      <div className="col-md-12 mb-2">
        <strong>Planos anteriores para consulta</strong>
      </div>
      {versoesMapeado.map(plano => (
        // colocar em um outro arquivo
        <CardCollapse
          key={`secao-informacoes-plano-${plano.id}-collapse-key`}
          titulo={`Informações do Plano - v${plano.numero} (${moment(
            plano.criadoEm
          ).format('DD/MM/YYYY')})`}
          indice={`secao-informacoes-plano-${plano.id}-collapse-indice`}
          alt={`secao-informacoes-plano-${plano.id}-alt`}
        >
          <MontarDadosPorSecaoVersao
            dados={{
              id: planoId,
              questionarioId,
            }}
            versao={plano.id}
          />
        </CardCollapse>
      ))}
    </>
  );
};

SecaoVersaoPlanoCollapse.propTypes = {
  versoes: PropTypes.oneOfType([PropTypes.object]),
  questionarioId: PropTypes.oneOfType([PropTypes.number]),
  planoId: PropTypes.oneOfType([PropTypes.number]),
};

SecaoVersaoPlanoCollapse.defaultProps = {
  versoes: [],
  questionarioId: 0,
  planoId: 0,
};

export default SecaoVersaoPlanoCollapse;
