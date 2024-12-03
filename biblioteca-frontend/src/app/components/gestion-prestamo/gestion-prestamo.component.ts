import { Component } from '@angular/core';
import { PrestamoService } from '../../services/prestamo.service';
import { TokenService } from '../../services/token.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-gestion-prestamo',
  templateUrl: './gestion-prestamo.component.html',
  styleUrl: './gestion-prestamo.component.css'
})
export class GestionPrestamoComponent {

  libros: any[] = []
  token: any = this.tokenService.decodificarToken();
  isLoading:boolean = true

  constructor(private prestamoService: PrestamoService, private tokenService: TokenService, private toastr: ToastrService){  }

  ngOnInit(){
    this.consultarLibros();
  }

  devolverPrestamo(){
    this.prestamoService.devolverPrestamo(this.token.id).subscribe({
      next: () => {
        this.toastr.success("Préstamo devuelto exitosamente", "préstamo devuelto");
        this.consultarLibros();
        
      },
      error: err => {
        console.log(err);
      }
    });
  }

  consultarLibros(){
    this.isLoading = true;
    this.prestamoService.consultarLibrosPrestamo(this.token.id).subscribe({
      next: response => {
        this.libros = response.result;
        this.isLoading=false;
      },
      error: err => {
        console.log(err);
      }
    });
  }

}
