import React, { useEffect, useState } from 'react';
import styled from 'styled-components';

// Ant
import { Table } from 'antd';
import ResumosGraficosPAPServico from '~/servicos/Paginas/Relatorios/PAP/ResumosGraficos';

const Tabela = styled(Table)``;

const TabelaResultados = () => {
  const [colunas, setColunas] = useState([
    {
      title: 'Eixo',
      dataIndex: 'Eixo',
      colSpan: 3,
      width: 200,
      render: (text, row, index) => {
        return {
          children: text,
          props: {
            rowSpan: index % 3 === 0 ? 3 : 0,
            style: { fontWeight: 'bold' },
          },
        };
      },
    },
    {
      title: 'Objetivo',
      dataIndex: 'Objetivo',
      colSpan: 0,
      width: 150,
      render: (text, row, index) => {
        return {
          children: text,
          props: {
            rowSpan: index % 3 === 0 ? 3 : 0,
            style: { fontWeight: 'bold' },
          },
        };
      },
    },
    {
      title: 'Resposta',
      dataIndex: 'Resposta',
      colSpan: 0,
      width: 150,
      render: text => {
        return {
          children: text,
          props: {
            rowSpan: 1,
            style: { fontWeight: 'bold' },
          },
        };
      },
    },
  ]);

  const [filtro] = useState(true);

  const buscarDadosApi = () => {
    ResumosGraficosPAPServico.ListarResultados(filtro).then(retorno => {
      const { data, status } = retorno;

      if (data && status === 200) {
        const montaColunas = [];

        const eixos = data[0].Eixos;

        eixos.map(eixo => {
          eixo.Objetivos.map(objetivo => {
            objetivo.Anos.map(ano => {
              montaColunas.push({
                title: `${ano.AnoDescricao}`,
                dataIndex: `${ano.AnoDescricao}`,
              });
              ano.Respostas.map(resposta => {
                // console.log(
                //   `${eixo.EixoDescricao}\n`,
                //   `${objetivo.ObjetivoDescricao}\n`,
                //   `${ano.AnoDescricao}\n`,
                //   `${resposta.RespostaDescricao}\n`,
                //   `${resposta.Quantidade}\n`
                // );
              });
            });
          });
        });

        montaColunas.push({
          title: 'Total',
          dataIndex: 'Total',
        });

        setColunas([...colunas, ...montaColunas]);
      }
    });
  };

  useEffect(() => {
    buscarDadosApi();
  }, []);

  const dados = [
    {
      Id: 0,
      Eixo: 'Analisa, interpreta e soluciona problemas envolvendo',
      Objetivo: 'Dados apresentados em tabelas e gráficos',
      Resposta: 'Realizou Plenamente',
      '3ºC': 13,
      '4ºC': 2,
      Total: 15,
    },
    {
      Id: 1,
      Eixo: 'Analisa, interpreta e soluciona problemas envolvendo',
      Objetivo: 'Dados apresentados em tabelas e gráficos',
      Resposta: 'Realizou',
      '3ºC': 11,
      '4ºC': 2,
      Total: 13,
    },
    {
      Id: 2,
      Eixo: 'Analisa, interpreta e soluciona problemas envolvendo',
      Objetivo: 'Dados apresentados em tabelas e gráficos',
      Resposta: 'Não Realizou',
      '3ºC': 1,
      '4ºC': 2,
      Total: 3,
    },
    {
      Id: 3,
      Eixo: 'Faz outra coisa mas também analisa',
      Objetivo: 'Bem legal',
      Resposta: 'Realizou Plenamente',
      '3ºC': 2,
      '4ºC': 5,
      Total: 7,
    },
    {
      Id: 4,
      Eixo: 'Faz outra coisa mas também analisa',
      Objetivo: 'Bem legal',
      Resposta: 'Realizou',
      '3ºC': 8,
      '4ºC': 9,
      Total: 17,
    },
  ];

  return (
    <Tabela
      pagination={false}
      columns={colunas}
      dataSource={dados}
      rowKey="Id"
      size="middle"
      className="my-2"
      bordered
    />
  );
};

export default TabelaResultados;
