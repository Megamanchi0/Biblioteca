import { Component } from '@angular/core';
import { PerfilService } from '../../services/perfil.service';
import { TokenService } from '../../services/token.service';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrl: './menu.component.css'
})
export class MenuComponent {

  isLogged(){
    return this.loginService.isLogged();
  }

  cerrarSesion(){
    this.loginService.cerrarSesion();
  }

  esAdmin(){
    const token = this.servicioToken.decodificarToken();
    return this.isLogged() && token.role == "Administrador";
  }

  constructor(private loginService: PerfilService, private servicioToken: TokenService){

  }
}
