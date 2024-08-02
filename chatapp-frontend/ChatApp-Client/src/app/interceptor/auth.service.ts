import { Token } from '@angular/compiler';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { map, Observable } from 'rxjs';
import { TokenResponse } from '../interfaces/token-response';
import { HttpClient } from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private router : Router) { }



  getToken(){
    return localStorage.getItem('token');
  }

  isAuthenticated(){
    if(localStorage.getItem("token")){
      return true;
    }

    this.router.navigateByUrl("/login");
    return false;
  }
}