import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Processo, ProcessoAdicionarCommand, TipoProcesso } from '../../models/processo.model';
import { ProcessoService } from '../../services/processo.service';
import { ActivatedRoute, Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-adicionar-processo',
  templateUrl: './adicionar-processo.component.html',
  styleUrls: ['./adicionar-processo.component.scss']
})

export class AdicionarProcessoComponent implements OnInit {
  private _id: string | null = null;

  tipos = [TipoProcesso.AuxiliadoPorFerramenta, TipoProcesso.Manual, TipoProcesso.Sistemico];
  form: FormGroup;
  subprocessos: Processo[] = [];

  constructor(
    private fb: FormBuilder,
    private service: ProcessoService,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private router: Router,
  ) {
    this.form = this.fb.group({
      nome: ['', Validators.required],
      descricao: ['', Validators.required],
      tipo: [TipoProcesso.AuxiliadoPorFerramenta, Validators.required],
      ferramenta: [''],
      subprocessos: [],
    });
  }

  ngOnInit(): void {
    this._id = this.route.snapshot.paramMap.get('id');

    this.service.buscar().subscribe(res => {
      this.subprocessos = res.filter(processo => processo.id !== this._id);
    });
    
    if (this._id) {
      this.service.buscarPorId(this._id).subscribe(res => {
        this.form.patchValue({ ...res })

        if (res.tipo === TipoProcesso.AuxiliadoPorFerramenta) {
          this.form.get('ferramenta')?.enable();
        } else {
          this.form.get('ferramenta')?.disable();
        }
      })
    }
  }

  onSubmit(): void {
    const processo: ProcessoAdicionarCommand = this.buildCommand()

    if (!this.form.valid) {
      return;
    }

    if (this._id) {
      this.service.editar(processo).subscribe(() => this.onSucess());
    } else {
      this.service.adicionar(processo).subscribe(() => this.onSucess());
    }
  }

  compareSubprocessos(sp1: Processo, sp2: Processo): boolean {
    return sp1 && sp2 ? sp1.id === sp2.id : sp1 === sp2;
  }

  montarTitulo() {
    const verbo = this._id ? 'Editar' : 'Adicionar';
    return `${verbo} Processo`;
  }

  onVoltar() {
    this.router.navigate(['processos'])
  }

  formataTipo(tipo: TipoProcesso) {
    switch (tipo) {
      case TipoProcesso.AuxiliadoPorFerramenta: return "Auxiliado por Ferramenta";
      case TipoProcesso.Manual: return "Manual";
      case TipoProcesso.Sistemico: return "Sistêmico";
    }
  }

  onTipoChange(tipo: any) {
    if (tipo.value === TipoProcesso.AuxiliadoPorFerramenta) {
      this.form.get('ferramenta')?.enable();
    } else {
      this.form.get('ferramenta')?.disable();
    }
  }

  private onSucess(): void {
    this.snackBar.open('Processo inserido com sucesso.', 'x', { duration: 5000 });
    this.router.navigate(['processos'])
  }

  private buildCommand(): ProcessoAdicionarCommand {
    return {
      ...this.form.value,
      id: this._id,
      subprocessos: this.form.value.subprocessos?.map((x: Processo) => x.id)
    };
  }
}
