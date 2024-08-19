export interface Processo {
    id: string;
    nome: string;
    descricao: string;
    tipo: TipoProcesso;
    ferramenta: string;
    dataCriacao: Date;
    dataUltimaAlteracao: Date;
    areaResponsavel: string;
    subprocessos: Processo[];
}

export interface ProcessoAdicionarCommand {
    id: string;
    nome: string;
    descricao: string;
    tipo: TipoProcesso;
    ferramenta: string;
    areaResponsavel: string;
    subprocessos: string[];
}

export enum TipoProcesso {
    Sistemico,
    Manual,
    AuxiliadoPorFerramenta
}
