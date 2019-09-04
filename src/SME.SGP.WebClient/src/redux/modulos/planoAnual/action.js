export function Salvar(indice, bimestre) {
    return {
        type: '@bimestres/SalvarBimestre',
        payload: {
            indice, bimestre
        },
    };
}