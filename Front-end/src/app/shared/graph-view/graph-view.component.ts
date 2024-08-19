import { Component, Input, OnChanges, SimpleChanges, ElementRef, ViewChild, AfterViewInit, Output, EventEmitter, Inject, PLATFORM_ID } from '@angular/core';
import { Processo } from '../../processos/models/processo.model';
import { Network } from 'vis-network';
import { DataSet } from 'vis-data'
import { isPlatformBrowser } from '@angular/common';

@Component({
  selector: 'app-graph-view',
  templateUrl: './graph-view.component.html',
  styleUrls: ['./graph-view.component.scss']
})
export class GraphViewComponent implements OnChanges {
  @Input() processos: Processo[] = [];
  @Output() voltar: EventEmitter<any> = new EventEmitter();
  @ViewChild('networkContainer', { static: false }) networkContainer!: ElementRef;
  private network!: Network;


  ngOnChanges(changes: SimpleChanges): void {
    if (changes['processos'] && this.processos && this.processos.length > 0) {
      this.buildGraph();
    }
  }

  buildGraph(): void {
    if (!this.networkContainer) return;

    if (this.network) {
      this.network.destroy();
    }

    const nodes = new DataSet([]);
    const edges = new DataSet([]);

    if (this.processos) {
      this.processos.forEach((processo) => {
        this.addProcessoToGraph(processo, null, nodes, edges);
      });
    }
    const data = { nodes, edges };
    const options = this.buildOptions();

    this.network = new Network(this.networkContainer.nativeElement, data, options);
  }

  onVoltar() {
    this.voltar.emit();
  }

  private addProcessoToGraph(processo: Processo, parentId: string | null, nodes: DataSet<any>, edges: DataSet<any>): void {
    const nodeId = processo.id;
    if (!nodes.get(nodeId)) {
      nodes.add({
        id: nodeId,
        label: `<b>${processo.nome} \n\n ${processo.descricao}`,
      });
    }

    if (parentId) {
      edges.add({
        from: parentId,
        to: nodeId,
      });
    }

    if (processo.subprocessos) {
      processo.subprocessos.forEach((subprocesso) => this.addProcessoToGraph(subprocesso, nodeId, nodes, edges));
    }
  }

  private buildOptions() {
    return {
      nodes: {
        shape: 'dot',
        size: 20,
        font: {
          multi: 'html',
          size: 18,
          color: '#000000',
        },
        borderWidth: 2,
        shadow: true,
      },
      edges: {
        width: 2,
        shadow: true,
        arrows: 'to',
      },
      layout: {
        hierarchical: {
          direction: 'UD',
          sortMethod: 'directed',
        }
      },
      interaction: { hover: true },
      physics: {
        enabled: false,
        stabilization: false,
        barnesHut: {
          gravitationalConstant: -2000,
          springConstant: 0.04,
          springLength: 300
        }
      }
    };
  }
}
