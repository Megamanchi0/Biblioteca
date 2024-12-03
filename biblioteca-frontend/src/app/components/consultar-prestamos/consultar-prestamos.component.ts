import { Component } from '@angular/core';
import { PrestamoService } from '../../services/prestamo.service';

@Component({
  selector: 'app-consultar-prestamos',
  templateUrl: './consultar-prestamos.component.html',
  styleUrl: './consultar-prestamos.component.css'
})
export class ConsultarPrestamosComponent {

  prestamos: any[] = [];
  isLoading: boolean = true;

  constructor(private prestamoService: PrestamoService){  }

  ngOnInit(){
    this.consultarPrestamos();
  }

  consultarPrestamos(){
    this.isLoading=true;
    this.prestamoService.consultarPrestamos().subscribe({
      next: res => {
        this.prestamos = res
        this.isLoading=false
      },
      error: err => {
        console.log(err);
      }
    })
  }

}
