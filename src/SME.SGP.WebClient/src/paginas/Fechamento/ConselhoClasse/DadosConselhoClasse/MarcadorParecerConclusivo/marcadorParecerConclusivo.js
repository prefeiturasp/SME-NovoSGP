import React, { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import styled from 'styled-components';
import { Base, Loader } from '~/componentes';

const LabelParecer = styled.div`
  font-size: 12px;
  color: rgb(33, 37, 41);
  background-color: ${Base.Roxo};
  color: ${Base.Branco};
  display: 'flex';
  align-items: 'center';
  justify-content: 'center';
  text-align: center;
  border-radius: 4px;
  margin-right: 14px;
  height: 23px;
  padding-top: 3px;

  span {
    margin: 11px;
  }
`;

const MarcadorParecerConclusivo = () => {
  const marcadorParecerConclusivo = useSelector(
    store => store.conselhoClasse.marcadorParecerConclusivo
  );

  const gerandoParecerConclusivo = useSelector(
    store => store.conselhoClasse.gerandoParecerConclusivo
  );

  const [parecer, setParecer] = useState('');

  useEffect(() => {
    if (marcadorParecerConclusivo) {
      const { nome } = marcadorParecerConclusivo;

      if (nome) {
        setParecer(`Parecer conclusivo: ${nome}`);
      } else {
        setParecer('');
      }
    }
  }, [marcadorParecerConclusivo]);

  const montarDescricao = () => {
    if (gerandoParecerConclusivo) {
      return 'Gerando parecer conclusivo';
    }
    return parecer;
  };

  return (
    <>
      {parecer ? (
        <div className="col-m-12 d-flex justify-content-end mb-2">
          <LabelParecer>
            <Loader loading={gerandoParecerConclusivo} tip="">
              <span>{montarDescricao()}</span>
            </Loader>
          </LabelParecer>
        </div>
      ) : (
        ''
      )}
    </>
  );
};

export default MarcadorParecerConclusivo;
