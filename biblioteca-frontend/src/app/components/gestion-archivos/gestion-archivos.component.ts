import { Component } from '@angular/core';
import { ArchivoService } from '../../services/archivo.service';
import { saveAs } from 'file-saver';
// import JSZip from 'jszip';
import { Archivo } from '../../models/archivo.model';
import { TokenService } from '../../services/token.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-gestion-archivos',
  templateUrl: './gestion-archivos.component.html',
  styleUrl: './gestion-archivos.component.css'
})
export class GestionArchivosComponent {
  archivos: Archivo[]=[];
  archivoZip?: Blob;
  idPerfil?: number;
  isLoading: boolean = true;
  busqueda: string = "";
  listaTotal: any[] = [];

  listaAuxiliar: any[]=[];
  paginaActual:number = 0;
  numeroRegistros: number = 5;
  numeroPaginas: number = 0;
  arreglo: any = [];
  error: boolean = false;

  constructor(private archivoService: ArchivoService, private tokenService: TokenService, private toastr: ToastrService){ }

  ngOnInit(){
    this.consultarArchivos(0);
    this.consultarId();
  }

  esAdmin(){
    const token = this.tokenService.decodificarToken();
    return token.role == "Administrador";
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
    this.archivos = this.listaAuxiliar.slice(pagina*this.numeroRegistros,pagina*this.numeroRegistros+this.numeroRegistros);
  }

  consultarId(){
    const token = this.tokenService.decodificarToken();
    this.idPerfil = token.id;
  }

  consultarArchivos(pagina:number){
    this.archivoService.consultarTodos().subscribe({
      next: res => {
        this.archivos = [...res];
        this.listaAuxiliar = [...res];
        this.listaTotal = [...res];
        this.isLoading = false;
        this.calcularPaginas(pagina);
        if (this.archivos.length<=0 && this.listaAuxiliar.length>0) {
          this.consultarArchivos(pagina-1);
          this.paginaActual--;
        }
      },
      error: err => {
        console.log(err);
        this.isLoading = false;
        this.error = true;
      }
    });
  }

  cargarArchivo(evento: Event){
    const target = evento.target as HTMLInputElement;
    const archivo = target.files![0]
    this.archivoService.cargarArchivo(archivo, this.idPerfil!).subscribe({
      next: res => {
        alert(res.message);
        this.consultarArchivos(this.paginaActual);
      },
      error: err => {
        console.log(err);
      }
    });
  }

  descargarArchivo(i: number){
    this.archivoService.descargarArchivo(this.archivos[i].id, this.idPerfil!).subscribe({
      next: res => {
        saveAs(res, this.archivos[i].nombre);
        this.consultarArchivos(this.paginaActual);
      },
      error: err => {
        console.log(err);
      }
    });
  }

  eliminarArchivo(i: number){
    this.archivoService.eliminarArchivo(this.archivos[i].id, this.idPerfil!).subscribe({
      next: res => {
        this.toastr.error(res.message, "Archivo eliminado");
        this.consultarArchivos(this.paginaActual);
      },
      error: err => {
        console.log(err);
      }
    });
  }

  buscar(){
    this.paginaActual=0;
    this.listaAuxiliar = this.listaTotal.filter(e => {
      if(e.nombre.toLowerCase().includes(this.busqueda.toLowerCase())){
        return e;
      }
    })
    this.calcularPaginas(this.paginaActual);
  }

  descargarTodos(){
    this.archivoService.consultarTodosZip().subscribe({
      next: res => {
        saveAs(res, "archivos.zip");
      },
      error: err => {
        console.log(err);
      }
    });
  }

  paginar(evento:any){
    this.calcularPaginas(evento.target.id);
    this.paginaActual = evento.target.id
  }

  // descomprimirArchivo(blob: Blob){
  //     const zip = new JSZip();
      
  //     zip.loadAsync(blob).then((zip) => {
  //       const arreglo: Archivo[] = [];
  //       Object.keys(zip.files).forEach((filename) => {
  //         const file = zip.files[filename];
          
  //         if (!file.dir) {
  //           file.async('blob').then((content) => {
  //             arreglo.push({blob:content, nombre: filename.split(`\\`)[1], ruta: filename});
  //             this.archivos = [...arreglo];
  //           });
  //         }
  //       });
  //     }).catch((err) => {
  //       console.error('Error al descomprimir el archivo:', err);
  //     });
  //   }

}
