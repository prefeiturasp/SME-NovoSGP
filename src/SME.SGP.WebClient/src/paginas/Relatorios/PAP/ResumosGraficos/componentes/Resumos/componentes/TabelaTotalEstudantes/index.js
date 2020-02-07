import React, { useEffect, useState } from 'react';
import shortid from 'shortid';
import styled from 'styled-components';

// Ant
import { Table } from 'antd';

// Componentes
import { Base, Loader } from '~/componentes';
import ResumosGraficosPAPServico from '~/servicos/Paginas/Relatorios/PAP/ResumosGraficos';

const Tabela = styled(Table)`
  th.headerTotal {
    background-color: ${Base.Roxo};
    color: ${Base.Branco};
  }
`;

const TabelaTotalEstudantes = () => {
  const [colunas, setColunas] = useState([
    {
      title: 'Ano',
      dataIndex: 'TipoDado',
      fixed: 'left',
      width: 150,
      render: text => {
        return {
          children: text,
          props: {
            style: { fontWeight: 'bold' },
          },
        };
      },
    },
  ]);

  const [filtro] = useState(true);
  const [dadosTabela, setDadosTabela] = useState([]);
  const [carregandoDados, setCarregandoDados] = useState(false);

  const buscarDadosApi = () => {
    setCarregandoDados(true);

    ResumosGraficosPAPServico.ListarTotalEstudantes(filtro)
      .then(resposta => {
        if (resposta.data) {
          const montaColunas = [];

          if (filtro) {
            resposta.data[0].Ciclos.forEach(ciclo => {
              const coluna = {};
              coluna.title = ciclo.CicloDescricao;
              coluna.dataIndex = ciclo.CicloDescricao;
              if (montaColunas.indexOf(coluna) === -1) {
                montaColunas.push(coluna);
              }
            });

            const montaDados = [];

            // Linha Quantidade

            const dadoQuantidade = {};
            dadoQuantidade.key = shortid.generate();
            dadoQuantidade.TipoDado = 'Quantidade';

            resposta.data[0].Ciclos.forEach(ano => {
              dadoQuantidade[`${ano.CicloDescricao}`] = ano.Quantidade;
            });
            dadoQuantidade.Total = resposta.data[0].QuantidadeTotal;

            montaDados.push(dadoQuantidade);

            // Linha Porcentagem

            const dadoPorcentagem = {};
            dadoPorcentagem.key = shortid.generate();
            dadoPorcentagem.TipoDado = 'Porcentagem';

            resposta.data[0].Ciclos.forEach(ano => {
              dadoPorcentagem[`${ano.CicloDescricao}`] = ano.Porcentagem;
            });
            dadoPorcentagem.Total = resposta.data[0].PorcentagemTotal;

            montaDados.push(dadoPorcentagem);

            setDadosTabela(montaDados);
          } else {
            resposta.data[0].Anos.forEach(ano => {
              const coluna = {};
              coluna.title = ano.AnoDescricao;
              coluna.dataIndex = ano.AnoDescricao;
              if (montaColunas.indexOf(coluna) === -1) {
                montaColunas.push(coluna);
              }
            });

            const montaDados = [];

            // Linha Quantidade

            const dadoQuantidade = {};
            dadoQuantidade.key = shortid.generate();
            dadoQuantidade.TipoDado = 'Quantidade';

            resposta.data[0].Anos.forEach(ano => {
              dadoQuantidade[`${ano.AnoDescricao}`] = ano.Quantidade;
            });
            dadoQuantidade.Total = resposta.data[0].QuantidadeTotal;

            montaDados.push(dadoQuantidade);

            // Linha Porcentagem

            const dadoPorcentagem = {};
            dadoPorcentagem.key = shortid.generate();
            dadoPorcentagem.TipoDado = 'Porcentagem';

            resposta.data[0].Anos.forEach(ano => {
              dadoPorcentagem[`${ano.AnoDescricao}`] = ano.Porcentagem;
            });
            dadoPorcentagem.Total = resposta.data[0].PorcentagemTotal;

            montaDados.push(dadoPorcentagem);

            setDadosTabela(montaDados);
          }

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

          console.log(montaColunas);

          setColunas([...colunas, ...montaColunas]);
        }
      })
      .finally(() => {
        setCarregandoDados(false);
      });
  };

  useEffect(() => {
    buscarDadosApi();
  }, []);

  return (
    <Loader loading={carregandoDados}>
      <Tabela
        pagination={false}
        columns={colunas}
        dataSource={dadosTabela}
        rowKey="key"
        size="middle"
        className="my-2"
        style={{ borderLeft: `7px solid ${Base.Azul}` }}
        bordered
      />
    </Loader>
  );
};

export default TabelaTotalEstudantes;
