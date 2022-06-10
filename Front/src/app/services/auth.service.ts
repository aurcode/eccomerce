import { Injectable } from '@angular/core';
import {BehaviorSubject, Observable} from "rxjs";
import { HttpClient } from '@angular/common/http';
import {environment as e} from "../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  isLoggedSubject: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(this.existsToken())

  constructor(private http: HttpClient) { }

  public isLoggedIn(): Observable<boolean> {
    return this.isLoggedSubject.asObservable();
    //return this.http.get(e.JWT_URL+"me", {headers: {"Authorization": "Bearer " + this.getToken()}}).subscribe();
  }

  public async isLogged(): Promise<void> {
    try {
      await this.http.get(e.JWT_URL+"me", {headers: {"Authorization": "Bearer " + this.getToken()}}).subscribe(
        res => console.log(res),
        err => {
          this.logout();
        },
      );
    } catch (e) {
      this.logout()
    }
  }

  public login(user: string, pwd: string): Observable<any> {
    return this.http.post<any>(e.JWT_URL+"token", {"userName":user, "password":pwd})
    //return user === AuthService.USER && pwd === AuthService.PWD
  }

  public storeToken(token: string): void {
    localStorage.setItem("token", token);
    this.isLoggedSubject.next(true);
  }

  public logout(): void {
    localStorage.removeItem("token");

    this.isLoggedSubject.next(false);
  }

  public getToken(): any {
    return localStorage.getItem("token")
  }

  private existsToken(): boolean {
    return !!localStorage.getItem("token")
  }
}
