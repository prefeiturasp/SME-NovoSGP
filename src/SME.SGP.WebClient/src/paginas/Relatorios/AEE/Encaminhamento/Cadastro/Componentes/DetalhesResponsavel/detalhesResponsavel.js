import PropTypes from 'prop-types';
import React from 'react';
import styled from 'styled-components';
import { Base } from '~/componentes/colors';

const DadosAluno = styled.div`
  border: 1px solid ${Base.CinzaDesabilitado};
  display: flex;
  height: 68px;
  flex-direction: column;
  justify-content: center;

  p {
    margin-bottom: 0;
  }
`;

const CabecalhoDetalhesResponsavel = styled.div`
  background: ${Base.CinzaFundo};
  border: 1px solid ${Base.CinzaDesabilitado};
  height: 41px;
  display: flex;
  align-items: center;
  font-weight: bold;

  span {
    color: ${Base.CinzaMako};
  }
`;

const DetalhesResponsavel = props => {
  const { rf, nome } = props;

  return (
    <div className="col-md-12">
      <CabecalhoDetalhesResponsavel className="col-md-12">
        <span>PAEE/PAAI resposável</span>
      </CabecalhoDetalhesResponsavel>
      <DadosAluno className="col-md-12">
        <p>{nome}</p>
        <p>RF: {rf}</p>
      </DadosAluno>
    </div>
  );
};

DetalhesResponsavel.propTypes = {
  rf: PropTypes.oneOfType([PropTypes.string, PropTypes.number]),
  nome: PropTypes.oneOfType([PropTypes.string]),
};

DetalhesResponsavel.defaultProps = {
  rf: '',
  nome: '',
};

export default DetalhesResponsavel;
