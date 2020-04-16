import * as moment from 'moment';
import React, { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import styled from 'styled-components';
import { Base } from '~/componentes';

const Periodo = styled.div`
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

const MarcadorPeriodoInicioFim = () => {
  const fechamentoPeriodoInicioFim = useSelector(
    store => store.conselhoClasse.fechamentoPeriodoInicioFim
  );

  const [dataInicio, setDataInicio] = useState('');
  const [dataFim, setDataFim] = useState('');

  useEffect(() => {
    if (fechamentoPeriodoInicioFim) {
      const {
        periodoFechamentoInicio,
        periodoFechamentoFim,
      } = fechamentoPeriodoInicioFim;

      if (periodoFechamentoInicio) {
        setDataInicio(moment(periodoFechamentoInicio).format('L'));
      } else {
        setDataInicio('');
      }

      if (periodoFechamentoFim) {
        setDataFim(moment(periodoFechamentoFim).format('L'));
      } else {
        setDataFim('');
      }
    }
  }, [fechamentoPeriodoInicioFim]);

  return (
    <>
      {dataInicio && dataFim ? (
        <div className="col-m-12 d-flex justify-content-end mb-2">
          <Periodo>
            <span>
              Período de fechamento de {dataInicio} até {dataFim}
            </span>
          </Periodo>
        </div>
      ) : (
        ''
      )}
    </>
  );
};

export default MarcadorPeriodoInicioFim;
