import React, { useState, useEffect } from 'react';
import styled from 'styled-components';

const Container = styled.div`
  .float-right {
    float: right;
  }
`;

const TempoExpiracaoSessao = () => {
  const [mostraTempoExpiracao, setMostraTempoExpiracao] = useState(false);

  useEffect(() => {
    const interval = setInterval(() => {
      setMostraTempoExpiracao(!mostraTempoExpiracao);
    }, 5000);
    return () => clearInterval(interval);
  }, [mostraTempoExpiracao]);

  return (
    <>
      {mostraTempoExpiracao ? (
        <Container>
          <button type="button" className="float-right">
            Click Me!
          </button>
        </Container>
      ) : (
        ''
      )}
    </>
  );
};

export default TempoExpiracaoSessao;
