import { CommonModule } from '@angular/common';
import { AfterViewChecked, Component, ElementRef, ViewChild } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpEvent, HttpHandler, HttpHeaders, HttpRequest } from '@angular/common/http';
import * as signalR from '@microsoft/signalr';
import { FormsModule } from '@angular/forms';
import { UserModel } from '../../interfaces/user-model';
import { ChatModel } from '../../interfaces/chat-model';
import { Router, RouterOutlet } from '@angular/router';
import { ImageModel } from '../../interfaces/image-model';
import { catchError, map, Observable, switchMap, throwError } from 'rxjs';
import { TokenResponse } from '../../interfaces/token-response';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule,FormsModule,RouterOutlet],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements AfterViewChecked {
  @ViewChild('chatHistory', { static: false }) private chatHistory!: ElementRef;
  route : Router = new Router();
  users: UserModel[] = [];
  chats: ChatModel[] = [];
  images : ImageModel[] = [];
  file: any;
  selectedUserId: string = "";
  selectedUser: UserModel = new UserModel();
  user = new UserModel();
  hub: signalR.HubConnection | undefined;
  message: string = "";  
  userName: string ="";
  userRole : string ="";
  private tokenKey = 'token';
  private refreshToken = 'refreshToken';
  private isRefreshing = false;
  searchTerm: string = ''; // Arama terimi
  filteredUsers: any[] = []; // Filtrelenmiş kullanıcı listesi
  
  constructor(
    private http: HttpClient
  ){
    this.user = JSON.parse(localStorage.getItem("Id") ?? "");
    this.userName = this.user.name;
    this.userRole = this.user.role;
    this.getUsers();

    this.hub = new signalR.HubConnectionBuilder().withUrl("https://localhost:7054/chat-hub").build();

    this.hub.start().then(()=> {
      
      console.log("Connection is started...");  
      
      this.hub?.invoke("Connect", this.user.id);

      this.hub?.on("Users", (res:UserModel) => {
        console.log("XX",res);
        this.users.find(p=> p.id == res.id)!.status = res.status;        
      });
      this.filteredUsers = this.users; 
      this.hub?.on("Messages",(res:ChatModel)=> {
        console.log("YY", res);        
        if(this.selectedUserId == res.userId){
          this.chats.push(res);
          this.markAsRead(res.userId,res.toUserId);
        }
      })
    })
  }

  ngAfterViewChecked() {        
    this.scrollToBottom();        
  } 

  scrollToBottom(): void {
    try {
        this.chatHistory.nativeElement.scrollTop = this.chatHistory.nativeElement.scrollHeight;
    } catch(err) { }                 
  }

  getUsers(){
    this.http.get<UserModel[]>("https://localhost:7054/api/Chats/GetUsers").subscribe(res=> {
      console.log("ss")
      this.users = res.filter(p => p.id != this.user.id);
    })
  }

  

  changeUser(user: UserModel){
    this.selectedUserId = user.id;
    this.selectedUser = user;

    this.http.get(`https://localhost:7054/api/Chats/GetChats?UserId=${this.user.id}&toUserId=${this.selectedUserId}`).subscribe((res:any)=>{
      console.log("changeuser", this.chats)
      this.chats = res;
      this.chats.forEach(chat=> {
        if(!chat.isRead && chat.toUserId == this.user.id){
          this.markAsRead(chat.userId,chat.toUserId);
        }
      })
    });
  }

  markAsRead(userId : string, toUserId : string) {
    const data = {userId, toUserId}
    this.http.post(`https://localhost:7054/api/Chats/MarkAsRead`, data).subscribe(() => {
      const chat = this.chats.find(c => c.userId === userId && c.toUserId === toUserId);    
        if(chat){
          chat.isRead = true;
        }   
    });
}

getToken(): string | null {
  return localStorage.getItem(this.tokenKey);
}

  register() {
    this.route.navigate(['/register']);
  }

  logout(){
    this.hub?.stop().then(()=>{
      console.log("Connection Closed");
      localStorage.clear();
      document.location.reload();
    })
    
  }

  sendMessage(){

    const data ={
      "userId": this.user.id,
      "toUserId": this.selectedUserId,
      "message": this.message
    }
    this.http.post<ChatModel>("https://localhost:7054/api/Chats/SendMessage",data).subscribe
      (res=> {
        this.chats.push(res);
        this.message = "";
        
    });
  }

  selectImage() {
    document.getElementById('fileInput')!.click();
  }

  onFileSelected(event: any) {
    this.file = event.target.files[0];
    if (this.file) {
      console.log(`Selected file: ${this.file.name}`);
      this.sendImage();
    }
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = localStorage.getItem(this.tokenKey);
    if (token) {
      req = this.addToken(req, token);
    }

    return next.handle(req).pipe(
      catchError(error => {
        if (error instanceof HttpErrorResponse && error.status === 401) {
          return this.handle401Error(req, next);
        } else {
          return throwError(error);
        }
      })
    );
  }

  private addToken(req: HttpRequest<any>, token: string) {
    return req.clone({
      headers: req.headers.set('Authorization', `Bearer`+token)
    });
  }
  UpdateRefreshToken(): Observable<TokenResponse> {
    const refresh = localStorage.getItem('refreshToken');
    return this.http.get<TokenResponse>("https://localhost:7054/api/Auth/RefreshToken?refreshToken=" +refresh).pipe(
      map(response => {
        localStorage.setItem(this.refreshToken,response.refreshToken);
        localStorage.setItem(this.tokenKey,response.token);
        return response;
      })
    );
  }
  
  private handle401Error(req: HttpRequest<any>, next: HttpHandler) {
    if (!this.isRefreshing) {
      this.isRefreshing = true;
      return this.UpdateRefreshToken().pipe(
        switchMap((response: any) => {
          this.isRefreshing = false;
          return next.handle(this.addToken(req, response.token));
        }),
        catchError((err) => {
          this.isRefreshing = false;
          this.logout();
          return throwError(err);
        })
      );
    } else {
      return throwError('Token refresh is already in progress.');
    }
  }
  searchUsers(): void {
    if (this.searchTerm) {
      this.filteredUsers = this.users.filter(user => 
        user.name.toLowerCase().includes(this.searchTerm.toLowerCase())
      );
    } else {
      this.filteredUsers = this.users;
    }
  }
  sendImage() {
    debugger;
    if (this.file && this.selectedUserId) {
    debugger;
      const formData = new FormData();
      formData.append('UserId', this.user.id);
      formData.append('toUserId', this.selectedUserId);
      formData.append('File', this.file);
      const token = this.getToken();
      let header = new HttpHeaders().set("Authorization","bearer "+token)
      this.http.post<ChatModel>("https://localhost:7054/api/Chats/SendImage", formData,{headers:header}).subscribe(res => {
        this.chats.push(res);
        this.file = null;
      });
      
      
    }

}

}