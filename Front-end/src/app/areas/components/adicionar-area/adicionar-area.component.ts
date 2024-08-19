import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { TipoProcesso, Processo, ProcessoAdicionarCommand } from '../../../processos/models/processo.model';
import { AreaService } from '../../services/area.service';
import { Area, AreaAdicionarCommand } from '../../models/area.model';
import { ProcessoService } from '../../../processos/services/processo.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-adicionar-area',
  templateUrl: './adicionar-area.component.html',
  styleUrls: ['./adicionar-area.component.scss']
})

export class AdicionarAreaComponent implements OnInit {
  private _id: string | null = null;

  tipos = [TipoProcesso.AuxiliadoPorFerramenta, TipoProcesso.Manual, TipoProcesso.Sistemico];
  form: FormGroup;
  processos$: Observable<Processo[]>;

  constructor(
    private fb: FormBuilder,
    private areaService: AreaService,
    private processoService: ProcessoService,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private router: Router,
  ) {
    this.form = this.fb.group({
      nome: ['', Validators.required],
      descricao: ['', Validators.required],
      processos: [[]],
    });
    this.processos$ = this.processoService.buscar();
  }

  ngOnInit(): void {

    this._id = this.route.snapshot.paramMap.get('id');
    if (this._id) {
      this.areaService.buscarPorId(this._id).subscribe(res => {
        this.form.patchValue({ ...res })
      })
    }
  }

  onSubmit(): void {
    const processo: AreaAdicionarCommand = this.buildCommand()

    if (!this.form.valid) {
      return;
    }

    if (this._id) {
      this.areaService.editar(processo).subscribe(() => this.onSucess());
    } else {
      this.areaService.adicionar(processo).subscribe(() => this.onSucess());
    }
  }

  compareProcessos(sp1: Processo, sp2: Processo): boolean {
    return sp1 && sp2 ? sp1.id === sp2.id : sp1 === sp2;
  }

  montarTitulo() {
    const verbo = this._id ? 'Editar' : 'Adicionar';
    return `${verbo} Área`;
  }

  onVoltar() {
    this.router.navigate([''], { relativeTo: this.route })
  }

  onExport(processo: Processo) {
    const naoExiste = () => !processosAtuais.map((x: Processo) => x.id).includes(processo.id)
    const processosAtuais = this.form.get('processos')?.value;
    if (naoExiste()) {
      this.form.get('processos')?.setValue([...processosAtuais, processo])
    }
  }

  onRemove(processo: Processo) {
    const processosAtuais = this.form.get('processos')?.value;
    const processos = processosAtuais.filter((x: Processo) => x.id !== processo.id);
    this.form.get('processos')?.setValue(processos)
  }

  private onSucess(): void {
    const verbo = this._id ? 'editada' : 'inserida';
    this.snackBar.open(`Área ${verbo} com sucesso.`, 'x', { duration: 5000 });
    this.router.navigate([''], { relativeTo: this.route })
  }

  private buildCommand(): AreaAdicionarCommand {
    return {
      ...this.form.value,
      id: this._id,
      processos: this.form.value.processos?.map((x: Processo) => x.id)
    };
  }
}
