import React from 'react';
import t from 'prop-types';

function DataInicioFim({ dadosAula }) {
  return (
    !!dadosAula.dataInicio &&
    !!dadosAula.dataFim &&
    window.moment(dadosAula.dataInicio).format(`YYYY-MM-DD`) !==
      window.moment(dadosAula.dataFim).format(`YYYY-MM-DD`) && (
      <span>
        <span>
          Data Início: &nbsp;
          <strong>
            {window.moment(dadosAula.dataInicio).format(`DD/MM/YYYY`)}
          </strong>
        </span>
        &nbsp; &nbsp;
        <span>
          Data Fim: &nbsp;
          <strong>
            {window.moment(dadosAula.dataFim).format(`DD/MM/YYYY`)}
          </strong>
        </span>
      </span>
    )
  );
}

DataInicioFim.propTypes = {
  dadosAula: t.oneOfType([t.any]).isRequired,
};

export default DataInicioFim;
