import PropTypes from 'prop-types';
import React from 'react';
import { useSelector } from 'react-redux';
import CardCollapse from '~/componentes/cardCollapse';
import MontarDadosPorSecao from '~/paginas/Relatorios/AEE/Encaminhamento/Cadastro/Componentes/SecaoEncaminhamento/DadosSecaoEncaminhamento/montarDadosPorSecao';
import DadosSecaoPlano from './DadosSecaoPlano/dadosSecaoPlano';

const SecaoPlanoCollapse = props => {
  const { match } = props;
  const planoAEEDados = useSelector(store => store.planoAEE.planoAEEDados);

  return (
    <>
      {planoAEEDados?.secao ? (
        <CardCollapse
          key="secao-informacoes-plano-collapse-key"
          titulo={planoAEEDados?.secao.nome}
          indice="secao-informacoes-plano--collapse-indice"
          alt="secao-informacoes-plano--alt"
        >
          <>
            <MontarDadosPorSecao dados={planoAEEDados?.secao} match={match} />
          </>
        </CardCollapse>
      ) : (
        ''
      )}
      {planoAEEDados?.planosAnteriores ? (
        <>
          <div className="col-md-12 mb-2">
            <strong>Planos anteriores para consulta</strong>
          </div>
          {planoAEEDados?.planosAnteriores.map((plano, index) => (
            <CardCollapse
              key={`secao-informacoes-plano-${index}-collapse-key`}
              titulo={plano.nome}
              indice={`secao-informacoes-plano-${index}-collapse-indice`}
              alt={`secao-informacoes-plano-${index}-alt`}
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
