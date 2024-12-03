import { Component, ElementRef, ViewChild } from '@angular/core';
import { LibroService } from '../../services/libro.service';
import { TokenService } from '../../services/token.service';
import { PrestamoService } from '../../services/prestamo.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-prestamo-libros',
  templateUrl: './prestamo-libros.component.html',
  styleUrl: './prestamo-libros.component.css'
})
export class PrestamoLibrosComponent {
  libros: any[] = [];
  listaAuxiliar: any[]=[];
  busqueda: string = '';
  paginaActual:number = 0;
  numeroRegistros: number = 10;
  numeroPaginas: number = 0;

  isLoading: boolean = true;

  listaLibros: any[] = [];

  arreglo: any = [];

  claseLibros: string = "false";

  librosSeleccionados: any[] = [];

  constructor(private libroService: LibroService, private tokenService: TokenService,
     private prestamoService: PrestamoService, private toastr: ToastrService){
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
    this.libros = this.listaAuxiliar.slice(pagina*this.numeroRegistros,pagina*this.numeroRegistros+this.numeroRegistros);
  }

  ngOnInit(){
    this.consultarLibros(0);
  }

  consultarLibros(pagina:number){
    this.isLoading=true;
    this.libroService.obtenerLibros(true).subscribe({
      next: respuesta => {
        this.listaLibros=respuesta;
        this.listaAuxiliar = respuesta;
        this.calcularPaginas(pagina);
        if (this.libros.length<=0 && this.listaLibros.length>0) {
          this.consultarLibros(pagina-1);
          this.paginaActual--;
        }
        this.isLoading=false;
      },
      error: err =>{
        console.log(err);
      }
    })
  }

  buscar(){
    this.paginaActual=0;
    this.listaAuxiliar = this.listaLibros.filter(e => {
      if(e.titulo.toLowerCase().includes(this.busqueda.toLowerCase())
        ||e.autor.toLowerCase().includes(this.busqueda.toLowerCase())
        ||e.genero.toLowerCase().includes(this.busqueda.toLowerCase())
        ||e.anio.toString().includes(this.busqueda.toLowerCase())
      ){
        return e;
      }
    })
    this.calcularPaginas(this.paginaActual);
  }

  seleccionar(evento: any){
    if (evento.target.checked) {
      this.librosSeleccionados.push(this.libros.find(libro => libro.id == evento.target.value));
    }else{
      this.librosSeleccionados = this.librosSeleccionados.filter(libro => libro.id != evento.target.value);
    }
  }

  estaSeleccionado(idLibro:number){
    return this.librosSeleccionados.find(L => L.id == idLibro);
  }

  solicitarPrestamo(){
    const token = this.tokenService.decodificarToken();
    const datosPrestamo = {
      idPerfil: token.id,
      libros: this.librosSeleccionados
    }
    this.prestamoService.solicitarPrestamo(datosPrestamo).subscribe({
      next: respuesta => {
        this.toastr.success(respuesta.result, "Préstamo exitoso");
        this.consultarLibros(this.paginaActual);
      },
      error: err => {
        this.toastr.error(err.error, "Error");
      }
    });
  }

  paginar(evento:any){
    this.calcularPaginas(evento.target.id);
    this.paginaActual = evento.target.id
  }

}
