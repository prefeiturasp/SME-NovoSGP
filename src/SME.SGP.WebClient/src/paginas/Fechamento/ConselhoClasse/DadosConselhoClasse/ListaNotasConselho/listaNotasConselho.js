import React from 'react';

const ListaNotasConselho = () => {
  return (
    <>
      <div className="table-responsive">
        <table className="table mt-4">
          <thead>
            <tr>
              <th>Componete</th>
              <th>Nota/Conceito</th>
              <th>Nota pós-conselho</th>
              <th>Aulas</th>
              <th>Faltas</th>
              <th>Ausências Compensadas</th>
              <th>%Freq.</th>
            </tr>
          </thead>
          <tbody></tbody>
        </table>
      </div>
    </>
  );
};

export default ListaNotasConselho;
