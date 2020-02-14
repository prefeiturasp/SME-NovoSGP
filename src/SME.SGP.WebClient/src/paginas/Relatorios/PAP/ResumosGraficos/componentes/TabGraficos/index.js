import React, { useMemo, useState, useEffect } from 'react';
import t from 'prop-types';
import shortid from 'shortid';

// Componentes
import { BarraNavegacao, Graficos } from '~/componentes';
import EixoObjetivo from './componentes/EixoObjetivo';

// Estilos
import { Linha } from '~/componentes/EstilosGlobais';

function TabGraficos({ dados, periodo, ciclos }) {
  const [itemAtivo, setItemAtivo] = useState(null);
  const [cicloOuAno] = useState(ciclos ? 'ciclos' : 'anos');

  const objetoExistaNaLista = (objeto, lista) => {
    return lista.some(
      elemento => JSON.stringify(elemento) === JSON.stringify(objeto)
    );
  };

  const dadosTabelaFrequencia = useMemo(() => {
    const frequenciaDados = dados.frequencia;
    const dadosFormatados = [];

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

        x.linhas[0][cicloOuAno].forEach((y, key) => {
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
  }, [cicloOuAno, dados.frequencia]);

  const dadosTabelaTotalEstudantes = useMemo(() => {
    const montaDados = [];
    const dadoQuantidade = {};
    dadoQuantidade.Id = shortid.generate();
    dadoQuantidade.TipoDado = 'Quantidade';
    dadoQuantidade.FrequenciaDescricao = 'Total';

    dados.totalEstudantes[cicloOuAno].forEach(ano => {
      dadoQuantidade[
        `${cicloOuAno === 'ciclos' ? ano.cicloDescricao : ano.anoDescricao}`
      ] = ano.quantidade;
    });
    dadoQuantidade.Total = dados.totalEstudantes.quantidadeTotal;

    montaDados.push(dadoQuantidade);

    const dadoPorcentagem = {};
    dadoPorcentagem.Id = shortid.generate();
    dadoPorcentagem.TipoDado = 'Porcentagem';
    dadoPorcentagem.FrequenciaDescricao = 'Total';

    dados.totalEstudantes[cicloOuAno].forEach(ano => {
      dadoPorcentagem[
        `${cicloOuAno === 'ciclos' ? ano.cicloDescricao : ano.anoDescricao}`
      ] = Math.round(ano.porcentagem, 2);
    });

    dadoPorcentagem.Total = dados.totalEstudantes.porcentagemTotal;
    montaDados.push(dadoPorcentagem);

    return montaDados;
  }, [cicloOuAno, dados.totalEstudantes]);

  const dadosTabelaResultados = useMemo(() => {
    const resultados = [];
    dados.resultados.items.forEach(item => {
      let objetivo = {
        eixoDescricao: item.eixoDescricao,
        dados: [],
      };

      item.objetivos.forEach(obj => {
        const dadosObjetivo = [];
        objetivo = {
          ...objetivo,
          objetivoDescricao: obj.objetivoDescricao,
        };

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

            obj[cicloOuAno].forEach(years => {
              objetivoQuantidade = {
                ...objetivoQuantidade,
                [cicloOuAno === 'ciclos'
                  ? years.cicloDescricao
                  : years.anoDescricao]: z.quantidade,
              };

              objetivoPorcentagem = {
                ...objetivoPorcentagem,
                [cicloOuAno === 'ciclos'
                  ? years.cicloDescricao
                  : years.anoDescricao]: z.porcentagem,
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
          id: shortid.generate(),
          dados: dadosObjetivo,
        };

        resultados.push(objetivo);
      });
    });

    return resultados;
  }, [cicloOuAno, dados.resultados.items]);

  const dadosTabelaInformacoesEscolares = useMemo(() => {
    const resultados = [];
    dados.informacoesEscolares.forEach(item => {
      let objetivo = {
        eixoDescricao: item.eixoDescricao,
        dados: [],
      };

      item.objetivos.forEach(obj => {
        const dadosObjetivo = [];
        objetivo = {
          ...objetivo,
          objetivoDescricao: obj.objetivoDescricao,
        };

        obj[cicloOuAno].forEach(y => {
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

            obj[cicloOuAno].forEach(years => {
              objetivoQuantidade = {
                ...objetivoQuantidade,
                [cicloOuAno === 'ciclos'
                  ? years.cicloDescricao
                  : years.anoDescricao]: z.quantidade,
              };

              objetivoPorcentagem = {
                ...objetivoPorcentagem,
                [cicloOuAno === 'ciclos'
                  ? years.cicloDescricao
                  : years.anoDescricao]: z.porcentagem,
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
          id: shortid.generate(),
          dados: dadosObjetivo,
        };

        resultados.push(objetivo);
      });
    });

    return resultados;
  }, [cicloOuAno, dados.informacoesEscolares]);

  const [objetivos, setObjetivos] = useState([
    {
      id: shortid.generate(),
      eixoDescricao: 'Total',
      objetivoDescricao: 'Total de alunos no PAP',
      dados: dadosTabelaTotalEstudantes,
    },
  ]);

  useEffect(() => {
    if (periodo === '1') {
      setObjetivos(atual => [
        ...atual,
        ...dadosTabelaInformacoesEscolares,
        ...dadosTabelaResultados,
      ]);
    } else {
      setObjetivos(atual => [...atual, ...dadosTabelaResultados]);
    }
  }, [
    dadosTabelaFrequencia,
    dadosTabelaInformacoesEscolares,
    dadosTabelaResultados,
    periodo,
  ]);

  useEffect(() => {
    setItemAtivo(objetivos[0]);
  }, [objetivos]);

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
  periodo: t.string,
};

TabGraficos.defaultProps = {
  dados: [],
  periodo: '',
};

export default TabGraficos;
