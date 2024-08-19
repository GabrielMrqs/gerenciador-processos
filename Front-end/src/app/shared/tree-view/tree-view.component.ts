import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { Processo } from '../../processos/models/processo.model';
import { NestedTreeControl } from '@angular/cdk/tree';
import { MatTreeNestedDataSource } from '@angular/material/tree';

interface ProcessoNode {
  id: string;
  nome: string;
  subprocessos?: ProcessoNode[];
  hasParent?: boolean;
}


@Component({
  selector: 'app-tree-view',
  templateUrl: './tree-view.component.html',
  styleUrls: ['./tree-view.component.scss']
})
export class TreeViewComponent implements OnChanges {
  @Input() processos: Processo[] = [];
  @Input() enableRemove: boolean = false;
  @Output() removerNode = new EventEmitter<Processo>();

  treeControl = new NestedTreeControl<ProcessoNode>(node => node.subprocessos);
  dataSource = new MatTreeNestedDataSource<ProcessoNode>();

  ngOnChanges(changes: SimpleChanges) {
    if (changes['processos'] && this.processos && this.processos.length > 0) {
      const processData = this.convertProcessosToProcessoNodes(this.processos);
      this.dataSource.data = processData;
    }
  }

  hasChild = (_: number, node: ProcessoNode) => !!node.subprocessos && node.subprocessos.length > 0;

  onRemove(node: ProcessoNode): void {
    this.removeNode(this.dataSource.data, node);
    this.dataSource.data = [...this.dataSource.data];
    this.removerNode.emit(node as Processo);
  }

  private convertProcessosToProcessoNodes(processos: Processo[], hasParent: boolean = false): ProcessoNode[] {
    return processos.map(processo => ({
      id: processo.id,
      nome: processo.nome,
      subprocessos: processo.subprocessos ? this.convertProcessosToProcessoNodes(processo.subprocessos, true) : undefined,
      hasParent: hasParent
    }));
  }

  private removeNode(nodes: ProcessoNode[], targetNode: ProcessoNode): void {
    const index = nodes.findIndex(n => n.id === targetNode.id);
    if (index !== -1) {
      nodes.splice(index, 1);
    }
  }
}
