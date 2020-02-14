import React, { useMemo, useState, useEffect } from 'react';
import t from 'prop-types';
import shortid from 'shortid';

// Componentes
import { BarraNavegacao, Graficos } from '~/componentes';
import EixoObjetivo from './componentes/EixoObjetivo';

// Estilos
import { Linha } from '~/componentes/EstilosGlobais';

function TabGraficos({ dados }) {
  const [itemAtivo, setItemAtivo] = useState(null);
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
    const montaDados = [];
    const dadoQuantidade = {};
    dadoQuantidade.Id = shortid.generate();
    dadoQuantidade.TipoDado = 'Quantidade';
    dadoQuantidade.FrequenciaDescricao = 'Total';

    dados.totalEstudantes.anos.forEach(ano => {
      dadoQuantidade[`${ano.anoDescricao}`] = ano.quantidade;
    });
    dadoQuantidade.Total = dados.totalEstudantes.quantidadeTotal;

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

  const [objetivos, setObjetivos] = useState([
    {
      id: shortid.generate(),
      eixoDescricao: 'Frequência',
      objetivoDescricao: 'Frequência na turma de PAP',
      dados: dadosTabelaFrequencia,
    },
    {
      id: shortid.generate(),
      eixoDescricao: 'Total',
      objetivoDescricao: 'Total de alunos no PAP',
      dados: dadosTabelaTotalEstudantes,
    },
  ]);

  const objetoExistaNaLista = (objeto, lista) => {
    return lista.some(
      elemento => JSON.stringify(elemento) === JSON.stringify(objeto)
    );
  };

  const dadosTabelaResultados = useMemo(() => {
    const resultados = [];
    dados.resultados.items.forEach(item => {
      let objetivo = {
        id: shortid.generate(),
        eixoDescricao: item.eixoDescricao,
        dados: [],
      };

      item.objetivos.forEach(obj => {
        objetivo = {
          ...objetivo,
          objetivoDescricao: obj.objetivoDescricao,
        };

        const dadosObjetivo = [];
        obj.anos.forEach(y => {
          let objetivoQuantidade = {};
          let objetivoPorcentagem = {};

          y.respostas.forEach(z => {
            objetivoQuantidade = {
              TipoDado: 'Quantidade',
              FrequenciaDescricao: z.respostaDescricao,
            };

            objetivoPorcentagem = {
              TipoDado: 'Porcentagem',
              FrequenciaDescricao: z.respostaDescricao,
            };

            obj.anos.forEach(years => {
              objetivoQuantidade = {
                ...objetivoQuantidade,
                [years.anoDescricao]: z.quantidade,
              };

              objetivoPorcentagem = {
                ...objetivoPorcentagem,
                [years.anoDescricao]: z.porcentagem,
              };
            });

            if (!objetoExistaNaLista(objetivoQuantidade, dadosObjetivo)) {
              dadosObjetivo.push(objetivoQuantidade);
            } else if (
              !objetoExistaNaLista(objetivoPorcentagem, dadosObjetivo)
            ) {
              dadosObjetivo.push(objetivoPorcentagem);
            }
          });
        });

        objetivo = {
          ...objetivo,
          dados: dadosObjetivo,
        };
      });

      resultados.push(objetivo);
    });

    console.log(resultados);

    return resultados;
  }, [dados]);

  useEffect(() => {
    setObjetivos(atual => [...atual, ...dadosTabelaResultados]);
    // setItemAtivo(objetivos[0].dados[0]);
    console.log('executou objetivos', objetivos);
  }, [dadosTabelaResultados]);

  useEffect(() => {
    console.log(itemAtivo);
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
