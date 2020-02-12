import React, { useMemo, useState, useEffect } from 'react';
import t from 'prop-types';
import shortid from 'shortid';

// Componentes
import { BarraNavegacao, Base, Graficos } from '~/componentes';
import EixoObjetivo from './componentes/EixoObjetivo';

// Estilos
import { Linha } from '~/componentes/EstilosGlobais';

function TabGraficos({ dados }) {
  const dadosBackend = [
    {
      key: '0',
      DescricaoFrequencia: 'Frequente',
      TipoDado: 'Quantidade',
      Cor: Base.Laranja,
      '3C': 11,
      '4C': 15,
      '4E': 20,
      '5C': 25,
      '6C': 25,
      '6B': 25,
      Total: 36,
    },
    {
      key: '1',
      DescricaoFrequencia: 'Frequente',
      TipoDado: 'Porcentagem',
      Cor: Base.Laranja,
      '3C': 11,
      '4C': 15,
      '4E': 20,
      '5C': 25,
      '6C': 25,
      '6B': 25,
      Total: 36,
    },
    {
      key: '2',
      DescricaoFrequencia: 'Pouco frequente',
      TipoDado: 'Quantidade',
      Cor: Base.Vermelho,
      '3C': 11,
      '4C': 15,
      '4E': 20,
      '5C': 25,
      '6C': 25,
      '6B': 25,
      Total: 36,
    },
    {
      key: '3',
      DescricaoFrequencia: 'Pouco frequente',
      TipoDado: 'Porcentagem',
      Cor: Base.Vermelho,
      '3C': 11,
      '4C': 15,
      '4E': 20,
      '5C': 25,
      '6C': 25,
      '6B': 25,
      Total: 36,
    },
  ];

  const dadosTabelaFrequencia = useMemo(() => {
    const frequenciaDados = dados.frequencia;
    const dadosFormatados = [];
    const mapa = { turma: 'anos', ciclos: 'ciclos' };

    if (frequenciaDados) {
      frequenciaDados.forEach(x => {
        let quantidade = {
          FrequenciaDescricao: x.frequenciaDescricao,
          TipoDado: 'Quantidade',
        };

        let porcentagem = {
          FrequenciaDescricao: x.frequenciaDescricao,
          TipoDado: 'Porcentagem',
        };

        x.linhas[0][mapa.turma].forEach((y, key) => {
          quantidade = {
            ...quantidade,
            key: String(key),
            Descricao: y.descricao,
            [y.chave]: y.quantidade,
            Total: y.totalQuantidade,
          };

          porcentagem = {
            ...porcentagem,
            key: String(key),
            Descricao: y.descricao,
            [y.chave]: Math.round(y.porcentagem, 2),
            Total: Math.round(y.totalPorcentagem, 2),
          };
        });

        dadosFormatados.push(quantidade, porcentagem);
      });
    }

    return dadosFormatados;
  }, [dados]);

  const dadosTabelaTotalEstudantes = useMemo(() => {
    //Linha Quantidade
    const montaDados = [];

    const dadoQuantidade = {};
    dadoQuantidade.Id = shortid.generate();
    dadoQuantidade.TipoDado = 'Quantidade';
    dadoQuantidade.FrequenciaDescricao = 'Total';

    console.log(dados.totalEstudantes);
    dados.totalEstudantes.anos.forEach(ano => {
      dadoQuantidade[`${ano.anoDescricao}`] = ano.quantidade;
    });
    dadoQuantidade.Total = dados.totalEstudantes.quantidadeTotal;

    // Linha Porcentagem
    montaDados.push(dadoQuantidade);

    const dadoPorcentagem = {};
    dadoPorcentagem.Id = shortid.generate();
    dadoPorcentagem.TipoDado = 'Porcentagem';
    dadoPorcentagem.FrequenciaDescricao = 'Total';

    dados.totalEstudantes.anos.forEach(ano => {
      dadoPorcentagem[`${ano.anoDescricao}`] = Math.round(ano.porcentagem, 2);
    });

    dadoPorcentagem.Total = dados.totalEstudantes.porcentagemTotal;
    montaDados.push(dadoPorcentagem);

    return montaDados;
  }, [dados]);

  const objetivos = [
    {
      id: 1,
      eixoDescricao: 'Frequência',
      objetivoDescricao: 'Frequência na turma de PAP',
      dados: dadosTabelaFrequencia,
    },
    {
      id: 2,
      eixoDescricao: 'Total',
      objetivoDescricao: 'Total de alunos no PAP',
      dados: dadosTabelaTotalEstudantes,
    },
  ];
  const [itemAtivo, setItemAtivo] = useState(objetivos[0]);

  useEffect(() => {
    console.log(dadosTabelaTotalEstudantes);
  }, [dadosTabelaTotalEstudantes]);

  useEffect(() => {
    console.log(
      Object.keys(itemAtivo.dados[0]).filter(
        x =>
          [
            'TipoDado',
            'FrequenciaDescricao',
            'key',
            'Descricao',
            'Total',
            'Id',
          ].indexOf(x) === -1
      ),
      dadosTabelaTotalEstudantes
    );
  }, [itemAtivo]);
  return (
    <>
      <Linha style={{ marginBottom: '8px' }}>
        <BarraNavegacao
          itens={objetivos}
          itemAtivo={
            itemAtivo
              ? objetivos.filter(x => x.id === itemAtivo.id)[0]
              : objetivos[0]
          }
          onChangeItem={item => setItemAtivo(item)}
        />
      </Linha>
      <Linha style={{ marginBottom: '35px' }}>
        <EixoObjetivo
          eixo={itemAtivo && { descricao: itemAtivo.eixoDescricao }}
          objetivo={itemAtivo && { descricao: itemAtivo.objetivoDescricao }}
        />
      </Linha>
      <Linha style={{ marginBottom: '35px', textAlign: 'center' }}>
        <h4>Quantidade</h4>
        {itemAtivo && itemAtivo.dados && (
          <div style={{ height: 300 }}>
            <Graficos.Barras
              dados={itemAtivo.dados.filter(x => x.TipoDado === 'Quantidade')}
              indice="FrequenciaDescricao"
              chaves={Object.keys(itemAtivo.dados[0]).filter(
                x =>
                  [
                    'TipoDado',
                    'FrequenciaDescricao',
                    'key',
                    'Descricao',
                    'Total',
                    'Id',
                  ].indexOf(x) === -1
              )}
            />
          </div>
        )}
      </Linha>
      <Linha style={{ marginBottom: '35px', textAlign: 'center' }}>
        <h4>Porcentagem</h4>
        {itemAtivo && itemAtivo.dados && (
          <div style={{ height: 300 }}>
            <Graficos.Barras
              dados={
                itemAtivo &&
                itemAtivo.dados.filter(x => x.TipoDado === 'Porcentagem')
              }
              indice="FrequenciaDescricao"
              chaves={Object.keys(itemAtivo && itemAtivo.dados[0]).filter(
                x =>
                  [
                    'TipoDado',
                    'FrequenciaDescricao',
                    'key',
                    'Descricao',
                    'Total',
                  ].indexOf(x) === -1
              )}
              porcentagem
            />
          </div>
        )}
      </Linha>
    </>
  );
}

TabGraficos.propTypes = {
  dados: t.oneOfType([t.any]),
};

TabGraficos.defaultProps = {
  dados: [],
};

export default TabGraficos;
