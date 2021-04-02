import PropTypes from 'prop-types';
import React, { useState } from 'react';
import { Base } from '~/componentes';
import CardCollapse from '~/componentes/cardCollapse';
import ServicoDashboardAEE from '~/servicos/Paginas/Dashboard/ServicoDashboardAEE';
import GraficoBarrasPadraoAEE from '../../graficoBarrasPadraoAEE';

const QuantidadeEstudantesEncaminhamentosDeferidos = props => {
  const { anoLetivo, dreId, ueId } = props;

  const configCabecalho = {
    altura: '44px',
    corBorda: Base.AzulBordaCollapse,
  };

  const [exibir, setExibir] = useState(false);

  return (
    <div className="mt-3">
      <CardCollapse
        key="quantidade-estudantes-encaminhamentos-deferidos-collapse-key"
        titulo="Quantidade de estudantes com encaminhamentos deferidos "
        indice="quantidade-estudantes-encaminhamentos-deferidos-collapse-indice"
        alt="quantidade-estudantes-encaminhamentos-deferidos-alt"
        configCabecalho={configCabecalho}
        show={exibir}
        onClick={() => {
          setExibir(!exibir);
        }}
      >
        {exibir ? (
          <div className="col-md-12">
            <GraficoBarrasPadraoAEE
              anoLetivo={anoLetivo}
              dreId={dreId}
              ueId={ueId}
              nomeIndiceDesc="descricao"
              nomeValor="quantidade"
              ServicoObterValoresGrafico={
                ServicoDashboardAEE.obterEncaminhamentosDeferidos
              }
            />
          </div>
        ) : (
          ''
        )}
      </CardCollapse>
    </div>
  );
};

QuantidadeEstudantesEncaminhamentosDeferidos.propTypes = {
  anoLetivo: PropTypes.oneOfType(PropTypes.any),
  dreId: PropTypes.string,
  ueId: PropTypes.string,
};

QuantidadeEstudantesEncaminhamentosDeferidos.defaultProps = {
  anoLetivo: null,
  dreId: '',
  ueId: '',
};

export default QuantidadeEstudantesEncaminhamentosDeferidos;
