import { Component } from '@angular/core';
import { ArchivoService } from '../../services/archivo.service';
import { AccionService } from '../../services/accion.service';
import { Console } from 'console';

@Component({
  selector: 'app-historial-archivos',
  templateUrl: './historial-archivos.component.html',
  styleUrl: './historial-archivos.component.css'
})
export class HistorialArchivosComponent {
  registros: any[] = [];
  listaAuxiliar: any[]=[];
  listaTotal: any[] = [];
  paginaActual:number = 0;
  numeroRegistros: number = 10;
  numeroPaginas: number = 0;
  arreglo: any = [];
  isLoading: boolean = true;
  acciones: any[] = [];
  error: boolean = false;

  constructor(private archivoSevice: ArchivoService, private accionService: AccionService){ }
  ngOnInit(){
    this.consultarHistorial(0);
    this.consultarAcciones();
  }

  avanzar(){
    this.paginaActual ++;
    this.calcularPaginas(this.paginaActual);
  }

  retroceder(){
    this.paginaActual --;
    this.calcularPaginas(this.paginaActual);
  }

  calcularPaginas(pagina:number){
    this.numeroPaginas = Math.ceil(this.listaAuxiliar.length/this.numeroRegistros);
    this.arreglo.length = this.numeroPaginas;
    this.registros = this.listaAuxiliar.slice(pagina*10,pagina*10+10);
  }

  consultarHistorial(pagina:number){
    this.isLoading=true;
    this.archivoSevice.consultarHistorial().subscribe({
      next: res => {
        this.listaAuxiliar = [...res];
        this.listaTotal = [...res];
        this.isLoading=false;
        this.calcularPaginas(pagina);
      },
      error: err => {
        console.log(err);
        this.isLoading=false;
        this.error = true;
      }
    });
  }

  filtrar(evento: Event){
    const select = evento.target as HTMLSelectElement;
    this.paginaActual=0;
    if (select.value=="0") {
      this.listaAuxiliar = [...this.listaTotal];
    }else{
      this.listaAuxiliar = this.listaTotal.filter(r => r.idAccion==select.value);
    }
    this.calcularPaginas(this.paginaActual);
  }

  consultarAcciones(){
    this.accionService.consultarAcciones().subscribe({
      next: res =>{
        this.acciones = res;
      }
    });
  }

  paginar(evento:any){
    this.calcularPaginas(evento.target.id);
    this.paginaActual = evento.target.id
  }

}
