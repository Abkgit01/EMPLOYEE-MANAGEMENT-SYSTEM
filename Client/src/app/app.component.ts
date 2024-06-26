import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit{
  title = 'EMS';
  users: any

  constructor(private http: HttpClient) {
  }


  ngOnInit() {
    this.getUsers();
  }

  getUsers(){
    this.http.get('https://localhost:7082/api/Users/GetAllUsers').subscribe(response =>{
      this.users = response;
      }, error => {
        console.log(error)
      })
  }
}
