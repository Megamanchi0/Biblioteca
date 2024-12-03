import { Component } from '@angular/core';
import { TokenService } from '../../services/token.service';
import { PerfilService } from '../../services/perfil.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {

  constructor(private tokenService: TokenService, private loginService: PerfilService){  }

  isLogged(){
    return this.loginService.isLogged();
  }

  esAdmin(){
    const token = this.tokenService.decodificarToken();
    return this.isLogged() && token.role == "Administrador";
  }

}
