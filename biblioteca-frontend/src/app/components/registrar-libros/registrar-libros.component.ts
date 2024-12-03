import { Component } from '@angular/core';
import { FormGroup,FormBuilder, Validators } from '@angular/forms';
import { LibroService } from '../../services/libro.service';
import { GeneroService } from '../../services/genero.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-registrar-libros',
  templateUrl: './registrar-libros.component.html',
  styleUrl: './registrar-libros.component.css'
})
export class RegistrarLibrosComponent {
  form: FormGroup;
  libros: any = [];
  listaGeneros: any = [];
  accion = "Registro de libros";
  msgError = "msgError-oculto";
  busqueda: string = "";
  numerr: Number = 2;
  listaLibros: any[] = [];
  listaAuxiliar: any[]=[];
  arreglo: any = []

  isLoading: boolean = true;

  paginaActual: number = 0;
  numeroPaginas: number = 0;
  numeroRegistros:number = 10

  constructor(private fb: FormBuilder, private libroService: LibroService, private generoService: GeneroService, private toastr: ToastrService){
    this.form=this.fb.group({
      id: 0,
      titulo: ["", Validators.required],
      autor: ["", Validators.required],
      anio: ["", Validators.required],
      genero: "",
      idGenero: [0, Validators.required],
      cantidadEjemplares: [0, Validators.required]
    })
  }

  avanzar(){
    this.paginaActual ++;
    this.calcularPaginas(this.paginaActual);
  }

  retroceder(){
    this.paginaActual --;
    this.calcularPaginas(this.paginaActual);

  }

  calcularPaginas(pagina: number){
    this.numeroPaginas = Math.ceil(this.listaAuxiliar.length/this.numeroRegistros);
    this.arreglo.length = this.numeroPaginas;
    this.libros = this.listaAuxiliar.slice(pagina*10,pagina*10+10);
  }

  ngOnInit(): void{
    this.consultarLista(0);
    this.consultarGeneros();
  }

  consultarLista(pagina: number){
    this.isLoading = true;
    this.libroService.obtenerLibros(false).subscribe({
      next: (respuesta) => {
        this.listaAuxiliar = respuesta;
        this.listaLibros = respuesta;
        this.calcularPaginas(pagina);
        if (this.libros.length<=0 && this.listaLibros.length>=0) {
          this.consultarLista(pagina-1);
          this.paginaActual--;
        }
        this.isLoading=false;
      },
      error: (err) => {
        console.log(err)
      }
    })
  }

  guardarLibro(){
    const libro = this.form.value;

    if (libro.idGenero==0) {
    this.msgError = "alert alert-danger text-center";
      return;
    }

    if (libro.id == 0) {
      this.libroService.guardarLibro(libro).subscribe({
        next: (res) => {
          this.toastr.success(res.message, 'Registro guardado');
          this.consultarLista(this.paginaActual);
        },
        error: (err) => {
          this.toastr.error(err.error, "Error");
        }
      })
    }else{
      this.actualizarLibro(libro)
    }

    this.form.reset();
    this.form.get('idGenero')?.setValue(0);
    this.form.get('id')?.setValue(0);
    this.form.get('cantidadEjemplares')?.setValue(0);
    this.accion = "Registro de libros";
    this.msgError = "msgError-oculto";
  }

  actualizarLibro(libro: any){
    this.libroService.actualizarLibro(libro).subscribe({
      next: (res) => {
        this.toastr.info(res.message, "Registro actualizado");
        this.consultarLista(this.paginaActual);
      },
      error: (err) => {
        this.toastr.error(err.error, "Error");
      }
    })
  }

  eliminarLibro(id: Number){
    this.libroService.eliminarLibro(id).subscribe({
      next: (res) => {
        this.toastr.error(res.message, 'Registro eliminado');
        this.consultarLista(this.paginaActual);
      },
      error: (err) => {
        this.toastr.error(err.error, "Error");
        console.log(err.error)
      }
    })
  }

  consultarGeneros(){
    this.generoService.consultarGeneros().subscribe({
      next: (res) => {
        this.listaGeneros = res
      },
      error: (err) => {
        console.log(err)
      }
    })
  }

  llenarFormulario(libro: any){
    this.accion = "Actualizar libro";
    this.form.setValue({
      id: libro.id,
      titulo: libro.titulo,
      autor: libro.autor,
      anio: libro.anio,
      genero: libro.genero,
      idGenero: libro.idGenero,
      cantidadEjemplares: 5
    });
    this.form.get('cantidadEjemplares')?.disable();

  }

  aumentarStock(){
    let cantidad=this.form.get('cantidadEjemplares');
    cantidad?.setValue(cantidad.value+1);
  }

  disminuirStock(){
    let cantidad=this.form.get('cantidadEjemplares');
    cantidad?.setValue(cantidad.value-1);
  }

  buscar(){
    let pagina = 0;
    if (this.busqueda=="") {
      pagina = this.paginaActual;
    }
    this.paginaActual=pagina;
    this.listaAuxiliar = this.listaLibros.filter(e => {
      if(e.titulo.toLowerCase().includes(this.busqueda.toLowerCase())
        ||e.autor.toLowerCase().includes(this.busqueda.toLowerCase())
        ||e.genero.toLowerCase().includes(this.busqueda.toLowerCase())
        ||e.anio.toString().includes(this.busqueda.toLowerCase())
      ){
        return e;
      }
    })
    this.calcularPaginas(pagina);
  }

  paginar(evento:any){
    this.calcularPaginas(evento.target.id);
    this.paginaActual = evento.target.id
  }
  
}
