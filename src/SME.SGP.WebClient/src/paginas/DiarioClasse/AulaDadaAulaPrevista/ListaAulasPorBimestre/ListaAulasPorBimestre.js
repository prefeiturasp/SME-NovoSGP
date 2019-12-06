import React, { useState, useEffect } from 'react';
import { Corpo, CampoDesabilitado } from './ListaAulasPorBimestre.css';

const ListaAulasPorBimestre = props => {
  const { dados } = props;
  const { totalPrevistas, totalCriadasProfTitular, totalCriadasProfCj, totalDadas, totalRespostas } = dados;

  const formatarData = data => {
    return window.moment(data).format('DD/MM');
  }

  return (
    <Corpo>
      <table className="table mb-0 ">
        <thead className="tabela-frequencia-thead">
          <tr>
            <th className="width-60"></th>
            <th className="text-left fundo-cinza">Previstas</th>
            <th className="text-left fundo-cinza">Criadas</th>
            <th className="text-left fundo-cinza">Dadas</th>
            <th className="text-left fundo-cinza">Respostas</th>
          </tr>
        </thead>
        {dados && dados.bimestres ? dados.bimestres.map(item => {
          return (
            <tr>
              <td className="fundo-cinza">
                <span className="negrito">{`${item.bimestre}º Bimestre`}</span>
                <span>{` - ${formatarData(item.inicio)} à ${formatarData(item.fim)}`}</span>
              </td>
              <td>
                {item.previstas.quantidade}
              </td>
              <td>
                {item.criadas.professorTitular}
              </td>
              <td>
                {item.dadas}
              </td>
              <td>
                {item.respostas}
              </td>
            </tr>
          );
        })
          : null}
        <tr className="fundo-cinza">
          <td>
            <span className="negrito">Total</span>
          </td>
          <td>
            <CampoDesabilitado>
              <span>{totalPrevistas}</span>
            </CampoDesabilitado>
          </td>
          <td>
            <CampoDesabilitado>
              <span>{totalCriadasProfTitular}</span>
            </CampoDesabilitado>
          </td>
          <td>
            <CampoDesabilitado>
              <span>{totalDadas}</span>
            </CampoDesabilitado>
          </td>
          <td>
            <CampoDesabilitado>
              <span>{totalRespostas}</span>
            </CampoDesabilitado>
          </td>
        </tr>
      </table>
    </Corpo>
  )
}

export default ListaAulasPorBimestre;
