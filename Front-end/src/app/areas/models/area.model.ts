import { Processo } from "../../processos/models/processo.model";

export interface Area {
    id: string;
    nome: string;
    descricao: string;
    processos: Processo[];
}

export interface AreaAdicionarCommand {
    id: string;
    nome: string;
    descricao: string;
    processos: string[];
}
