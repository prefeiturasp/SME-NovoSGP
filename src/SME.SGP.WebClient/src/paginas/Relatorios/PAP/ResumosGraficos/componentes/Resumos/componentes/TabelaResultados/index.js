import React, { useEffect, useState } from 'react';
import styled from 'styled-components';

// Ant
import { Table } from 'antd';
import ResumosGraficosPAPServico from '~/servicos/Paginas/Relatorios/PAP/ResumosGraficos';

const Tabela = styled(Table)``;

const TabelaResultados = () => {
  const [filtro] = useState(true);

  const buscarDadosApi = () => {
    ResumosGraficosPAPServico.ListarResultados(filtro).then(retorno => {
      const { data, status } = retorno;

      if (data && status === 200) {
        const montaColunas = [];

        const eixos = data[0].Eixos;

        console.log(typeof eixos);

        eixos.map(eixo => {
          eixo.Objetivos.map(objetivo => {
            objetivo.Anos.map(ano => {
              ano.Respostas.map(resposta => {
                console.log(
                  eixo.EixoDescricao,
                  objetivo.ObjetivoDescricao,
                  ano.AnoDescricao,
                  resposta.RespostaDescricao
                );
              });
            });
          });
        });
      }
    });
  };

  useEffect(() => {
    buscarDadosApi();
  }, []);

  return (
    <Tabela
      pagination={false}
      columns={[]}
      dataSource={[]}
      rowKey="key"
      size="middle"
      className="my-2"
      bordered
    />
  );
};

export default TabelaResultados;
