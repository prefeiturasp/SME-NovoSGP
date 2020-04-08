import React from 'react';
import {
  Lista,
  CampoDesabilitado,
  BarraLateral,
} from './listaNotasConselho.css';
import { Dados } from './mock';
import shortid from 'shortid';

const ListaNotasConselho = () => {
  const dadosLista = Dados;
  return (
    <Lista>
      <div className="table-responsive pl-2 pr-2">
        <table className="table mt-4">
          <thead className="tabela-conselho-thead">
            <tr>
              <th colSpan="2">Componete</th>
              <th>Nota/Conceito</th>
              <th>Nota pós-conselho</th>
              <th>Aulas</th>
              <th>Faltas</th>
              <th>Ausências Compensadas</th>
              <th>%Freq.</th>
            </tr>
          </thead>
          <tbody className="tabela-conselho-tbody">
            {dadosLista.componentes.map(item => {
              return (
                <tr key={shortid.generate()}>
                  <BarraLateral style={{ width: '7px' }} />
                  <td>
                    <div>{item.nome}</div>
                  </td>
                  <td>
                    <CampoDesabilitado>
                      <span>{item.notaConceito}</span>
                    </CampoDesabilitado>
                  </td>
                  <td>{item.notaPosConslelho}</td>
                  <td>{item.quantidadeAulas}</td>
                  <td>{item.faltas}</td>
                  <td>{item.ausenciasCompensadas}</td>
                  <td>{item.frequencia}%</td>
                </tr>
              );
            })}
          </tbody>
        </table>
      </div>
    </Lista>
  );
};

export default ListaNotasConselho;
