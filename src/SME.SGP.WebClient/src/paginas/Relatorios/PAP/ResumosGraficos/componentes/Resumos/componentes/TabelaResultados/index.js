import React, { useEffect, useState } from 'react';
import styled from 'styled-components';
import shortid from 'shortid';

// Ant
import { Table } from 'antd';

// Services
import ResumosGraficosPAPServico from '~/servicos/Paginas/Relatorios/PAP/ResumosGraficos';
import { Base } from '~/componentes';

const Tabela = styled(Table)`
  th.headerTotal {
    background-color: ${Base.Roxo};
    color: ${Base.Branco};
  }
`;

const TabelaResultados = () => {
  let tamanhoObjetivos = 2;
  let tamanhoRespostas = 4;

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
            rowSpan:
              index % (tamanhoObjetivos * tamanhoRespostas) === 0
                ? tamanhoObjetivos * tamanhoRespostas
                : 0,
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
            rowSpan: index % tamanhoRespostas === 0 ? tamanhoRespostas : 0,
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
        const montaDados = [];

        const eixos = [...data[0].Eixos];

        eixos.forEach(eixo => {
          // if (eixo.Objetivos.length > tamanhoObjetivos) {
          //   tamanhoObjetivos = eixo.Objetivos.length;
          // }
          eixo.Objetivos.forEach(objetivo => {
            objetivo.Anos.forEach(ano => {
              montaColunas.push({
                title: `${ano.AnoDescricao}`,
                dataIndex: `${ano.AnoDescricao}`,
              });
              ano.Respostas.forEach(resposta => {
                const item = {};
                item.Id = shortid.generate();
                item.Eixo = eixo.EixoDescricao;
                item.Objetivo = objetivo.ObjetivoDescricao;
                item.Resposta = resposta.RespostaDescricao;
                objetivo.Anos.forEach(anoResposta => {
                  item[anoResposta.AnoDescricao] = resposta.Quantidade;
                });
                montaDados.push(item);
              });
              // if (ano.Respostas.length > tamanhoRespostas) {
              //   tamanhoRespostas = ano.Respostas.length;
              // }
            });
          });
        });

        console.log(montaDados);

        montaColunas.push({
          title: 'Total',
          dataIndex: 'Total',
          width: 100,
          fixed: 'right',
          className: 'headerTotal',
          render: text => {
            return {
              children: text,
              props: {
                style: { backgroundColor: Base.CinzaTabela },
              },
            };
          },
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
      Id: shortid.generate(),
      Eixo: 'Analisa, interpreta e soluciona problemas envolvendo',
      Objetivo: 'Dados apresentados em tabelas e gráficos',
      Resposta: 'Realizou Plenamente',
      '3ºC': 13,
      '4ºC': 2,
      Total: 15,
    },
    {
      Id: shortid.generate(),
      Eixo: 'Analisa, interpreta e soluciona problemas envolvendo',
      Objetivo: 'Dados apresentados em tabelas e gráficos',
      Resposta: 'Realizou',
      '3ºC': 11,
      '4ºC': 2,
      Total: 13,
    },
    {
      Id: shortid.generate(),
      Eixo: 'Analisa, interpreta e soluciona problemas envolvendo',
      Objetivo: 'Dados apresentados em tabelas e gráficos',
      Resposta: 'Não Realizou',
      '3ºC': 1,
      '4ºC': 2,
      Total: 3,
    },
    {
      Id: shortid.generate(),
      Eixo: 'Analisa, interpreta e soluciona problemas envolvendo',
      Objetivo: 'Dados apresentados em tabelas e gráficos',
      Resposta: 'Não avaliado',
      '3ºC': 0,
      '4ºC': 1,
      Total: 1,
    },
    {
      Id: shortid.generate(),
      Eixo: 'Analisa, interpreta e soluciona problemas envolvendo',
      Objetivo: 'Significados do campo multiplicativo',
      Resposta: 'Realizou Plenamente',
      '3ºC': 1,
      '4ºC': 2,
      Total: 3,
    },
    {
      Id: shortid.generate(),
      Eixo: 'Analisa, interpreta e soluciona problemas envolvendo',
      Objetivo: 'Significados do campo multiplicativo',
      Resposta: 'Realizou',
      '3ºC': 1,
      '4ºC': 2,
      Total: 3,
    },
    {
      Id: shortid.generate(),
      Eixo: 'Analisa, interpreta e soluciona problemas envolvendo',
      Objetivo: 'Significados do campo multiplicativo',
      Resposta: 'Não Realizou',
      '3ºC': 1,
      '4ºC': 2,
      Total: 3,
    },
    {
      Id: shortid.generate(),
      Eixo: 'Analisa, interpreta e soluciona problemas envolvendo',
      Objetivo: 'Significados do campo multiplicativo',
      Resposta: 'Não avaliado',
      '3ºC': 1,
      '4ºC': 2,
      Total: 3,
    },
    {
      Id: shortid.generate(),
      Eixo: 'Faz outra coisa mas também analisa',
      Objetivo: 'Bem legal',
      Resposta: 'Realizou Plenamente',
      '3ºC': 2,
      '4ºC': 5,
      Total: 7,
    },
    {
      Id: shortid.generate(),
      Eixo: 'Faz outra coisa mas também analisa',
      Objetivo: 'Bem legal',
      Resposta: 'Realizou',
      '3ºC': 8,
      '4ºC': 9,
      Total: 17,
    },
    {
      Id: shortid.generate(),
      Eixo: 'Faz outra coisa mas também analisa',
      Objetivo: 'Bem legal',
      Resposta: 'Não Realizou',
      '3ºC': 10,
      '4ºC': 11,
      Total: 21,
    },
    {
      Id: shortid.generate(),
      Eixo: 'Faz outra coisa mas também analisa',
      Objetivo: 'Bem legal',
      Resposta: 'Não avaliado',
      '3ºC': 1,
      '4ºC': 0,
      Total: 1,
    },
    {
      Id: shortid.generate(),
      Eixo: 'Faz outra coisa mas também analisa',
      Objetivo: 'Outra coisa',
      Resposta: 'Realizou Plenamente',
      '3ºC': 2,
      '4ºC': 5,
      Total: 7,
    },
    {
      Id: shortid.generate(),
      Eixo: 'Faz outra coisa mas também analisa',
      Objetivo: 'Outra coisa',
      Resposta: 'Realizou',
      '3ºC': 8,
      '4ºC': 9,
      Total: 17,
    },
    {
      Id: shortid.generate(),
      Eixo: 'Faz outra coisa mas também analisa',
      Objetivo: 'Outra coisa',
      Resposta: 'Não Realizou',
      '3ºC': 10,
      '4ºC': 11,
      Total: 21,
    },
    {
      Id: shortid.generate(),
      Eixo: 'Faz outra coisa mas também analisa',
      Objetivo: 'Outra coisa',
      Resposta: 'Não avaliado',
      '3ºC': 1,
      '4ºC': 0,
      Total: 1,
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
