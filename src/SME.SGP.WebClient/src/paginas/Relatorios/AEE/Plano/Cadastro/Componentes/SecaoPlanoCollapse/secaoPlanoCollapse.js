import PropTypes from 'prop-types';
import React from 'react';
import { useSelector } from 'react-redux';
import CardCollapse from '~/componentes/cardCollapse';
import DadosSecaoPlano from './DadosSecaoPlano/dadosSecaoPlano';
import MontarDadosPorSecao from './DadosSecaoPlano/montarDadosPorSecao';

const SecaoPlanoCollapse = props => {
  const { match } = props;

  const planoAEEDados = useSelector(store => store.planoAEE.planoAEEDados);

  return (
    <>
      {planoAEEDados?.questoes.length ? (
        <CardCollapse
          key="secao-informacoes-plano-collapse-key"
          titulo="Informações do Plano"
          indice="secao-informacoes-plano--collapse-indice"
          alt="secao-informacoes-plano--alt"
        >
          <MontarDadosPorSecao
            dados={{ id: 0, questionarioId: planoAEEDados?.questionarioId }}
            dadosQuestionarioAtual={planoAEEDados?.questoes}
            match={match}
          />
        </CardCollapse>
      ) : (
        ''
      )}
      {planoAEEDados?.versoes ? (
        <>
          <div className="col-md-12 mb-2">
            <strong>Planos anteriores para consulta</strong>
          </div>
          {planoAEEDados?.versoes.map(plano => (
            // colocar em um outro arquivo
            <CardCollapse
              key={`secao-informacoes-plano-${plano.id}-collapse-key`}
              titulo={`Informações do Plano - v${plano.numero}`}
              indice={`secao-informacoes-plano-${plano.id}-collapse-indice`}
              alt={`secao-informacoes-plano-${plano.id}-alt`}
            >
              <>
                <DadosSecaoPlano match={match} />
              </>
            </CardCollapse>
          ))}
        </>
      ) : (
        ''
      )}
    </>
  );
};

SecaoPlanoCollapse.propTypes = {
  match: PropTypes.oneOfType([PropTypes.object]),
};

SecaoPlanoCollapse.defaultProps = {
  match: {},
};

export default SecaoPlanoCollapse;
